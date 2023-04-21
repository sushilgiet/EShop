using ProductCatalog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application.Contracts.Persistence
{

    public interface ICatalogItemRepository : IGenericRepository<CatalogItem>
    {
        
    }

   
}
