using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Persistance
{
    class CatalogTypeRepository : GenericRepository<CatalogType>, ICatalogTypeRepository
    {
        private readonly ProductCatalogContext _context;

        public CatalogTypeRepository(ProductCatalogContext context) : base(context)
        {
            _context = context;
        }
    }
}
