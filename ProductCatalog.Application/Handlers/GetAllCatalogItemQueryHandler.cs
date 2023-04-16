using MediatR;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain;

namespace ProductCatalog.Application.Handlers
{
    public class GetAllCatalogItemQueryHandler : IRequestHandler<GetAllCatalogItemQuery, IEnumerable<CatalogItem>>
    {
        private readonly ICatalogItemRepository _repo;
        public GetAllCatalogItemQueryHandler(ICatalogItemRepository repo)
        {
            this._repo = repo;
        }
        public async Task<IEnumerable<CatalogItem>> Handle(GetAllCatalogItemQuery request, CancellationToken cancellationToken)
        {
            return await _repo.GetAll();
        }
    }
}
