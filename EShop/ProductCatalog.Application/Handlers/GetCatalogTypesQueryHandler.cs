using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Handlers
{
    public class GetCatalogTypesQueryHandler : IRequestHandler<GetCatalogTypesQuery, IEnumerable<CatalogType>>
    {
        private readonly ICatalogTypeRepository _repo;
        Microsoft.Extensions.Logging.ILogger<GetCatalogTypesQueryHandler> _logger;
        public GetCatalogTypesQueryHandler(ICatalogTypeRepository repo, Microsoft.Extensions.Logging.ILogger<GetCatalogTypesQueryHandler> logger)
        {
            this._repo = repo;
            this._logger = logger;
        }



        public async Task<IEnumerable<CatalogType>> Handle(GetCatalogTypesQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get All Catalog Types");
            return await _repo.GetAll();
        }
    }
}
