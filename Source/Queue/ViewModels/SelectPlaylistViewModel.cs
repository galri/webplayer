using Infrastructure.Models;
using Infrastructure.Service;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;

namespace Webplayer.Modules.Structure.ViewModels
{
    class SelectPlaylistViewModel : BindableBase, ISelectPlaylistViewModel
    {
        private IQueueController _queueController;
        private IPlaylistService _playlistService;
        private Playlist _currentPlaylist;

        public ObservableCollection<Playlist> Playlists { get; set; }

        public Playlist CurrentPLaylist
        {
            get
            {
                return _currentPlaylist;
            }
            set
            {
                SetProperty(ref _currentPlaylist, value);
            }
        }

        public ICommand ChangePlaylistToSelected { get; }

        public SelectPlaylistViewModel(IPlaylistService platlistService, IQueueController queueController)
        {
            _queueController = queueController;
            _playlistService = platlistService;
            Playlists = new ObservableCollection<Playlist>(_playlistService.NameOfAllPlaylists().Select(t => new Playlist() { Name = t }));
            if(_queueController.Queue != null)
                CurrentPLaylist = Playlists.FirstOrDefault(t => t.Name == _queueController.Queue.Name);
            var select = new CompositeCommand();
            select.RegisterCommand(new DelegateCommand(ChangePlaylist,() => CurrentPLaylist != null));
            select.RegisterCommand(DialogHost.CloseDialogCommand);
            ChangePlaylistToSelected = select;
        }

        private void ChangePlaylist()
        {
            if(CurrentPLaylist != null)
            {
                var playlist = _playlistService.LoadPlaylist(CurrentPLaylist.Name);
                _queueController.ChangePlaylist(playlist);
            }
        }
    }
}
