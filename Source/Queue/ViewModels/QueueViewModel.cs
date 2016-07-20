using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Playlist QueuePlaylist { get; set; }

        private readonly IPlaylistService _playlistService;
        private ObservableCollection<BaseSong> _queue;

        public ObservableCollection<BaseSong> Queue
        {
            get { return _queue; }
            set { SetProperty(ref _queue, value); }
        }

        public BaseSong SongPlaying { get; set; }

        public ICommand SaveQueueCommand { get; }

        public ICommand LoadQueueCommand { get; set; }

        public QueueViewModel(Playlist queuePlaylist, IPlaylistService playlistService
            )
        {
            QueuePlaylist = queuePlaylist;
            Queue = QueuePlaylist.Songs;
            _playlistService = playlistService;
            SaveQueueCommand = new DelegateCommand(SaveAction);
            LoadQueueCommand = new DelegateCommand(LoadAction);
            
        }

        private void LoadAction()
        {
            var playlist = _playlistService.LoadPlaylist("first");
            QueuePlaylist.Songs = playlist.Songs;
            QueuePlaylist.Name = playlist.Name;
            Queue = QueuePlaylist.Songs;
        }

        private void SaveAction()
        {
            _playlistService.SavePlaylist(QueuePlaylist);
        }
    }
}
