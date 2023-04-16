using Newtonsoft.Json;
using ShoppingBasketAPI.Models;

namespace ShoppingBasketAPI.Repository
{
    public interface IBasketRepository
    {
        Task<bool> DeleteBasketAsync(string id);


        Task<Basket> GetBasketAsync(string buyerId);


        IEnumerable<string> GetBuyers();
      

        Task<Basket> UpdateBasketAsync(Basket basket);
    }
}