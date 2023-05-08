using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using ShoppingBasket.API.Core.Interfaces;

namespace BasketService.Test
{
    public class MockRedisBasketRepository
    {
        public MockRedisBasketRepository()
        {
           
        }
        public static Mock<IBasketRepository> GetCatalogBrandRepository()
        {
            var mockDatabase = new Mock<StackExchange.Redis.IDatabase>();

            var mockMultiplexer = new Mock<StackExchange.Redis.IConnectionMultiplexer>();

            mockMultiplexer
                .Setup(_ => _.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(mockDatabase.Object);

            var mockrepo = new Mock<IBasketRepository>();
            mockrepo.Setup(r => r.GetBasketAsync(It.IsAny<string>()));
            mockrepo.Setup(r => r.GetBasketAsync(It.IsAny<string>()));
            mockrepo.Setup(r => r.GetBasketAsync(It.IsAny<string>()));
            mockrepo.Setup(r => r.GetBasketAsync(It.IsAny<string>()));
            return mockrepo;
        }

        internal static Mock<IBasketRepository>? GetBasketRepository()
        {
            throw new NotImplementedException();
        }
    }
}
