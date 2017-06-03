using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using Webplayer.Modules.Spotify.Models;
using System.Windows.Media.Imaging;
using Prism.Logging;

namespace Webplayer.Modules.Spotify.Services
{
    public class NewSpotifySongService : ISpotifySongSearch
    {
        private const string TAG = "NewSpotifySongService";
        private readonly SpotifyWebAPIProvider _api;
        private string _query;
        private SearchItem _searchItem;
        private bool _apiNeedsReseting;
        private Paging<FullTrack> _page;
        private ILoggerFacade _logger;

        public string Query
        {
            get { return _query; }
            set
            {
                _query = value;
                _apiNeedsReseting = true;
            }
        }

        public NewSpotifySongService(SpotifyWebAPIProvider api,ILoggerFacade logger)
        {
            _api = api;
            _logger = logger;
        }

        public async Task<List<SpotifySong>> FetchAsync()
        {
            if (_apiNeedsReseting)
            {
                _searchItem = await _api.Api.SearchItemsAsync(Query, SearchType.Track);
                CheckForPageError(_searchItem);
                _page = _searchItem.Tracks;
            }
            else
            {
                 _page = await _api.Api.GetNextPageAsync<FullTrack>(_searchItem.Tracks);
            }

            var result = new List<SpotifySong>();
            foreach (var track in _page.Items)
            {
                result.Add(ToSong(track));
            }
            
            return result;
        }

        private void CheckForPageError(SearchItem searchItem)
        {
            if (searchItem.Error != null)
            {
                throw new Exception($"Error: {searchItem.Error.Message} " +
                                    $"with code: {searchItem.Error.Status}");
            }
        }

        //TODO: make more error proff.......
        private SpotifySong ToSong(FullTrack track)
        {
            var thumbNail = new Uri(track.Album.Images.First().Url);

            return new SpotifySong(track.Name, thumbNail,
                TimeSpan.FromMilliseconds(track.DurationMs),
                new Uri(track.Uri, UriKind.Absolute))
            {
                Album = track.Album.Name,
                Artist = track.Artists.First().Name,
            };
        }

        public async Task<SpotifySong> FetchSongAsync(string id)
        {
            var parsed = ParseId(id);
            var result = await _api.Api.GetTrackAsync(parsed);
            //Must be a better way to check this
            if(result.HasError() && result.Error.Message == "invalid id")
            {
                throw new ArgumentException(nameof(id));
            }
            else if(result.HasError())
            {
                _logger.Log($"{TAG}Unknown error {result.Error.Status} {result.Error.Message}", Prism.Logging.Category.Debug, Priority.Medium);
                throw new Exception("unknown error");
            }
            var song = ToSong(result);
            return song;
        }

        private string ParseId(string id)
        {
            try
            {
                _logger.Log($"{TAG} trying to resolve spotify id", Prism.Logging.Category.Info, Priority.Low);

                //example spotify:track:01wtyqu6JITgDWi4jKdk0R
                if (id.Count(t => t == ':') == 2)
                {
                    var items = id.Split(':');
                    var toReturn = items.Last();
                    _logger.Log($"{TAG} colon returning {toReturn} from {id}", Prism.Logging.Category.Info, Priority.Low);
                    return toReturn;
                }

                //example share uri https://open.spotify.com/track/01wtyqu6JITgDWi4jKdk0R
                Uri uriResult;
                if (Uri.TryCreate(id, UriKind.Absolute, out uriResult))
                {
                    var toReturn = uriResult.Segments.Last();
                    _logger.Log($"{TAG} uri returning {toReturn} from {id}", Prism.Logging.Category.Info, Priority.Low);
                    return toReturn;
                }

            }
            catch (Exception)
            {
                _logger.Log($"{TAG} Unable to resolve spotify id {id}", Prism.Logging.Category.Exception, Priority.High);
            }

            _logger.Log($"{TAG} Unable to resolve spotify id {id}", Prism.Logging.Category.Info, Priority.Low);
            return id;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
