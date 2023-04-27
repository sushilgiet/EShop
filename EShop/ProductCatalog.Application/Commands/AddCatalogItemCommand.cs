using MediatR;
using ProductCatalog.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ProductCatalog.Application.Commands
{
    public class AddCatalogItemCommand:IRequest<CatalogItem>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureFileName { get; set; }
        public string PictureUrl { get; set; }
        public int CatalogTypeId { get; set; }
        public int CatalogBrandId { get; set; }
    }
}
