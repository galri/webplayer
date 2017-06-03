using Infrastructure.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Webplayer.Services
{
    class DialogService : IDialogService
    {
        public void ShowError(string title, string message)
        {
            MessageBox.Show(message, title);
        }
    }
}
