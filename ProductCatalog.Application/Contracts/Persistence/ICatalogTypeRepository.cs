using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductCatalog.Domain;
namespace ProductCatalog.Application.Contracts.Persistence
{
    
    public interface ICatalogTypeRepository : IGenericRepository<CatalogType>
    {

    }
}
