using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncThumbnailGen
{
    public class ProductCatalogContext : DbContext
    {
        public ProductCatalogContext(DbContextOptions<ProductCatalogContext> options)
            : base(options)
        {
        }

        public DbSet<ProductCatalog.Domain.CatalogType> CatalogTypes { get; set; }

        public DbSet<ProductCatalog.Domain.CatalogBrand> CatalogBrands { get; set; }

        public DbSet<ProductCatalog.Domain.CatalogItem> CatalogItems { get; set; }
    }
}
