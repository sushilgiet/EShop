using MediatR;
using ProductCatalog.Domain;

namespace ProductCatalog.Application.Queries
{
    public class GetCatalogTypesQuery : IRequest<IEnumerable<CatalogType>>
    {

    }
}
