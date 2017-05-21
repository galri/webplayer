using MaterialDesignThemes.Wpf;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webplayer.Modules.Youtube.Models;
using Webplayer.Modules.Youtube.Services;

namespace Webplayer.Modules.Youtube.ViewModels
{
    class YoutubeFindSingleViewModel : IYoutubeFindSingleViewModel
    {
        private const string TAG = "YoutubeFindSingleViewModel";
        private readonly ILoggerFacade _logger;
        private readonly IYoutubeSongSearchService _searchService;

        public YoutubeSong Result { get; set; }

        public string SearchString { get; set; }

        public ICommand SearchCommand { get; set; }

        public InteractionRequest<INotification> NotificationRequest { get; set; }

        public YoutubeFindSingleViewModel(ILoggerFacade logger, IYoutubeSongSearchService searchService)
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
            catch (ArgumentException ex)
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
