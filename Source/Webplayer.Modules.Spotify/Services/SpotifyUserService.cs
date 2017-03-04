using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpotifyAPI.Web.Auth;
using SpotifyAPI.Web.Enums;
using SpotifyAPI.Web.Models;


namespace Webplayer.Modules.Spotify.Services
{
    interface ISpotifyUserService
    {
        /// <summary>
        /// User allowed app to fetch info.
        /// </summary>
        bool Authenticated { get; set; }

        Task TryAuthenticate();
    }

    class SpotifyUserService : ISpotifyUserService
    {

        public SpotifyUserService()
        {

        }

        public bool Authenticated { get; set; }

        public async Task TryAuthenticate()
        {
            try
            {
                var factory = new WebAPIFactory(
                    "http://localhost",
                    8000,
                    "XXXXXXXXXXXXXXXX",
                    Scope.PlaylistModifyPublic | Scope.PlaylistReadPrivate,
                    TimeSpan.FromSeconds(20));
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
