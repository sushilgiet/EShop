using MediatR;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Queries
{
    public class GetAllCatalogItemQuery : IRequest<IEnumerable<CatalogItem>>
    {
      
    }
}
