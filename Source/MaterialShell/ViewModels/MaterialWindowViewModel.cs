using Prism;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialShell.ViewModels
{
    class MaterialWindowViewModel : BindableBase, IMainWindowViewModel
    {
        private bool _isActive;

        public bool IsActive
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                if (value == _isActive)
                    return;
                _isActive = value;
                IsActiveChanged?.Invoke(this, new EventArgs());
            }
        }

        public event EventHandler IsActiveChanged;

        public MaterialWindowViewModel()
        {
            ;
        }
    }
}
