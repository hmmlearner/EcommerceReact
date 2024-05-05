using EcommerceReact.Server.DTO.Customer;
using EcommerceReact.Server.DTO.Product;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;
using EcommerceReact.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceReact.Server.Controllers
{

    /// <summary>
    /// Controller for managing product-related operations for setting up products.
    /// </summary>
    [ApiController]
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly ILogger _logger;
        private readonly IProductRepository _productRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProductController"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="productRepository">The product repository.</param>
        public ProductController(ILogger<Product> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

        /// <summary>
        /// Retrieves a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product information.</returns>
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<ProductRetrieveDto>>> GetProduct(int id)
        {
            try
            {
                var productResponse = await _productRepository.GetProductById(id);
                return productResponse == null ? NotFound() : Ok(productResponse);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to retrieve product {ex.Message}");
                return BadRequest($"Failed to retrieve product {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new product.
        /// </summary>
        /// <param name="productdto">The product data.</param>
        /// <returns>The created product information.</returns>
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<ProductRetrieveDto>>> CreateProduct([FromBody] ProductCreateDto productdto)
        {
            try
            {
                var newProductReponse = await _productRepository.CreateProduct(productdto);
                return (newProductReponse == null) ? BadRequest("Couldn't create Product") : Ok(newProductReponse);

            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't create product {ex.Message}");
            }
        }
    }
}
