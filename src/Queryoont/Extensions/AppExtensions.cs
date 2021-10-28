using System;
using Microsoft.Extensions.DependencyInjection;
using Queryoont.Infrastructure;
using Queryoont.Serialization;

namespace Queryoont.Extensions
{
    public static class AppExtensions
    {
        public static IServiceCollection AddQueryoont(this IServiceCollection services)
        {
            services.AddServices();
            services.AddSingleton<IJsonSerializer, JsonSerializer>();
            return services;
        }

        public static IServiceCollection AddQueryoont(this IServiceCollection services, IJsonSerializer serializer)
        {
            services.AddServices();
            services.AddSingleton(serializer);
            return services;
        }

        public static IServiceCollection AddQueryoont(this IServiceCollection services, Func<IJsonSerializer> serializer)
        {
            services.AddServices();
            services.AddSingleton(serializer);
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IQueryExecution, QueryExecution>();
            return services;
        }

    }
}