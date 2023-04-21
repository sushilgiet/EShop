using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EShop.UI.Models;

namespace EShop.UI.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync();
        Task<CatalogItem> GetCatalogItemAsync(int id);
        Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync();
        Task<IEnumerable<CatalogType>> GetCatalogTypesAsync();
        Task<CatalogItem> AddCatalogItemAsync(CatalogItem item);
        Task<string> UploadProductImage(IFormFile imageURL);
        Task<CatalogItem> UpdateCatalogItemAsync(CatalogItem item);
        Task DeleteCatalogItem(int id);
        void ProductImageUploadEvent(string uri, int id);
        Task<PriceChangeRequest> NotifyPriceUpdateAsync(PriceChangeRequest item);
    }
}
