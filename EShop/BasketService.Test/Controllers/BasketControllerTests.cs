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
            _mockmediator.Setup(m => m.Send(It.IsAny<GetBasketQuery>(), CancellationToken.None));
            var controller = new BasketController(_mockmediator.Object, _mocklogger.Object);
            var result = await controller.Get("");
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public async Task PostAsyncTest()
        {
            Basket details = new Basket();

            _mockmediator.Setup(m => m.Send(It.IsAny<UpdateBasketItemCommand>(), CancellationToken.None)).Returns(
                (UpdateBasketItemCommand command, CancellationToken token) => 
                {

                    return command.Basketdetails;
                
                }
                );
            var controller = new BasketController(_mockmediator.Object, _mocklogger.Object);
            var result = await controller.Get("");
            Assert.True(false, "This test needs an implementation");
        }

        [Fact()]
        public async Task DeleteTest()
        {
            _mockmediator.Setup(m => m.Send(It.IsAny<string>(), CancellationToken.None)).Returns(() =>
            {
                //_mocklogger.Verify()
                return Task.FromResult(Unit.Value);
            });
            var controller = new BasketController(_mockmediator.Object, _mocklogger.Object);
            var result=await controller.DeleteAsync("");
            Assert.True(false, "This test needs an implementation");
        }
    }
}