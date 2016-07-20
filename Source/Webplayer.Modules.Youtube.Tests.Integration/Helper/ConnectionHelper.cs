using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webplayer.Modules.Youtube.Tests.Integration.Properties;

namespace Webplayer.Modules.Youtube.Tests.Integration.Helper
{
    class ConnectionHelper
    {
        public static SQLiteConnection CreateMemoryTable()
        {
            //inmemory https://www.connectionstrings.com/sqlite/
            const string connectionString = "Data Source=:memory:;Version=3;New=True;";

            var conn = new SQLiteConnection(connectionString);

            conn.Open();
            var createTable = conn.CreateCommand();
            var createText = Resources.CreateYoutubeSongTable;
            createTable.CommandText = createText;
            createTable.ExecuteNonQuery();

            return conn;
        }
    }
}
