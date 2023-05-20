using Microsoft.Extensions.Logging;
using Moq;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Handlers;
using ProductCatalog.Application.Queries;

namespace CatalogService.Test.Handlers
{
    public class GetCatalogItemQueryHandlerTest
    {
        private readonly Mock<ICatalogItemRepository> _repo;
        private readonly Mock<ILogger<GetCatalogBrandsQueryHandler>> _logger;
        public GetCatalogItemQueryHandlerTest()
        {
            _repo = MockCatalogItemRepository.GetCatalogItemRepository();
            _logger = new Mock<ILogger<GetCatalogBrandsQueryHandler>>();
        }
        [Theory]
        [InlineData(3, true)]
        [InlineData(4, true)]
        public async void GetCatalogItem_ItemHasReqId_ReturnsTrue(int id, bool expectedresult)
        {
            var handler = new GetCatalogItemQueryHandler(_repo.Object, _logger.Object);
            var item = await handler.Handle(new GetCatalogItemQuery { Id = id }, CancellationToken.None);
            Assert.True(item != null);
            Assert.Equal(item.Id == id, expectedresult);
        }



    }
   
}
