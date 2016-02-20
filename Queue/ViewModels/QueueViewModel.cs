using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webplayer.Modules.Structure.ViewModels
{
    class QueueViewModel : BindableBase, IQueueViewModel
    {
        public ObservableCollection<ISongModel> Queue { get; set; }

        public ISongModel SongPlaying { get; set; }

        public QueueViewModel(IUnityContainer container)
        {
            Queue = container.Resolve<IPlaylist>(SharedResourcesNames.QueuePlaylist).Songs;
        }
    }
}
