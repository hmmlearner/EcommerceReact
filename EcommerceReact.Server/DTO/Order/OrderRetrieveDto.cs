using EcommerceReact.Server.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace EcommerceReact.Server.DTO.Order
{
    public class OrderRetrieveDto
    {
        public int OrderNumber { get; set; }
        public int CustomerId { get; set; }

        [Required]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required]
        public string ShippingName { get; set; } = string.Empty;
        [Required]
        public String ShippingStreetAddress { get; set; } = string.Empty;
        [Required]
        public string ShippingCity { get; set; } = string.Empty;
        [Required]
        public string ShippingState { get; set; } = string.Empty;
        [Required]
        public string ShippingPostCode { get; set; } = string.Empty;

        public string ShippingCountry { get; set; } = "Australia";

        public Double OrderTotal { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public ShippingMethod ShippingMethod { get; set; }

        public List<OrderLine> OrderItems { get; set; } = new List<OrderLine>();
    }
}
