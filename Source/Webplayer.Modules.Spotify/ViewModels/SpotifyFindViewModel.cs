using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Webplayer.Modules.Spotify.Models;
using Webplayer.Modules.Spotify.Services;
using Infrastructure.Service;
using Infrastructure.Models;
using System.Windows.Controls;
using System.Windows;
using Prism;
using Infrastructure;

namespace Webplayer.Modules.Spotify.ViewModels
{
    class SpotifyFindViewModel : BindableBase, ISpotifyFindViewModel, IActiveAware
    {

        private bool _isActive;
        private readonly ISpotifySongSearch _songSearchService;
        private string _query;
        private IQueueController _queueController;
        private Visibility _searchFieldVisibility = Visibility.Hidden;

        public string SearchQuery
        {
            get { return _query; }
            set
            {
                if (SetProperty(ref _query, value))
                {
                    
                }
            }
        }

        public ObservableCollection<SpotifySong> SearchResult { get; set; } = new ObservableCollection<SpotifySong>();

        public ICommand SearchCommand { get; set; }

        public ICommand FetchMoreResultCommand { get; set; }

        public ICommand FocusSearchFieldCommand { get; set; }

        public DelegateCommand<object> AddSongCommand { get; set; }

        public Visibility SearchFieldVisibility
        {
            get
            {
                return _searchFieldVisibility;
            }
            set
            {
                SetProperty(ref _searchFieldVisibility, value);
            }
        }

        #region IActiveAware
        public bool IsActive
        {
            get
            {
                return _isActive;
            }

            set
            {
                if(SetProperty(ref _isActive, value))
                    IsActiveChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler IsActiveChanged;
        #endregion

        public SpotifyFindViewModel(ISpotifySongSearch songSearchService, IQueueController queueController)
        {
            _songSearchService = songSearchService;
            _queueController= queueController;
            SearchCommand = new DelegateCommand(SearchAction);
            FetchMoreResultCommand = new DelegateCommand(MoreAction);
            AddSongCommand = new DelegateCommand<object>(AddAction);
            FocusSearchFieldCommand = new DelegateCommand(FocusAction);
            GlobalCommands.ShowSearchFieldInActiveCommand.RegisterCommand(FocusSearchFieldCommand);
        }

        private void FocusAction()
        {
            if (!IsActive)
                return;
            SearchFieldVisibility = Visibility.Visible;
        }

        private void AddAction(object param)
        {
            _queueController.AddSongToQueue(((BaseSong)((Button)((RoutedEventArgs)param).Source).DataContext));
        }

        private async void MoreAction()
        {
            if (_songSearchService != null && _songSearchService.Query != null)
                foreach (var song in await _songSearchService.FetchAsync())
                {
                    SearchResult.Add(song);
                }
        }

        private async void SearchAction()
        {
            if(SearchFieldVisibility != Visibility.Visible)
            {
                SearchFieldVisibility = Visibility.Visible;
                return;
            }

            SearchResult.Clear();

            if(string.IsNullOrWhiteSpace(SearchQuery))
                return;

            //TODO:Check for link or uri

            _songSearchService.Query = SearchQuery;

            foreach (var song in await _songSearchService.FetchAsync())
            {
                SearchResult.Add(song);
            }

            SearchFieldVisibility = Visibility.Hidden;
        }
    }
}
