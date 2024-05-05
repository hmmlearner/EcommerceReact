using EcommerceReact.Server.DTO.Product;
using EcommerceReact.Server.DTO.ShoppingCart;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;
using EcommerceReact.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceReact.Server.Controllers
{
    
    ///<summary>
    /// Controller for managing the shopping cart.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/Cart")]
    public class CartController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public CartController(ILogger<Product> logger, IShoppingCartRepository shoppingCartRepository)
        {
            _logger = logger;
            _shoppingCartRepository = shoppingCartRepository;
        }

        /// <summary>
        /// Adds a product to the shopping cart.
        /// </summary>
        /// <param name="productId">The ID of the product to add.</param>
        /// <param name="quantity">The quantity of the product to add.</param>
        /// <returns>The updated shopping cart.</returns>
        [Route("AddToCart")]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ShoppingCartRetrieveDto>>> AddToCart(int productId, int quantity)
        {
            try
            {
                var addToCartReponse = await _shoppingCartRepository.AddToShoppingCart(productId, quantity);
                return (addToCartReponse == null) ? BadRequest("Couldn't add item to cart") : Ok(addToCartReponse);

            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't add item to cart {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves the shopping cart.
        /// </summary>
        /// <returns>The shopping cart.</returns>
        [Route("RetrieveCart")]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<ShoppingCartRetrieveDto>>> RetrieveCart()
        {
            try
            {
                var cartReponse = await _shoppingCartRepository.RetrieveShoppingCart();
                return (cartReponse == null) ? BadRequest("Couldn't retrieve cart") : Ok(cartReponse);

            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't retrieve cart {ex.Message}");
            }
        }

        /// <summary>
        /// Deletes an item from the shopping cart.
        /// </summary>
        /// <param name="productId">The ID of the product to delete.</param>
        /// <param name="quantity">The quantity of the product to delete.</param>
        /// <returns>The updated shopping cart.</returns>
        [Route("DeleteItem")]
        [HttpDelete]
        public async Task<ActionResult<ServiceResponse<ShoppingCartRetrieveDto>>> DeleteItemFromCart(int productId, int quantity)
        {
            try
            {
                var cartReponse = await _shoppingCartRepository.DeleteFromShoppingCart(productId, quantity);
                return (cartReponse == null) ? BadRequest("Couldn't retrieve cart") : Ok(cartReponse);

            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't retrieve cart {ex.Message}");
            }
        }
    }
}
