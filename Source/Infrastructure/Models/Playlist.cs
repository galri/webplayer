using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class Playlist : BindableBase
    {
        public string Id { get; set; }

        private string _name;
        private ObservableCollection<BaseSong> _songs = 
            new ObservableCollection<BaseSong>();

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                SetProperty(ref _name, value);
            }
        }

        public ObservableCollection<BaseSong> Songs
        {
            get { return _songs; }

            set
            {
                var old = _songs;
                if (SetProperty(ref _songs, value))
                {
                    old.CollectionChanged -= SongsOnCollectionChanged;
                    _songs.CollectionChanged += SongsOnCollectionChanged;
                }
            }
        }

        public ObservableCollection<BaseSong> SongsRemoved { get; } = 
            new ObservableCollection<BaseSong>();

        public Playlist()
        {
            _songs.CollectionChanged += SongsOnCollectionChanged;
        }

        private void SongsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (var oldItem in e.OldItems)
                {
                    SongsRemoved.Add((BaseSong)oldItem);
                }
            }
        }
    }
}
