using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Threading.Tasks;
using WebMVC.Models;

namespace WebMVC.Services
{
    public class CatalogService : ICatalogService
    {
        private string _catalogServiceUrl;
        public CatalogService(IConfiguration config)
        {
            _catalogServiceUrl = config["CatalogUrl"];
        }
        public async Task<CatalogItem> GetCatalogItemAsync(int id)
        {
            var client = new HttpClient();
            string url = _catalogServiceUrl + "/CatalogItems/" + id;
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            CatalogItem item = JsonConvert.DeserializeObject<CatalogItem>(json);

            return item;
        }

        public async Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
            string url =$"{_catalogServiceUrl}/CatalogItems/";
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            IEnumerable<CatalogItem> items = JsonConvert.DeserializeObject<IEnumerable<CatalogItem>>(json);
            return items;
        }
        public async Task<IEnumerable<CatalogType>> GetCatalogTypesAsync()
        {
            var client = new HttpClient();
            string url = $"{_catalogServiceUrl}/CatalogTypes/";
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            IEnumerable<CatalogType> items = JsonConvert.DeserializeObject<IEnumerable<CatalogType>>(json);
            return items;
        }
        public async Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync()
        {
            var client = new HttpClient();
            string url = $"{_catalogServiceUrl}/CatalogBrands/";
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            IEnumerable<CatalogBrand> items = JsonConvert.DeserializeObject<IEnumerable<CatalogBrand>>(json);
            return items;
        }
        public async Task<string> GetCatalogItemImageAsync(IFormFile imageURL)
        {
            var client = new HttpClient();
            var content = new MultipartFormDataContent();
            var fileContent = new StreamContent(imageURL.OpenReadStream());
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
            content.Add(fileContent, "file", imageURL.FileName);

            string url = $"{_catalogServiceUrl}/UploadProductImage";
            var response = await client.PostAsync(url, content);
            var bloburl = await response.Content.ReadAsStringAsync();
            return bloburl;
            
        }
    

        public async Task<CatalogItem> AddCatalogItemAsync(CatalogItem item)
        {
            var client = new HttpClient();
            var content = JsonConvert.SerializeObject(item);
            string url = $"{_catalogServiceUrl}/CatalogItems";
            var response = await client.PostAsJsonAsync(url, item);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject <CatalogItem>(json);
        }
        public async Task<CatalogItem> UpdateCatalogItemAsync(CatalogItem item)
        {
            var client = new HttpClient();
            var content = JsonConvert.SerializeObject(item);
            string url = $"{_catalogServiceUrl}/CatalogItems/"+ item.Id;
            var response = await client.PutAsJsonAsync(url,item);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CatalogItem>(json);
        }
        //PutCatalogItem
        public async void NotifyProductImageUpload(string uri,int id)
        {
            var client = new HttpClient();
            string url = $"{_catalogServiceUrl}/NotifyProductImageUpload";
            BlobInformation blobInformation = new BlobInformation();
            blobInformation.BlobUri = new Uri(uri);
            blobInformation.Id = id;
            await client.PostAsJsonAsync(url, blobInformation);
        }
    }
}
