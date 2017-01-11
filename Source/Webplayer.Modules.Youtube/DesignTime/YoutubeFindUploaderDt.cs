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
        } = "mrsuicidesheep";

        public YoutubeUploader SelectedUploader
        {
            get;

            set;
        }

        public ObservableCollection<YoutubeUploader> Uploaders
        {
            get;

            set;
        } = new ObservableCollection<YoutubeUploader>
        {
            new YoutubeUploader
            {
                Name = "Mr Suicide Sheep",
                Thumbnail = new Uri("https://yt3.ggpht.com/-nkC01SMBUms/AAAAAAAAAAI/AAAAAAAAAAA/X4GElTDAELg/s240-c-k-no-mo-rj-c0xffffff/photo.jpg"),
                Description = "On this channel you will find a wide variety of different electronic and sometimes non-electronic music. I strive to find the best and most enjoyable music for you ...",
            },
            new YoutubeUploader
            {
                Name = "Mr Suicide Sheep",
                Thumbnail = new Uri("https://yt3.ggpht.com/-nkC01SMBUms/AAAAAAAAAAI/AAAAAAAAAAA/X4GElTDAELg/s240-c-k-no-mo-rj-c0xffffff/photo.jpg"),
                Description = "On this channel you will find a wide variety of different electronic and sometimes non-electronic music. I strive to find the best and most enjoyable music for you ...",
            },
        };

        public YoutubeFindUploaderDt()
        {
            SelectedUploader = Uploaders[1];
        }
    }
}
