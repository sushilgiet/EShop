using EventBus.Abstractions;
using MediatR;
using ProductCatalog.Application.Commands;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Application.Events;
using ProductCatalog.Domain;
using ProductCatalog.Domain.Entities;
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

        public async Task<Unit> Handle(UpdateCatalogItemCommand request, CancellationToken cancellationToken)
        {
            var catalogItem = await _repo.GetById(request.Id);
            var oldPrice = catalogItem.Price;
            var raiseProductPriceUpdatedEvent = oldPrice != request.Price;
            var newCatalogItem = new CatalogItem(
                request.Name, request.Description, request.Price, request.PictureFileName,request.PictureUrl, request.CatalogTypeId, request.CatalogBrandId, request.Id);
            newCatalogItem.Validate();
            foreach (var pi in newCatalogItem.GetType().GetProperties())
            {
                pi.SetValue(catalogItem, pi.GetValue(newCatalogItem));
            }
            await _repo.Update(catalogItem);
            if (raiseProductPriceUpdatedEvent)
            {
                var priceChangedEvent = new ProductPriceChangedEvent(request.Id, request.Price, oldPrice);
                _eventBus.Publish(priceChangedEvent);
            }


            return await Task.FromResult(Unit.Value);
        }

      
    }
}
