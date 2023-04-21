using MediatR;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Commands
{
    public class AddCatalogItemCommand:IRequest<CatalogItem>
    {
       public CatalogItem Item { get; set; }
    }
}
