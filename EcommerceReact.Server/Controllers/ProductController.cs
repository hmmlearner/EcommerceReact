using EcommerceReact.Server.DTO.Customer;
using EcommerceReact.Server.DTO.Product;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;
using EcommerceReact.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceReact.Server.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : Controller
    {
        private readonly ILogger _logger;
        private readonly IProductRepository _productRepository;

        public ProductController(ILogger<Product> logger, IProductRepository productRepository)
        {
            _logger = logger;
            _productRepository = productRepository;
        }

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
