using Xunit;
using ShoppingBasket.API.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasketService.Test.Handlers;
using Moq;
using ShoppingBasket.API.Core.Interfaces;
using ShoppingBasket.API.Handlers.Tests;
using ShoppingBasket.API.Core.Entities;
using ShoppingBasket.API.Commands;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using ShoppingBasket.API.Core.Aggregates;

namespace ShoppingBasket.API.Handlers.Tests
{
    public class UpdateBasketCommandHandlerTests
    {
        private readonly Mock<IBasketRepository> _repo;
        private Dictionary<string, List<BasketItem>> _baskets;
        public UpdateBasketCommandHandlerTests()
        {
            _repo = MockBasketRepository.GetBasketRepository();
            _baskets = MockBasketRepository.Baskets;
        }

        [Fact()]
        public async Task HandleTestAsync()
        {
           var result= _baskets.GetValueOrDefault("User1").ToList();

            Basket basket = new Basket()
            {
                BuyerId = "User1",
                Items = result
            };
            UpdateBasketItemCommand command = new UpdateBasketItemCommand
            {
                Basketdetails = basket

            };
            var handler = new UpdateBasketCommandHandler(_repo.Object);
            var basketitem=await handler.Handle(command, CancellationToken.None);
            Assert.True(basket.BuyerId== basketitem.BuyerId);
        }
    }

    
 }
   




