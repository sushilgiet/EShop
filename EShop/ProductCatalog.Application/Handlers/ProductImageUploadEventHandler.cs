using Azure.Storage.Queues;
using MediatR;
using Microsoft.Azure.ServiceBus.Core;
using Microsoft.Extensions.Configuration;
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
        public ProductImageUploadEventHandler(IConfiguration configuration,IMessageSender messageSender)
        {
            _configuration = configuration;
            _messageSender= messageSender;
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
            //QueueClient queueClient = new QueueClient(_configuration.GetConnectionString("CatalogImageContainer"), _configuration["CatalogImageQueue"]);
            //string blobString = ToBase64(notification.BlobInformation);
            //await queueClient.SendMessageAsync(blobString);
            _messageSender.SendMessageAsync(notification.BlobInformation);
        }

        private string ToBase64(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] bytes = Encoding.Default.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }
    }



}
