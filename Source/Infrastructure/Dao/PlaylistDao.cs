using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Dao
{
    public class PlaylistDao : IPlaylistDao
    {
        private const string TableName = "Playlist";
        private const string ColumnPlaylistName = "Name";
        private const string ColumnPlaylistId = "Id";

        private readonly SQLiteConnection _conn;

        public PlaylistDao(SQLiteConnection conn)
        {
            _conn = conn;
        }

        public string GetPlaylistIdFromName(string name)
        {
            var command = _conn.CreateCommand();
            command.CommandText = $"Select {ColumnPlaylistId} from {TableName} " +
                                  $"where {ColumnPlaylistName} == @name";
            command.Parameters.Add(new SQLiteParameter("@name", name));

            var id = command.ExecuteScalar();

            return (string)id;
        }

        public List<string> NameOfAllPlaylists()
        {
            var command = _conn.CreateCommand();
            command.CommandText = $"Select * from {TableName}";
            var reader = command.ExecuteReader();

            var playlistNames = new List<string>();
            while (reader.Read())
            {
                var name = (string)reader[ColumnPlaylistName];
                playlistNames.Add(name);
            }

            return playlistNames;
        }

        private int ToPlaylist(DbDataReader reader)
        {
            return (int)reader["id"];
        }
    }
}
