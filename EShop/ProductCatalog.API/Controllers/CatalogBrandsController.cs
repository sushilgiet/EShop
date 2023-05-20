using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Contracts;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatalogBrandsController : ControllerBase
    {

       
        IMediator _mediator;
        ILogger<CatalogBrandsController> _logger;
        public CatalogBrandsController(IMediator mediator, ILogger<CatalogBrandsController> logger)
        {

            _mediator = mediator;
            _logger = logger;
        }

        // GET: api/CatalogBrands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogBrand>>> GetCatalogBrands()
        {
            var query = new GetCatalogBrandsQuery();
            var brands = await _mediator.Send(query);
            _logger.LogInformation("Get All Catalog brands");
            return Ok(brands);
        }

     

    }
}
