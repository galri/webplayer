using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Webplayer.Modules.Spotify.Models;

namespace Webplayer.Modules.Spotify.ViewModels
{
    public interface ISpotifyFindViewModel
    {
        string SearchQuery { get; set; }

        ObservableCollection<SpotifySong> SearchResult { get; set; }

        ICommand SearchCommand { get; set; }

        ICommand FetchMoreResultCommand { get; set; }
    }
}