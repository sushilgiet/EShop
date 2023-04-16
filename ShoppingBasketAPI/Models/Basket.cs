namespace ShoppingBasketAPI.Models
{
    public class Basket
    {
        public string BuyerId { get; set; }
        public List<BasketItem> Items { get; set; }
    }

}
