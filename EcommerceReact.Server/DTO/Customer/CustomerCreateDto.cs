using AutoMapper;
using EcommerceReact.Server.Models;
using System.ComponentModel.DataAnnotations;
//using AutoMapper.Configuration.Annotations

namespace EcommerceReact.Server.DTO.Customer
{

    public class CustomerCreateDto
    {
        [Required]
        public string Name { get; set; }
        public string Email { get; set; }
        
        public string Password { get; set; } = string.Empty;
        public CustomerType IsAdmin { get; set; } = CustomerType.Customer;
        public string StreetAddress { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get;set; } = "Australia";
        public string Phone { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
    }
}
