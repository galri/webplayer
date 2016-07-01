using Infrastructure;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webplayer.Modules.Structure.ViewModels
{
    interface IQueueViewModel
    {
         ObservableCollection<ISongModel> Queue { get; set; }

         ISongModel SongPlaying { get; set; }
    }
}
