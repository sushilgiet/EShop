namespace ShoppingBasket.API.Queries
{
    using MediatR;
    using ShoppingBasket.API.Core.Aggregates;

    public class GetBasketQuery:IRequest<Basket>
    {
       public string Id { get; set; }
    }
}
