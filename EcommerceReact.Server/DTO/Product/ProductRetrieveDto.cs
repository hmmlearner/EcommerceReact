using EcommerceReact.Server.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceReact.Server.DTO.Product
{
    public class ProductRetrieveDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;

        public double Price { get; set; } 

        public double SalePrice { get; set; } 

        public double WasPrice { get; set; } 
        public string ImageUrl { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string Category { get; set; } = string.Empty;
        public int Inventory { get; set; }

    }
}
