using Moq;
using ShoppingBasket.API.Core.Aggregates;
using ShoppingBasket.API.Core.Entities;
using ShoppingBasket.API.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketService.Test.Handlers
{
    public static class MockBasketRepository
    {
        public static Dictionary<string,List<BasketItem>> Baskets { get; set; }
        static MockBasketRepository()
        {
            Baskets = new Dictionary<string, List<BasketItem>>
            {
                {"User1",new List<BasketItem>{ new BasketItem {Id="1",ProductId=1,UnitPrice=199,Quantity=2 }, new BasketItem { Id = "2", ProductId = 2, UnitPrice = 99, Quantity = 1 } } },
                {"User2",new List<BasketItem>{ new BasketItem {Id="2",ProductId=3,UnitPrice=109,Quantity=2 }, new BasketItem { Id = "3", ProductId = 4, UnitPrice = 90, Quantity = 1 } } }
            };

        }
        public static Mock<IBasketRepository> GetBasketRepository()
        {

            var mockrepo = new Mock<IBasketRepository>();
         
            mockrepo.Setup(r => r.GetBasketAsync(It.IsAny<string>())).ReturnsAsync(
               (string key) =>
               {
                   List<BasketItem> basketitems = Baskets.GetValueOrDefault(key);
                   return new Basket { BuyerId=key,Items= basketitems };
               });
            mockrepo.Setup(r => r.DeleteBasketAsync(It.IsAny<string>())).Callback<string>(
                c =>
                {
                    Baskets.Remove(c);
                    Thread.Sleep(1000);
                }
                );
            mockrepo.Setup(r => r.UpdateBasketAsync(It.IsAny<Basket>())).ReturnsAsync(
               (Basket basket) =>
               {
                   Baskets.Remove(basket.BuyerId);
                   Baskets.Add(basket.BuyerId, basket.Items);
                   return basket;
               });
              mockrepo.Setup( r => r.GetBuyers()).Returns(Baskets.Keys);
           
            return mockrepo;
        }
    }
}
