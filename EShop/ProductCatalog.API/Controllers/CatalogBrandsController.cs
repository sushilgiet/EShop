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
        public CatalogBrandsController(IMediator mediator)
        {

            _mediator = mediator;
        }

        // GET: api/CatalogBrands
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogBrand>>> GetCatalogBrands()
        {
            var query = new GetCatalogBrandsQuery();
            var brands = await _mediator.Send(query);
            return Ok(brands);
        }

     

    }
}
