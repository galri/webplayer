using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webplayer.Modules.Youtube.Models;
using Webplayer.Modules.Youtube.Services;
using Webplayer.Modules.Youtube.ViewModels;

namespace Webplayer.Modules.Youtube.DesignTime
{
    class YoutubeFindVmDt : IYoutubeFindViewModel
    {
        public ObservableCollection<YoutubeSong> SearchResult { get; set; } = new ObservableCollection<YoutubeSong>()
        {
            new YoutubeSong()
            {
                Artist = "artist",
                Title = "title",
                 Description = "desc",
                 Embeddable = true,
            },
            new YoutubeSong()
            {
                Artist = "artist2",
                Title = "title2",
                 Description = "desc2",
                 Embeddable = true,
            },
        };

        public string SearchQuery { get; set; } = "query";

        public bool CanFetchMore { get; set; } = true;

        public ContentType SearchType { get; set; } = ContentType.Video;

        public ICommand SearchCommand { get; set; }

        public ICommand FetchMoreResultCommand { get; set; }

        public ICommand PreviewCommand { get; set; }

        public ICommand ShowPlaylistSearchCommand { get; set; }

        public ICommand RemoveUploadFilterCommand { get; set; }

        public YoutubeUploader UploaderFilter { get; set; }

        public SongSearcOrdering OrderingFilter { get; set; }
    }
}
