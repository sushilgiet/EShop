using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IBasketService
    {
        Task<Models.Basket> GetBasket(string userId);
        Task AddItemToBasket(string userId, BasketItem product);
        Task<Basket> UpdateBasket(Basket basket);
        Task ClearBasket(string userId);
    }

}
