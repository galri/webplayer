using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;
using Webplayer.Modules.Spotify.Models;

namespace Webplayer.Modules.Spotify.Services
{
    interface ISpotifyUserService
    {
        /// <summary>
        /// User allowed app to fetch info.
        /// </summary>
        bool Authenticated { get; set; }

        Task TryAuthenticate();

        Task<IEnumerable<SpotifyPlaylist>> GetPlaylists();

        Task<IEnumerable<SpotifySong>> GetSongsForPlaylists(SpotifyPlaylist playlist);
    }

    public class SpotifyUserService : ISpotifyUserService
    {
        public static SpotifyWebAPIProvider _api;
        private PrivateProfile _userProfile;

        public SpotifyUserService(SpotifyWebAPIProvider api)
        {
            _api = api;
        }

        public bool Authenticated { get; set; }

        public async Task<IEnumerable<SpotifyPlaylist>> GetPlaylists()
        {
            if (!Authenticated)
                throw new InvalidOperationException("SpotifyUserService needs to authenticated before calling user methods");
        

            var getResult = await _api.Api.GetUserPlaylistsAsync(_userProfile.Id,100);
            if (getResult.HasError())
            {
                throw new Exception($"{getResult.Error.Status}: {getResult.Error.Message}");
            }
            
            return Map(getResult.Items);
        }

        private IEnumerable<SpotifyPlaylist> Map(List<SimplePlaylist> items)
        {
            var result = new List<SpotifyPlaylist>();
            foreach (var item in items)
            {
                var itemResult = new SpotifyPlaylist();
                itemResult.Id = item.Id;
                itemResult.Name = item.Name;
                itemResult.OwnerId = item.Owner.Id;
                result.Add(itemResult);
            }
            return result;
        }

        public async Task TryAuthenticate()
        {
            try
            {
                var factory = new WebAPIFactory(
                    "http://localhost",
                    8000,
                    "e2883255a1be4dd29de744e8a7c124eb",
                    Scope.PlaylistModifyPublic | Scope.PlaylistReadPrivate,
                    TimeSpan.FromSeconds(30));
                _api.Api = await factory.GetWebApi();
                _userProfile = await _api.Api.GetPrivateProfileAsync();
                Authenticated = true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<SpotifySong>> GetSongsForPlaylists(SpotifyPlaylist playlist)
        {
            var getResult = await _api.Api.GetPlaylistTracksAsync(playlist.OwnerId, playlist.Id, limit: 200);
            if (getResult.HasError())
            {
                throw new Exception($"{getResult.Error.Status}: {getResult.Error.Message}");
            }

            var result = new List<SpotifySong>();
            foreach (var item in getResult.Items)
            {
                var song = new SpotifySong(item.Track.Name,
                    new Uri(item.Track.Album.Images.First().Url,UriKind.Absolute),
                    new TimeSpan(item.Track.DurationMs),
                    new Uri(item.Track.Uri,UriKind.Absolute));
                result.Add(song);
            }

            return result;
        }
    }
}
