using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Infrastructure.Models;
using Prism.Interactivity.InteractionRequest;

namespace Webplayer.Modules.Structure.ViewModels
{
    interface IQueueViewModel
    {
        ObservableCollection<BaseSong> Queue { get;  }

        BaseSong SelectSong { get; set; }

        BaseSong SongPlaying { get; set; }

        ICommand SaveQueueCommand { get; }

        ICommand LoadQueueCommand { get; set; }

        ICommand DeleteSongFromQueueCommand { get; set; }
    }
}
