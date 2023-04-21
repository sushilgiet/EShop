namespace ShoppingBasket.API.Commands
{
    using MediatR;

    public class DeleteBasketItemCommand :IRequest<Unit>
    {
        public string Id { get; set; }
    }
}
