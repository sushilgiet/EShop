using Moq;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain.Entities;

namespace CatalogService.Test.Handlers
{
    public static class MockCatalogTypeRepository
    {
        public static List<CatalogType> CatalogTypes { get; set; }
        static MockCatalogTypeRepository()
        {
            CatalogTypes = new List<CatalogType>()
            {
            new CatalogType() { Type = "Running"},
            new CatalogType() { Type = "Basketball" },
            new CatalogType() { Type = "Tennis" },
            };
        }
        public static Mock<ICatalogTypeRepository> GetCatalogTypeRepository()
        {

            var mockrepo = new Mock<ICatalogTypeRepository>();
            mockrepo.Setup(r => r.GetAll()).ReturnsAsync(CatalogTypes);
            return mockrepo;
        }
    }
}
