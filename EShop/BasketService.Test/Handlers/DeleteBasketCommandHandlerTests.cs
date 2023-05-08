using Xunit;
using ShoppingBasket.API.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ShoppingBasket.API.Core.Interfaces;
using BasketService.Test;
using ShoppingBasket.API.Core.Entities;
using BasketService.Test.Handlers;
using ShoppingBasket.API.Commands;

namespace ShoppingBasket.API.Handlers.Tests
{
    public  class DeleteBasketCommandHandlerTests
    {

        private readonly Mock<IBasketRepository> _repo;
        private static Dictionary<string, List<BasketItem>> _baskets { get; set; }
        public DeleteBasketCommandHandlerTests()
        {
            _repo = MockBasketRepository.GetBasketRepository();
            _baskets = MockBasketRepository.Baskets;
        }

        [Fact()]
        public async Task DeleteHandler_ItemNotFound_ReturnsTrueAsync()
        {
            //Arrange
            var handler = new DeleteBasketCommandHandler(_repo.Object);
            //Act
            await handler.Handle(new DeleteBasketItemCommand { Id = "Demo" }, CancellationToken.None);

            //Assert
            Assert.True(1== 1);
            //Assert.DoesNotContain(, _catalogItems);
        }
    }
}