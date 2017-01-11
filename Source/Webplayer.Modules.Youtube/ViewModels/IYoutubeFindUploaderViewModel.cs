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
    interface IYoutubeFindUploaderViewModel
    {
        ObservableCollection<YoutubeUploader> Uploaders { get; set; }

        YoutubeUploader SelectedUploader { get; set; }

        string SearchQuery { get; set; }

        ICommand SearchCommand { get; set; }

        ICommand MoreCommand { get; set; }
    }
}
