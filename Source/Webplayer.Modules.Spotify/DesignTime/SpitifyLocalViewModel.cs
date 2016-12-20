using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Spotify.Models;
using Webplayer.Modules.Spotify.ViewModels;

namespace Webplayer.Modules.Spotify.DesignTime
{
    class SpitifyLocalViewModel : ISpotifyLocalPlayerViewModel
    {
        public SpotifySong CurrentSong
        {
            get;

            set;
        } = new SpotifySong("Designtime title", new Uri("Resources/testart.jpg"), new TimeSpan(0, 3, 26), null);

        public bool IsPlaying
        {
            get;

            set;
        } = true;
    }
}
