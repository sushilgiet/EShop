using MediatR;

namespace ProductCatalog.Application.Commands
{
    public class DeleteCatalogItemCommand: IRequest<Unit>
    {
       public int Id { get; set; }
    }
}
