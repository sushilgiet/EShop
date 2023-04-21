using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Abstractions
{
    public interface IEventBus
    {
        void Publish<T>(T e) where T : IntegrationEvent;
        void Subscribe<T, TH>(TH eventHandler)
                where T : IntegrationEvent
                where TH : IIntegrationEventHandler<T>;
    }
}
