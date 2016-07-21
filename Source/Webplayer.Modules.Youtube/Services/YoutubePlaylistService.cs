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
using Prism.Logging;
using Webplayer.Modules.Youtube.Models;

namespace Webplayer.Modules.Youtube.Services
{
    /// <summary>
    /// Loads and saves songs to sqlite db.
    /// </summary>
    public class YoutubePlaylistService : ISongServicePlaylistSaver
    {
        private readonly SQLiteConnection _c;
        private readonly ILoggerFacade _logger;
        public const string YoutubeTableName = "YoutubeSong";
        private const string PlaylistIdColumnName = "playlistid";
        private const string PlaylistNrColumnName = "playlistnr";
        private const string SongIdColumnName = "songid";
        private const string SongTitleColumnName = "title";

        public string SongService { get; } = "";

        public YoutubePlaylistService(SQLiteConnection c, ILoggerFacade logger)
        {
            _c = c;
            _logger = logger;
        }

        public void SaveSong(BaseSong song,Playlist playlist)
        {
            var ySong = ToYoutubeSong(song);
            var command = _c.CreateCommand();
            command.CommandText = $"replace into {YoutubeTableName} " +
                                  $"values (@sid,@nr,@pid,@title)";
            command.Parameters.Add(new SQLiteParameter("@sid", ySong.VideoId));
            command.Parameters.Add(new SQLiteParameter("@nr", ySong.PlaylistNr));
            command.Parameters.Add(new SQLiteParameter("@pid", playlist.Id));
            command.Parameters.Add(new SQLiteParameter("@title", ySong.Title));

            var affected = command.ExecuteNonQuery();
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

        public void RemoveSong(BaseSong song, Playlist playlist)
        {
            var ySong = ToYoutubeSong(song);

            using (var command = _c.CreateCommand())
            {
                command.CommandText = $"delete from {YoutubeTableName} where " +
                      $"{PlaylistIdColumnName} = @plid and " +
                      $"{PlaylistNrColumnName} = @plnr and " +
                      $"{SongIdColumnName} = @songid";
                command.Parameters.Add(new SQLiteParameter("@plid", playlist.Id));
                command.Parameters.Add(new SQLiteParameter("@plnr", ySong.PlaylistNr));
                command.Parameters.Add(new SQLiteParameter("@songid", ySong.VideoId));
                 
                var affected = command.ExecuteNonQuery();

                if (affected < 1)
                {
                    _logger.Log("No item deleted when trying to remoce youtube song from db",Category.Warn, Priority.High);
                }
            }
        }

        private YoutubeSong Convert(DbDataReader reader)
        {
            return new YoutubeSong()
            {
                VideoId = (string)reader[SongIdColumnName],
                PlaylistId = (string)reader[PlaylistIdColumnName],
                PlaylistNr = (int)reader[PlaylistNrColumnName],
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
