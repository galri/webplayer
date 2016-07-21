using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Infrastructure.Models;

namespace Infrastructure.Service
{
    /// <summary>
    /// Handles song playing. should uses instanse of <seealso cref="IPlaylist"/> to get
    /// songs from.
    /// </summary>
    public interface IQueueController
    {
        Playlist Queue { get; }

        BaseSong CurrentSong { get; set; }

        bool IsPlaying { get; set; }

        event EventHandler<SongChangedEventArgs> CurrentSongChangedEvent;

        event EventHandler<PlayingChangedEventArgs> IsPlayingChangedEvent;

        event EventHandler<PlaylistChangedEventArgs> PlaylistChangedEvent;

        void NextSong();

        void PreviousSong();

        void AddSongToQueue(BaseSong song);

        void ChangePlaylist(Playlist playlist);
        void RemoveSongToQueue(BaseSong song);
        void SetQueueSongs(IList<BaseSong> queue);
    }

     //public delegate void CurrentSongChangedDelegate(ISongModel newSongModel);
     //public delegate void IsPlayingChangedDelegate(bool playingStatus);
    public class PlayingChangedEventArgs : EventArgs
    {
        public bool IsPlaying { get; set; }

        public PlayingChangedEventArgs(bool isPlaying)
        {
            IsPlaying = isPlaying;
        }
    }

    public class SongChangedEventArgs : EventArgs
    {
        public BaseSong CurrentSong { get; set; }

        public SongChangedEventArgs(BaseSong currentSong)
        {
            CurrentSong = currentSong;
        }
    }

    public class PlaylistChangedEventArgs : EventArgs
    {
        
    }
}