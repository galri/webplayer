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
using System.Windows.Input;
using Prism.Logging;
using Prism.Commands;

namespace Webplayer.Modules.Youtube.ViewModels
{
    class VideoInfoViewModel : BindableBase, IVideoInfoViewModel
    {
        private const string Tag = "VideoInfoViewModel";
        private readonly IQueueController _queueController;
        private YoutubeSong _video;
        //private YoutubePlayerState _playing;
        private IRegionManager _regionManager;
        private BitmapImage _videoThumbnail;
        private IThreadHelper _threadHelper;
        private bool _playing;
        private ILoggerFacade _logger;

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

        public YoutubeSong Video
        {
            get { return _video; }
            set
            {
                if (SetProperty(ref _video, value))
                {
                }
            }
        }

        //public YoutubePlayerState Playing
        //{
        //    get { return _playing; }
        //    set
        //    {
        //        if(SetProperty(ref _playing, value))
        //        {
        //            if(value ==YoutubePlayerState.ended)    
        //                _queueController.NextSong();

        //            if (_playing == YoutubePlayerState.playing)
        //            {
        //                var info = _regionManager.Regions[RegionNames.InfoRegion];
        //                var view = info.Views.First(t => t is IVIdeoInfoView);
        //                _threadHelper.RunOnUIThread(() =>  info.Activate(view));
        //            }
        //        }
        //    }
        //}

        public bool Playing
        {
            get
            {
                return _playing;
            }

            set
            {
                if(SetProperty(ref _playing, value))
                {
                    _logger.Log($"{Tag} Isplaying {value}",Category.Info,Priority.Medium);
                    _queueController.IsPlaying = value;

                    if (value)
                    {
                        _logger.Log($"{Tag} trying to show player", Category.Info, Priority.Low);
                        var info = _regionManager.Regions[RegionNames.InfoRegion];
                        var view = info.Views.First(t => t is IVIdeoInfoView);
                        _threadHelper.RunOnUIThread(() => info.Activate(view));
                    }
                }
            }
        }

        public ICommand NextCommand { get; set; }

        public VideoInfoViewModel(IQueueController queueController, IRegionManager manager, IThreadHelper threadHelper
            ,ILoggerFacade logger)
        {
            _logger = logger;
            _queueController = queueController;
            _regionManager = manager;
            _threadHelper = threadHelper;
            _queueController.CurrentSongChangedEvent += QueueControllerOnCurrentSongChangedEvent;
            _queueController.IsPlayingChangedEvent += QueueControllerOnIsPlayingChangedEvent;
            NextCommand = new DelegateCommand(ExecuteNext);
        }

        private void ExecuteNext()
        {
            _logger.Log($"{Tag} next song", Category.Info, Priority.Low);
            _queueController.NextSong();
        }

        private void QueueControllerOnIsPlayingChangedEvent(object sender, PlayingChangedEventArgs e)
        {
            var song = _queueController.CurrentSong as YoutubeSong;
            if(song != null)
            {

                Playing = e.IsPlaying;
            }
            else
            {
                Playing = false;
            }
        }

        private void QueueControllerOnCurrentSongChangedEvent(object sender, SongChangedEventArgs e)
        {
            var ySong = e.CurrentSong as YoutubeSong;
            if (ySong != null)
            {
                Video = ySong;
                Playing = true;
                if(ySong.Picture != null)
                    VideoThumbnail = new BitmapImage( ySong.Picture);
            }
            else
            {
                Playing = false;
            }
        }
    }
}
