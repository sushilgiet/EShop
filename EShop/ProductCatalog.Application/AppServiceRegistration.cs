using EventBus.Abstractions;
using EventBus.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductCatalog.Application.Contracts;

using MediatR;
using System.Reflection;
using Microsoft.Azure.ServiceBus.Core;

namespace ProductCatalog.Application
{
    public static class AppServiceRegistration
    {
        public static IServiceCollection ConfigureApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
           
            services.AddSingleton<IEventBus>(x =>
            {
                return new AzureEventBus(configuration["ServiceBusConnectionString"], configuration["TopicName"]);
            });
           
            services.AddMediatR(Assembly.GetExecutingAssembly());
            return services;
        }
    }
}
