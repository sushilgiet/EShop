using MediatR;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Queries
{
    public class GetCatalogTypesQuery : IRequest<IEnumerable<CatalogType>>
    {

    }
}
