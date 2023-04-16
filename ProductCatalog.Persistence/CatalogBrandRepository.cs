using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Persistance
{
    
    class CatalogBrandRepository : GenericRepository<CatalogBrand>, ICatalogBrandRepository
    {
        private readonly ProductCatalogContext _context;

        public CatalogBrandRepository(ProductCatalogContext context) : base(context)
        {
            _context = context;
        }
    }
}
