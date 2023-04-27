using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Application.Commands
{
    public class UpdateCatalogItemCommand:IRequest<Unit>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureFileName { get; set; }
        public string PictureUrl { get; set; }
        public int CatalogTypeId { get; set; }
        public int CatalogBrandId { get; set; }
        public int Id { get; set; }
    }
}
