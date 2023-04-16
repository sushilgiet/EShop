using MediatR;
using ProductCatalog.Domain;

namespace ProductCatalog.Application.Queries
{
    public class GetAllCatalogItemQuery : IRequest<IEnumerable<CatalogItem>>
    {
      
    }
}
