using Moq;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Handlers;
using ProductCatalog.Domain.Entities;

namespace CatalogService.Test.Handlers
{
    public class DeleteCatalogItemCommandHandlerTest
    {
        private readonly Mock<ICatalogItemRepository> _repo;
        private List<CatalogItem> _catalogItems;
        public DeleteCatalogItemCommandHandlerTest()
        {
            _repo = MockCatalogItemRepository.GetCatalogItemRepository();
            _catalogItems = MockCatalogItemRepository.CatalogItems;
        }
        [Fact]
        public async void DeleteCatalogItem_ItemNotFound_ReturnsTrue()
        {
            //Arrange
            var catalogItem = _catalogItems.Find(t=>t.Id==6);
            var handler = new DeleteCatalogItemCommandHandler(_repo.Object);
            int count = _catalogItems.Count();
            //Act
            await handler.Handle(new ProductCatalog.Application.Commands.DeleteCatalogItemCommand { Id = catalogItem.Id }, CancellationToken.None);
           
            //Assert
            Assert.True(catalogItem != null);
            Assert.DoesNotContain(catalogItem, _catalogItems);
        }
    }
   
}
