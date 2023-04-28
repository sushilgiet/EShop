using Azure;
using Azure.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EShop.UI.Models;
using Microsoft.Identity.Client;

namespace EShop.UI.Services
{
    public class BasketService : IBasketService
    {
        private readonly string _remoteServiceBaseUrl;
        private readonly string _ocpApimSubscriptionKey;
        private readonly IConfiguration _configuration;
        private HttpClient _httpClient;
        public BasketService(IConfiguration config,IHttpClientFactory httpClientFactory)
        {
             _configuration = config;
            _remoteServiceBaseUrl = _configuration["ShoppingCartUrl"];
            _ocpApimSubscriptionKey = _configuration["OcpApimSubscriptionKey"];
            _httpClient = httpClientFactory.CreateClient("EshopHttpClient");
          
        }
        public async Task AddItemToBasket(string userId, BasketItem product)
        {
            ClearAuthorizationHeaders();
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
            ClearAuthorizationHeaders();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("BasketAPI").Result);
            await _httpClient.DeleteAsync($"{_remoteServiceBaseUrl}/basket/{userId}");
        }

        public async Task<Basket> GetBasket(string userId)
        {
            try
            {
                ClearAuthorizationHeaders();
                _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
                _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("BasketAPI").Result);
                var dataString = await _httpClient.GetStringAsync($"{_remoteServiceBaseUrl}/basket/{userId}");
                var response = JsonConvert.DeserializeObject<Basket>(dataString.ToString());
                if (response == null)
                {
                    return response = new Basket()
                    {
                        BuyerId = userId,
                        Items = new List<BasketItem>()
                    };
                }
                return response;
            }
            catch(Exception ex)
            {
                return new Basket()
                {
                    BuyerId = userId,
                    Items = new List<BasketItem>()
                };
            }
           
            return null;
        }

        public async Task<Basket> UpdateBasket(Basket basket)
        {
            ClearAuthorizationHeaders();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _ocpApimSubscriptionKey);
            _httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + GetAccessToken("BasketAPI").Result);
            var json = JsonConvert.SerializeObject(basket);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json"); // use MediaTypeNames.Application.Json in Core 3.0+ and Standard 2.1+

            var response = await _httpClient.PostAsync($"{_remoteServiceBaseUrl}/basket/", stringContent);
           
            return basket; 
        }
        private async Task<string> GetAccessToken(string apiName)
        {

            string clientId = _configuration["AzureAd:ClientId"];
            string tenantId = _configuration["AzureAd:TenantId"];
            string secret = _configuration["AzureAd:Secret"];
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                  .Create(clientId)
                 .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
                 .WithClientSecret(secret)
                 .Build();
            string[] scopes = { _configuration[apiName + ":Scope"] };

            var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
            return result.AccessToken;

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
        private void ClearAuthorizationHeaders()
        {
            _httpClient.DefaultRequestHeaders.Remove("Authorization");
            _httpClient.DefaultRequestHeaders.Remove("Ocp-Apim-Subscription-Key");
        }
    }
}