using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync();
        Task<CatalogItem> GetCatalogItemAsync(int id);
        Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync();
        Task<IEnumerable<CatalogType>> GetCatalogTypesAsync();
        Task<CatalogItem> AddCatalogItemAsync(CatalogItem item);
        Task<string> GetCatalogItemImageAsync(IFormFile imageURL);
        Task<CatalogItem> UpdateCatalogItemAsync(CatalogItem item);
      
        void NotifyProductImageUpload(string uri, int id);
    }
}
