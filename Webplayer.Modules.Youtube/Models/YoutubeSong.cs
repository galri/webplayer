using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Webplayer.Modules.Youtube.Models
{
    class YoutubeSong : BaseSong 
    {
        private string myVideoID;
        private bool myEmbeddable;
        private string myDescription;

        public string VideoID
        {
            get { return myVideoID; }
            set { myVideoID = value; }
        }

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

        public Uri Uri
        {
            get
            {
                return new Uri("https://www.youtube.com/watch?v=" + VideoID);
            }
        }

        public YoutubeSong(BitmapImage p, string title, string videoId, TimeSpan t)
            : base(title, p, t)
        {
            VideoID = videoId;
        }

        public override string ToString()
        {
            return base.Title;
        }
    }
}
