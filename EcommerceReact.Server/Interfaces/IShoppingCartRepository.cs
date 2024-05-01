using EcommerceReact.Server.DTO.Product;
using EcommerceReact.Server.DTO.ShoppingCart;
using EcommerceReact.Server.Models;

namespace EcommerceReact.Server.Interfaces
{
    public interface IShoppingCartRepository
    {
        public Task<ServiceResponse<ShoppingCartRetrieveDto>> AddToShoppingCart(int productId, int quantity);

        public Task<ServiceResponse<ShoppingCartRetrieveDto>> DeleteFromShoppingCart(int productId, int quantity);
        public Task<ServiceResponse<ShoppingCartRetrieveDto>> RetrieveShoppingCart();

        public Task<ServiceResponse<string>> DeleteShoppingCart();

    }
}
