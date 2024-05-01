using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace EcommerceReact.Server.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        [Required]
        public string SKU { get; set; } = string.Empty;

        [Required]
        [Range(1, 10000)]
        [Display(Name = "Price")]
        public double Price { get; set; } 

        [Range(1, 10000)]
        [Display(Name = "Sale Price")]
        public double SalePrice { get; set; } 

        [Range(1, 10000)]
        [Display(Name = "Was Price")]
        public double WasPrice { get; set; }
        [ValidateNever]
        public string ImageUrl { get; set; } = string.Empty;
        [Required]

        [ForeignKey("CategoryId")]
        public int CategoryId { get; set; }

        //add Inventory with range 0-1000
        [Range(0, 10000)]
        public int Inventory { get; set; }

        //public Category Category { get; set; }
        public DateTime CreatedDate { get; set; }   

    }
}
