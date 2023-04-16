using System.Collections.Generic;
using System.Linq;

namespace WebMVC.Models
{
    public class CatalogViewModel
    {
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Brands { get; set; }
        public List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> Types { get; set; }
        public string CatalogTypeId { get; set; }
        public string CatalogBrandId { get; set; }
        public CatalogItem CatalogItem { get; set; }
        public CatalogViewModel(IEnumerable<CatalogBrand> brands, IEnumerable<CatalogType> types, CatalogItem catalogItem)
        {
           
            CatalogItem = catalogItem;
            Brands= brands.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                         Value = i.Id.ToString(),
                         Text = i.Brand
             }).ToList();
            Types = types.Select(i => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Value = i.Id.ToString(),
                Text = i.Type
            }).ToList();
            CatalogTypeId = CatalogItem == null? "0": CatalogItem.CatalogTypeId.ToString();
            CatalogBrandId = CatalogItem == null ? "0" : CatalogItem.CatalogBrandId.ToString(); ;

        }
        public CatalogViewModel()
        {
           Brands = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
           Types = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();

                
        }
    }
}
