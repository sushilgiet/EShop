using MediatR;
using ShoppingBasket.API.Core.Aggregates;
using ShoppingBasket.API.Core.Interfaces;
using ShoppingBasket.API.Queries;

namespace ShoppingBasket.API.Handlers
{
    public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Basket>
    {
        private readonly IBasketRepository _repository;

        public GetBasketQueryHandler(IBasketRepository repository)
        {
            _repository = repository;
        }

        public async Task<Basket> Handle(GetBasketQuery request, CancellationToken cancellationToken)
        {
          return await  _repository.GetBasketAsync(request.Id);
        }
    }
}
