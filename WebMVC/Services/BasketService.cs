using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class BasketService : IBasketService
    {
        private readonly string _remoteServiceBaseUrl;
        public BasketService(IConfiguration config)
        {
            _remoteServiceBaseUrl = config["ShoppingCartUrl"];
        }
        public async Task AddItemToBasket(string userId, BasketItem product)
        {
            var basket = await GetBasket(userId);
            var basketItem = basket.Items
                .Where(p => p.ProductId == product.ProductId)
                .FirstOrDefault();
            if (basketItem == null)
            {
                basket.Items.Add(product);
            }
            else
            {
                basketItem.Quantity += 1;
            }
            await UpdateBasket(basket);
        }

        public async Task ClearBasket(string userId)
        {
            var client = new HttpClient();
            await client.DeleteAsync($"{_remoteServiceBaseUrl}/basket/{userId}");
        }

        public async Task<Basket> GetBasket(string userId)
        {
            var client = new HttpClient();
            var dataString = await client.GetStringAsync($"{_remoteServiceBaseUrl}/basket/{userId}");
            var response = JsonConvert.DeserializeObject<Basket>(dataString.ToString());
            if (response == null)
            {
                response = new Basket()
                {
                    BuyerId = userId,
                    Items = new List<BasketItem>()
                };
            }
            return response;
        }

        public async Task<Basket> UpdateBasket(Basket basket)
        {
            var client = new HttpClient();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(basket), System.Text.Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_remoteServiceBaseUrl}/basket/", content);
            response.EnsureSuccessStatusCode();
            return basket;
        }

    }
}