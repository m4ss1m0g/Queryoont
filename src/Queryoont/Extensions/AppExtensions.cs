using System;
using Microsoft.Extensions.DependencyInjection;
using Queryoont.Serialization;

namespace Queryoont.Extensions
{
    public static class AppExtensions
    {
        public static IServiceCollection AddQueryoont(this IServiceCollection services)
        {
            services.AddSingleton<IJsonSerializer, JsonSerializer>();
            return services;
        }

        public static IServiceCollection AddQueryoont(this IServiceCollection services, IJsonSerializer serializer)
        {
            services.AddSingleton(serializer);
            return services;
        }

        public static IServiceCollection AddQueryoont(this IServiceCollection services, Func<IJsonSerializer> serializer)
        {
            services.AddSingleton(serializer);
            return services;
        }

    }
}