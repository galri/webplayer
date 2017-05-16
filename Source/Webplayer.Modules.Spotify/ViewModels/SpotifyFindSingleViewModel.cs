using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Logging;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Webplayer.Modules.Spotify.Models;
using Webplayer.Modules.Spotify.Services;

namespace Webplayer.Modules.Spotify.ViewModels
{
    class SpotifyFindSingleViewModel : BindableBase, ISpotifyFindSingleViewModel
    {
        private const string TAG = "SpotifyFindSingleViewModel";
        private SpotifySong _result;
        private string _searchString;
        private ISpotifySongSearch _searchService;
        private ILoggerFacade _logger;

        public SpotifySong Result
        {
            get
            {
                return _result;
            }
            set
            {
                SetProperty(ref _result, value);
            }
        }

        public string SearchString
        {
            get
            {
                return _searchString;
            }
            set
            {
                SetProperty(ref _searchString, value);
            }
        }

        public DelegateCommand SearchCommand { get; set; }

        public InteractionRequest<INotification> NotificationRequest {get; set;}

        public SpotifyFindSingleViewModel(ISpotifySongSearch searchService, ILoggerFacade logger)
        {
            _searchService = searchService;
            _logger = logger;
            SearchCommand = new DelegateCommand(SearchAction);
            NotificationRequest = new InteractionRequest<INotification>();
        }

        private async void SearchAction()
        {
            try
            {
                var searchResult = await _searchService.FetchSongAsync(SearchString);
                Result = searchResult;
                DialogHost.CloseDialogCommand.Execute(true, null);
            }
            catch(ArgumentException ex)
            {
                _logger.Log($"{TAG}: exception {ex}", Category.Exception, Priority.Medium);
                NotificationRequest.Raise(new Notification
                {
                    Title = "Search Error",
                    Content = "Unknow id"
                });
            }
            catch (Exception ex)
            {
                _logger.Log($"{TAG}: exception {ex}", Category.Exception, Priority.Medium);
                NotificationRequest.Raise(new Notification
                {
                    Title = "Search Error",
                    Content = "Unknown Error"
                });
            }
        }
    }
}
