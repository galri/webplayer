using System.Windows.Input;
using Infrastructure.Models;

namespace Webplayer.Modules.Structure.ViewModels
{
    public interface IStatusViewModel
    {
        bool IsPlaying { get; }
        ICommand PreviousCommand { get; set; }
        ICommand PlayPauseCommand { get; set; }

        /// <summary>
        /// Find Next Song in queue, set is as current
        /// </summary>
        ICommand NextCommand { get; set; }

        BaseSong CurrenSong { get; }
    }
}