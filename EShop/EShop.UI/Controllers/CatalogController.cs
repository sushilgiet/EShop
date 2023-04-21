using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using EShop.UI.Models;
using EShop.UI.Services;
using static System.Net.WebRequestMethods;

namespace EShop.UI.Controllers
{
    [Authorize]
    public class CatalogController : Controller
    {
        ICatalogService _catalogService;
        IConfiguration _configuration;
        IBasketService _basketService;
       

        public CatalogController(ICatalogService service,IConfiguration configuration, IBasketService basketService)
        {
            _catalogService = service;
            _configuration = configuration;
            _basketService = basketService;
         
        }
        public async Task<IActionResult> Index()
        {
            var items = await _catalogService.GetCatalogItemsAsync();
            ViewBag.CatalogBrands = await _catalogService.GetCatalogBrandsAsync();
            ViewBag.CatalogTypes = await _catalogService.GetCatalogTypesAsync();
            ViewBag.CartCount= GetCartTotal().Result;
            return View(items);
        }

        private async Task<int> GetCartTotal()
        {
            
            var baskets = await _basketService.GetBasket(User.Identity.Name);
            return baskets.Items.Count;
        }

        public async Task<IActionResult> Details(int id)
        {
            ViewBag.CatalogBrands = await _catalogService.GetCatalogBrandsAsync();
            ViewBag.CatalogTypes = await _catalogService.GetCatalogTypesAsync();
            var item = await _catalogService.GetCatalogItemAsync(id);
            ViewBag.CartCount = GetCartTotal().Result;
            return View(item);
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var brands = await _catalogService.GetCatalogBrandsAsync();
            var types= await _catalogService.GetCatalogTypesAsync();
            CatalogViewModel vm = new CatalogViewModel(brands, types, new CatalogItem());
            ViewBag.CartCount = GetCartTotal().Result;
            return View(vm);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CatalogViewModel vm, IFormFile imageURL)
        {
            //NotifyProductImageUpload
            vm.CatalogItem.PictureUrl =await _catalogService.UploadProductImage(imageURL);
            vm.CatalogItem.PictureFileName=imageURL.FileName;
            vm.CatalogItem.CatalogBrandId = Convert.ToInt32(vm.CatalogBrandId);
            vm.CatalogItem.CatalogTypeId = Convert.ToInt32(vm.CatalogTypeId);
            CatalogItem item=await _catalogService.AddCatalogItemAsync(vm.CatalogItem);
            _catalogService.ProductImageUploadEvent(item.PictureUrl, item.Id);
            return RedirectToAction("Index", "Catalog");
        }
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var item = await _catalogService.GetCatalogItemAsync(id);
            var brands = await _catalogService.GetCatalogBrandsAsync();
            var types = await _catalogService.GetCatalogTypesAsync();
            CatalogViewModel vm = new CatalogViewModel(brands,types,item);
            ViewBag.CartCount = GetCartTotal().Result;
            return View(vm);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(CatalogViewModel vm, IFormFile imageURL)
        {
            
            if(imageURL != null)
            {
                vm.CatalogItem.PictureUrl = await _catalogService.UploadProductImage(imageURL);
                vm.CatalogItem.PictureFileName = imageURL.FileName;
            }
            
            vm.CatalogItem.CatalogBrandId = Convert.ToInt32(vm.CatalogBrandId);
            vm.CatalogItem.CatalogTypeId = Convert.ToInt32(vm.CatalogTypeId);
            CatalogItem item = await _catalogService.UpdateCatalogItemAsync(vm.CatalogItem);

            if (imageURL != null)
                _catalogService.ProductImageUploadEvent(vm.CatalogItem.PictureUrl, vm.CatalogItem.Id);
            return RedirectToAction("Index", "Catalog");
        }
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _catalogService.DeleteCatalogItem(id);
            return RedirectToAction("Index", "Catalog");
        }
    
        public async Task<IActionResult> NotifyPriceChange(int id)
        {

            var item = await _catalogService.GetCatalogItemAsync(id);
            PriceChangeRequest notification = null;
          
            if (item != null)
            {
                 notification = new PriceChangeRequest
                {
                    ProductName=item.Name,
                    Price = item.Price,
                    Email=string.Empty,
                    UserId="Demouser",
                    ProductId=item.Id
                };
            }
            return View(notification);
        }
        [HttpPost]
        public async Task<IActionResult> NotifyPriceChange(PriceChangeRequest notification)
        {
            string username = "N/A";
            if (User != null && User.Identity!=null && User.Identity.IsAuthenticated)
            {
                notification.UserName = User.Claims.FirstOrDefault(claim => claim.Type == "name")?.Value;
                notification.UserId = User.Identity.Name;
            }
            else
            {
                notification.UserId = notification.Email;
                notification.UserName = notification.Email;
            }
            
               
            await _catalogService.NotifyPriceUpdateAsync(notification);
            return RedirectToAction("Index", "Catalog");
        }
    }
}
