using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webplayer.Modules.Structure.ViewModels
{
    class NameDialogViewModel : BindableBase
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
                SetProperty(ref _name, value);
            }
        }
    }
}
