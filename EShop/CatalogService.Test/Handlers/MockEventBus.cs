using Moq;
using EventBus.Abstractions;
using EventBus.Events;

namespace CatalogService.Test.Handlers
{
    public static class MockEventBus
    {
        public static Mock<IEventBus> GetEventBus()
        {
            var mockeventbus = new Mock<IEventBus>();
            mockeventbus.Setup(r => r.Publish(It.IsAny<IntegrationEvent>()));
            return mockeventbus;
        }
    }
}
