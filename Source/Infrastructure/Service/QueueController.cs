using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Infrastructure.Models;
using Prism.Mvvm;
using System.Linq;

namespace Infrastructure.Service
{
    public class QueueController : BindableBase, IQueueController
    {
        public Playlist Queue
        {
            get
            {
                return _queue;
            }
            private set
            {
                if(SetProperty(ref _queue, value))
                    PlaylistChangedEvent?.Invoke(this, new PlaylistChangedEventArgs());
            }
        }

        private BaseSong _currentSong;
        private bool _isPlaying;
        private Playlist _queue;

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
                if (CurrentSong == null && Queue.Songs.Count == 0)
                    value = false;
                else if(CurrentSong == null && value)
                {
                    CurrentSong = Queue.Songs.First();
                }

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
            var defaultPlaylist = new Playlist() { Name = "Default" };
            ChangePlaylist(defaultPlaylist);
        }

        public QueueController()
        {
        }

        public void NextSong()
        {
            var index = Queue.Songs.IndexOf(CurrentSong) + 1;
            if (index < Queue.Songs.Count && index > 0)
            {
                CurrentSong = Queue.Songs[index];
            }
        }

        public void PreviousSong()
        {
            var index = Queue.Songs.IndexOf(CurrentSong) - 1;
            if (index > -1)
            {
                CurrentSong = Queue.Songs[index];
            }
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
            if (CurrentSong == null)
                CurrentSong = song;
        }

        public void ChangePlaylist(Playlist playlist)
        {
            //Queue.Name = playlist.Name;
            //Queue.Songs.Clear();
            //Queue.Songs.AddRange(playlist.Songs);
            //Queue.Id = playlist.Id;
            Queue = playlist;
            //PlaylistChangedEvent?.Invoke(this,new PlaylistChangedEventArgs());
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