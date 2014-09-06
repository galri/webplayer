using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Webplayer.Services.Youtube
{
    class YoutubeSong : Song
    {
        private string myVideoID;
        public string VideoID { get { return myVideoID; } set { myVideoID = value; } }
        private string myDescription;
        public string Description
        {
            get
            {
                return myDescription;
            }
            set
            {
                myDescription = value;
            }
        }
        private bool myEmbeddable;
        public bool Embeddable
        {
            get
            {
                return myEmbeddable;
            }
            set
            {
                myEmbeddable = value;
            }
        }

        public Uri Uri { get { return new Uri("https://www.youtube.com/watch?v=" + VideoID); } }

        /*public YoutubeSong(BitmapImage p, Video v, TimeSpan t)
            : base(v.Title, p, t)
        {
            //TODO: legger til helle urien=
            myVideoID = v.WatchPage.AbsoluteUri;
        }*/
        public YoutubeSong(BitmapImage p, string title, string videoId, TimeSpan t)
            : base(title, p, t)
        {
            VideoID = videoId;
        }

        public override string ToString()
        {
            return base.Tittel;
        }
    }
}
