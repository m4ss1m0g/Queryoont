using System;
using System.Data;
using Dapper;

namespace Queryoont.Api.Configurations
{
    public class DatabaseConfigurations
    {
        public static void InitializedDb(IDbConnection connection)
        {
            connection.Execute("ATTACH DATABASE ':memory:' AS db");
            connection.Execute(Customers);
            connection.Execute(CustomerTypes);
            FillData(connection);
            // connection.Close();
        }

        private static void FillData(IDbConnection connection)
        {
            connection.Execute("INSERT INTO [db].CustomerTypes (Id, Description) VALUES (1, 'Type 1')");
            connection.Execute("INSERT INTO [db].CustomerTypes (Id, Description) VALUES (2, 'Type 2')");

            connection.Execute("INSERT INTO [db].Customers (Id, Name, CustomerId) VALUES (1, 'Customer 1', 1)");
            connection.Execute("INSERT INTO [db].Customers (Id, Name, CustomerId) VALUES (2, 'Customer 2', 1)");
            connection.Execute("INSERT INTO [db].Customers (Id, Name, CustomerId) VALUES (3, 'Customer 3', 2)");
            connection.Execute("INSERT INTO [db].Customers (Id, Name, CustomerId) VALUES (4, 'Customer 4', 2)");
            connection.Execute("INSERT INTO [db].Customers (Id, Name, CustomerId) VALUES (5, 'Customer 5', 2)");
        }

        private static string Customers => @"
            CREATE TABLE [db].[Customers] (
                  [Id] INTEGER PRIMARY KEY NOT NULL
                , [Name] varchar(100) NOT NULL COLLATE NOCASE
                , [CustomerId] INTEGER
                ); 
        ";

        private static string CustomerTypes => @"
            CREATE TABLE [db].[CustomerTypes] (
                  [Id] INTEGER PRIMARY KEY NOT NULL
                , [Description] varchar(100) NOT NULL COLLATE NOCASE
                ); 
        ";

    }
}