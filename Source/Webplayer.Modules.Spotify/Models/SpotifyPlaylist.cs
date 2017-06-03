using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webplayer.Modules.Spotify.Models
{
    public class SpotifyPlaylist : Playlist
    {
        public string OwnerId { get; internal set; }
    }
}
