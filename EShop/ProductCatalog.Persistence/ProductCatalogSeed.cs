﻿using ProductCatalog.Domain.Entities;
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
            return new List<CatalogItem>()
            {
            //    new CatalogItem() { CatalogTypeId=2,CatalogBrandId=3, Description = "Shoes for next century", Name = "World Star", Price = 199.5M, PictureUrl = "http://storage/api/pic/1",PictureFileName=string.Empty },
            //    new CatalogItem() { CatalogTypeId=1,CatalogBrandId=2, Description = "Will make you world champions", Name = "White Line", Price= 88.50M, PictureUrl = "http://storage/api/pic/2" ,PictureFileName=string.Empty},
            //    new CatalogItem() { CatalogTypeId=2,CatalogBrandId=3, Description = "You have already won gold medal", Name = "Prism White Shoes", Price = 129, PictureUrl = "http://storage/api/pic/3",PictureFileName=string.Empty },
            //    new CatalogItem() { CatalogTypeId=2,CatalogBrandId=2, Description = "Olympic runner", Name = "Foundation Hitech", Price = 12, PictureUrl = "http://storage/api/pic/4",PictureFileName=string.Empty },
            //    new CatalogItem() { CatalogTypeId=2,CatalogBrandId=1, Description = "Roslyn Red Sheet", Name = "Roslyn White", Price = 188.5M, PictureUrl = "http://storage/api/pic/5",PictureFileName=string.Empty },
            //    new CatalogItem() { CatalogTypeId=3,CatalogBrandId=1, Description = "Rolan Garros", Name = "Paris Blues", Price = 312, PictureUrl = "http://storage/api/pic/15",PictureFileName=string.Empty }
            };
        }
    }
}