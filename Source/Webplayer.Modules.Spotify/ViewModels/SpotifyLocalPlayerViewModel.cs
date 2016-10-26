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
                        _api.Play();
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
                    if (_localTrack == null || _localTrack.TrackResource.Uri == _currentSong.Uri.ToString())
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

        public SpotifyLocalPlayerViewModel(IQueueController queueController)
        {
            _api = new SpotifyLocalAPI();
            if (!SpotifyLocalAPI.IsSpotifyWebHelperRunning())
            {
                SpotifyLocalAPI.RunSpotifyWebHelper();
            }
            if (!SpotifyLocalAPI.IsSpotifyRunning())
            {
                SpotifyLocalAPI.RunSpotify();
            }
            _api.OnTrackChange += _api_OnTrackChange;
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
