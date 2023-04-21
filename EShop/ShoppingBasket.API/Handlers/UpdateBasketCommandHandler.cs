using MediatR;
using ShoppingBasket.API.Commands;
using ShoppingBasket.API.Core.Aggregates;
using ShoppingBasket.API.Core.Interfaces;

namespace ShoppingBasket.API.Handlers
{
    public class UpdateBasketCommandHandler : IRequestHandler<UpdateBasketItemCommand, Basket>
    {
        private readonly IBasketRepository _repository;
        public UpdateBasketCommandHandler(IBasketRepository repository)
        {
            _repository = repository;
        }

        public async Task<Basket> Handle(UpdateBasketItemCommand request, CancellationToken cancellationToken)
        {
           return await _repository.UpdateBasketAsync(request.Basketdetails);
        }
    }
}
