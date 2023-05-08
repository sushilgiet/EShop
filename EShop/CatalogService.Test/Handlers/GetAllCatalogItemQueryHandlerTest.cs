using Moq;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Handlers;
using ProductCatalog.Application.Queries;

namespace CatalogService.Test.Handlers
{
    public class GetAllCatalogItemQueryHandlerTest
    {
        private readonly Mock<ICatalogItemRepository> _repo;
        public GetAllCatalogItemQueryHandlerTest()
        {
            _repo = MockCatalogItemRepository.GetCatalogItemRepository();
        }
        [Fact]
        public async Task GetAllCatalogItem_ContainsItems_ReturnsTrue()
        {
            var handler = new GetAllCatalogItemQueryHandler(_repo.Object);
            var results = await handler.Handle(new GetAllCatalogItemQuery(), CancellationToken.None);
            Assert.True(results.Any());
        }
    }
   
}
