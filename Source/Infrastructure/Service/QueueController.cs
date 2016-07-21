using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

        public QueueController(IPlaylistService service) : this()
        {
            var defaultPlaylist = service.LoadPlaylist("first");
            ChangePlaylist(defaultPlaylist);
        }

        public QueueController()
        {
        }

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
            song.PlaylistId = Queue.Id;
            //song.PlaylistNr = Queue.Songs.IndexOf(song);
            PlaylistChangedEvent?.Invoke(this, new PlaylistChangedEventArgs());
        }

        public void ChangePlaylist(Playlist playlist)
        {
            Queue.Name = playlist.Name;
            Queue.Songs.Clear();
            Queue.Songs.AddRange(playlist.Songs);
            Queue.Id = playlist.Id;
            PlaylistChangedEvent?.Invoke(this,new PlaylistChangedEventArgs());
        }

        public void RemoveSongToQueue(BaseSong song)
        {
            Queue.Songs.Remove(song);
            PlaylistChangedEvent?.Invoke(this, new PlaylistChangedEventArgs());
        }

        public void SetQueueSongs(IList<BaseSong> queue)
        {
            Queue.Songs.Clear();
            Queue.Songs.AddRange(queue);
            PlaylistChangedEvent?.Invoke(this, new PlaylistChangedEventArgs());
        }
    }
}