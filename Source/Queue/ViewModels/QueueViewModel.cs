using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interactivity;
using Infrastructure.Models;
using Infrastructure.Service;
using Prism.Commands;

namespace Webplayer.Modules.Structure.ViewModels
{
    class QueueViewModel : BindableBase, IQueueViewModel
    {
        private readonly IQueueController _queueController;
        private readonly IPlaylistService _playlistService;
        private BaseSong _selectSong;

        public ObservableCollection<BaseSong> Queue
        {
            get { return _queueController.Queue.Songs; }
        }

        public BaseSong SelectSong
        {
            get { return _selectSong; }
            set { SetProperty(ref _selectSong, value); }
        }

        public BaseSong SongPlaying { get; set; }

        public ICommand SaveQueueCommand { get; }

        public ICommand LoadQueueCommand { get; set; }

        public ICommand DeleteSongFromQueueCommand { get; set; }

        public QueueViewModel(IQueueController queueController, 
            IPlaylistService playlistService)
        {
            _queueController = queueController;
            _playlistService = playlistService;
            SaveQueueCommand = new DelegateCommand(SaveAction);
            LoadQueueCommand = new DelegateCommand(LoadAction);
            DeleteSongFromQueueCommand = new DelegateCommand(DeleteQueueSong);

            _queueController.PlaylistChangedEvent += QueueControllerOnPlaylistChangedEvent;
        }

        private void DeleteQueueSong()
        {
            if (SelectSong != null)
            {
                _queueController.RemoveSongToQueue(SelectSong);
            }
        }

        private void QueueControllerOnPlaylistChangedEvent(object sender, PlaylistChangedEventArgs playlistChangedEventArgs)
        {
            OnPropertyChanged("Queue");
        }

        private void LoadAction()
        {
            var playlist = _playlistService.LoadPlaylist("first");
            _queueController.ChangePlaylist(playlist);
        }

        private void SaveAction()
        {
            _playlistService.SavePlaylist(_queueController.Queue);
        }
    }
}
