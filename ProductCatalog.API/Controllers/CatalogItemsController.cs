using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client.Extensions.Msal;
using ProductCatalog.Application;
using ProductCatalog.Application.Contracts;
using ProductCatalog.Domain;
using System.Reflection.Metadata;
using static System.Reflection.Metadata.BlobBuilder;
using Azure.Storage.Queues;                                                             
using System.Text;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using ProductCatalog.Application.Queries;
using ProductCatalog.Application.Commands;

namespace ProductCatalog.API.Controllers
{
    //[Authorize]
    [Route("[controller]")]
    [ApiController]
    public class CatalogItemsController : ControllerBase
    {
         
         IConfiguration _configuration;
         ILogger _logger;
         IMediator _mediator;

        public CatalogItemsController(IConfiguration configuration,ILogger<CatalogItemsController> logger, IMediator mediator)
        {
            
            _configuration= configuration;
            _logger= logger;
            _mediator = mediator;
        }

        // GET: api/CatalogItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogItem>>> GetCatalogItems()
        {
            // return Ok(await _boItem.GetCatalogItems());
             var query = new GetAllCatalogItemQuery();
             return Ok(await _mediator.Send(query));
            ;
        }

        // GET: api/CatalogItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CatalogItem>> GetCatalogItem(int id)
        {
            //var catalogItem = await _boItem.GetCatalogItem(id);
            var query = new GetCatalogItemQuery { Id=id};
            var catalogItem = await _mediator.Send(query);
            if (catalogItem == null)
            {
                return NotFound();
            }

            return catalogItem;
        }

        // PUT: api/CatalogItems/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCatalogItem(int id, CatalogItem catalogItem)
        {
            if (id != catalogItem.Id)
            {
                return BadRequest();
            }
            var command = new UpdateCatalogItemCommand { ProductToUpdate = catalogItem };
            await _mediator.Send(command);
            return NoContent();
        }

        // POST: api/CatalogItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CatalogItem>> PostCatalogItem(CatalogItem catalogItem)
        {
            var command = new AddCatalogItemCommand { Item = catalogItem };
            var item=await _mediator.Send(command);
            return CreatedAtAction("GetCatalogItem", new { id = catalogItem.Id }, item);
        }

        // DELETE: api/CatalogItems/5
        [HttpDelete("{id}")]
        public async Task DeleteCatalogItem(int id)
        {
            var command = new DeleteCatalogItemCommand { Id=id};
            var item = await _mediator.Send(command);
           
        }
        [HttpPost("/UploadProductImage")]
        public async Task<ActionResult<string>> UploadProductImage(IFormFile file)
        {
            
            string cs = _configuration.GetConnectionString("CatalogImageContainer");
            BlobServiceClient blobServiceClient = new BlobServiceClient(cs);
            BlobContainerClient blobClient = blobServiceClient.GetBlobContainerClient(_configuration.GetValue<string>("CatalogContainer"));
            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var blob = await blobClient.UploadBlobAsync(blobName, file.OpenReadStream());
            return blobClient.Uri.ToString() + "/" + blobName;

        }
        [HttpPost("/NotifyProductImageUpload")]
        public async Task<ActionResult<string>> NotifyProductImageUpload(BlobInformation blobInformation)
        {

            var notification = new ProductImageUploadNotification
            {
                BlobInformation= blobInformation
            };

            await _mediator.Publish(notification);
            return Ok();
        }
       
    }
}
