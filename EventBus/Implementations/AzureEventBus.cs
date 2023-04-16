using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Implementations
{
    public class AzureEventBus : IEventBus
    {
        private string _connectionString;
        private string _topicName;
        string _subscriptionName;
        public AzureEventBus(string serviceBusConnectionString, string topicName, string subscriptionName = "")
        {
            _topicName = topicName;
            _connectionString = serviceBusConnectionString;
            _subscriptionName = subscriptionName;
        }
        public void Publish<TEvent>(TEvent evt) where TEvent : IntegrationEvent
        {
            TopicClient client;
            client = new TopicClient(_connectionString, _topicName);
            string eventName = evt.GetType().Name.Replace("IntegrationEvent", "").ToLower();
            string str = JsonConvert.SerializeObject(evt);
            var message = new Message
            {
                MessageId = Guid.NewGuid().ToString(),
                Body = Encoding.UTF8.GetBytes(str),
                Label = eventName,
            };
            client.SendAsync(message).Wait(); ;
        }
        public void Subscribe<TEvent, TEventHandler>(TEventHandler handler)
                where TEvent : IntegrationEvent
                where TEventHandler : IIntegrationEventHandler<TEvent>
        {
            string eventName = typeof(TEvent).Name.Replace("IntegrationEvent", "").ToLower();
            var _subscriptionClient = new SubscriptionClient(_connectionString, _topicName, _subscriptionName + eventName);

            IEnumerable<RuleDescription> rules = _subscriptionClient.GetRulesAsync().Result;
            if (rules.Where(rule => rule.Name == eventName).Count() == 0)
            {
                _subscriptionClient.AddRuleAsync(new RuleDescription
                {
                    Filter = new CorrelationFilter { Label = eventName },
                    Name = eventName
                }).GetAwaiter().GetResult();
            }
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false,
            };
            _subscriptionClient.RegisterMessageHandler(
               async (message, token) =>
               {
                   // Process the message
                   string str = Encoding.UTF8.GetString(message.Body);
                   Console.WriteLine("In message handler: " + str);
                   var evt = JsonConvert.DeserializeObject<TEvent>(str);
                   handler.Handle(evt);
                   await _subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
               },
               messageHandlerOptions);
        }
        static Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            Console.WriteLine("ERROR: " + args.Exception.Message);
            return Task.FromResult(0);
        }
    }
}
