using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure;
using Infrastructure.Models;
using Infrastructure.Service;
using Webplayer.Modules.Youtube.Models;

namespace Webplayer.Modules.Youtube.Services
{
    /// <summary>
    /// Loads and saves songs to sqlite db.
    /// </summary>
    public class YoutubePlaylistService : ISongServicePlaylistSaver
    {
        private readonly SQLiteConnection _c;
        public const string YoutubeTableName = "YoutubeSong";
        private const string PlaylistIdColumnName = "playlistid";
        private const string SongTitleColumnName = "title";

        public string SongService { get; } = "";

        public YoutubePlaylistService(SQLiteConnection c)
        {
            _c = c;
        }

        public void SaveSong(BaseSong song, int playlistNr, int playlistId)
        {
            throw new NotImplementedException();
            //var youtubeSong = ToYoutubeSong(song);
            //var param = new Dictionary<string, string>()
            //{
            //    {"@id", youtubeSong.VideoId}
            //};
            //var sql = $"(songid, playlistid, playlistnr) " +
            //          $"values({youtubeSong.VideoId}, {youtubeSong.PlaylistId}, {playlistNr})";


            //_dbService.Replace<YoutubeSong>(Table,sql,new Dictionary<string, string>());
        }

        public BaseSong LoadSong(string id)
        {
            throw new NotImplementedException();
            //var param = new Dictionary<string, string>()
            //{
            //    {"@id", id}
            //};

            //var items = _dbService.Select<YoutubeSong>(Table, "id = @id", param,Convert);

            //return items.First();
        }

        public List<BaseSong> LoadSongsBelongingToPlaylist(int playlistId)
        {
            var res = new List<BaseSong>();

            var command = _c.CreateCommand();
            command.CommandText = $"Select * from {YoutubeTableName} " +
                                  $"where {PlaylistIdColumnName} == " +
                                  $"@playlistid";
            command.Parameters.Add(new SQLiteParameter("@playlistid", playlistId));

            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var song = Convert(reader);
                res.Add(song);
            }

            return res;
        }

        private YoutubeSong Convert(DbDataReader reader)
        {
            return new YoutubeSong()
            {
                VideoId = (string)reader["songid"],
                PlaylistId = ((int)reader[PlaylistIdColumnName]).ToString(),
                Order = (int)reader["playlistnr"],
                Title = (string)reader[SongTitleColumnName],
            };
        }
        
        public bool CanSave(BaseSong song)
        {
            return song is YoutubeSong;
        }

        public YoutubeSong ToYoutubeSong(BaseSong song)
        {
            var res = song as YoutubeSong;
            if(res == null)
                throw new ArgumentException("This service can only work with youtube songs.");

            return res;
        }
    }
}
