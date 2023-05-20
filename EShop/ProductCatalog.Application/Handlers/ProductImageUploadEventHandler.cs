using Azure.Storage.Queues;
using MediatR;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ProductCatalog.Application.Contracts;
using ProductCatalog.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMessageSender = ProductCatalog.Application.Contracts.IMessageSender;

namespace ProductCatalog.Application.Handlers
{

    public class ProductImageUploadEventHandler : INotificationHandler<ProductImageUploadEvent>
    {
        IConfiguration _configuration;
        IMessageSender _messageSender;
        Microsoft.Extensions.Logging.ILogger<ProductImageUploadEventHandler> _logger;
        public ProductImageUploadEventHandler(IConfiguration configuration,IMessageSender messageSender, Microsoft.Extensions.Logging.ILogger<ProductImageUploadEventHandler> logger)
        {
            _configuration = configuration;
            _messageSender = messageSender;
            _logger = logger;
        }
        public ProductImageUploadEventHandler(IMessageSender messageSender)
        {
           
            _messageSender = messageSender;
        }

        public async Task Handle(ProductImageUploadEvent notification, CancellationToken cancellationToken)
        {
            
            await SendMessage(notification);
        }

        public async Task SendMessage(ProductImageUploadEvent notification)
        {
           
            _messageSender.SendMessageAsync(notification.BlobInformation);
            _logger.LogInformation("Notification to generate thumbnail queue",new { Blob= notification.BlobInformation });
        }

        private string ToBase64(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] bytes = Encoding.Default.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }
    }



}
