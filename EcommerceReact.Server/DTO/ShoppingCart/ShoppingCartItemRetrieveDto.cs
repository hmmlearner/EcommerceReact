namespace EcommerceReact.Server.DTO.ShoppingCart
{
    public class ShoppingCartItemRetrieveDto
    {
        public int ShoppingCartItemId { get; set; }
        public int Quantity { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;
        public string SKU { get; set; } = string.Empty;
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public double WasPrice { get; set; }
        public string ImageUrl { get; set; } = string.Empty;    

    }
}
