﻿using Xunit;
using ProductCatalog.Application.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ProductCatalog.Application.Contracts;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Application.Events;

namespace CatalogService.Test.Handlers
{
    public class ProductImageUploadEventHandlerTests
    {
        Mock<IMessageSender> _mocksender;
        List<BlobInformation> _messages;
        public ProductImageUploadEventHandlerTests()
        {
            _messages = new List<BlobInformation>();
            _mocksender = new Mock<IMessageSender>();
            _mocksender.Setup(r => r.SendMessageAsync(It.IsAny<BlobInformation>())).Callback<BlobInformation>(
               c => _messages.Add(c)
                );
        }
        [Fact()]
        public async Task Handle_IsMessageAdded_true()
        {
            var message = new BlobInformation
            {
                Id = 1
            };
            var handler = new ProductImageUploadEventHandler(_mocksender.Object);
            await handler.Handle(new ProductImageUploadEvent
            {
                BlobInformation = message
            }, CancellationToken.None);
            Assert.Contains(message, _messages);
        }


    }
}