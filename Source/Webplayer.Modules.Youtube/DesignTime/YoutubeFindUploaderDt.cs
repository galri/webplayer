using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webplayer.Modules.Youtube.Models;
using Webplayer.Modules.Youtube.ViewModels;

namespace Webplayer.Modules.Youtube.DesignTime
{
    class YoutubeFindUploaderDt : IYoutubeFindUploaderViewModel
    {
        public ICommand MoreCommand
        {
            get;

            set;
        }

        public ICommand SearchCommand
        {
            get;

            set;
        }

        public string SearchQuery
        {
            get;

            set;
        } = "String wuery";

        public YoutubeUploader SelectedUploader
        {
            get;

            set;
        }

        public ObservableCollection<YoutubeUploader> Uploaders
        {
            get;

            set;
        }

        public YoutubeFindUploaderDt()
        {
            Uploaders = new ObservableCollection<YoutubeUploader>()
            {
                new YoutubeUploader()
                {
                    Name = "First",
                    Description = "description",
                    Id = "asd",
                },
                new YoutubeUploader()
                {
                    Name = "First",
                    Description = "description",
                    Id = "asd",
                }
            };

            SelectedUploader = Uploaders[1];
        }
    }
}
