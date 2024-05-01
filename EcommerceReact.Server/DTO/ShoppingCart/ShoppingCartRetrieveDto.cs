using EcommerceReact.Server.Models;

namespace EcommerceReact.Server.DTO.ShoppingCart
{
    public class ShoppingCartRetrieveDto
    {
        public int ProductTotal { get; set; }
        public double CartTotal { get; set; }

        public int ShoppingCartId { get; set; }
        //public string Name { get; set; }
        //public string Email { get; set; }
        //public CustomerType IsAdmin { get; set; } = CustomerType.Customer;
        //public string StreetAddress { get; set; } = string.Empty;
        //public string City { get; set; } = string.Empty;
        //public string State { get; set; } = string.Empty;
        //public string PostalCode { get; set; } = string.Empty;

        public List<ShoppingCartItemRetrieveDto> Items { get; set;}
    }
}
