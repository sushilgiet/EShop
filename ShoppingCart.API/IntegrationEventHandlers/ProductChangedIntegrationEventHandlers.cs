using EventBus.Abstractions;
using ShoppingBasketAPI.Repository;
using ShoppingBasket.IntegrationEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingBasket.API.IntegrationEventHandlers
{
    public class ProductChangedIntegrationEventHandlers :
      IIntegrationEventHandler<ProductPriceUpdatedIntegrationEvent>
    {
        IBasketRepository _repository;
        public ProductChangedIntegrationEventHandlers(IBasketRepository repo)
        {
            _repository = repo;
        }
        public async void Handle(ProductPriceUpdatedIntegrationEvent evt)
        {
            var userIds = _repository.GetBuyers();
            foreach (var id in userIds)
            {
                var basket = await _repository.GetBasketAsync(id);
                var product = basket.Items.Where(product => product.ProductId == evt.ProductId).FirstOrDefault();
                if (product != null)
                {
                    product.UnitPrice = evt.NewPrice;
                }
                await _repository.UpdateBasketAsync(basket);
            }
        }
    }
}

