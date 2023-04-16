using ProductCatalog.Application.Contracts;
using ProductCatalog.Application.Contracts.Persistence;
using ProductCatalog.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCatalog.Application
{
    public class CatalogBrandBO : ICatalogBrandBO
    {
        private readonly ICatalogBrandRepository _repo;

        public CatalogBrandBO(ICatalogBrandRepository repository)
        {
            _repo = repository;
        }
        public async Task<IEnumerable<CatalogBrand>> GetCatalogBrands()
        {
            return await _repo.GetAll();
        }

    }
}
