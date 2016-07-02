using System.Windows.Input;

namespace Webplayer.Modules.Structure.ViewModels
{
    public interface IStatusViewModel
    {
        bool IsPlaying { get; }
        ICommand PreviousCommand { get; set; }
        ICommand PlayPauseCommand { get; set; }
        ICommand NextCommand { get; set; }
    }
}