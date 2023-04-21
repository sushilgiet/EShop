using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Domain.Entities
{
    public class CatalogItem:BaseEntity<int>
    {
     
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string? PictureFileName { get; set; }
        public string PictureUrl { get; set; }
        public int CatalogTypeId { get; set; }
        public int CatalogBrandId { get; set; }

    }
}
