using EventBus.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventBus.Abstractions
{
    public interface IIntegrationEventHandler
    {
    }
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        void Handle(TIntegrationEvent evt);
    }

}
