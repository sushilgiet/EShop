using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalog.Domain.Entities
{
    public class CatalogType : BaseEntity<int>
    {
        public string Type { get; set; }
    }
}
