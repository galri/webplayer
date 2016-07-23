using YoutubePlayerLib;

namespace Webplayer.Modules.Youtube.ViewModels
{
    public interface IVideoInfoViewModel
    {
        string VideoId { get; set; } 

        YoutubePlayerState Playing { get; set; }
    }
}