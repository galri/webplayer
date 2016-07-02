using System;
using System.Windows.Data;
using System.Windows.Input;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;

namespace Webplayer.Modules.Structure.ViewModels
{
    public class StatusViewModel : BindableBase, IStatusViewModel
    {
        private readonly IQueueController _queueController;
        private bool _isPlaying;

        public bool IsPlaying
        {
            get { return _isPlaying; }
            set
            {
                if (SetProperty(ref _isPlaying, value))
                {
                    _queueController.IsPlaying = value;
                }
            }
        }

        public ICommand PreviousCommand { get; set; }
        public ICommand PlayPauseCommand { get; set; }
        public ICommand NextCommand { get; set; }

        public StatusViewModel(IQueueController queueController)
        {
            _queueController = queueController;
            _queueController.IsPlayingChangedEvent += _queueController_IsPlayingChangedEvent;
            PlayPauseCommand = new DelegateCommand(PlayPauseAction);
            NextCommand = new DelegateCommand(NextAction);
            PreviousCommand = new DelegateCommand(PreviousAction);
        }

        private void PreviousAction()
        {
            
        }

        private void NextAction()
        {
            
        }

        private void PlayPauseAction()
        {
            IsPlaying = !IsPlaying;
        }

        private void _queueController_IsPlayingChangedEvent(bool playingStatus)
        {
            IsPlaying = playingStatus;
        }
    }
}