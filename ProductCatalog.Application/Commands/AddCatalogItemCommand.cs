using ProductCatalog.Domain;
using MediatR;
namespace ProductCatalog.Application.Commands
{
    public class AddCatalogItemCommand:IRequest<CatalogItem>
    {
       public CatalogItem Item { get; set; }
    }
}
