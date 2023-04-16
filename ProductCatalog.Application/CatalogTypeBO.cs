using ProductCatalog.Application.Contracts;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application
{
    public class CatalogTypeBO : ICatalogTypeBO
    {
        private readonly ICatalogTypeRepository _repo;

        public CatalogTypeBO(ICatalogTypeRepository repository)
        {
            _repo = repository;
        }
        public async Task<IEnumerable<CatalogType>> GetCatalogTypes()
        {
            return await _repo.GetAll();
        }

    }
}
