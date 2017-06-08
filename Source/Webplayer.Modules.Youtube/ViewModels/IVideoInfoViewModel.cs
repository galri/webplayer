using System.ComponentModel;
using System.Windows.Input;
using Webplayer.Modules.Youtube.Models;
using YoutubePlayerLib;

namespace Webplayer.Modules.Youtube.ViewModels
{
    public interface IVideoInfoViewModel : INotifyPropertyChanged
    {
        YoutubeSong Video { get; set; }

        bool Playing { get; set; }

        ICommand NextCommand { get; set; }
    }
}