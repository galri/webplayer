using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tests.Integration.Helper
{
    public static class CommandExtensions
    {
        public static void InsertTestData(this SQLiteConnection conn, string sqlInsert)
        {
            var command = conn.CreateCommand();
            command.CommandText = sqlInsert;
            command.ExecuteScalar();
        }
    }
}
