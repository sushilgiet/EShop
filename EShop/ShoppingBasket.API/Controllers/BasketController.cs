
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShoppingBasket.API.Core.Aggregates;
using ShoppingBasket.API.Core.Interfaces;
using System.Net;
using MediatR;
using ShoppingBasket.API.Queries;
using ShoppingBasket.API.Commands;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ShoppingBasket.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
       
        private readonly IMediator _mediator;
        private readonly ILogger<BasketController> _logger;
        public BasketController(IMediator mediator, ILogger<BasketController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        // GET basket/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(Basket), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> Get(string id)
        {
            _logger.LogInformation("Get Request for Basket for user id:" + id, new { Id = id });
            GetBasketQuery query=new GetBasketQuery { Id = id };
            var basket = await _mediator.Send(query);
            _logger.LogInformation("Basket for user id:"+id,new  {Id=id });
            return Ok(basket);
        }

        // POST basket
        [HttpPost]
        [ProducesResponseType(typeof(Basket), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> PostAsync([FromBody]Basket value)
        {
            _logger.LogInformation("Updated basket item request for user id:" + value.BuyerId, new { Basket = value });

            UpdateBasketItemCommand command =new UpdateBasketItemCommand { Basketdetails= value };
            var basket = await _mediator.Send(command);
            _logger.LogInformation("Basket item updated for user id:" +value.BuyerId, new { Basket = value, DT = DateTime.UtcNow });

            return Ok(basket);
        }

        // DELETE bakset/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            DeleteBasketItemCommand command = new DeleteBasketItemCommand { Id=id };
            await _mediator.Send(command);
            _logger.LogInformation("Basket Item Deleted:" + id, new { Id = id,DT=DateTime.UtcNow });

            return NoContent();
        }

    }
}
