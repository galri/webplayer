using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Youtube.Models;

namespace Webplayer.Modules.Youtube.Services
{
    interface IYoutubeSongSearchService : ISearchService<YoutubeSong>
    {
        string UploaderId { get; set; }

        SongSearcOrdering Ordering { get; set; }
    }

    public enum SongSearcOrdering
    {
        Relevance,
        Date,
        Rating,
        Alphabetical,
        ViewCount,
    }
}
