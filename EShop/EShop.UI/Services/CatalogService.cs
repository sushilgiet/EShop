﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using EShop.UI.Models;
using static System.Formats.Asn1.AsnWriter;

namespace EShop.UI.Services
{
    public class CatalogService : ICatalogService
    {
        private string _catalogServiceUrl;
        private string _ocpApimSubscriptionKey;
        private IConfiguration _configuration;
        private int _retryCount;
        public CatalogService(IConfiguration config)
        {
            _configuration= config;
            _catalogServiceUrl = _configuration["CatalogUrl"];
            _ocpApimSubscriptionKey = _configuration["OcpApimSubscriptionKey"];
            _retryCount = 3;
        }
        public async Task<CatalogItem> GetCatalogItemAsync(int id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("CatalogAPI").Result);
            string url = _catalogServiceUrl + "/CatalogItems/" + id;
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            CatalogItem item = JsonConvert.DeserializeObject<CatalogItem>(json);

            return item;
        }
       
        public async Task<IEnumerable<CatalogItem>> GetCatalogItemsAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            string accessToken = GetAccessToken("CatalogAPI").Result;
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + accessToken);
            client.DefaultRequestHeaders.CacheControl = new CacheControlHeaderValue
            {
                NoCache = true
            };
            string url =$"{_catalogServiceUrl}/CatalogItems/";
            int retry = 0;
            IEnumerable<CatalogItem> items = null;
            while (retry < _retryCount)
            {
                try
                {
                    var response = await client.GetAsync(url);
                    var json = await response.Content.ReadAsStringAsync();
                    items = JsonConvert.DeserializeObject<IEnumerable<CatalogItem>>(json);
                    retry++;
                }
                catch(Exception ex)
                {
                    if(retry==3)
                    throw ex;
                }
            }
            
            return items;
        }
        public async Task<IEnumerable<CatalogType>> GetCatalogTypesAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("CatalogAPI").Result);
            string url = $"{_catalogServiceUrl}/CatalogTypes/";
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            IEnumerable<CatalogType> items = JsonConvert.DeserializeObject<IEnumerable<CatalogType>>(json);
            return items;
        }
        public async Task<IEnumerable<CatalogBrand>> GetCatalogBrandsAsync()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("CatalogAPI").Result);
            string url = $"{_catalogServiceUrl}/CatalogBrands/";
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            IEnumerable<CatalogBrand> items = JsonConvert.DeserializeObject<IEnumerable<CatalogBrand>>(json);
            return items;
        }
        public async Task<string> UploadProductImage(IFormFile imageURL)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("CatalogAPI").Result);
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
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("CatalogAPI").Result);
            var content = JsonConvert.SerializeObject(item);
            string url = $"{_catalogServiceUrl}/CatalogItems";
            var response = await client.PostAsJsonAsync(url, item);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject <CatalogItem>(json);
        }
        public async Task<CatalogItem> UpdateCatalogItemAsync(CatalogItem item)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("CatalogAPI").Result);
            var content = JsonConvert.SerializeObject(item);
            string url = $"{_catalogServiceUrl}/CatalogItems/"+ item.Id;
            var response = await client.PutAsJsonAsync(url,item);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<CatalogItem>(json);
        }
        //PutCatalogItem
        public async void ProductImageUploadEvent(string uri,int id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("CatalogAPI").Result);
            string url = $"{_catalogServiceUrl}/ProductImageUploadEvent";
            BlobInformation blobInformation = new BlobInformation();
            blobInformation.BlobUri = new Uri(uri);
            blobInformation.Id = id;
            await client.PostAsJsonAsync(url, blobInformation);
        }
        public async Task DeleteCatalogItem(int id)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("CatalogAPI").Result);
            string url = $"{_catalogServiceUrl}/CatalogItems/{id}";
            var response = await client.DeleteAsync(url);
           
        }
        private string GetAccessToken()
        {
            string key = _configuration["jwttokenkey"];
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var header = new JwtHeader(credentials);
            var payload = new JwtPayload
               {
                      { "exp", 1714202031},
                      { "iss", "Issuer 1" },
                      { "aud", "Audience 1"},
                      { "claim1", "value1"},
               };

            var secToken = new JwtSecurityToken(header, payload);
            var handler = new JwtSecurityTokenHandler();
            var accesstoken = handler.WriteToken(secToken);
            return accesstoken;
        }

        private async Task<string> GetAccessToken(string apiName)
        {
            
            string clientId = _configuration["AzureAd:ClientId"];
            string tenantId = _configuration["AzureAd:TenantId"];
            string secret= _configuration["AzureAd:Secret"];
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                  .Create(clientId)
                 .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
                 .WithClientSecret(secret)
                 .Build();
            string[] scopes = { _configuration[apiName + ":Scope"] };
      
            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;

        }

        public async Task<PriceChangeRequest> NotifyPriceUpdateAsync(PriceChangeRequest item)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("NotificationAPI").Result);
            var content = JsonConvert.SerializeObject(item);
            string url = $"{_configuration["NotificationUrl"]}/PriceUpdateNotification";
            var response = await client.PostAsJsonAsync(url, item);
            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<PriceChangeRequest>(json);
        }
    }
}
//var retryPolicy = Policy
//        .Handle<HttpRequestException>()
//        .WaitAndRetryAsync(retryCount, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

//using var client = new HttpClient();
//var response = await retryPolicy.ExecuteAsync(async () => await client.SendAsync(request));

//if (response.IsSuccessStatusCode)
//{
//    return response;
//}