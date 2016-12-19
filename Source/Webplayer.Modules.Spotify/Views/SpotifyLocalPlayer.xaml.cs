using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using SpotifyAPI.Local.Models;
using SpotifyAPI.Local;
using SpotifyAPI.Web;
using SpotifyAPI.Web.Models;
using System.Threading;
using Webplayer.Services.Spotify;
using System.ComponentModel;
using System.Drawing;
using Webplayer.Modules.Spotify.Models;

namespace Webplayer.Services.Spotify
{
    /// <summary>
    /// Interaction logic for SpotifyLocalPlayer.xaml
    /// 
    /// uses https://github.com/JohnnyCrazy/SpotifyAPI-NET and is build upon the example that follows.
    /// </summary>
    public partial class SpotifyLocalPlayer : UserControl, INotifyPropertyChanged
    {
        //INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void onPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(name));
        }

        //Object properties
        private BitmapImage myThumbnail = new BitmapImage(new Uri("/Media/NoAlbumArt.png", UriKind.RelativeOrAbsolute));
        
        //spotify library Objects
        private SpotifyLocalAPI spotify;
        

        #region Prop
        public BitmapImage Thumbnail { 
            get { return myThumbnail; } 
            set { 
                myThumbnail = value;
                onPropertyChanged("Thumbnail"); 
            } 
        }
        #endregion
        
        public SpotifyLocalPlayer()
        {
            InitializeComponent();

            //Try to connect to spotify
            if (!tryConnect())
            {
                MessageBox.Show("Couldn't connect to spotify");
            }
        }

        /// <summary>
        /// Tro to establish a connection to the local spotify player.
        /// </summary>
        /// <returns>true on sucsess</returns>
        private bool tryConnect()
        {
            //TODO: Eventhandlers for error connecting, and lett ui handle communication.
            spotify = new SpotifyLocalAPI();
            
            //if (!spotify.IsSpotifyRunning())
            //{
            //    spotify.RunSpotify();
            //    Thread.Sleep(5000);
            //}

            //if (!SpotifyAPI.IsSpotifyWebHelperRunning())
            //{
            //    spotify.RunSpotifyWebHelper();
            //    Thread.Sleep(4000);
            //}

            //if (!spotify.Connect())
            //{
            //    Boolean retry = true;
            //    while (retry)
            //    {
            //        if (MessageBox.Show("Couldnt start/detect spotify. Retry?", "Error", 
            //            MessageBoxButton.YesNo) == System.Windows.MessageBoxResult.Yes)
            //        {
            //            if (spotify.Connect())
            //                retry = false;
            //            else
            //                retry = true;
            //        }
            //        else
            //        {
            //            return false;
            //        }
            //    }
            //}
            //mh = spotify.GetMusicHandler();
            //eh = spotify.GetEventHandler();
            return true;
        }

        private void SpotifyPlayer_Load(object sender, RoutedEventArgs e)
        {
            //spotify.Update();
            //setTrackInfoFromLocal();

            ////sett events
            //eh.OnTrackChange += new SpotifyEventHandler.TrackChangeEventHandler(trackchange);
            //eh.OnTrackTimeChange += new SpotifyEventHandler.TrackTimeChangeEventHandler(timechange);
            //eh.OnPlayStateChange += new SpotifyEventHandler.PlayStateEventHandler(playstatechange);
            //eh.OnVolumeChange += new SpotifyEventHandler.VolumeChangeEventHandler(volumechange);
            ////eh.SetSynchronizingObject(this);

            ////Capture changes in spotify.
            //eh.ListenForEvents(true);
        }

        /// <summary>
        /// Update UI from what is playing now.
        /// </summary>
        private void setTrackInfoFromLocal()
        {
            try
            {
                
                //gets and sets current song info.
                //Bitmap humbnail = spotify.GetMusicHandler().GetCurrentTrack().GetAlbumArt(AlbumArtSize.SIZE_320);
                //Track playing = spotify.GetMusicHandler().GetCurrentTrack();
                //Thumbnail = SpotifySongSearch.fetchThumbnail(playing.track_resource.uri);

                //BitmapImage img = humbnail.ToBitmapImage();
                //Thumbnail = img;
                //SpotifySong asd = new SpotifySong(spotify.GetMusicHandler().GetCurrentTrack());
                //Thumbnail = asd.Picture;

                /*
                linkSong.Text = mh.GetCurrentTrack().GetTrackName();
                linkSong.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetTrackURI());
                linkArtist.Text = mh.GetCurrentTrack().GetArtistName();
                linkArtist.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetArtistURI());
                linkAlbum.Text = mh.GetCurrentTrack().GetAlbumName();
                linkAlbum.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetAlbumURI());

                //time
                TimeSpan time = TimeSpan.FromSeconds(mh.GetTrackPosition());
                lblTimeProgress.Text = time.Minutes.ToString() + " : " + time.Seconds.ToString();
                */
            }
            catch (Exception e)
            {
                //throw new Exception("couldn't set new track info", e);
            }
        }

        /// <summary>
        /// Update UI from song.
        /// </summary>
        /// <param name="song"></param>
        private void setTrackInfoFromSong(SpotifySong song)
        {
            try
            {
                Thumbnail = new BitmapImage( song.Picture);
                /*
                //gets and sets current song info.
                pbAlbum.Image = song.Picture;

                linkSong.Text = song.Tittel;
                linkSong.LinkClicked += (senderTwo, args) => Process.Start(song.Uri.ToString());
                linkArtist.Text = song.Author;
                linkArtist.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetArtistURI());
                linkAlbum.Text = song.Tittel;
                linkAlbum.LinkClicked += (senderTwo, args) => Process.Start(mh.GetCurrentTrack().GetAlbumURI());

                //time
                TimeSpan time = TimeSpan.FromSeconds(mh.GetTrackPosition());
                lblTimeProgress.Text = time.Minutes.ToString() + " : " + time.Seconds.ToString();
            
                 */ 
            }
            catch (Exception e)
            {
                throw new Exception("couldn't set new track info", e);
            }
        }

        #region Events
        private void volumechange(VolumeChangeEventArgs e)
        {

        }
        private void playstatechange(PlayStateEventArgs e)
        {

        }
        private void trackchange(TrackChangeEventArgs e)
        {
            /*
            progressBar1.Maximum = (int)mh.GetCurrentTrack().GetLength() * 100;
            linkLabel1.Text = e.new_track.GetTrackName();
            linkLabel2.Text = e.new_track.GetArtistName();
            linkLabel3.Text = e.new_track.GetAlbumName();
            pictureBox1.Image = await e.new_track.GetAlbumArtAsync(AlbumArtSize.SIZE_160);
            pictureBox2.Image = await e.new_track.GetAlbumArtAsync(AlbumArtSize.SIZE_640);
            label7.Text = mh.IsAdRunning().ToString();
        
             */

            //pause();
            //trackEnded(this, new SpotifyTrackEndedArgs(new SpotifySong(e.old_track), new SpotifySong(e.new_track)));
            

        }
        private void timechange(TrackTimeChangeEventArgs e)
        {
            if (e != null)
            {
                //TimeSpan time = TimeSpan.FromSeconds(e.track_time);
                ////lblTimeProgress.Text = time.Minutes.ToString() + " : " + time.Seconds.ToString();
                //SpotifySong songPlaying = getSpotifySongPlaying();


                ////Next song.
                //if (isPlaying() && songPlaying.Length.TotalSeconds < e.track_time + 1)
                //{
                //    pause();
                //    raiseSongEndedEvent(getSpotifySongPlaying(), null);
                //}
                ///*
                //label4.Text = formatTime(e.track_time) + "/" + formatTime(mh.GetCurrentTrack().GetLength());
                //progressBar1.Value = (int)e.track_time * 100;
                //*/
            }
        }
        #endregion

        /// <summary>
        /// Play the select track on spotify local.
        /// </summary>
        /// <param name="song"></param>


        void setTrack(SpotifySong song)
        {
            //mh.PlayURL(song.Uri.ToString());
            //start();
            //setTrackInfoFromLocal();
            setTrackInfoFromSong(song);
        }

        /// <summary>
        /// Returns the current spotify song, null if there 
        /// </summary>
        //SpotifySong getSpotifySongPlaying()
        //{
        //    SpotifySong song = new SpotifySong(mh.GetCurrentTrack());
        //    return song;
        //}

        /// <summary>
        /// checks if spotify is playing a song
        /// </summary>
        /// <returns></returns>
        //public bool isPlaying()
        //{
        //    return mh.IsPlaying();
        //}

        /// <summary>
        /// start playing the current spoify song.
        /// </summary>
        //public void start()
        //{
        //    mh.Play();
        //}

        /// <summary>
        /// Stop playing the current spotify sonng
        /// </summary>
        //public void pause()
        //{
        //    mh.Pause();
        //}

        /// <summary>
        /// Starts or pauses spotify depending on current state.
        /// </summary>
        //public void pauseStart()
        //{
        //    if (mh.IsPlaying())
        //    {
        //        pause();
        //    }
        //    else
        //    {
        //        start();
        //    }
        //}

        #region events

        #region SongEnded
        //http://stackoverflow.com/questions/3779674/how-do-i-attach-property-to-a-custom-event 
        //public class SpotifySongEndedEventArgs : RoutedEventArgs
        //{
        //    public SpotifySongEndedEventArgs() : base() { }
        //    public SpotifySongEndedEventArgs(RoutedEvent routedEvent) : base(routedEvent) { }
        //    public SpotifySongEndedEventArgs(RoutedEvent routedEvent, object source) : base(routedEvent, source) { }
        //    public SpotifySong PreviousSong
        //    {
        //        get;
        //        set;
        //    }
        //    public SpotifySong NextSong
        //    {
        //        get;
        //        set;
        //    }

        //}

        //public delegate void SpotifySongEndedEventHandler(object sender, SpotifySongEndedEventArgs e);

        //public static readonly RoutedEvent SongEndedEvent = 
        //    EventManager.RegisterRoutedEvent("SongEnded", RoutingStrategy.Bubble,
        //    typeof(SpotifySongEndedEventHandler), typeof(SpotifyLocalPlayer));
        //public event SpotifySongEndedEventHandler SongEnded
        //{
        //    add { AddHandler(SongEndedEvent, value); }
        //    remove { RemoveHandler(SongEndedEvent, value); }
        //}
        void raiseSongEndedEvent(SpotifySong old, SpotifySong news)
        {
            //SpotifySongEndedEventArgs newEventArgs = new SpotifySongEndedEventArgs(SpotifyLocalPlayer.SongEndedEvent);
            //newEventArgs.PreviousSong = old;
            //this.Dispatcher.Invoke((Action)(() =>
            //{
            //    // your code here.
            //    RaiseEvent(newEventArgs);
            //}));
        }
        #endregion  

        #endregion

    }
}
