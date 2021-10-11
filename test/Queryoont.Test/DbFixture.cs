using System;
using System.Data.SQLite;
using Dapper;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Queryoont.Test
{
    public class DbFixture : IDisposable
    {
        protected QueryFactory Db { get; }

        public DbFixture()
        {
            Db = GetDb();
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Db.Dispose();
        }

        protected static QueryFactory GetDb()
        {
            var connection = new SQLiteConnection("Data Source=:memory:");

            connection.Open();
            // connection.Execute("ATTACH DATABASE ':memory:' AS commons");
            connection.Execute("ATTACH DATABASE ':memory:' AS DB");
            connection.Execute(TableDDL);

            return new QueryFactory(connection, new SqliteCompiler());
        }

        private static string TableDDL => @"
                CREATE TABLE [Table1] (
                  [Id] INTEGER PRIMARY KEY NOT NULL
                , [Name] nvarchar(255) NOT NULL COLLATE NOCASE
                );        
            ";


    }
}