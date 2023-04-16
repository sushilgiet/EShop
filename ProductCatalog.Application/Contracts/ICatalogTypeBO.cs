using ProductCatalog.Domain;

namespace ProductCatalog.Application.Contracts
{
    public interface ICatalogTypeBO
    {
        Task<IEnumerable<CatalogType>> GetCatalogTypes();
    }
}