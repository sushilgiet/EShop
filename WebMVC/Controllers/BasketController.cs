using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using WebMVC.Services;
using WebMVC.Models;

namespace WebMVC.Controllers
{
    public class BasketController : Controller
    {

        IBasketService _basketService;
        ICatalogService _catalogService;
        string userId = "DemoUser";
        public BasketController(IBasketService basketService, ICatalogService catalogService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
        }
        public async Task<IActionResult> Index()
        {
          
            var basket = await _basketService.GetBasket(userId);
            ViewBag.CartCount = GetCartTotal().Result;
            return View(basket);
        }
        private async Task<int> GetCartTotal()
        {
            var baskets = await _basketService.GetBasket(userId);
            return baskets.Items.Count;
        }

        public async Task<IActionResult> AddToBasket(int productId)
        {
            CatalogItem catItem = await _catalogService.GetCatalogItemAsync(productId);
            if (catItem != null)
            {
                var userId = "DemoUser";
                var product = new BasketItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    ProductName = catItem.Name,
                    PictureUrl = catItem.PictureUrl,
                    UnitPrice = catItem.Price,
                    ProductId = catItem.Id
                };
              
                await _basketService.AddItemToBasket(userId, product);
               
            }
            return RedirectToAction("Index", "Catalog");
        }

    }
}