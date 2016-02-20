using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Models
{
    public class SimplePlaylist : BindableBase, IPlaylist
    {
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                if(_name != value)
                {
                    _name = value;
                    OnPropertyChanged(() => Name);
                }
            }
        }

        public ObservableCollection<ISongModel> Songs
        {
            get;

            set;
        } = new ObservableCollection<ISongModel>();
    }
}
