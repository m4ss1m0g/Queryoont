using System.Data;
using System.Data.SQLite;
using Microsoft.Extensions.DependencyInjection;
using Queryoont.Extensions;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Queryoont.Api.Configurations
{
    public static class ServicesConfigurations
    {
        private static readonly IDbConnection _connection = new SQLiteConnection("Data Source=:memory:");

        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            _connection.Open();
            DatabaseConfigurations.InitializedDb(_connection);
            services.AddSingleton<IDbConnection>((s) => _connection);
            services.AddSingleton<QueryFactory>((s) =>
            {
                var compiler = new SqliteCompiler();
                var conn = s.GetService<IDbConnection>();
                return new QueryFactory(conn, compiler);
            });

            return services;
        }

        public static IServiceCollection AddLibs(this IServiceCollection services)
        {
            services.AddDatabase();
            services.AddQueryoont();
            return services;
        }
    }
}