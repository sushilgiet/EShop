using MediatR;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain;

namespace ProductCatalog.Application.Handlers
{
    public class GetCatalogTypesQueryHandler : IRequestHandler<GetCatalogTypesQuery, IEnumerable<CatalogType>>
    {
        private readonly ICatalogTypeRepository _repo;
        public GetCatalogTypesQueryHandler(ICatalogTypeRepository repo) => this._repo = repo;



        public async Task<IEnumerable<CatalogType>> Handle(GetCatalogTypesQuery query, CancellationToken cancellationToken)
        {
            return await _repo.GetAll();
        }
    }
}
