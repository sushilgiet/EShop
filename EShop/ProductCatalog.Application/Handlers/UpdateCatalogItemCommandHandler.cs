using EventBus.Abstractions;
using MediatR;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Events;
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
        private readonly IEventBus    _eventBus;
        public UpdateCatalogItemCommandHandler(ICatalogItemRepository repo, IEventBus bus)
        {
            _repo = repo;
            _eventBus = bus;
        }

        async Task<Unit> IRequestHandler<UpdateCatalogItemCommand, Unit>.Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            var catalogItem = await _repo.GetById(request.ProductToUpdate.Id);
            var oldPrice = catalogItem.Price;
            var raiseProductPriceUpdatedEvent = oldPrice != request.ProductToUpdate.Price;
            foreach (var pi in request.ProductToUpdate.GetType().GetProperties())
            {
                pi.SetValue(catalogItem, pi.GetValue(request.ProductToUpdate));
            }
            await _repo.Update(catalogItem);
            if (raiseProductPriceUpdatedEvent)
            {
                var priceChangedEvent = new ProductPriceChangedEvent(request.ProductToUpdate.Id, request.ProductToUpdate.Price, oldPrice);
                _eventBus.Publish(priceChangedEvent);
            }

            
            return await Task.FromResult(Unit.Value); 
        }
    }
}
