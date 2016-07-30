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

namespace Webplayer.Modules.Spotify.ViewModels
{
    class SpotifyFindViewModel : BindableBase, ISpotifyFindViewModel
    {
        private readonly ISpotifySongSearch _songSearchService;
        private string _query;
        private IQueueController _queueController;

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

        public DelegateCommand<object> AddSongCommand { get; set; }

        public SpotifyFindViewModel(ISpotifySongSearch songSearchService, IQueueController queueController)
        {
            _songSearchService = songSearchService;
            _queueController= queueController;
            SearchCommand = new DelegateCommand(SearchAction);
            FetchMoreResultCommand = new DelegateCommand(MoreAction);
            AddSongCommand = new DelegateCommand<object>(AddAction);
        }

        private void AddAction(object param)
        {
            _queueController.AddSongToQueue(((BaseSong)((Button)((RoutedEventArgs)param).Source).DataContext));
        }

        private async void MoreAction()
        {
            foreach (var song in await _songSearchService.FetchAsync())
            {
                SearchResult.Add(song);
            }
        }

        private async void SearchAction()
        {
            SearchResult.Clear();

            if(string.IsNullOrWhiteSpace(SearchQuery))
                return;

            //TODO:Check for link or uri

            _songSearchService.Query = SearchQuery;

            foreach (var song in await _songSearchService.FetchAsync())
            {
                SearchResult.Add(song);
            }
        }
    }
}
