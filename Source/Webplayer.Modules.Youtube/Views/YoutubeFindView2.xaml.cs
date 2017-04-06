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

namespace Webplayer.Modules.Youtube.Views
{
    /// <summary>
    /// Interaction logic for YoutubeFindView2.xaml
    /// </summary>
    public partial class YoutubeFindView : IYoutubeFindView
    {
        public YoutubeFindView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get; set;
        } = "Youtube";

        private void SearchField_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBorder.Visibility = Visibility.Hidden;
        }
    }
}
