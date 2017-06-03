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
using MaterialDesignThemes.Wpf;
using Microsoft.Practices.Unity;
using Webplayer.Modules.Spotify.Views;
using Prism.Regions;
using Prism.Logging;

namespace Webplayer.Modules.Spotify.ViewModels
{
    class SpotifyFindViewModel : BindableBase, ISpotifyFindViewModel, IActiveAware
    {
        private const string Tag = "SpotifyFindViewModel";
        private bool _isActive;
        private readonly ISpotifySongSearch _songSearchService;
        private string _query;
        private IQueueController _queueController;
        private Visibility _searchFieldVisibility = Visibility.Hidden;
        private IUnityContainer _container;
        private readonly IRegionManager _regionManager;
        private ILoggerFacade _logger;
        private IDialogService _dialogService;

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

        public ICommand SearchSingleCommand { get; }

        public ICommand ShowAccountCommand { get; set; }

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

        public SpotifyFindViewModel(ISpotifySongSearch songSearchService, IQueueController queueController,
            IUnityContainer container,IRegionManager regionManager, ILoggerFacade logger, IDialogService dialogService)
        {
            _dialogService = dialogService;
            _logger = logger;
            _regionManager = regionManager;
            _container = container;
            _songSearchService = songSearchService;
            _queueController= queueController;
            SearchCommand = new DelegateCommand(SearchAction);
            FetchMoreResultCommand = new DelegateCommand(MoreAction);
            AddSongCommand = new DelegateCommand<object>(AddAction);
            FocusSearchFieldCommand = new DelegateCommand(FocusAction);
            GlobalCommands.ShowSearchFieldInActiveCommand.RegisterCommand(FocusSearchFieldCommand);
            SearchSingleCommand = new DelegateCommand(SingleSearchAction);
            ShowAccountCommand = new DelegateCommand(ExecuteFindAccount);
        }

        private void ExecuteFindAccount()
        {
            var acount = _regionManager.Regions["SpotTrans"].Views.OfType<ISpotifyAcountView>();
            _regionManager.Regions["SpotTrans"].Activate(acount.First());
        }

        private async void SingleSearchAction()
        {
            var v = _container.Resolve<ISpotifyFindSingleView>();
            await DialogHost.Show(v, "RootDialog");
            var result = ((ISpotifyFindSingleViewModel)((UserControl)v).DataContext).Result;
            SearchResult.Clear();
            SearchResult.Add(result);
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

            try
            {
                _songSearchService.Query = SearchQuery;
                foreach (var song in await _songSearchService.FetchAsync())
                {
                    SearchResult.Add(song);
                }
            }
            catch (Exception ex)
            {
                _logger.Log($"{Tag} FetchFetchSongs error {ex.Message}", Category.Exception, Priority.High);
                _dialogService.ShowError("Spotify search", ex.Message);
            }

            SearchFieldVisibility = Visibility.Hidden;
        }
    }
}
