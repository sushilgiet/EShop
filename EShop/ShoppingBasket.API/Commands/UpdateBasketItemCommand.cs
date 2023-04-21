namespace ShoppingBasket.API.Commands
{
    using MediatR;
    using ShoppingBasket.API.Core.Aggregates;

    public class UpdateBasketItemCommand:IRequest<Basket>
    {
        public Basket Basketdetails { get; set; }
    }
}
