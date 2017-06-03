using Infrastructure.Service;
using Prism.Commands;
using Prism.Logging;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webplayer.Modules.Spotify.Models;
using Webplayer.Modules.Spotify.Services;
using Webplayer.Modules.Spotify.Views;

namespace Webplayer.Modules.Spotify.ViewModels
{
    class SpotifyAcountViewModel : BindableBase, ISpotifyAcountViewModel
    {
        private const string Tag = "SpotifyAcountViewModel";
        private readonly IRegionManager _regionManager;
        private readonly ILoggerFacade _logger;
        private bool _apiConnected;
        private readonly ISpotifyUserService _userService;
        private readonly IDialogService _dialog;
        private SpotifyPlaylist _selectedPlaylist;
        private IThreadHelper _threadHelper;
        private SpotifySong _selectedPlaylistSong;
        private IQueueController _queueController;

        public ICommand ShowSearchCommand { get; set; }

        public ICommand TryConnectApiCommand { get; set; }

        public ICommand AddSongCommand { get; set; }

        public ObservableCollection<SpotifyPlaylist> Playlists { get; set; } = new ObservableCollection<SpotifyPlaylist>();

        public SpotifyPlaylist SelectedPlaylist
        {
            get
            {
                return _selectedPlaylist;
            }
            set
            {
                if(SetProperty(ref _selectedPlaylist, value))
                {
                    PlayListSongs.Clear();
                    Task.Run(FetchFetchSongs);
                }
            }
        }

        public ObservableCollection<SpotifySong> PlayListSongs { get; set; } = new ObservableCollection<SpotifySong>();

        public SpotifySong SelectedPlaylistSong
        {
            get
            {
                return _selectedPlaylistSong;
            }
            set
            {
                SetProperty(ref _selectedPlaylistSong, value);
            }
        }

        private async Task FetchFetchSongs()
        {
            try
            {
                _logger.Log($"{Tag} FetchFetchSongs start",Category.Info,Priority.Low);
                var songs = await _userService.GetSongsForPlaylists(SelectedPlaylist);
                _threadHelper.RunOnUIThread(() =>  PlayListSongs.AddRange(songs));
                _logger.Log($"{Tag} FetchFetchSongs success", Category.Info, Priority.Low);
            }
            catch (Exception ex)
            {
                _dialog.ShowError("Fetch songs", ex.Message);
                _logger.Log($"{Tag} FetchFetchSongs error {ex.Message}", Category.Exception, Priority.High);
            }
        }

        public bool ApiConnected
        {
            get { return _apiConnected; }
            set
            {
                if (SetProperty(ref _apiConnected, value) && value)
                {
                    Playlists.Clear();
                    Task.Run(async () =>
                   {
                       var result = await _userService.GetPlaylists();
                       _threadHelper.RunOnUIThread(() => Playlists.AddRange(result));
                   });
                }
            }
        }

        public SpotifyAcountViewModel(IRegionManager rm, ILoggerFacade logger, 
            ISpotifyUserService userService, IDialogService dialog, IThreadHelper threadHelper, IQueueController queueController)
        {
            _queueController = queueController;
            _regionManager = rm;
            _logger = logger;
            _userService = userService;
            _dialog = dialog;
            _threadHelper = threadHelper;
            ShowSearchCommand = new DelegateCommand(ExecuteSearch);
            TryConnectApiCommand = new DelegateCommand(ExecuteConnectApi);
            AddSongCommand = new DelegateCommand(ExecuteAddSong);
        }

        private void ExecuteAddSong()
        {
            if (SelectedPlaylistSong == null)
            {
                _dialog.ShowError("Add song", "Please select a song");
                return;
            }

            try
            {
                _logger.Log($"{Tag} Trying to connect api", Category.Info, Priority.Medium);
                _queueController.AddSongToQueue(SelectedPlaylistSong);
            }
            catch (Exception ex)
            {
                _logger.Log($"{Tag} Error ExecuteAddSong : {ex}", Category.Exception, Priority.High);
                _dialog.ShowError("Spotify connect error", ex.Message);
            }
        }

        private async void ExecuteConnectApi()
        {
            try
            {
                _logger.Log($"{Tag} Trying to connect api",Category.Info,Priority.Medium);
                await _userService.TryAuthenticate();
                ApiConnected = _userService.Authenticated;
            }
            catch (Exception e)
            {
                _logger.Log($"{Tag} Error connecting : {e}", Category.Exception, Priority.High);
                _dialog.ShowError("Spotify connect error", e.Message);
            }
        }

        private void ExecuteSearch()
        {
            var acount = _regionManager.Regions["SpotTrans"].Views.OfType<ISpoifyFindView>();
            _regionManager.Regions["SpotTrans"].Activate(acount.First());
        }
    }
}
