using Microsoft.AspNetCore.Mvc;
using Notification.API.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.ComponentModel;
using System.Collections;
using Azure.Identity;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notification.API.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceUpdateNotificationController : ControllerBase
    {
        IConfiguration _configuration;
        ILogger<PriceUpdateNotificationController> _logger;
       
        public PriceUpdateNotificationController(IConfiguration configuration, ILogger<PriceUpdateNotificationController> logger)
        {
            _configuration = configuration;
            _logger = logger;
            
        }
        [HttpPost]
        public async Task<CatalogItemPriceChangeNotification> PostAsync(CatalogItemPriceChangeNotification notification)
        {
            DefaultAzureCredential credential = new DefaultAzureCredential();
            CosmosClient client = new CosmosClient(_configuration["CosmoDB:endpoint"], _configuration["CosmoDBkey"]);
            Database database = client.GetDatabase(_configuration["CosmoDB:databasename"]);
            Microsoft.Azure.Cosmos.Container container = database.GetContainer("NotificationInfo");
            dynamic notify = new
            {
                id = Guid.NewGuid().ToString(),
                UserId = notification.UserId,
                ProductName= notification.ProductName,
                NotifyPrice= notification.NewPrice,
                ProductId=notification.ProductId
            };
            await container.CreateItemAsync(notify);
            container = database.GetContainer("UserInfo");
            dynamic user = new
            {
                id = Guid.NewGuid().ToString(),
                UserId = notification.UserId,
                Email = notification.Email
            };
            await container.CreateItemAsync(user);
            return notification;
        }



    }
}
