using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Queryoont.Infrastructure;
using Queryoont.Middleware;

namespace Queryoont.Extensions
{
    public static class AppExtensions
    {
        public static IApplicationBuilder UseQueryoont(this IApplicationBuilder app)
        {
            app.UseMiddleware<SqlQuery>();
            return app;
        }

        public static IServiceCollection AddQueryoont(this IServiceCollection services)
        {
            services.AddTransient<IQueryHandler, QueryHandler>();
            return services;
        }
    }
}