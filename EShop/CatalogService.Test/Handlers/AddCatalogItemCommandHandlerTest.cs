using Moq;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Handlers;
using ProductCatalog.Domain.Entities;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatalogService.Test.Handlers
{
    public class AddCatalogItemCommandHandlerTest
    {
        private readonly Mock<ICatalogItemRepository> _repo;
        public AddCatalogItemCommandHandlerTest()
        {
            _repo = MockCatalogItemRepository.GetCatalogItemRepository();
        }
        [Fact]
        public async Task AddCatalogItem_IsCatalogItemAdded_ReturnTrue()
        {
            var catalogItem = new CatalogItem ("Paris Blues","Rolan Garros" ,312,"http://storage/api/pic/15", string.Empty,3, 1);
            var command = new AddCatalogItemCommand
            {
                Name = catalogItem.Name,
                Price = catalogItem.Price,
                CatalogBrandId = catalogItem.CatalogBrandId,
                CatalogTypeId = catalogItem.CatalogTypeId,
                Description = catalogItem.Description,
                PictureFileName = catalogItem.PictureFileName,
                PictureUrl = catalogItem.PictureUrl
            };

            var handler = new AddCatalogItemCommandHandler(_repo.Object);
            var addedItem = await handler.Handle(command, CancellationToken.None);

            Assert.NotNull(addedItem);
            Assert.Contains(addedItem, MockCatalogItemRepository.CatalogItems);
        }
    }
   
}
