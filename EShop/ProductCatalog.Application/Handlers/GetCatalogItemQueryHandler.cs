using MediatR;
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
        public GetCatalogItemQueryHandler(ICatalogItemRepository repo)
        {
            this._repo = repo;
        }
        public async Task<CatalogItem> Handle(GetCatalogItemQuery query, CancellationToken cancellationToken)
        {
            return await _repo.GetById(query.Id);
        }
    }
}
