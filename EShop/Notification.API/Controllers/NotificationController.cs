using Microsoft.AspNetCore.Mvc;
using Notification.API.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using System.ComponentModel;
using System.Collections;
using Microsoft.AspNetCore.Authorization;



// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Notification.API.Controller
{
    [Authorize]
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
        public async Task<IActionResult> PostAsync(PriceChangeRequest notification)
        {
            await AddPriceChangeNotification(notification);
            return NoContent();
        }

        private async Task AddPriceChangeNotification(PriceChangeRequest notification)
        {
            CosmosClient client = new CosmosClient(_configuration["CosmoDB:endpoint"], _configuration["CosmoDBkey"]);
            Database database = client.GetDatabase(_configuration["CosmoDB:databasename"]);
            Microsoft.Azure.Cosmos.Container container = database.GetContainer("NotificationInfo");
            dynamic notify = new
            {
                id = Guid.NewGuid().ToString(),
                UserId = notification.UserId,
                ProductName = notification.ProductName,
                NotifyPrice = notification.NewPrice,
                ProductId = notification.ProductId,
                IsMailSent = false
            };
            await container.CreateItemAsync(notify);
            container = database.GetContainer("UserInfo");
            dynamic user = new
            {
                id = Guid.NewGuid().ToString(),
                UserId = notification.UserId,
                Email = notification.Email,
                Name = notification.UserName
            };
           
            await container.CreateItemAsync(user);
        }
    }
}
