using ProductCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Persistance
{
    public class ProductCatalogSeed
    {
        public static async Task SeedAsync(ProductCatalogContext context)
        {
            if (!context.CatalogBrands.Any())
            {
                context.CatalogBrands.AddRange(GetPreconfiguredCatalogBrands());
                await context.SaveChangesAsync();
            }
            if (!context.CatalogTypes.Any())
            {
                context.CatalogTypes.AddRange(GetPreconfiguredCatalogTypes());
                await context.SaveChangesAsync();
            }

            if (!context.CatalogItems.Any())
            {
                context.CatalogItems.AddRange(GetPreconfiguredItems());
                await context.SaveChangesAsync();
            }
        }

        static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
        {
            return new List<CatalogBrand>()
            {
                new CatalogBrand() { Brand = "Addidas"},
                new CatalogBrand() { Brand = "Puma" },
                new CatalogBrand() { Brand = "Nike" }
            };
        }

        static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
        {
            return new List<CatalogType>()
        {
            new CatalogType() { Type = "Running"},
            new CatalogType() { Type = "Basketball" },
            new CatalogType() { Type = "Tennis" },
        };
        }
        static IEnumerable<CatalogItem> GetPreconfiguredItems()
        {
            // name, string description, decimal price, string pictureFileName, string pictureUrl, int catalogTypeId, int catalogBrandId,int id
            return new List<CatalogItem>()
            {
             new CatalogItem( "World Star","Shoes for next century",199.5M,string.Empty,"http://storage/api/pic/1",2,3),
              new CatalogItem( "White Line","Will make you world champions",99.5M,string.Empty,"http://storage/api/pic/2",1,2),
               new CatalogItem( "Prism White Shoes","You have already won gold medal",199.5M,string.Empty,"http://storage/api/pic/3",2,2),
                new CatalogItem( "Foundation Hitech","Olympic runner",80.5M,string.Empty,"http://storage/api/pic/4",2,1),
                 new CatalogItem( "Roslyn White","Roslyn Red Sheet",87.5M,string.Empty,"http://storage/api/pic/5",2,3),
                  new CatalogItem( "Paris Blues","Rolan Garros",86.5M,string.Empty,"http://storage/api/pic/6",3,1)
            };
        }
    }
}
