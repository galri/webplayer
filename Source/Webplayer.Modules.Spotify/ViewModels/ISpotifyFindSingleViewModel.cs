using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Spotify.Models;

namespace Webplayer.Modules.Spotify.ViewModels
{
    interface ISpotifyFindSingleViewModel
    {
        SpotifySong Result { get; }

        string SearchString { get; }
    }
}
