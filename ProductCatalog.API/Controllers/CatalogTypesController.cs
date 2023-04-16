using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductCatalog.Application.Contracts;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain;

namespace ProductCatalog.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CatalogTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public  CatalogTypesController(IMediator mediator)
        {

            _mediator = mediator;
        }

    // GET: api/CatalogTypes
    [HttpGet]
        public async Task<ActionResult<IEnumerable<CatalogType>>> GetCatalogTypes()
        {
            var query = new GetCatalogTypesQuery();
            var types = await _mediator.Send(query);
            return Ok(types);
        }



    }
}
