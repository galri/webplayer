using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Service;
using Prism.Mvvm;
using Webplayer.Modules.Youtube.Models;
using YoutubePlayerLib;

namespace Webplayer.Modules.Youtube.ViewModels
{
    class VideoInfoViewModel : BindableBase, IVideoInfoViewModel
    {
        private readonly IQueueController _queueController;
        private string _videoId;
        private YoutubePlayerState _playing;

        public string VideoId
        {
            get { return _videoId; }
            set { SetProperty(ref _videoId,value); }
        }

        public YoutubePlayerState Playing
        {
            get { return _playing; }
            set
            {
                if(SetProperty(ref _playing, value))
                {
                    if(value ==YoutubePlayerState.ended)    
                        _queueController.NextSong();
                    
                }
            }
        }

        public VideoInfoViewModel(IQueueController queueController)
        {
            _queueController = queueController;
            _queueController.CurrentSongChangedEvent += QueueControllerOnCurrentSongChangedEvent;
            _queueController.IsPlayingChangedEvent += QueueControllerOnIsPlayingChangedEvent;
        }

        private void QueueControllerOnIsPlayingChangedEvent(object sender, PlayingChangedEventArgs e)
        {
            var song = _queueController.CurrentSong as YoutubeSong;
            if(song != null)
                {

                if (e.IsPlaying)
                {
                    Playing = YoutubePlayerState.playing;;
                }
                else
                {
                    Playing = YoutubePlayerState.paused;
                }
            }
            else
            {
                Playing = YoutubePlayerState.paused;
            }
        }

        private void QueueControllerOnCurrentSongChangedEvent(object sender, SongChangedEventArgs e)
        {
            var ySong = e.CurrentSong as YoutubeSong;
            if (ySong != null)
            {
                VideoId = ySong.VideoId;
                Playing = YoutubePlayerState.playing;
            }
            else
            {
                Playing = YoutubePlayerState.paused;
            }
        }
    }
}
