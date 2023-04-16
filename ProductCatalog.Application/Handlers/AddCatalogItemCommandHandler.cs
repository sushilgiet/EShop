using MediatR;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Handlers
{
    public class AddCatalogItemCommandHandler : IRequestHandler<AddCatalogItemCommand,CatalogItem>
    {

        private readonly ICatalogItemRepository _repo;
        public AddCatalogItemCommandHandler(ICatalogItemRepository repo)
        {
            ICatalogItemRepository _repo = repo;
        }

        public async Task<CatalogItem> Handle(AddCatalogItemCommand request, CancellationToken cancellationToken)
        {
            return await _repo.Add(request.Item);
        }
    }
  
}
