using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Services;

namespace Webplayer
{
    partial class MainWindow
    {
        /*
        private Bitmap TYoutube;
        private Bitmap TSpotify;

        public PlayList(){
            System.Reflection.Assembly myAssembly = System.Reflection.Assembly.GetExecutingAssembly();

            Stream myStream = myAssembly.GetManifestResourceStream("youtube_spiller.Thumbnail_Spotify.jpg");
            TSpotify = new Bitmap(myStream);

            myStream = myAssembly.GetManifestResourceStream("youtube_spiller.Thumbnail_Youtube.jpg");
            TYoutube = new Bitmap(myStream);
        }
        */

        

        /// <summary>
        /// Returns the current chosen song.
        /// </summary>
        /// <returns></returns>
        //public Song currentSong()
        //{
        //    if (playList.Items.Count == 0)
        //    {
        //        return null;
        //    }

        //    return (Song)playList.Items.CurrentItem;
        //}

        ///// <summary>
        ///// choses the next song and returns it.
        ///// </summary>
        ///// <returns>Return song, or null if the last one</returns>
        //public Song nextSong()
        //{
        //    if (playList.SelectedIndex == -1)
        //    {
        //        //empty
        //        return null;
        //    }

        //    //collects the current. and checks if there are more
        //    int chosen = playList.SelectedIndex;
        //    if (chosen == playList.Items.Count - 1)
        //    {
        //        return null;
        //    }
        //    playList.SelectedIndex = chosen + 1;
        //    return (Song)playList.Items[chosen + 1];
        //}
        
        ///// <summary>
        ///// Return song or null if the first one.
        ///// </summary>
        ///// <returns></returns>
        //public Song previousSong()
        //{
        //    if (playList.SelectedIndex == -1)
        //    {
        //        //empty
        //        return null;
        //    }

        //    //collects the current. and checks if the current is the last
        //    int chosen = playList.SelectedIndex;
        //    if (chosen == 0)
        //    {
        //        return null;
        //    }
        //    playList.SelectedIndex = chosen - 1;
        //    return (Song)playList.Items[chosen - 1];
        //}

        /*
        /// <summary>
        /// Removes alll songs from the list.
        /// </summary>
        public void cleanUp(){

        }
        /*
        /// <summary>
        /// adds song to the playlist.
        /// </summary>
        private void addSong(Song newSong){
            this.Rows.Add(newSong.Picture, newSong);
        }

        public void addSong(YoutubeSong newSong)
        {
            Rows.Add(TYoutube, newSong);
        }

        public void addSong(SpotifySong newSong)
        {
            Rows.Add(TSpotify, newSong);
        }*/
    }
}
