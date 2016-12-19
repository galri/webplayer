using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Infrastructure.Models;
using Webplayer.Modules.Structure.ViewModels;

namespace Webplayer.Modules.Structure.DesignTime
{
    class QueueDtVm : IQueueViewModel
    {
        public ICommand DeleteSongFromQueueCommand
        {
            get;

            set;
        }

        public ICommand LoadQueueCommand
        {
            get;

            set;
        }

        public ICommand PlaySongCommand
        {
            get;

            set;
        }

        public ObservableCollection<BaseSong> Queue
        {
            get;
        }

        public ICommand SaveQueueCommand
        {
            get;
        }

        public BaseSong SelectSong
        {
            get;

            set;
        }

        public BaseSong SongPlaying
        {
            get;

            set;
        }

        public string QueueName
        {
            get;

            set;
        } = "Queue name";

        public QueueDtVm()
        {
            Queue = new ObservableCollection<BaseSong>
            {
                new DtSong
                {
                    Title = "Mame",
                    Artist = "Artist",
                    Length = new TimeSpan(1,32,17),
                },
                new DtSong
                {
                    Title = "Mame",
                    Artist = "Artist",
                    Length = new TimeSpan(1,32,17),
                }
            };
        }
    }
}
