namespace ShoppingBasket.API.Core.Entities
{
    public abstract class BaseEntity<TId>
    {

        public TId Id { get; set; }
    }
}
