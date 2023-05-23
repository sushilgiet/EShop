using Xunit;
using ShoppingBasket.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using Microsoft.Extensions.Configuration;
using Azure;
using ShoppingBasket.API.Queries;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.API.Commands;
using ShoppingBasket.API.Core.Aggregates;
using BasketService.Test.Handlers;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using ShoppingBasket.API.Core.Entities;

namespace ShoppingBasket.API.Controllers.Tests
{
    public class BasketControllerTests
    {
        Mock<IMediator> _mockmediator;
        Mock<IConfiguration> _mockconfiguration;
        Mock<ILogger<BasketController>> _mocklogger;
       
        public  BasketControllerTests()
        {
            _mockmediator = new Mock<IMediator>();
            _mockconfiguration = new Mock<IConfiguration>();
            _mockconfiguration.Setup(x => x["MySetting"]).Returns("my value");
            _mocklogger = new Mock<ILogger<BasketController>>();
            
        }

        [Fact()]
        public async Task GetTestAsync()
        {
          
            _mockmediator.Setup(m => m.Send(It.IsAny<GetBasketQuery>(), CancellationToken.None)).Returns((GetBasketQuery query, CancellationToken token) =>
            {
                List<BasketItem> basketitems = MockBasketRepository.Baskets.GetValueOrDefault(query.Id);
                var basket= new Basket { BuyerId = query.Id, Items = basketitems };
                return Task<Basket>.FromResult(basket);
            }); ;
            var controller = new BasketController(_mockmediator.Object, _mocklogger.Object);
            var result = await controller.Get("User1");
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact()]
        public async Task PostAsyncTest()
        {
            Basket details = new Basket { BuyerId = "User3", Items = new List<BasketItem> { new BasketItem { Id = "1", ProductId = 1, UnitPrice = 199, Quantity = 2 }, new BasketItem { Id = "2", ProductId = 2, UnitPrice = 99, Quantity = 1 } } }
;

            _mockmediator.Setup(m => m.Send(It.IsAny<UpdateBasketItemCommand>(), CancellationToken.None)).Returns(Task.FromResult(details));
            var controller = new BasketController(_mockmediator.Object, _mocklogger.Object);
            var result = await controller.PostAsync(details);
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact()]
        public async Task DeleteAsync_Returns_NoContent()
        {
            _mockmediator.Setup(m => m.Send(It.IsAny<string>(), CancellationToken.None)).Callback(() => Task.FromResult(Unit.Value));
            var controller = new BasketController(_mockmediator.Object, _mocklogger.Object);
            var result=await controller.DeleteAsync("User2");
            Assert.IsType<NoContentResult>(result);
        }
    }
}