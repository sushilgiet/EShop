using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Handlers
{
    public class GetCatalogBrandsQueryHandler : IRequestHandler<GetCatalogBrandsQuery, IEnumerable<CatalogBrand>>
    {
        private readonly ICatalogBrandRepository _repo;
        private readonly ILogger<GetCatalogBrandsQueryHandler> _logger;
        public GetCatalogBrandsQueryHandler(ICatalogBrandRepository repo, Microsoft.Extensions.Logging.ILogger<GetCatalogBrandsQueryHandler> logger)
        {
            this._repo = repo;
            this._logger = logger;
        }



        public async Task<IEnumerable<CatalogBrand>> Handle(GetCatalogBrandsQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Get All Brands Query");
            return await _repo.GetAll();
        }
    }
}
