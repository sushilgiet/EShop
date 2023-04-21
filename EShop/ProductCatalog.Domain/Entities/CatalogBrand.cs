using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Domain.Entities
{
    public class CatalogBrand : BaseEntity<int>
    {
      
        public string Brand { get; set; }
    }
}
