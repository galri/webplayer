using Infrastructure.Service;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Spotify.Models;
using SpotifyAPI.Local;
using SpotifyAPI.Local.Enums;
using SpotifyAPI.Local.Models;
using Prism.Regions;
using Infrastructure;
using Webplayer.Modules.Spotify.Views;
using Prism.Logging;
using Webplayer.Modules.Spotify.Services;

namespace Webplayer.Modules.Spotify.ViewModels
{
    class SpotifyLocalPlayerViewModel : BindableBase, ISpotifyLocalPlayerViewModel
    {
        public bool IsPlaying
        {
            get
            {
                return _isPlaying;
            }
            set
            {
                if (SetProperty(ref _isPlaying, value))
                {
                    if(value)
                    {
                        Task.Run(async () => await _api.Play());

                        var info = _regionManager.Regions[RegionNames.InfoRegion];
                        var view = info.Views.First(t => t is ISpotifyLocalPlayerView);
                        info.Activate(view);
                    }
                    else
                    {
                        _api.Pause();
                    }
                }
            }
        }

        public SpotifySong CurrentSong
        {
            get
            {
                return _currentSong;
            }
            set
            {
                SetProperty(ref _currentSong, value);
                    
                if (IsPlaying)
                {
                    if (_localTrack == null || _localTrack.TrackResource.Uri != _currentSong.Uri.ToString())
                        _api.PlayURL(_currentSong.Uri.ToString());
                    else
                        _api.Play();
                }
                else
                {
                    _api.Pause();
                }
                
            }
        }
        
        private IQueueController _queueController;
        private bool _isPlaying;
        private SpotifySong _currentSong;
        private SpotifyLocalAPI _api;
        private Track _localTrack;
        private IRegionManager _regionManager;
        private ILoggerFacade _logger;
        private const String TAG = "SpotifyLocalPlayerViewModel";
        private ISpotifySongSearch _songService;
        private IThreadHelper _treadHelper;

        public SpotifyLocalPlayerViewModel(IQueueController queueController, IRegionManager regionManager,
            ILoggerFacade logger, ISpotifySongSearch songService, IThreadHelper helper)
        {
            _treadHelper = helper;
            _songService = songService;
            _logger = logger;
            _api = new SpotifyLocalAPI();
            _regionManager = regionManager;
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                SpotifyLocalAPI.RunSpotifyWebHelper();
            }
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                SpotifyLocalAPI.RunSpotify();
            }
            _api.OnTrackChange += _api_OnTrackChange;
            _api.OnTrackTimeChange += TrackTimeChanged;
            _api.OnPlayStateChange += PlayStateChanged;
            bool retryConnect;
            _api.ListenForEvents = true;
            do
            {
                //TODO: urgent need dialog!
                var connected = _api.Connect();
                retryConnect = !connected;

            } while (retryConnect);
            _queueController = queueController;
            _queueController.CurrentSongChangedEvent += QueueControllerOnCurrentSongChangedEvent;
            _queueController.IsPlayingChangedEvent += QueueControllerOnIsPlayingChangedEvent;
            //Task.Run(async () => await FetchStartUpSong());
        }

        /// <summary>
        /// Fetch current song from spoitfy, add it to queue.
        /// </summary>
        /// <remarks>
        /// Needs more work, also plays the song.
        /// </remarks>
        /// <returns></returns>
        private async Task FetchStartUpSong()
        {
            try
            {
                _logger.Log($"{TAG} trying to get current track from spotify local.", Category.Debug, Priority.Low);
                var status = _api.GetStatus();
                var currentTack = status.Track;
                var uri = currentTack.TrackResource.Uri;
                _logger.Log($"{TAG} got current track {uri}", Category.Debug, Priority.Low);
                var song = await _songService.FetchSongAsync(uri);
                _treadHelper.RunOnUIThread(() =>
                {
                    _queueController.CurrentSongChangedEvent -= QueueControllerOnCurrentSongChangedEvent;
                    _queueController.AddSongToQueue(song);
                    _queueController.CurrentSongChangedEvent += QueueControllerOnCurrentSongChangedEvent;
                });
            }
            catch (Exception ex)
            {
                _logger.Log($"{TAG} Error trying to get current track from spotify local {ex}.", Category.Exception, Priority.Medium);
            }
        }

        private void PlayStateChanged(object sender, PlayStateEventArgs e)
        {
            if (!e.Playing)
            {
                var state = _api.GetStatus();
                var length = state.Track.Length;
                var currentLength = state.PlayingPosition;
                if(0 == currentLength)
                {
                    _queueController.NextSong();
                }
            }
        }

        private void TrackTimeChanged(object sender, TrackTimeChangeEventArgs e)
        {
            
        }

        private void _api_OnTrackChange(object sender, TrackChangeEventArgs e)
        {
            _localTrack = e.NewTrack;
        }

        private void QueueControllerOnIsPlayingChangedEvent(object sender, PlayingChangedEventArgs e)
        {
            if(_queueController.CurrentSong is SpotifySong)
                IsPlaying = e.IsPlaying;
            
        }

        private void QueueControllerOnCurrentSongChangedEvent(object sender, SongChangedEventArgs e)
        {
            var song = e.CurrentSong as SpotifySong;
            if(song != null)
            {
                IsPlaying = true;
                CurrentSong = song;
            }
            else
            {
                IsPlaying = false;
            }
        }
    }
}
