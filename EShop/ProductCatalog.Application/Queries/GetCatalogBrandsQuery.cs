using MediatR;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Queries
{
    public class GetCatalogBrandsQuery : IRequest<IEnumerable<CatalogBrand>>
    {

    }
}
