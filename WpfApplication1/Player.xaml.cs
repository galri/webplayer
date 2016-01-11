using System;
using System.Collections.Generic;
using System.ComponentModel;
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
//using Webplayer.Services.Youtube;

namespace Webplayer.Services
{
    /// <summary>
    /// Interaction logic for Player.xaml
    /// </summary>
    public partial class Player : UserControl, INotifyPropertyChanged
    {
        private Service serviceCurrentlyUsed = Service.None;
        public Service ServiceCurrentlyUsed { 
            get 
            {
                return serviceCurrentlyUsed;
            }
            set
            {
                serviceCurrentlyUsed = value;
                onPropertyChanged("ServiceCurrentlyUsed");
            }
        }
       
        //public RoutedEvent 

        public event PropertyChangedEventHandler PropertyChanged;
        private void onPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(name));
        }

        public Player()
        {
            InitializeComponent();
            sPlayer.SongEnded += new SpotifyLocalPlayer.SpotifySongEndedEventHandler(spotifyEnded);
            yPlayer.VideoOver += youtubeEnded;

            
        }

        internal void play(Song song)
        {
            //if (song != null)
            //{
                //if (song is YoutubeSong)
                //{
                //    //pauses the spotify player incase it was a spotify song playing before.
                //    sPlayer.pause();

                //    //show only yPlayer
                //    ServiceCurrentlyUsed = Service.Youtube;

                //    //disenabled and hide progressbar since youtube has it own
                //    //tbSongProgress.Enabled = false;
                //    //tbSongProgress.Visible = false;

                //    YoutubeSong v = (YoutubeSong)song;
                //    //yPlayer.setMovie(v.Uri.ToString());
                //    yPlayer.VideoID = v.VideoID;
                //    yPlayer.play();
                //}
                //else if (song is SpotifySong)
            //    {
            //        //pauses the youtube player incase it was a youtube video playing before.
            //        yPlayer.pause();

            //        //enables the progressbar.
            //        //tbSongProgress.Enabled = false;
            //        //tbSongProgress.Visible = true;
            //        //tbSongProgress.Maximum = (int)song.Length.TotalSeconds;

            //        ServiceCurrentlyUsed = Service.Spotify;

            //        SpotifySong v = (SpotifySong)song;
            //        //sPlayer.getSpotifySongPlaying = v;
            //        //sPlayer.pauseStart();
            //        sPlayer.setTrack(v);
            //    }
            //    else
            //    {
            //        MessageBox.Show("ukjent sang type. urk");
            //    }
            //}
        }

        /// <summary>
        /// Pause the current underplayer, if non is selected nothing happends.
        /// </summary>
        public void pause()
        {
            switch (serviceCurrentlyUsed)
            {
                case Service.None:
                    //not possible
                    break;
                case Service.Youtube:
                    yPlayer.pause();
                    break;
                case Service.Spotify:
                    sPlayer.pause();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Play the current underplayer
        /// </summary>
        public void play()
        {
            switch (serviceCurrentlyUsed)
            {
                case Service.None:
                    //not possible
                    break;
                case Service.Youtube:
                    yPlayer.play();
                    break;
                case Service.Spotify:
                    sPlayer.start();
                    break;
                default:
                    break;
            }
        }

        public void playPause()
        {
            switch (serviceCurrentlyUsed)
            {
                case Service.None:
                    //not possible
                    break;
                case Service.Youtube:
                    if (yPlayer.playerStatus == YoutubePlayerLib.YoutubePlayerState.playing)
                    {
                        yPlayer.pause();
                    }
                    else
                    {
                        yPlayer.play();
                    }
                    break;
                case Service.Spotify:
                    if (sPlayer.isPlaying())
                    {
                        sPlayer.pause();
                    }
                    else
                    {
                        sPlayer.start();
                    }
                    break;
                default:
                    break;
            }
        }

        #region Events

        #region songover
        public class SongEndedEventArgs : RoutedEventArgs
        {
            public Service ServiceUsed { get; set; }
            public Song SongPlayed { get; set; }

            public SongEndedEventArgs() : base() { }
            public SongEndedEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
            public SongEndedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
        }


        public delegate void SongEndedEventHandler(object sender, SongEndedEventArgs s);

        public static readonly RoutedEvent SongEndedEvent = 
            EventManager.RegisterRoutedEvent("SongEnded", RoutingStrategy.Bubble,
            typeof(SongEndedEventHandler), typeof(Player));


        public event SongEndedEventHandler SongEnded
        {
            add { AddHandler(SongEndedEvent, value); }
            remove { RemoveHandler(SongEndedEvent, value); }
        }

        void raiseSongEndedEvent(Service s, Song lastSong)
        {
            SongEndedEventArgs newEventArgs = new SongEndedEventArgs(Player.SongEndedEvent);
            newEventArgs.ServiceUsed = s;
            newEventArgs.SongPlayed = lastSong;
            //http://stackoverflow.com/questions/9732709/the-calling-thread-cannot-access-this-object-because-a-different-thread-owns-it
            this.Dispatcher.Invoke((Action)(() =>
            {
                RaiseEvent(newEventArgs);
            }));
        }

        #region functions that handles subplayer over
        void youtubeEnded(object sender, RoutedEventArgs e)
        {
            //TODO: add the current song.
            raiseSongEndedEvent(Service.Youtube, null);
        }

        public void spotifyEnded(object sender, Webplayer.Services.Spotify.SpotifyLocalPlayer.SpotifySongEndedEventArgs e)
        {
            raiseSongEndedEvent(Service.Spotify, e.PreviousSong);
        }
        #endregion

        #endregion

        #endregion

        /// <summary>
        /// What functions an underplayer must have-
        /// </summary>
        private interface IPlayer
        {
            
        }
    }
}
