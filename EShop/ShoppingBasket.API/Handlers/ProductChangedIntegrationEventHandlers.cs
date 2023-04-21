using EventBus.Abstractions;
using ShoppingBasket.API.Core.Interfaces;
using ShoppingBasket.API.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingBasket.API.Handlers
{
    public class ProductPriceChangedEventHandlers :
      IIntegrationEventHandler<ProductPriceChangedEvent>
    {
        IBasketRepository _repository;
        public ProductPriceChangedEventHandlers(IBasketRepository repo)
        {
            _repository = repo;
        }
        public async void Handle(ProductPriceChangedEvent evt)
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

