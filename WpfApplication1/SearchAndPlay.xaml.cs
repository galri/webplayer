using System;
using System.Collections.Generic;
using System.Globalization;
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
using Webplayer.Services;
using Webplayer.Services.Spotify;
//using Webplayer.Services.Youtube;

namespace Webplayer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TreeViewItem youtube = new TreeViewItem(){Header = Service.Youtube, IsExpanded = true, Visibility = Visibility.Hidden};
        private TreeViewItem spotify = new TreeViewItem(){Header = Service.Spotify, IsExpanded = true, Visibility = Visibility.Hidden};

        SpotifySongSearch spotifySearch;
        //YoutubeSongSearch youtubeSearch;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void frmMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            twSearchResult.Items.Clear();
            twSearchResult.Items.Add(youtube);
            twSearchResult.Items.Add(spotify);
        }

        /// <summary>
        /// uses the inner services searcj object to add more search results in the treeviewitemheader. 
        /// </summary>
        /// <param name="removeMoreButton"> keep or remove the "get more search results button". this is only viable if the list is not empty or button was not added.</param>
        private void addSpotifySongs(bool removeMoreButton)
        {
            //removes the more button
            if (removeMoreButton)
            {
                spotify.Items.RemoveAt(spotify.Items.Count - 1);
            }

            foreach (SpotifySong item in spotifySearch.next())
            {
                spotify.Items.Add(item);
            }
            spotify.Items.Add(new buttonshow() { context = Service.Spotify });
        }

        /// <summary>
        /// uses the inner services searcj object to add more search results in the treeviewitemheader. 
        /// </summary>
        /// <param name="removeMoreButton"> keep or remove the "get more search results button". this is only viable if the list is not empty or button was not added.</param>
        private void addYoutubeSongs(bool removeMoreButton)
        {
            //removes the more button
            if (removeMoreButton)
            {
                youtube.Items.RemoveAt(youtube.Items.Count - 1);
            }

            //add searchresult
            //foreach (YoutubeSong item in youtubeSearch.next())
            //{
            //    youtube.Items.Add(item);
            //}

            //a little ugly but it werks.
            youtube.Items.Add(new buttonshow() { context = Service.Youtube });
        }

        /// <summary>
        /// play the next song, if there is one, in the playlist
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NextSong(object sender, RoutedEventArgs e)
        {
            //player.play(nextSong());

          /*  
            //TODO: put in i funksjonen.
            //retrive the song object
            Song next = nextSong();

            //Only continue if there is is a next song
            if (next != null)
            {
                player.play(next);
            }
           * */
        }

        /// <summary>
        /// Play the previuos song, if there is one, in the playlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreviousSong(object sender, RoutedEventArgs e)
        {
            //player.play(previousSong());
        }

        /// <summary>
        /// Play or pause the current selected song in the playlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PlayPauseSong(object sender, RoutedEventArgs e)
        {
            //player.playPause();
        }

        /// <summary>
        /// Search button function.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void search(object sender, RoutedEventArgs e)
        {
            spotify.Items.Clear();
            youtube.Items.Clear();

            //TODO: forhindre i at bruker skriver in noe annet en tall og bare innenfor tall grenser (y = 50) (s=?)
            //search after song
            if (chkSong.IsEnabled)
            {
                if ((bool)chkYoutube.IsChecked)
                {
                    //show header
                    youtube.Visibility = System.Windows.Visibility.Visible;

                    //create a new search object with the new search creteria.
                    //youtubeSearch = new YoutubeSongSearch(Convert.ToInt16(tbYoutubeSongNr.Text), txtquery.Text,(bool)chIncludeThumbnail.IsChecked, (bool)chYoutubeIncludeUnEmddeble.IsChecked);

                    //adds the new songs. 
                    addYoutubeSongs(false);
                }
                if ((bool)chkSpotify.IsChecked)
                {
                    //show header
                    spotify.Visibility = System.Windows.Visibility.Visible;

                    spotifySearch = new SpotifySongSearch(Convert.ToInt16(tbSpotifySongNr.Text), txtquery.Text,(bool)chIncludeThumbnail.IsChecked);
                    addSpotifySongs(false);
                }
            }

            //search after playlist
            if (chkPlaylist.IsEnabled)
            {
                //TODO: this.
            }
        }

        /// <summary>
        /// used in wpf. The more Button calls this function and sends over the source object in treeviewitem.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void getMoreSearches(object sender, RoutedEventArgs e)
        {
            buttonshow service = (buttonshow)((Button)sender).CommandParameter;
            switch (service.context){
                case Service.Youtube:
                    addYoutubeSongs(true);
                    break;
                case Service.Spotify:
                    addSpotifySongs(true);
                    break;
            }
        }

        /// <summary>
        /// Used by the wpf, add button in datatemplate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNewSongToPlaylist(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(((SpotifySong)((Button)sender).CommandParameter).Tittel);
            playList.Items.Add(((Song)((Button)sender).CommandParameter));
        }

        /// <summary>
        /// plays the song that was double clikced in the playlist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            //finds out which item was doublecliked.
            //http://stackoverflow.com/questions/929179/get-the-item-doubleclick-event-of-listview
            DependencyObject obj = (DependencyObject)e.OriginalSource;
            while (obj != null && obj != playList)
            {
                if (obj.GetType() == typeof(ListViewItem))
                {
                    // Do something here
                    //MessageBox.Show("A ListViewItem was double clicked!");
                    ListViewItem asd = (ListViewItem)obj;
                    //player.play((Song)asd.Content);
                    break;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
        }

        private void playList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void player_SongEnded(object sender, Player.SongEndedEventArgs s)
        {
            //the same as the next button is pushed.
            //i knowe that the objects sent over is never used.
            NextSong(null, null);
        }


    }
        //just aholder to say to the datatemplate that more button should be shown
        //TODO: find a better solution.
        public class buttonshow
        {
            public Service context { get; set; }
        }
}
