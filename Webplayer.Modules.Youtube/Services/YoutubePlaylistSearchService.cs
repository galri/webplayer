using Google.Apis.YouTube.v3;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Youtube.Models;

namespace Webplayer.Modules.Youtube.Services
{
    class YoutubePlaylistSearchService : IYoutubePlaylistSearchService
    {
        private string _query;
        private object _sr;
        private YouTubeService _youtubeService;
        private ILoggerFacade _logger;

        public string Query
        {
            get
            {
                return _query;
            }
            set
            {
                _query = value;

                _sr = createSearchResource();
            }
        }

        public YoutubePlaylistSearchService(ILoggerFacade logger, YouTubeService youtubeService)
        {
            _logger = logger;
            _youtubeService = youtubeService;
        }


        private SearchResource.ListRequest createSearchResource()
        {
            SearchResource.ListRequest sr = _youtubeService.Search.List("snippet");
            sr.Q = Query;
            sr.Type = "Playlist";
            sr.VideoEmbeddable = SearchResource.ListRequest.VideoEmbeddableEnum.True__;

            return sr;
        }

        public IEnumerable<YoutubePlaylist> FetchNextSearchResult()
        {
            throw new NotImplementedException();
        }

        public YoutubePlaylist GetPlaylist(string id)
        {
            throw new NotImplementedException();
            var res = new YoutubePlaylist();
            var searher = _youtubeService.PlaylistItems.List("snippet");
            searher.PlaylistId = id;
            var response = searher.Execute();

            foreach (var item in response.Items)
            {
               
            }
            
        }
    }
}
