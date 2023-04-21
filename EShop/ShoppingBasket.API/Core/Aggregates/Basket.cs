using ShoppingBasket.API.Core.Entities;

namespace ShoppingBasket.API.Core.Aggregates
{
    public class Basket: IAggregateRoot
    {
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; }
    }

}
