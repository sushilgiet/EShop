﻿using Newtonsoft.Json;
using ShoppingBasket.API.Core.Aggregates;
using ShoppingBasket.API.Core.Interfaces;
using StackExchange.Redis;

namespace ShoppingBasket.API.Infrastructure
{
    public class RedisBasketRepository : IBasketRepository
    {
        private readonly ConnectionMultiplexer _redis;
        private readonly IDatabase _database;
        public RedisBasketRepository(ConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string id)
        {
            return await _database.KeyDeleteAsync(id);
        }

        public async Task<Basket> GetBasketAsync(string buyerId)
        {
            var data = await _database.StringGetAsync(buyerId);
            if (data.IsNullOrEmpty)
            {
                return null;
            }
            return JsonConvert.DeserializeObject<Basket>(data);
        }

        public IEnumerable<string> GetBuyers()
        {
            var endpoint = _redis.GetEndPoints();
            var server = _redis.GetServer(endpoint.First());
            var data = server.Keys();
            return data?.Select(k => k.ToString());
        }

        public async Task<Basket> UpdateBasketAsync(Basket basket)
        {
            try
            {
                var created = await _database.StringSetAsync(basket.BuyerId, JsonConvert.SerializeObject(basket));
                if (!created)
                {
                    throw new ApplicationException("Basket cannot be saved...");
                }
            }
            catch(Exception ex) 
            {
                throw;
            }
          
            return await GetBasketAsync(basket.BuyerId);
        }
    }

}
