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
    /// Interaction logic for YoutubeSearchDialogView.xaml
    /// </summary>
    public partial class YoutubeSearchDialogView : IYoutubeSearchDialogView
    {


        public bool ShowingUploaderView
        {
            get { return (bool)GetValue(ShowingUploaderViewProperty); }
            set { SetValue(ShowingUploaderViewProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ShowingUploaderView.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ShowingUploaderViewProperty =
            DependencyProperty.Register("ShowingUploaderView", typeof(bool), typeof(YoutubeSearchDialogView), new PropertyMetadata(false));



        public YoutubeSearchDialogView()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ShowingUploaderView = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ShowingUploaderView = false;
        }
    }
}
