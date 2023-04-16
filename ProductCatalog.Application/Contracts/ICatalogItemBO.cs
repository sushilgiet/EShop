using ProductCatalog.Domain;

namespace ProductCatalog.Application.Contracts
{
    public interface ICatalogItemBO
    {
        Task<CatalogItem> Add(CatalogItem item);
        Task Delete(int id);
        Task<CatalogItem> GetCatalogItem(int id);
        Task<IEnumerable<CatalogItem>> GetCatalogItems();
        Task Update(CatalogItem item);
    }
}