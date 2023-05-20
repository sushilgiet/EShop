using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Handlers
{
    public class GetAllCatalogItemQueryHandler : IRequestHandler<GetAllCatalogItemQuery, IEnumerable<CatalogItem>>
    {
        private readonly ICatalogItemRepository _repo;
        private readonly ILogger<GetAllCatalogItemQueryHandler> _logger;
        public GetAllCatalogItemQueryHandler(ICatalogItemRepository repo, Microsoft.Extensions.Logging.ILogger<GetAllCatalogItemQueryHandler> logger)
        {
            this._repo = repo;
            this._logger = logger;
        }
        public async Task<IEnumerable<CatalogItem>> Handle(GetAllCatalogItemQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Query for all catalog items");
            return await _repo.GetAll();
        }
    }
}
