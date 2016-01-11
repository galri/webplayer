﻿using Infrastructure.Models;
using SpotifyAPI.Local.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Webplayer.Modules.Spotify.Models
{
    class SpotifySong : BaseSong
    {
        private Uri myUri;

        public SpotifySong(string tittel, BitmapImage picture, TimeSpan length, Uri spotifyRessource)
            : base(tittel, picture, length)
        {
            Uri = spotifyRessource;
        }

        public Uri Uri { get { return myUri; } set { myUri = value; } }

        public override string ToString()
        {
            return Tittel;
        }
        // override object.Equals
        public override bool Equals(object obj)
        {
            //       
            // See the full list of guidelines at
            //   http://go.microsoft.com/fwlink/?LinkID=85237  
            // and also the guidance for operator== at
            //   http://go.microsoft.com/fwlink/?LinkId=85238
            //

            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            // TODO: write your implementation of Equals() here
            return base.Equals(obj);
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            // TODO: write your implementation of GetHashCode() here
            return base.GetHashCode();
        }
    }
}