using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace ShoppingBasket.IntegrationEvents
{
    public class ProductPriceUpdatedIntegrationEvent : IntegrationEvent
    {
        public int ProductId { get; private set; }
        public decimal NewPrice { get; private set; }
        public decimal OldPrice { get; private set; }
        public ProductPriceUpdatedIntegrationEvent(int productId, decimal newPrice, decimal oldPrice)
        {
            ProductId = productId;
            NewPrice = newPrice;
            OldPrice = oldPrice;
        }
    }
}

