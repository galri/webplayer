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
using Webplayer.Services.Spotify;

namespace Webplayer.Modules.Spotify.Views
{
    /// <summary>
    /// Interaction logic for SpotifyFindView.xaml
    /// </summary>
    public partial class SpotifyFindView : ISpoifyFindView
    {
        public string Title { get; set; } = "Spotify";
        public SpotifyFindView()
        {
            InitializeComponent();
        }
    }
}
