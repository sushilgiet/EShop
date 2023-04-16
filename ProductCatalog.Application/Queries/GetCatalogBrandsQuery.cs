using MediatR;
using ProductCatalog.Domain;

namespace ProductCatalog.Application.Queries
{
    public class GetCatalogBrandsQuery : IRequest<IEnumerable<CatalogBrand>>
    {

    }
}
