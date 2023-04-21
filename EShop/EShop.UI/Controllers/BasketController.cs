using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using EShop.UI.Services;
using EShop.UI.Models;
using Microsoft.AspNetCore.Authorization;

namespace EShop.UI.Controllers
{
    [Authorize]
    public class BasketController : Controller
    {

        IBasketService _basketService;
        ICatalogService _catalogService;
      
        public BasketController(IBasketService basketService, ICatalogService catalogService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
        }
        public async Task<IActionResult> Index()
        {
            var user = User.Identity.Name;
            var basket = await _basketService.GetBasket(user);
            ViewBag.CartCount = GetCartTotal().Result;
            return View(basket);
        }
        private async Task<int> GetCartTotal()
        {
            var user = User.Identity.Name;
            var baskets = await _basketService.GetBasket(user);
            return baskets.Items.Count;
        }

        public async Task<IActionResult> AddToBasket(int productId)
        {
            CatalogItem catItem = await _catalogService.GetCatalogItemAsync(productId);
            if (catItem != null)
            {
                var user = User.Identity.Name;
                var product = new BasketItem()
                {
                    Id = Guid.NewGuid().ToString(),
                    Quantity = 1,
                    ProductName = catItem.Name,
                    PictureUrl = catItem.PictureUrl,
                    UnitPrice = catItem.Price,
                    ProductId = catItem.Id
                };
              
                await _basketService.AddItemToBasket(user, product);
               
            }
            return RedirectToAction("Index", "Catalog");
        }
        public async Task<IActionResult> ClearBasket()
        {
            var user = User.Identity.Name;
            await _basketService.ClearBasket(user);

            
            return RedirectToAction("Index", "Catalog");
        }
    }
}