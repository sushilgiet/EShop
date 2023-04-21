using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.Contracts.Persistence;

namespace ProductCatalog.Application.Handlers
{
    public class DeleteCatalogItemCommandHandler : IRequestHandler<DeleteCatalogItemCommand, Unit>
    {
        private readonly ICatalogItemRepository _repo;

        public DeleteCatalogItemCommandHandler(ICatalogItemRepository repo)
        {
            _repo = repo;
        }

       
        public async Task<Unit> Handle(DeleteCatalogItemCommand request, CancellationToken cancellationToken)
        {
            await _repo.Delete(request.Id);
            return Unit.Value;
        }
    }
}
    
