using Prism.Mvvm;

namespace Infrastructure.Models
{
    public class QueueController : BindableBase, IQueueController
    {
        private ISongModel _currentSong;
        private bool _isPlaying;

        public ISongModel CurrentSong
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
                    IsPlayingChangedEvent?.Invoke(value);
                }
            }
        }

        

        public event CurrentSongChangedDelegate CurrentSongChangedEvent;

        public event IsPlayingChangedDelegate IsPlayingChangedEvent;

        private void CurrentSongChanged()
        {
            CurrentSongChangedEvent?.Invoke(CurrentSong);
        }
    }
}