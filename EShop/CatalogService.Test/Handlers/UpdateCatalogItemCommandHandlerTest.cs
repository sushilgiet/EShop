using Moq;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Handlers;

namespace CatalogService.Test.Handlers
{
    public class UpdateCatalogItemCommandHandlerTest
    {
        private readonly Mock<ICatalogItemRepository> _repo;
        public UpdateCatalogItemCommandHandlerTest()
        {
            _repo = MockCatalogItemRepository.GetCatalogItemRepository();
        }
        [Fact]
        public async void UpdateCatalogItem_IsUpdatedItemNotNull_ReturnsTrue()
        {
            //Arrange
            var catalogItem = MockCatalogItemRepository.CatalogItems.FirstOrDefault();
            var eventbus = MockEventBus.GetEventBus();
            var handler = new UpdateCatalogItemCommandHandler(_repo.Object, eventbus.Object);
            //Act
            await handler.Handle(new ProductCatalog.Application.Commands.UpdateCatalogItemCommand {
                Name = catalogItem.Name,
                Price = catalogItem.Price,
                CatalogBrandId = catalogItem.CatalogBrandId,
                CatalogTypeId = catalogItem.CatalogTypeId,
                Description = catalogItem.Description,
                PictureFileName = catalogItem.PictureFileName,
                PictureUrl = catalogItem.PictureUrl,
                Id = catalogItem.Id

            }, CancellationToken.None);
            //Assert
            Assert.True(catalogItem != null);
        }
    }
   
}
