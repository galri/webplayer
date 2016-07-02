namespace Infrastructure
{
    /// <summary>
    /// Handles song playing.
    /// </summary>
    public interface IQueueController
    {
        ISongModel CurrentSong { get; set; }

        bool IsPlaying { get; set; }

        event CurrentSongChangedDelegate CurrentSongChangedEvent;

        event IsPlayingChangedDelegate IsPlayingChangedEvent;
    }

     public delegate void CurrentSongChangedDelegate(ISongModel newSongModel);
     public delegate void IsPlayingChangedDelegate(bool playingStatus);
}