namespace ShoppingBasket.API.Core.Entities
{
    public class BasketItem : BaseEntity<string>
    {

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string PictureUrl { get; set; }
    }

}
