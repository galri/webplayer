using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using Webplayer.Modules.Spotify.Models;

namespace Webplayer.Modules.Spotify.Services
{
    public class NewSpotifySongService : ISpotifySongSearch
    {
        private readonly SpotifyWebAPI _api;
        private string _query;
        private SearchItem _searchItem;
        private bool _apiNeedsReseting;
        private int _nextOffset;
        private Paging<FullTrack> _page;

        public string Query
        {
            get { return _query; }
            set
            {
                _query = value;
                _apiNeedsReseting = true;
            }
        }

        public NewSpotifySongService(SpotifyWebAPI api)
        {
            _api = api;
        }

        public async Task<List<SpotifySong>> FetchAsync()
        {
            if (_apiNeedsReseting)
            {
                _nextOffset = 0;
                _searchItem = await _api.SearchItemsAsync(Query, SearchType.Track);
                CheckForPageError(_searchItem);
                _page = _searchItem.Tracks;
            }
            else
            {
                //int resultCount = 20;
                //_searchItem = await _api.SearchItemsAsync(Query, SearchType.Artist, resultCount, _nextOffset);
                //_nextOffset += resultCount;
                 _page = await _api.GetNextPageAsync<FullTrack>(_searchItem.Tracks);
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

        private SpotifySong ToSong(FullTrack track)
        {
            return new SpotifySong(track.Name,null,
                TimeSpan.FromMilliseconds(track.DurationMs),
                new Uri(track.Uri,UriKind.Absolute));
        }

        public Task<SpotifySong> FetchSongAsync(string id)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
