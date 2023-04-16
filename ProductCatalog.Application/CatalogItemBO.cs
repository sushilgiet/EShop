using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventBus.Abstractions;
using ProductCatalog.Application.Contracts;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain;

namespace ProductCatalog.Application
{
    public class CatalogItemBO : ICatalogItemBO
    {
        private readonly ICatalogItemRepository _repo;
        IEventBus _bus;
        public CatalogItemBO(ICatalogItemRepository repository, IEventBus bus)
        {
            _repo = repository;
            _bus = bus;
        }
        public async Task<CatalogItem> Add(CatalogItem item)
        {
            return await _repo.Add(item);
           
        }

        public async Task Delete(int id)
        {
            await _repo.Delete(id);
        }

        public async Task<CatalogItem> GetCatalogItem(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task<IEnumerable<CatalogItem>> GetCatalogItems()
        {
            return await _repo.GetAll();
        }

        public async Task Update(CatalogItem productToUpdate)
        {
            var catalogItem = await _repo.GetById(productToUpdate.Id);
            var oldPrice = catalogItem.Price;
            var raiseProductPriceUpdatedEvent = oldPrice != productToUpdate.Price;
            foreach (var pi in productToUpdate.GetType().GetProperties())
            {
                pi.SetValue(catalogItem, pi.GetValue(productToUpdate));
            }
            await _repo.Update(catalogItem);
            if (raiseProductPriceUpdatedEvent)
            {
                var priceChangedEvent = new ProductPriceUpdatedIntegrationEvent(productToUpdate.Id, productToUpdate.Price, oldPrice);
                _bus.Publish(priceChangedEvent);
            }
        }
       
    }
}
