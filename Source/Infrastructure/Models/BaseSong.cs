using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Infrastructure.Models
{
    public abstract class BaseSong
    {
        protected BaseSong()
        {
            
        }

        protected BaseSong(string tittel, BitmapImage picture, TimeSpan length)
        {
            Title = tittel;
            Picture = picture;
            Length = length;
        }

        public string Title { get; set; }

        public BitmapImage Picture { get; set; }

        public TimeSpan Length { get; set; }

        public string Artist { get; set; }

        public int Order { get; set; }
        

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
            if (Title != other.Title)
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
