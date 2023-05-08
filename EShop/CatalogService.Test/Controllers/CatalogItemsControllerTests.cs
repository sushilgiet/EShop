using Xunit;
using ProductCatalog.API.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Azure;
using ProductCatalog.Application.Queries;
using CatalogService.Test.Handlers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Domain.Entities;
using Microsoft.Azure.Amqp.Framing;
using ProductCatalog.Application.Commands;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using CatalogService.Test;
using ProductCatalog.Application.Events;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ProductCatalog.API.Controllers.Tests
{
    public class CatalogItemsControllerTests
    {
        Mock<IMediator> _mockmediator;
        Mock<IConfiguration> _mockconfiguration;
        Mock<ILogger<CatalogItemsController>> _mocklogger;
        List<CatalogItem> _catalogItems;
        public CatalogItemsControllerTests()
        {
            _mockmediator = new Mock<IMediator>();
            _mockconfiguration = new Mock<IConfiguration>();
            _mockconfiguration.Setup(x => x["MySetting"]).Returns("my value");
            _mocklogger = new Mock<ILogger<CatalogItemsController>>();
            _catalogItems = MockCatalogItemRepository.CatalogItems;
        }

        [Fact()]
        public async Task GetCatalogItems_CountGtThenZero_ReturnsTrue()
        {

            var response = MockCatalogItemRepository.CatalogItems;
            _mockmediator.Setup(m => m.Send(It.IsAny<GetAllCatalogItemQuery>(), CancellationToken.None))
            .ReturnsAsync(response);
            var controller = new CatalogItemsController(_mockconfiguration.Object, _mocklogger.Object, _mockmediator.Object);
            var result = await controller.GetCatalogItems();
            Assert.NotNull(result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var actualObjects = Assert.IsAssignableFrom<IEnumerable<CatalogItem>>(okResult.Value);
            Assert.Equal(_catalogItems.Count, actualObjects.Count());
           
        }

        [Fact()]
        public async Task GetCatalogItem_ExpectedObjMatched_ReturnsTrue()
        {
            var query = new GetCatalogItemQuery { Id = 3 };
            var expectedObject= _catalogItems.Find(o => o.Id == query.Id);
            _mockmediator.Setup(m => m.Send(It.IsAny<GetCatalogItemQuery>(), CancellationToken.None)).ReturnsAsync((GetCatalogItemQuery query, CancellationToken token) =>
            {
                return _catalogItems.Where(t => t.Id == query.Id).FirstOrDefault();
            });

            var controller = new CatalogItemsController(_mockconfiguration.Object, _mocklogger.Object, _mockmediator.Object);
            var result = await controller.GetCatalogItem(3);
            var actualObject = Assert.IsAssignableFrom<CatalogItem>(result.Value);
            Assert.Equal(expectedObject.Id, actualObject.Id);
            Assert.Equal(expectedObject.Name, actualObject.Name);
        }
        [Fact()]
        public async Task GetCatalogItem_IDNotFound_ReturnsStatusCode404()
        {
            var query = new GetCatalogItemQuery { Id = -1 };

            _mockmediator.Setup(m => m.Send(It.IsAny<GetCatalogItemQuery>(), CancellationToken.None)).ReturnsAsync((GetCatalogItemQuery query, CancellationToken token) =>
            {
                return _catalogItems.Where(t => t.Id == query.Id).FirstOrDefault();
            });

            var controller = new CatalogItemsController(_mockconfiguration.Object, _mocklogger.Object, _mockmediator.Object);
            var result = await controller.GetCatalogItem(-1);
            Assert.True(((Microsoft.AspNetCore.Mvc.StatusCodeResult)result.Result).StatusCode==404);
        }
        [Fact()]
        public async Task PutCatalogItem_ItemUpdated_Returns_NoContent()
        {
            var productToUpdate = MockCatalogItemRepository.CatalogItems.Find(t => t.Id == 1);
            productToUpdate.Name = "Update Product";
            _mockmediator.Setup(m => m.Send(It.IsAny<UpdateCatalogItemCommand>(), CancellationToken.None)).ReturnsAsync((UpdateCatalogItemCommand command, CancellationToken token) =>
            {
               
                return Unit.Value;
            });
            var controller = new CatalogItemsController(_mockconfiguration.Object, _mocklogger.Object, _mockmediator.Object);
            var result = await controller.PutCatalogItem(1, productToUpdate);
            Assert.IsType<NoContentResult>(result);
           


        }
        [Fact()]
        public async Task PutCatalogItem_IdNotPersent_ReturnsBadRequest()
        {
            var productToUpdate = MockCatalogItemRepository.CatalogItems.Find(t => t.Id == 1);
            productToUpdate.Name = "Update Product";
            _mockmediator.Setup(m => m.Send(It.IsAny<UpdateCatalogItemCommand>(), CancellationToken.None)).ReturnsAsync((UpdateCatalogItemCommand command, CancellationToken token) =>
            {
                return Unit.Value;
            });
            var controller = new CatalogItemsController(_mockconfiguration.Object, _mocklogger.Object, _mockmediator.Object);
            var result = await controller.PutCatalogItem(2, productToUpdate);

            Assert.IsType<BadRequestResult>(result);

        }
        [Fact()]
        public async Task PostCatalogItem_ReturnsCreatedAtActionResult()
        {
            CatalogItem item = new CatalogItem("Demo Product","Description",100.89m,"","",1,2);
            _mockmediator.Setup(m => m.Send(It.IsAny<UpdateCatalogItemCommand>(), CancellationToken.None)).ReturnsAsync((UpdateCatalogItemCommand command, CancellationToken token) =>
            {
                return Unit.Value;
            });
            var controller = new CatalogItemsController(_mockconfiguration.Object, _mocklogger.Object, _mockmediator.Object);
            var result = await controller.PostCatalogItem(item);
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact()]
        public async Task DeleteCatalogItem_ReturnsNoContent()
        {
            _mockmediator.Setup(r => r.Send(It.IsAny<DeleteCatalogItemCommand>(), CancellationToken.None)).ReturnsAsync(
              (DeleteCatalogItemCommand command, CancellationToken token) =>
              {
                  _catalogItems.RemoveAll(t => t.Id == command.Id);
                  return Unit.Value;

              });

            var controller = new CatalogItemsController(_mockconfiguration.Object, _mocklogger.Object, _mockmediator.Object);
            var result = await controller.DeleteCatalogItem(3);
            Assert.IsType<NoContentResult>(result);

        }



        [Fact()]
        public async Task ProductImageUploadEvent_ReturnsOK()
        {

            BlobInformation blob = new BlobInformation { Id = 1 };
            _mockmediator.Setup(r => r.Publish(It.IsAny<ProductImageUploadEvent>(), CancellationToken.None)).Returns(Task.FromResult(Unit.Value));
            var controller = new CatalogItemsController(_mockconfiguration.Object, _mocklogger.Object, _mockmediator.Object);
            var result = await controller.ProductImageUploadEvent(blob);
            Assert.IsType<OkResult>(result.Result);

        }
    }
}