using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Handlers;
using ProductCatalog.Application.Queries;

namespace CatalogService.Test.Handlers
{
    public class GetCatalogTypesQueryHandlerTest
    {
        private readonly Mock<ICatalogTypeRepository> _repo;
        private readonly Mock<ILogger<GetCatalogTypesQueryHandler>> _logger;
        public GetCatalogTypesQueryHandlerTest()
        {
            _repo = MockCatalogTypeRepository.GetCatalogTypeRepository();
            _logger = new Mock<ILogger<GetCatalogTypesQueryHandler>>();
        }
        [Fact]
        public async Task GetCatalogTypes_CountEqThree_ReturnsTrue()
        {
            var handler = new GetCatalogTypesQueryHandler(_repo.Object,_logger.Object);
            var results = await handler.Handle(new GetCatalogTypesQuery(), CancellationToken.None);
            Assert.True(results.Any());
            Assert.True(results.Count() == 3);
        }
    }
   
}
