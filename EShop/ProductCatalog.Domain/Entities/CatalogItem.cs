using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace ProductCatalog.Domain.Entities
{
    public class CatalogItem:BaseEntity<int>
    {
        public CatalogItem(string name, string description, decimal price, string pictureFileName, string pictureUrl, int catalogTypeId, int catalogBrandId,int id=0)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Description = description ?? throw new ArgumentNullException(nameof(description));
            Price = price;
            PictureFileName = pictureFileName;
            PictureUrl = pictureUrl ?? throw new ArgumentNullException(nameof(pictureUrl));
            CatalogTypeId = catalogTypeId;
            CatalogBrandId = catalogBrandId;
            Id= id;
        }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(500)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string PictureFileName { get; set; }
        public string PictureUrl { get; set; }
        public int CatalogTypeId { get; set; }
        public int CatalogBrandId { get; set; }
        public void Validate()
        {
            var context = new ValidationContext(this, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(this, context, results, validateAllProperties: true);

            if (results.Count > 0)
            {
                var validationErrors = new List<string>();
                foreach (var validationResult in results)
                {
                    validationErrors.Add(validationResult.ErrorMessage);
                }
                throw new ValidationException(string.Join(",", validationErrors));
            }
        }
    }

}






  