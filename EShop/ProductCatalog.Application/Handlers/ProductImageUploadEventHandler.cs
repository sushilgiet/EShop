using Azure.Storage.Queues;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ProductCatalog.Application.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Handlers
{

    public class ProductImageUploadEventHandler : INotificationHandler<ProductImageUploadEvent>
    {
        IConfiguration _configuration;
        public ProductImageUploadEventHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }
      

        public async Task Handle(ProductImageUploadEvent notification, CancellationToken cancellationToken)
        {
            QueueClient queueClient = new QueueClient(_configuration.GetConnectionString("CatalogImageContainer"), _configuration["CatalogImageQueue"]);
            string blobString = ToBase64(notification.BlobInformation);
            await queueClient.SendMessageAsync(blobString);
        }

        private string ToBase64(object obj)
        {
            string json = JsonConvert.SerializeObject(obj);
            byte[] bytes = Encoding.Default.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }
    }



}
