using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProductCatalog.Application.Contracts.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Persistance
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection ConfigurePersistenceServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ProductCatalogContext>(options =>
                 options.UseSqlServer(
                     configuration.GetConnectionString("ProductCatalogContext")));
            services.AddTransient<ICatalogItemRepository, CatalogItemRepository>();
            services.AddTransient<ICatalogTypeRepository, CatalogTypeRepository>();
            services.AddTransient<ICatalogBrandRepository, CatalogBrandRepository>();
            return services;
        }
    }
}
