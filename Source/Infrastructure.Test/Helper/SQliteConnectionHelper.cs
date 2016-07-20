using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Tests.Integration.Properties;

namespace Infrastructure.IntegrationTests.Helper
{
    class SQliteConnectionHelper
    {
        public static SQLiteConnection CreateMemory()
        {
            //inmemory https://www.connectionstrings.com/sqlite/
            const string connectionString = "Data Source=:memory:;Version=3;New=True;";

            var conn = new SQLiteConnection(connectionString);

            conn.Open();
            var createTable = conn.CreateCommand();
            createTable.CommandText = "Create table [test] (value nchar(5), id integer primary key)";
            createTable.ExecuteNonQuery();

            return conn;
        }

        internal static SQLiteConnection CreateMemoryPlaylistTable()
        {
            //inmemory https://www.connectionstrings.com/sqlite/
            const string connectionString = "Data Source=:memory:;Version=3;New=True;";

            var conn = new SQLiteConnection(connectionString);

            conn.Open();
            var createTable = conn.CreateCommand();
            var createTableText = Resources.test;
            createTable.CommandText = createTableText;
            createTable.ExecuteNonQuery();

            return conn;
        }

        public static SQLiteConnection CreateFile()
        {
            //file
            const string connectionString = 
            @"Data Source=C:\Users\sns.B19\Documents\webplayer\Source\webplayer.db;Version=3;";

            var conn = new SQLiteConnection(connectionString);

            conn.Open();
            var createTable = conn.CreateCommand();
            createTable.CommandText = "Create table [test] (value nchar(5), id integer primary key)";
            createTable.ExecuteNonQuery();

            return conn;
        }
    }
}
