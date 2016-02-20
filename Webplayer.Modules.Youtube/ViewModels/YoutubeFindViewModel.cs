using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Webplayer.Modules.Youtube.Models;
using Webplayer.Modules.Youtube.Services;

namespace Webplayer.Modules.Youtube.ViewModels
{
    class YoutubeFindViewModel : BindableBase, IYoutubeFindViewModel
    {
        private IList<ISongModel> Queue;
        private readonly IYoutubeSongSearchService _songSearchService;
        private ContentType _searchType;
        private ObservableCollection<YoutubeSong> _searchResult = new ObservableCollection<YoutubeSong>();
        private string _searchQuery;
        private bool _canFetchMore = true;

        public bool CanFetchMore
        {
            get
            {
                return _canFetchMore;
            }

            set
            {
                if(value != _canFetchMore)
                {
                    _canFetchMore = value;
                    OnPropertyChanged(() => CanFetchMore);
                }
            }
        }

        public ICommand FetchMoreResultCommand
        {
            get;

            set;
        }

        public ICommand SearchCommand
        {
            get;

            set;
        }

        public ICommand AddSongCommand { get; set; }

        public string SearchQuery
        {
            get
            {
                return _searchQuery;
            }

            set
            {
                if(_searchQuery != value)
                {
                    _searchQuery = value;
                    OnPropertyChanged(() => SearchQuery);
                }
            }
        }

        public ObservableCollection<YoutubeSong> SearchResult
        {
            get
            {
                return _searchResult;
            }

            set
            {
                if(_searchResult != value)
                {
                    _searchResult = value;
                    OnPropertyChanged(() => _searchResult);
                }
            }
        }

        public ContentType SearchType
        {
            get
            {
                return _searchType;
            }

            set
            {
                if(_searchType != value)
                {
                    _searchType = value;
                    OnPropertyChanged(() => SearchType);
                }
            }
        }

        public YoutubeFindViewModel(IUnityContainer container, IYoutubeSongSearchService songSearchService)
        {
            _songSearchService = songSearchService;
            Queue = container.Resolve<IPlaylist>(SharedResourcesNames.QueuePlaylist).Songs;
            SearchCommand = new DelegateCommand(SearchCommandAction);
            FetchMoreResultCommand = new DelegateCommand(FetchMoreResultCommandAction);
            AddSongCommand = new DelegateCommand<object>(AddSongAction);
        }

        private void AddSongAction(object param)
        {
            Queue.Add( ((ISongModel) ( (Button) ((RoutedEventArgs)param).Source).DataContext ));
        }

        private void FetchMoreResultCommandAction()
        {
            foreach (var item in _songSearchService.FetchNextSearchResult())
            {
                SearchResult.Add(item);
            }
        }

        private void SearchCommandAction()
        {
            _songSearchService.Query = SearchQuery;
            SearchResult.Clear();
            foreach (var item in _songSearchService.FetchNextSearchResult())
            {
                SearchResult.Add(item);
            }
        }
    }
}
