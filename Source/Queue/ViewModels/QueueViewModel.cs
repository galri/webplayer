using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using MaterialDesignThemes.Wpf;
using Webplayer.Modules.Structure.Views;

namespace Webplayer.Modules.Structure.ViewModels
{
    class QueueViewModel : BindableBase, IQueueViewModel
    {
        private readonly IQueueController _queueController;
        private readonly IPlaylistService _playlistService;
        private BaseSong _selectSong;

        public ObservableCollection<BaseSong> Queue { get; private set; } = new ObservableCollection<BaseSong>();

        public BaseSong SelectSong
        {
            get { return _selectSong; }
            set { SetProperty(ref _selectSong, value); }
        }

        public BaseSong SongPlaying { get; set; }

        public ICommand SaveQueueCommand { get; }

        public ICommand LoadQueueCommand { get; set; }

        public ICommand DeleteSongFromQueueCommand { get; set; }

        public ICommand PlaySongCommand { get; set; }

        public string QueueName
        {
            get
            {
                return _queueName;
            }

            set
            {
                SetProperty(ref _queueName, value);
            }
        }

        public QueueViewModel(IQueueController queueController, 
            IPlaylistService playlistService, IUnityContainer container)
        {
            _queueController = queueController;
            _playlistService = playlistService;
            _container = container;
            SaveQueueCommand = new DelegateCommand(SaveAction);
            LoadQueueCommand = new DelegateCommand(LoadAction);
            DeleteSongFromQueueCommand = new DelegateCommand<BaseSong>(DeleteQueueSong);
            PlaySongCommand = new DelegateCommand(PlaySong);

            _queueController.PlaylistChangedEvent += QueueControllerOnPlaylistChangedEvent;
            SetQueue();
            Queue.CollectionChanged += new NotifyCollectionChangedEventHandler(QueueOnCollectionChanged);
        }

        private void PlaySong()
        {
            if(SelectSong != null)
                _queueController.CurrentSong = SelectSong;
        }

        ///
        bool _ignoreQueueUpdates = false;
        private void QueueOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (!_ignoreQueueUpdates)
            {
                _ignoreQueueControllerUpdates = true;
                _queueController.SetQueueSongs(Queue);
                _ignoreQueueControllerUpdates = false;
            }

        }

        bool _ignoreQueueControllerUpdates = false;
        private string _queueName;
        private IUnityContainer _container;

        private void QueueControllerOnPlaylistChangedEvent(object sender, PlaylistChangedEventArgs playlistChangedEventArgs)
        {
            if(!_ignoreQueueControllerUpdates)
                SetQueue();
        }

        private void SetQueue()
        {
            _ignoreQueueUpdates = true;
            Queue.Clear();
            Queue.AddRange(_queueController.Queue.Songs);
            QueueName = _queueController.Queue.Name;
            _ignoreQueueUpdates = false;
        }

        private void DeleteQueueSong(BaseSong song)
        {
            if (song != null)
            {
                _queueController.RemoveSongToQueue(song);
            }
        }

        private async void LoadAction()
        {
            await DialogHost.Show(_container.Resolve<ISelectPlaylistView>(),"RootDialog");
        }

        private async void SaveAction()
        {
            var vm = new NameDialogViewModel()
            {
                Name = _queueController.Queue.Name,
            };
            var view = new NameDialogView()
            {
                DataContext = vm,
            };
            var res = await DialogHost.Show(view,"RootDialog");
            _queueController.Queue.Name = vm.Name;
            _playlistService.SavePlaylist(_queueController.Queue);
        }
    }
}
