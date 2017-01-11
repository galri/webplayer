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

namespace Webplayer.Modules.Youtube.ViewModels
{
    class YoutubeFindViewModel : BindableBase, IYoutubeFindViewModel
    {
        private readonly IYoutubeSongSearchService _songSearchService;
        private readonly IQueueController _queueController;
        private ContentType _searchType;
        private ObservableCollection<YoutubeSong> _searchResult = new ObservableCollection<YoutubeSong>();
        private string _searchQuery;
        private bool _canFetchMore = true;
        private IUnityContainer _container;
        private YoutubeUploader _uploader;

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

        public YoutubeFindViewModel(IUnityContainer container, 
            IYoutubeSongSearchService songSearchService,
            IQueueController queueController)
        {
            _songSearchService = songSearchService;
            _queueController = queueController;
            _container = container;
            SearchCommand = new DelegateCommand(SearchCommandAction);
            FetchMoreResultCommand = new DelegateCommand(FetchMoreResultCommandAction);
            AddSongCommand = new DelegateCommand<object>(AddSongAction);
            PreviewCommand = new DelegateCommand<object>(PreviewSong);
            RemoveUploadFilterCommand = new DelegateCommand(RemoveUploadFilterAction);

            ShowPlaylistSearchCommand = new DelegateCommand(ShowPlaylistSearchAction);
        }

        private void RemoveUploadFilterAction()
        {
            UploaderFilter = null;
        }

        private async void ShowPlaylistSearchAction()
        {
            var vm = _container.Resolve<IYoutubeFindUploaderViewModel>();
            var view = new YoutubeFindUploader();
            view.DataContext = vm;
            var result = await DialogHost.Show(view, "RootDialog");

            if (result is bool && ((bool)result))
            {
                UploaderFilter = vm.SelectedUploader;
            }
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
            if (String.IsNullOrEmpty(SearchQuery))
                return;

            foreach (var item in await _songSearchService.FetchAsync())
            {
                SearchResult.Add(item);
            }
        }

        private async void SearchCommandAction()
        {
            if (String.IsNullOrEmpty(SearchQuery))
                return;

            SearchResult.Clear();
            //Search after spesific song
            if (SearchQuery.StartsWith("v="))
            {
                var song = await _songSearchService.FetchSongAsync(SearchQuery.Substring(2));
                SearchResult.Add(song);
                return;
            }
            _songSearchService.Query = SearchQuery;
            if(UploaderFilter != null)
            {
                _songSearchService.UploaderId = UploaderFilter.Id;
            }
            else
            {
                _songSearchService.UploaderId = null;
            }

            foreach (var item in await _songSearchService.FetchAsync())
            {
                SearchResult.Add(item);
            }
        }
    }
}
