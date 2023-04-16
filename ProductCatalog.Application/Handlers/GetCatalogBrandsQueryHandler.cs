using MediatR;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain;

namespace ProductCatalog.Application.Handlers
{
    public class GetCatalogBrandsQueryHandler : IRequestHandler<GetCatalogBrandsQuery, IEnumerable<CatalogBrand>>
    {
        private readonly ICatalogBrandRepository _repo;
        public GetCatalogBrandsQueryHandler(ICatalogBrandRepository repo) => this._repo = repo;



        public async Task<IEnumerable<CatalogBrand>> Handle(GetCatalogBrandsQuery query, CancellationToken cancellationToken)
        {
            return await _repo.GetAll();
        }
    }
}
