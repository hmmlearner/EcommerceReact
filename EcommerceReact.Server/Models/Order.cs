using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcommerceReact.Server.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderNumber { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        [ValidateNever]
        public Customer customer { get; set; }
        [Required]
        public string PhoneNumber { get; set; } = "0123567890";
        [Required]
        public string ShippingName { get;set; }
        [Required]
        public String  ShippingStreetAddress { get; set; }
        [Required]
        public string ShippingCity { get; set; }
        [Required]
        public string ShippingState { get; set; }
        [Required]
        public string ShippingPostCode { get; set; }

        public string ShippingCountry { get; set; } = "Australia";

        public Double OrderTotal { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        public ShippingMethod ShippingMethod { get; set; }

        public List<OrderLine> OrderItems { get; set; }

        public string? SessionId { get; set; } = "";

    }
}
