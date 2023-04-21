using MediatR;
using ShoppingBasket.API.Commands;
using ShoppingBasket.API.Core.Interfaces;

namespace ShoppingBasket.API.Handlers
{
    public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketItemCommand,Unit>
    {
        private readonly IBasketRepository _repository;
        public DeleteBasketCommandHandler(IBasketRepository repository)
        {
            _repository= repository;
        }

        public async Task<Unit> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
        {
            _ = await _repository.DeleteBasketAsync(request.Id);
            return Unit.Value;
        }
    }
}
