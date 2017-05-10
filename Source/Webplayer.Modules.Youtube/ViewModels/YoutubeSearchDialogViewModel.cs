using MaterialDesignThemes.Wpf;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Webplayer.Modules.Youtube.Models;
using Webplayer.Modules.Youtube.Services;
using Webplayer.Modules.Youtube.Views;

namespace Webplayer.Modules.Youtube.ViewModels
{
    class YoutubeSearchDialogViewModel : BindableBase, IYoutubeSearchDialogViewModel
    {
        private readonly IUnityContainer _container;
        private IYoutubeChannelService _service;
        private string _uploaderSearchQuery;
        private ILoggerFacade _logger;
        private YoutubeUploader _selectUploader;
        private string _searchQuery;

        public ICommand RemoveUploadFilterCommand { get; set; }

        public SongSearcOrdering OrderingFilter { get; set; }
        
        public ICommand ShowPlaylistSearchCommand { get; set; }

        public ObservableCollection<YoutubeUploader> Uploaders { get; set; }

        public ICommand SearchUploaderCommand { get; set; }

        public string UploaderSearchQuery
        {
            get
            {
                return _uploaderSearchQuery;
            }
            set
            {
                SetProperty(ref _uploaderSearchQuery, value);
            }
        }

        public string SearchQuery
        {
            get
            {
                return _searchQuery;
            }
            set
            {
                SetProperty(ref _searchQuery, value);
            }
        }

        public YoutubeUploader SelectedUploader
        {
            get
            {
                return _selectUploader;
            }
            set
            {
                SetProperty(ref _selectUploader, value);
            }
        }

        public ICommand MoreUploadersCommand { get; set; }

        public YoutubeSearchDialogViewModel(IUnityContainer container, IYoutubeChannelService service,
            ILoggerFacade logger)
        {
            _container = container;
            _service = service;
            _logger = logger;
            ShowPlaylistSearchCommand = new DelegateCommand(ExecuteShowPlaylistSearchCommand);
            SearchUploaderCommand = new DelegateCommand(ExecuteSeachUploader);
            Uploaders = new ObservableCollection<YoutubeUploader>();
            MoreUploadersCommand = new DelegateCommand(MoreAction);
            RemoveUploadFilterCommand = new DelegateCommand(() => SelectedUploader = null);
        }

        private async void ExecuteSeachUploader()
        {
            try
            {
                Uploaders.Clear();
                _service = _container.Resolve<IYoutubeChannelService>();
                _service.Query = UploaderSearchQuery;
                MoreAction();
            }
            catch (Exception e)
            {
                //TODO: log
                _logger.Log(e.Message, Category.Exception, Priority.High);
                MessageBox.Show("Error occured during search");
            }
        }

        private async void MoreAction()
        {
            try
            {
                if (_service == null)
                    return;

                var next = await _service.FetchNextAsync();
                Uploaders.AddRange(next);
            }
            catch (Exception e)
            {
                _logger.Log(e.Message, Category.Exception, Priority.High);
                MessageBox.Show("Error occured during search");
            }
        }

        private async void ExecuteShowPlaylistSearchCommand()
        {
            var result = await DialogHost.Show(_container.Resolve<IYoutubeFindUploader>(), "RootDialog");
        }
    }
}
