using Xunit;
using ShoppingBasket.API.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ShoppingBasket.API.Core.Interfaces;
using BasketService.Test.Handlers;
using ShoppingBasket.API.Core.Entities;
using ShoppingBasket.API.Core.Aggregates;
using ShoppingBasket.API.Queries;

namespace ShoppingBasket.API.Handlers.Tests
{
    public class GetBasketQueryHandlerTests
    {
        private readonly Mock<IBasketRepository> _repo;
        private Dictionary<string, List<BasketItem>> _baskets;

     
        public GetBasketQueryHandlerTests()
        {
            _repo = MockBasketRepository.GetBasketRepository();
            _baskets = MockBasketRepository.Baskets;
        }
        [Fact()]
        public async Task GetBasketItem_ContainsId_ReturnsTrue()
        {
            var handler = new GetBasketQueryHandler(_repo.Object);
            var results = await handler.Handle(new GetBasketQuery { Id= "User1" }, CancellationToken.None);
            Assert.True(results.BuyerId== "User1");
        }
    }
}