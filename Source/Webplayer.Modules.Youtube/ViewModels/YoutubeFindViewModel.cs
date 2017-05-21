using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Infrastructure.Models;
using Infrastructure.Service;
using Webplayer.Modules.Youtube.Models;
using Webplayer.Modules.Youtube.Services;
using Webplayer.Modules.Youtube.Views;
using MaterialDesignThemes.Wpf;
using Prism;
using Prism.Logging;

namespace Webplayer.Modules.Youtube.ViewModels
{
    class YoutubeFindViewModel : BindableBase, IYoutubeFindViewModel, IActiveAware
    {
        private IYoutubeSongSearchService _songSearchService;
        private readonly IQueueController _queueController;
        private ContentType _searchType;
        private ObservableCollection<YoutubeSong> _searchResult = new ObservableCollection<YoutubeSong>();
        private string _searchQuery = "";
        private bool _canFetchMore = true;
        private IUnityContainer _container;
        private YoutubeUploader _uploader;
        private SongSearcOrdering _orderingFilter;
        private bool _isActive;
        private ILoggerFacade _logger;

        public event EventHandler IsActiveChanged;

        public bool CanFetchMore
        {
            get
            {
                return _canFetchMore;
            }

            set
            {
                if(value != _canFetchMore)
                {
                    _canFetchMore = value;
                    OnPropertyChanged(() => CanFetchMore);
                }
            }
        }

        public ICommand FetchMoreResultCommand
        {
            get;

            set;
        }

        public ICommand SearchCommand
        {
            get;

            set;
        }

        public ICommand AddSongCommand { get; set; }

        public ICommand PreviewCommand
        {
            get;

            set;
        }

        public ICommand FocusSearchFieldCommand { get; set; }

        public ICommand ShowPlaylistSearchCommand { get; set; }

        public string SearchQuery
        {
            get
            {
                return _searchQuery;
            }

            set
            {
                if(_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged(() => SearchQuery);
                }
            }
        }

        public ObservableCollection<YoutubeSong> SearchResult
        {
            get
            {
                return _searchResult;
            }

            set
            {
                if(_searchResult != value)
                {
                    _searchResult = value;
                    OnPropertyChanged(() => _searchResult);
                }
            }
        }

        public ContentType SearchType
        {
            get
            {
                return _searchType;
            }

            set
            {
                if(_searchType != value)
                {
                    _searchType = value;
                    OnPropertyChanged(() => SearchType);
                }
            }
        }

        public YoutubeUploader UploaderFilter
        {
            get
            {
                return _uploader;
            }

            set
            {
                if(SetProperty(ref _uploader, value))
                {
                    SearchResult.Clear();
                }
            }
        }

        public ICommand RemoveUploadFilterCommand
        {
            get;

            set;
        }

        public SongSearcOrdering OrderingFilter
        {
            get
            {
                return _orderingFilter;
            }

            set
            {
                SetProperty(ref _orderingFilter, value);
            }
        }

        public ICommand SearchSingleCommand { get; }

        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                if(SetProperty(ref _isActive, value))
                {
                    IsActiveChanged?.Invoke(this, new EventArgs());
                }
            }
        }

        public YoutubeFindViewModel(IUnityContainer container, 
            IYoutubeSongSearchService songSearchService,
            IQueueController queueController, ILoggerFacade logger)
        {
            _songSearchService = songSearchService;
            _queueController = queueController;
            _container = container;
            _logger = logger;
            SearchCommand = new DelegateCommand(SearchCommandAction);
            FetchMoreResultCommand = new DelegateCommand(FetchMoreResultCommandAction);
            AddSongCommand = new DelegateCommand<object>(AddSongAction);
            PreviewCommand = new DelegateCommand<object>(PreviewSong);
            RemoveUploadFilterCommand = new DelegateCommand(RemoveUploadFilterAction);
            FocusSearchFieldCommand = new DelegateCommand(FocusSearchFieldAction);
            SearchSingleCommand = new DelegateCommand(SingleSearchAction);
            ShowPlaylistSearchCommand = new DelegateCommand(ShowPlaylistSearchAction);
            GlobalCommands.ShowSearchFieldInActiveCommand.RegisterCommand(FocusSearchFieldCommand);
        }

        private async void SingleSearchAction()
        {
            var v = _container.Resolve<YoutubeFindSingleView>();
            await DialogHost.Show(v, "RootDialog");
            var result = ((YoutubeFindSingleViewModel)((UserControl)v).DataContext).Result;
            SearchResult.Clear();
            SearchResult.Add(result);
        }

        private void FocusSearchFieldAction()
        {
            if (!IsActive)
                return;
            SearchCommand.Execute(null);
        }

        private void RemoveUploadFilterAction()
        {
            UploaderFilter = null;
        }

        private async void ShowPlaylistSearchAction()
        {
            //var vm = _container.Resolve<IYoutubeFindUploaderViewModel>();
            var view = new YoutubeFindUploaderView();
            //view.DataContext = vm;
            var result = await DialogHost.Show(view, "RootDialog");

            //if (result is bool && ((bool)result))
            //{
            //    UploaderFilter = vm.SelectedUploader;
            //}
        }

        private async void PreviewSong(object param)
        {
            var shouldRestartSong = false;
            if(_queueController.IsPlaying)
            {
                _queueController.IsPlaying = false;
                shouldRestartSong = true;
            }
            var song = ((YoutubeSong)((Button)((RoutedEventArgs)param).Source).DataContext);
            var vm = new YoutubePreviewViewModel(song);
            var view = new YoutubePreviewView();
            view.DataContext = vm;
            await DialogHost.Show(view, "RootDialog");

            if(shouldRestartSong)
                _queueController.IsPlaying = true;
        }

        private void AddSongAction(object param)
        {
            _queueController.AddSongToQueue( ((BaseSong) ( (Button) ((RoutedEventArgs)param).Source).DataContext ));
        }

        private async void FetchMoreResultCommandAction()
        {
            try
            {

                foreach (var item in await _songSearchService.FetchAsync())
                {
                    SearchResult.Add(item);
                }
            }
            catch (Exception)
            {
                //TODO:write can fetchhmore method....
            }
        }

        private async void SearchCommandAction()
        {
            try
            {
                var view = _container.Resolve<IYoutubeSearchDialogView>();
                var result = await DialogHost.Show(view, "RootDialog");
                bool resultBool = false;
                if (result is bool)
                    resultBool = (bool)result;

                if (resultBool)
                {
                    SearchResult.Clear();
                    var vm = ((UserControl)view).DataContext as IYoutubeSearchDialogViewModel;
                    if (vm == null)
                        return;

                    _songSearchService.Query = vm.SearchQuery;
                    if (vm.SelectedUploader != null)
                    {
                        _logger.Log($"Filtering Youtube search on uploader{vm.SelectedUploader.Id}", Category.Debug, Priority.Low);
                        _songSearchService.UploaderId = vm.SelectedUploader.Id;
                    }
                    FetchMoreResultCommandAction();
                }
                else
                {
                    _logger.Log($"Youtube search dialog abort", Category.Debug, Priority.Low);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error occured searching for youtube result");
                _logger.Log(e.Message, Category.Debug, Priority.Low);
            }
        }
    }
}
