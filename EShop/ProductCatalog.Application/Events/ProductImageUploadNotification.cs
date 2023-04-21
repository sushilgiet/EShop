using MediatR;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Events
{
    public class ProductImageUploadEvent : INotification
    {
        public BlobInformation BlobInformation { get; set; }
    }



}
