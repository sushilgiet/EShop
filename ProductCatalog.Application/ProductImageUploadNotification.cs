using MediatR;
using ProductCatalog.Domain;

namespace ProductCatalog.Application
{
    public class ProductImageUploadNotification : INotification
    {
       public BlobInformation BlobInformation { get; set; }
    }

   

}
