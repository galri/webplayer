using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webplayer.Modules.Youtube.Models;

namespace Webplayer.Modules.Youtube.ViewModels
{
    interface IYoutubeFindViewModel 
    {
        ObservableCollection<YoutubeSong> SearchResult { get; set; }

        string SearchQuery { get; set; }

        bool CanFetchMore { get; set; }

        ContentType SearchType { get; set; }

        ICommand SearchCommand { get; set; }

        ICommand FetchMoreResultCommand { get; set; }
    }
}
