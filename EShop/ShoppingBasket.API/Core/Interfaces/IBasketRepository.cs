using Newtonsoft.Json;
using ShoppingBasket.API.Core.Aggregates;

namespace ShoppingBasket.API.Core.Interfaces
{
    public interface IBasketRepository
    {
        Task<bool> DeleteBasketAsync(string id);


        Task<Basket> GetBasketAsync(string buyerId);


        IEnumerable<string> GetBuyers();


        Task<Basket> UpdateBasketAsync(Basket basket);
    }
}