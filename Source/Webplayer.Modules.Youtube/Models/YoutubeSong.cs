using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Infrastructure.Names;
using System.Data.SQLite;
using System.Data.SQLite.Linq;
using System.ComponentModel.DataAnnotations.Schema;

namespace Webplayer.Modules.Youtube.Models
{
    [Table("YoutubeSong")]
    public class YoutubeSong : BaseSong 
    {
        [Key]
        [Column("songid")]
        public string VideoId { get; set; }

        public string Description { get; set; }

        public bool Embeddable { get; set; }

        public Uri Uri => new Uri("https://www.youtube.com/watch?v=" + VideoId);

        public YoutubeSong(Uri p, string title, string videoId, TimeSpan t)
            : base(title, p, t)
        {
            VideoId = videoId;
        }

        public YoutubeSong() : base ()
        {
        }

        public override string ToString()
        {
            return base.Title;
        }
    }
}
