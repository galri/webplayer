using Google.Apis.YouTube.v3;
using Infrastructure;
using Prism.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Youtube.Models;
using Google.Apis.YouTube.v3.Data;

namespace Webplayer.Modules.Youtube.Services
{
    /// <summary>
    /// searchService for 
    /// </summary>
    interface IYoutubeChannelService : IDisposable
    {
        string Query { get; set; }

        Task<List<YoutubeUploader>> FetchNextAsync();
    }

    class YoutubeChannelService : IYoutubeChannelService
    {
        private SearchResource.ListRequest _sr;
        private string _query;
        private string nextToken;
        private string previousToken;
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

                _sr = CreateSearchResource();
            }
        }

        public YoutubeChannelService(ILoggerFacade logger, YouTubeService youtubeService)
        {
            _youtubeService = youtubeService;
            _logger = logger;
        }

        private SearchResource.ListRequest CreateSearchResource()
        {
            SearchResource.ListRequest sr = _youtubeService.Search.List("snippet");
            sr.Q = Query;
            sr.Type = "channel";

            return sr;
        }

        public async Task<List<YoutubeUploader>> FetchNextAsync()
        {
            List<YoutubeUploader> res;
            if (nextToken != null)
            {
                //not the first time, use the token to get the rest of the search
                _sr.PageToken = nextToken;

                //finds informasjons about tracks
                res = MapResult(await _sr.ExecuteAsync());
            }
            else
            {
                //First time
                res = MapResult(await _sr.ExecuteAsync());
            }

            return res;
        }

        public List<YoutubeUploader> MapResult(SearchListResponse slr)
        {
            List<YoutubeUploader> list = new List<YoutubeUploader>();
            foreach (SearchResult result in slr.Items)
            {
                list.Add(MapChannel(result));
            }

            //save tokens
            nextToken = slr.NextPageToken;
            if (slr.PrevPageToken != null)
            {
                previousToken = slr.PrevPageToken;
            }

            return list;
        }

        private YoutubeUploader MapChannel(SearchResult result)
        {
            var thumbnail = new Uri(result.Snippet.Thumbnails.Default__.Url);
            return new YoutubeUploader
            {
                Id = result.Snippet.ChannelId,
                Name = result.Snippet.ChannelTitle,
                Description = result.Snippet.Description,
                Thumbnail = thumbnail,
            };
        }

        public void Dispose()
        {
            
        }
    }
}
