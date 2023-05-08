
using Azure.Storage.Queues;
using Newtonsoft.Json;
using ProductCatalog.Application.Contracts;
using ProductCatalog.Domain;
using ProductCatalog.Domain.Entities;
using System.Text;

namespace MessageSender
{
    public class MessageSender : IMessageSender
    {
        private string _connectionString;
        private string _queueName;
        public MessageSender(string connectionString, string queueName)
        {
            _connectionString = connectionString;
            _queueName = queueName;
        }
        public async void SendMessageAsync(BlobInformation blobInformation)
        {
            QueueClient queueClient = new QueueClient(_connectionString, _queueName);
            string blobString = ToBase64(blobInformation);
            await queueClient.SendMessageAsync(blobString);
        }

        private string ToBase64(BlobInformation blobInformation)
        {
            string json = JsonConvert.SerializeObject(blobInformation);
            byte[] bytes = Encoding.Default.GetBytes(json);
            return Convert.ToBase64String(bytes);
        }
    }
}