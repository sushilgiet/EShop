using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using ProductCatalog.Application.Queries;
using ProductCatalog.Application.Commands;
using ProductCatalog.Domain.Entities;
using ProductCatalog.Application.Events;
using Microsoft.AspNetCore.Authorization;

namespace ProductCatalog.API.Controllers
{
    [Authorize]
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
            var command = new UpdateCatalogItemCommand {
                Name = catalogItem.Name,
                Price = catalogItem.Price,
                CatalogBrandId = catalogItem.CatalogBrandId,
                CatalogTypeId = catalogItem.CatalogTypeId,
                Description = catalogItem.Description,
                PictureFileName = catalogItem.PictureFileName,
                PictureUrl = catalogItem.PictureUrl,
                Id= catalogItem.Id
            };
            await _mediator.Send(command);
            return NoContent();
        }

        // POST: api/CatalogItems
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<CatalogItem>> PostCatalogItem(CatalogItem catalogItem)
        {
            var command = new AddCatalogItemCommand { 
            Name= catalogItem.Name,
            Price= catalogItem.Price,
            CatalogBrandId= catalogItem.CatalogBrandId,
            CatalogTypeId= catalogItem.CatalogTypeId,
            Description= catalogItem.Description,
            PictureFileName= catalogItem.PictureFileName,
            PictureUrl= catalogItem.PictureUrl
            
            };
            var item=await _mediator.Send(command);
            return CreatedAtAction("GetCatalogItem", new { id = catalogItem.Id }, item);
        }

        // DELETE: api/CatalogItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCatalogItem(int id)
        {
            var command = new DeleteCatalogItemCommand { Id=id};
            var item = await _mediator.Send(command);
            return NoContent();

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
        [HttpPost("/ProductImageUploadEvent")]
        public async Task<ActionResult<string>> ProductImageUploadEvent(BlobInformation blobInformation)
        {

            var uploadEvent = new ProductImageUploadEvent
            {
                BlobInformation= blobInformation
            };

            await _mediator.Publish(uploadEvent);
            return Ok();
        }
       
    }
}
