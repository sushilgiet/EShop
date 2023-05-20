using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.Contracts.Persistence;

namespace ProductCatalog.Application.Handlers
{
    public class DeleteCatalogItemCommandHandler : IRequestHandler<DeleteCatalogItemCommand, Unit>
    {
        private readonly ICatalogItemRepository _repo;
        private readonly ILogger<DeleteCatalogItemCommandHandler> _logger;
        public DeleteCatalogItemCommandHandler(ICatalogItemRepository repo, ILogger<DeleteCatalogItemCommandHandler> logger)
        {
            _repo = repo;
            _logger = logger;
        }

       
        public async Task<Unit> Handle(DeleteCatalogItemCommand request, CancellationToken cancellationToken)
        {
            await _repo.Delete(request.Id);
            _logger.LogInformation("Catalog Item Deleted.", new {ItemId=request.Id});
            return Unit.Value;
        }
    }
}
    
