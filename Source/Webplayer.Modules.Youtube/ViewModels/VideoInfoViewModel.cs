using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Service;
using Prism.Mvvm;
using Webplayer.Modules.Youtube.Models;
using YoutubePlayerLib;
using Infrastructure;
using Prism.Regions;
using Webplayer.Modules.Youtube.Views;
using System.Windows.Media.Imaging;

namespace Webplayer.Modules.Youtube.ViewModels
{
    class VideoInfoViewModel : BindableBase, IVideoInfoViewModel
    {
        private readonly IQueueController _queueController;
        private string _videoId;
        private YoutubePlayerState _playing;
        private IRegionManager _regionManager;
        private BitmapImage _videoThumbnail;

        public BitmapImage VideoThumbnail
        {
            get
            {
                return _videoThumbnail;
            }
            set
            {
                SetProperty<BitmapImage>(ref _videoThumbnail, value);
            }
        }

        public string VideoId
        {
            get { return _videoId; }
            set
            {
                if (SetProperty(ref _videoId,value))
                {
                }
            }
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

                    if (_playing == YoutubePlayerState.playing)
                    {
                        var info = _regionManager.Regions[RegionNames.InfoRegion];
                        var view = info.Views.First(t => t is IVIdeoInfoView);
                        info.Activate(view);
                    }
                }
            }
        }

        public VideoInfoViewModel(IQueueController queueController, IRegionManager manager)
        {
            _queueController = queueController;
            _regionManager = manager;
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
                VideoThumbnail = ySong.Picture;
            }
            else
            {
                Playing = YoutubePlayerState.paused;
            }
        }
    }
}
