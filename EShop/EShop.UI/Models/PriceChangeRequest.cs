using System.ComponentModel.DataAnnotations;

namespace EShop.UI.Models
{
    public class PriceChangeRequest
    {
       
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        [Required]
        [Range(0.1, double.MaxValue, ErrorMessage = "The Price field must be greater than 0.")]
        public decimal NewPrice { get; set; }
        public string UserId { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public int ProductId { get; set; }
        public string UserName { get; set; }
    }

   
}
