using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System.Windows.Media.Imaging;

namespace Webplayer.Services.Youtube
{
    class YoutubeSongSearch
    {
        private int myQuantity;
        private string mySearch;
        private bool shallFetchThumbnail;
        private SearchResource.ListRequest.VideoEmbeddableEnum videoEmbeddable;
        private YouTubeService youtubeService;
        private string nextToken;
        private string previousToken;

        #region Properties
        public int Quantity
        {
            get
            {
                return myQuantity;
            }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Quantity must be greater then zero", "Quantity");
                }
                myQuantity = value;
            }
        }
        public string SearchWord
        {
            get
            {
                return mySearch;
            }
        }
        public bool ShallFetchThumbnail
        {
            get
            {
                return shallFetchThumbnail;
            }
            set
            {
                shallFetchThumbnail = value;
            }
        }
        /// <summary>
        /// false: fetch videos that can be played and videos that cannot be played.
        /// true: only fetch videos that can be played.
        /// wrong?
        /// </summary>
        public bool DontFetchUnEmbeddableVideos
        {
            set
            {
                videoEmbeddable = value ? SearchResource.ListRequest.VideoEmbeddableEnum.Any : SearchResource.ListRequest.VideoEmbeddableEnum.True;
            }
            get
            {
                if (videoEmbeddable.Equals(SearchResource.ListRequest.VideoEmbeddableEnum.Any))
                {
                    return true;
                }
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="quantity">number of search results to return each next</param>
        /// <param name="search">What to search after</param>
        /// <param name="fetchThumbnail">Wether to get album picture or not, not may save alot of time</param>
        public YoutubeSongSearch(int quantity, string search, bool fetchThumbnail, bool emdebble)
        {
            Quantity = quantity;
            mySearch = search;
            shallFetchThumbnail = fetchThumbnail;
            DontFetchUnEmbeddableVideos = emdebble;

            youtubeService = createYS();
        }

        /// <summary>
        /// Returns next/first list of songs.
        /// </summary>
        /// <returns>List with number of search results, 0 in size if none found</returns>
        public List<YoutubeSong> next()
        {
            //create search object
            SearchResource.ListRequest SR = createSearchResource();

            if (nextToken != null)
            {
                //not the first time, use the token to get the rest of the search
                SR.PageToken = nextToken;
            }
            
            //finds informasjons about tracks
            List<YoutubeSong> tracks = getSearchResult(SR.Execute());

            return tracks;
        }

        public List<YoutubeSong> previous()
        {
            //create search object
            SearchResource.ListRequest SR = createSearchResource();

            if (previousToken == null)
            {
                throw new InvalidOperationException("No previous token exists, this is most likely because no next() has been run on this object.");
            }
            SR.PageToken = previousToken;

            //finds informasjons about tracks
            List<YoutubeSong> tracks = getSearchResult(SR.Execute());

            return tracks;
        }
        
        private List<YoutubeSong> getSearchResult(SearchListResponse slr)
        {
            List<YoutubeSong> list = new List<YoutubeSong>();
            foreach (SearchResult result in slr.Items)
            {
                //TODO: will haft to make a new request to get length of video.
                BitmapImage thumbnail = getPicture(result.Snippet.Thumbnails);
                list.Add(new YoutubeSong(thumbnail, result.Snippet.Title, result.Id.VideoId, new TimeSpan()) 
                    { 
                        Description = result.Snippet.Description,
                        Embeddable = isEmbeddable(result.Id.VideoId)
                    }
                );

            }

            //save tokens
            nextToken = slr.NextPageToken;
            if (slr.PrevPageToken != null)
            {
                previousToken = slr.PrevPageToken;
            }

            return list;
        }

        private BitmapImage getPicture(ThumbnailDetails details)
        {
            //TODO: maybe fetch the biggest or what the user wants and not just the default.
            if (ShallFetchThumbnail)
            {
                return new BitmapImage(new Uri(details.Default.Url));
            }
            //sets a default image, since we shall not fetch it from the web.
            return new BitmapImage(new Uri("/Media/NoAlbumArt.png", UriKind.RelativeOrAbsolute));
            throw new NotImplementedException();
        }

        /// <summary>
        /// Creates a youtube searchlistrequest out of object properties.
        /// </summary>
        /// <returns></returns>
        private SearchResource.ListRequest createSearchResource()
        {
            SearchResource.ListRequest sr = youtubeService.Search.List("snippet");
            sr.Q = SearchWord;
            sr.Type = "video";
            sr.VideoEmbeddable = videoEmbeddable;
            sr.MaxResults = myQuantity;

            return sr;
        }

        public static YouTubeService createYS()
        {
            return new YouTubeService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyAtuwBQDfoweQqFuxNmXUNH-n70J1KL_54",
                ApplicationName = "Web playlist"
            });
        }

        /// <summary>
        /// See whether a video is embeddable or not
        /// NB! THis dos not always work see: http://stackoverflow.com/questions/17099980/v3-api-returns-blocked-content-even-with-videoembedable-true 
        /// for more information.
        /// </summary>
        /// <param name="VideoID"></param>
        /// <returns></returns>
        public static bool isEmbeddable(string VideoID)
        {
            VideosResource.ListRequest lr = createYS().Videos.List("status");
            lr.Id = VideoID;
            VideoListResponse searchResponse = lr.Execute();

            if (searchResponse.Items.Count == 0)
            {
                throw new ArgumentException("Didn't find a video with id", "VideoID");
            }
            return (bool)searchResponse.Items[0].Status.Embeddable;
        }
    }
}
