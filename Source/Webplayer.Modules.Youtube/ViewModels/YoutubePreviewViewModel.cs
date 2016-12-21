using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Youtube.Models;

namespace Webplayer.Modules.Youtube.ViewModels
{
    class YoutubePreviewViewModel : BindableBase, IYoutubePreviewViewModel
    {
        private YoutubeSong _song;

        public string Description
        {
            get
            {
                return _song.Description;
            }

            set
            {
                
            }
        }

        public string VideoId
        {
            get
            {
                return _song.VideoId;
            }

            set
            {
                
            }
        }

        public YoutubePreviewViewModel(YoutubeSong song)
        {
            _song = song;
        }
    }
}
