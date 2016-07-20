using System;
using System.Windows.Data;
using System.Windows.Input;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Service;
using Prism.Commands;
using Prism.Mvvm;

namespace Webplayer.Modules.Structure.ViewModels
{
    public class StatusViewModel : BindableBase, IStatusViewModel
    {
        private bool _isPlaying;

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (SetProperty(ref _isPlaying, value))
                {
                    QueueController.IsPlaying = value;
                }
            }
        }

        public ICommand PreviousCommand { get; set; }
        public ICommand PlayPauseCommand { get; set; }
        public ICommand NextCommand { get; set; }
        public BaseSong CurrenSong { get; private set; }

        public IQueueController QueueController { get; }

        public Playlist Queue => QueueController.Queue;

        public StatusViewModel(IQueueController queueController)
        {
            QueueController = queueController;
            QueueController.IsPlayingChangedEvent += _queueController_IsPlayingChangedEvent;
            QueueController.CurrentSongChangedEvent += QueueControllerOnCurrentSongChangedEvent;
            PlayPauseCommand = new DelegateCommand(PlayPauseAction);
            NextCommand = new DelegateCommand(NextAction);
            PreviousCommand = new DelegateCommand(PreviousAction);
        }

        private void QueueControllerOnCurrentSongChangedEvent(object sender, SongChangedEventArgs songChangedEventArgs)
        {
            CurrenSong = songChangedEventArgs.CurrentSong;
        }

        private void _queueController_IsPlayingChangedEvent(object sender, PlayingChangedEventArgs e)
        {
            IsPlaying = e.IsPlaying;
        }

        private void PreviousAction()
        {
            QueueController.PreviousSong();
        }

        private void NextAction()
        {
            QueueController.NextSong();
        }

        private void PlayPauseAction()
        {
            QueueController.IsPlaying = !QueueController.IsPlaying;
        }
    }
}