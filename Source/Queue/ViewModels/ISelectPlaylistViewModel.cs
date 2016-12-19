using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Webplayer.Modules.Structure.ViewModels
{
    interface ISelectPlaylistViewModel
    {
        ObservableCollection<Playlist> Playlists { get; set; }

        Playlist CurrentPLaylist { get; set; }

        ICommand ChangePlaylistToSelected { get; }
    }
}
