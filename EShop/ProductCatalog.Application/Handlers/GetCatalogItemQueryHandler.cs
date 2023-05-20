using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Queries;
using ProductCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Handlers
{
    public class GetCatalogItemQueryHandler : IRequestHandler<GetCatalogItemQuery,CatalogItem>
    {
        private readonly ICatalogItemRepository _repo;
        private readonly Microsoft.Extensions.Logging.ILogger<GetCatalogBrandsQueryHandler> _logger;
        public GetCatalogItemQueryHandler(ICatalogItemRepository repo, Microsoft.Extensions.Logging.ILogger<GetCatalogBrandsQueryHandler> logger)
        {
            this._repo = repo;
            this._logger = logger;
        }
        public async Task<CatalogItem> Handle(GetCatalogItemQuery query, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Execution of GetCatalogItemQuery",new {CatalogItemId=query.Id});
            return await _repo.GetById(query.Id);
        }
    }
}
