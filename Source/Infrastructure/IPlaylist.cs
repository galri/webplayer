using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure
{
    public interface IPlaylist
    {
        ObservableCollection<ISongModel> Songs { get; set; }

        string Name {get; set;}
    }
}
