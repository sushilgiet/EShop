using ProductCatalog.Domain;

namespace ProductCatalog.Application.Contracts
{
    public interface ICatalogBrandBO
    {
        Task<IEnumerable<CatalogBrand>> GetCatalogBrands();
    }
}