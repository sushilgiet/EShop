using Moq;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Handlers;
using ProductCatalog.Application.Queries;

namespace CatalogService.Test.Handlers
{
    public class GetCatalogBrandsQueryHandlerTest
    {
        private readonly Mock<ICatalogBrandRepository> _repo;

        public GetCatalogBrandsQueryHandlerTest()
        {
            _repo = MockCatalogBrandRepository.GetCatalogBrandRepository();
        }
        [Fact]
        public async Task GetCatalogBrands_BrandCountEqThree_ReturnsTrue()
        {
            var handler = new GetCatalogBrandsQueryHandler(_repo.Object);
            var results = await handler.Handle(new GetCatalogBrandsQuery(), CancellationToken.None);
            Assert.True(results.Any());
            Assert.True(results.Count() == 3);
        }
    }
   
}
