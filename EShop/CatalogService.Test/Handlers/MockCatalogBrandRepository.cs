using Moq;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain.Entities;

namespace CatalogService.Test.Handlers
{
    public static class MockCatalogBrandRepository
    {
        public static List<CatalogBrand> CatalogBrands { get; set; }
        static MockCatalogBrandRepository()
        {
            CatalogBrands = new List<CatalogBrand>()
            {
                new CatalogBrand() { Brand = "Addidas"},
                new CatalogBrand() { Brand = "Puma" },
                new CatalogBrand() { Brand = "Nike" }
            };
        }
        public static Mock<ICatalogBrandRepository> GetCatalogBrandRepository()
        {

            var mockrepo = new Mock<ICatalogBrandRepository>();
            mockrepo.Setup(r => r.GetAll()).ReturnsAsync(CatalogBrands);
            return mockrepo;
        }
    }
}
