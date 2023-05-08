using Moq;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus;
using Azure.Core;

namespace CatalogService.Test.Handlers
{
    public static class MockCatalogItemRepository
    {
        public static List<CatalogItem> CatalogItems { get; set; }
        static MockCatalogItemRepository()
        {
            CatalogItems = new List<CatalogItem>()
            {
                new CatalogItem("World Star", "Shoes for next century",199.5M,"http://storage/api/pic/1"," ",2,3,1),
                 new CatalogItem("White Line", "Will make you world champions",109.5M,"http://storage/api/pic/2"," ",1,3,2),
                  new CatalogItem("Prism White Shoes", "Shoes for next century",178.5M,"http://storage/api/pic/3"," ",1,1,3),
                   new CatalogItem("Olympic runner", "Shoes for next century",189.5M,"http://storage/api/pic/4"," ",1,3,4),
                    new CatalogItem("Roslyn Red Sheet", "You have already won gold medal",100.5M,"http://storage/api/pic/5"," ",2,2,5),
                       new CatalogItem("Paris Blues", "Rolan Garros",99.5M,"http://storage/api/pic/6"," ",3,2,6)
                //    new CatalogItem { CatalogTypeId=2,CatalogBrandId=3, Description = "Shoes for next century", Name = "World Star", Price = 199.5M, PictureUrl = "http://storage/api/pic/1",PictureFileName=string.Empty,Id=1 },
                //    new CatalogItem { CatalogTypeId=1,CatalogBrandId=2, Description = "Will make you world champions", Name = "White Line", Price= 88.50M, PictureUrl = "http://storage/api/pic/2" ,PictureFileName=string.Empty,Id=2},
                //    new CatalogItem{ CatalogTypeId=2,CatalogBrandId=3, Description = "You have already won gold medal", Name = "Prism White Shoes", Price = 129, PictureUrl = "http://storage/api/pic/3",PictureFileName=string.Empty,Id=3 },
                //    new CatalogItem { CatalogTypeId=2,CatalogBrandId=2, Description = "Olympic runner", Name = "Foundation Hitech", Price = 12, PictureUrl = "http://storage/api/pic/4",PictureFileName=string.Empty , Id=4},
                //    new CatalogItem { CatalogTypeId=2,CatalogBrandId=1, Description = "Roslyn Red Sheet", Name = "Roslyn White", Price = 188.5M, PictureUrl = "http://storage/api/pic/5",PictureFileName=string.Empty,Id=5 },
                //    new CatalogItem { CatalogTypeId=3,CatalogBrandId=1, Description = "Rolan Garros", Name = "Paris Blues", Price = 312, PictureUrl = "http://storage/api/pic/15",PictureFileName=string.Empty,Id=6 }
            };
        }
        public static Mock<ICatalogItemRepository> GetCatalogItemRepository()
        {

            var mockrepo = new Mock<ICatalogItemRepository>();
            mockrepo.Setup(r => r.GetAll()).ReturnsAsync(CatalogItems);
            mockrepo.Setup(r => r.Add(It.IsAny<CatalogItem>())).ReturnsAsync(
               (CatalogItem item) =>
               {
                   CatalogItems.Add(item);
                   return item;
               });
            mockrepo.Setup(r => r.Delete(It.IsAny<int>())).Callback<int>(
                c =>
                {

                    CatalogItems.RemoveAll(t => t.Id == c);
                    Thread.Sleep(1000);


                }
                );
            mockrepo.Setup(r => r.Update(It.IsAny<CatalogItem>())).Callback<CatalogItem>(
                c =>
                {
                    var index = CatalogItems.FindIndex(t => t.Id == c.Id);
                    CatalogItems.RemoveAll(t => t.Id == c.Id);
                    CatalogItems.Insert(index, c);

                }
                );
            mockrepo.Setup(r => r.GetById(It.IsAny<int>())).ReturnsAsync(
               (int id) =>
              {
                  return CatalogItems.Where(t => t.Id == id).FirstOrDefault();

              });
            return mockrepo;
        }
    }
}
