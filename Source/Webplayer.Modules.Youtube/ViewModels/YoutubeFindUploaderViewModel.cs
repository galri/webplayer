using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webplayer.Modules.Youtube.Models;
using Webplayer.Modules.Youtube.Services;

namespace Webplayer.Modules.Youtube.ViewModels
{
    class YoutubeFindUploaderViewModel : BindableBase, IYoutubeFindUploaderViewModel
    {
        private IYoutubeChannelService _service;
        private IUnityContainer _container;

        public ObservableCollection<YoutubeUploader> Uploaders { get; set; } = new ObservableCollection<YoutubeUploader>();

        public YoutubeUploader SelectedUploader { get; set; }

        public string SearchQuery { get; set; }

        public ICommand SearchCommand { get; set; } 

        public ICommand MoreCommand { get; set; }

        public YoutubeFindUploaderViewModel(IUnityContainer container)
        {
            _container = container;
            MoreCommand = new DelegateCommand(MoreAction);
            SearchCommand = new DelegateCommand(SearchAction);
        }

        private async void SearchAction()
        {
            Uploaders.Clear();
            _service = _container.Resolve<IYoutubeChannelService>();
            _service.Query = SearchQuery;
            MoreAction();
        }

        private async void MoreAction()
        {
            if (_service == null)
                return;

            var next = await _service.FetchNextAsync();
            Uploaders.AddRange(next);
        }
    }
}
