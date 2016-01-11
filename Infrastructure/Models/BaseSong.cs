using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Infrastructure.Models
{
    public class BaseSong
    {
        private string myTittel;
        private string myAuthor;
        private BitmapImage myPicture;
        private TimeSpan myLength;

        public BaseSong(string tittel, BitmapImage picture, TimeSpan length)
        {
            Tittel = tittel;
            Picture = picture;
            myLength = length;
        }

        public string Tittel
        {
            get
            {
                return myTittel;
            }
            set
            {
                myTittel = value;
            }
        }

        public BitmapImage Picture
        {
            get
            {
                return myPicture;
            }
            set
            {
                myPicture = value;
            }
        }

        public TimeSpan Length
        {
            get
            {
                return myLength;
            }
            set
            {
                myLength = value;
            }
        }

        public string Author
        {
            get
            {
                return myAuthor;
            }
            set
            {
                myAuthor = value;
            }
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
            BaseSong other = (BaseSong)obj;
            if (Tittel != other.Tittel)
            {
                return false;
            }

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
