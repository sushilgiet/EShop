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
    public class UpdateCatalogItemCommandHandler : IRequestHandler<UpdateCatalogItemCommand, Unit>
    {

        private readonly ICatalogItemRepository _repo;
        public UpdateCatalogItemCommandHandler(ICatalogItemRepository repo)
        {
            ICatalogItemRepository _repo = repo;
        }

        async Task<Unit> IRequestHandler<UpdateCatalogItemCommand, Unit>.Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            await _repo.Update(request.ProductToUpdate);
            return await Task.FromResult(Unit.Value); ;
        }
    }
}
