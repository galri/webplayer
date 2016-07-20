using System;
using System.Collections.ObjectModel;
using Infrastructure.Models;
using Prism.Mvvm;

namespace Infrastructure.Service
{
    public class QueueController : BindableBase, IQueueController
    {
        public Playlist Queue { get; } = new Playlist();

        private BaseSong _currentSong;
        private bool _isPlaying;

        public BaseSong CurrentSong
        {
            get { return _currentSong; }
            set
            {
                if (SetProperty(ref _currentSong, value))
                {
                    CurrentSongChanged();
                }
            }
        }

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (SetProperty(ref _isPlaying, value))
                {
                    IsPlayingChangedEvent?.Invoke(this,new PlayingChangedEventArgs(value));
                }
            }
        }
        
        public event EventHandler<SongChangedEventArgs> CurrentSongChangedEvent;

        public event EventHandler<PlayingChangedEventArgs> IsPlayingChangedEvent;

        public event EventHandler<PlaylistChangedEventArgs> PlaylistChangedEvent;

        public void NextSong()
        {
            throw new System.NotImplementedException();
        }

        public void PreviousSong()
        {
            throw new System.NotImplementedException();
        }

        private void CurrentSongChanged()
        {
            CurrentSongChangedEvent?.Invoke(this, new SongChangedEventArgs(CurrentSong));
        }

        public void AddSongToQueue(BaseSong song)
        {
            Queue.Songs.Add(song);
        }

        public void ChangePlaylist(Playlist playlist)
        {
            Queue.Name = playlist.Name;
            Queue.Songs = playlist.Songs;
            PlaylistChangedEvent?.Invoke(this,new PlaylistChangedEventArgs());
        }
    }
}