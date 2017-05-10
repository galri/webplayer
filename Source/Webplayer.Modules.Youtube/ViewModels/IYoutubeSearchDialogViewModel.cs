using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Webplayer.Modules.Youtube.Models;
using Webplayer.Modules.Youtube.Services;

namespace Webplayer.Modules.Youtube.ViewModels
{
    interface IYoutubeSearchDialogViewModel
    {
        ICommand RemoveUploadFilterCommand { get; set; }

        SongSearcOrdering OrderingFilter { get; set; }

        string SearchQuery { get; set; }

        YoutubeUploader SelectedUploader { get; set; }
    }
}
