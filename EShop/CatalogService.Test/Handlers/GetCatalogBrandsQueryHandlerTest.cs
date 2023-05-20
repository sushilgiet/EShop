using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Handlers;
using ProductCatalog.Application.Queries;

namespace CatalogService.Test.Handlers
{
    public class GetCatalogBrandsQueryHandlerTest
    {
        private readonly Mock<ICatalogBrandRepository> _repo;
        private readonly Mock<ILogger<GetCatalogBrandsQueryHandler>> _logger;
        public GetCatalogBrandsQueryHandlerTest()
        {
            _repo = MockCatalogBrandRepository.GetCatalogBrandRepository();
            _logger = new Mock<ILogger<GetCatalogBrandsQueryHandler>>();
        }
        [Fact]
        public async Task GetCatalogBrands_BrandCountEqThree_ReturnsTrue()
        {
            var handler = new GetCatalogBrandsQueryHandler(_repo.Object,_logger.Object);
            var results = await handler.Handle(new GetCatalogBrandsQuery(), CancellationToken.None);
            Assert.True(results.Any());
            Assert.True(results.Count() == 3);
        }
    }
   
}
