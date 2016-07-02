using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Webplayer.Modules.Structure.Helper;
using WPF.JoshSmith.ServiceProviders.UI;

namespace Webplayer.Modules.Structure.Views
{
    /// <summary>
    /// Interaction logic for QueueView.xaml
    /// </summary>
    public partial class QueueView : UserControl, IQueueView
    {
        private DragDropHelper dragDropHelper;

        public QueueView()
        {
            InitializeComponent();
            Loaded += ViewLoaded;
        }

        private void ViewLoaded(object sender, RoutedEventArgs e)
        {
            dragDropHelper = new DragDropHelper(Queue);
        }
    }
}
