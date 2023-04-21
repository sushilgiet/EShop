using Microsoft.EntityFrameworkCore;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICatalogItemRepository = ProductCatalog.Application.Contracts.Persistence.ICatalogItemRepository;

namespace ProductCatalog.Persistance
{
    class CatalogItemRepository : GenericRepository<CatalogItem>, ICatalogItemRepository
    {
        private readonly ProductCatalogContext _context;

        public CatalogItemRepository(ProductCatalogContext context) : base(context)
        {
            _context = context;
        }
        
    }
}
