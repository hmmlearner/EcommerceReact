using EcommerceReact.Server.DTO.Category;
using EcommerceReact.Server.DTO.Product;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;
using EcommerceReact.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceReact.Server.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private readonly ILogger<Category> _logger;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IProductRepository _productRepository;

        public CategoryController(ILogger<Category> logger, ICategoryRepository categoryRepository, IProductRepository productRepository)
        {
            _logger = logger;
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
        }


       


        [Route("{name}")]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<CategoryRetrieveDto>>> GetCategory(string name)
        {
            try
            {
                var categoryResponse = await _categoryRepository.GetCategoryByName(name);
                return categoryResponse == null ? NotFound() : Ok(categoryResponse);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to retrieve category {ex.Message}");
                return BadRequest($"Failed to retrieve category {ex.Message}");
            }
        }

        [Route("{name}/products")]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<CategoryProductsRetrieveDto>>> GetCategoryProducts(string name)
        {
            try
            {
                var categoryResponse = await _categoryRepository.GetCategoryByName(name);
                if(categoryResponse == null) 
                    return NotFound();
                int categoryid = categoryResponse.Data.Id;
                var categoryProductsResponse = await _productRepository.GetProductsByCategory(categoryid);
                //var categoryProducts = new CategoryProductsRetrieveDto()
                //{
                //    Id = categoryid,
                //    Name = categoryResponse.Data.Name,
                //    DisplayOrder = categoryResponse.Data.DisplayOrder,
                //    Products = categoryProductsResponse.Data
                //};

                return categoryProductsResponse == null ? NotFound() : Ok(categoryProductsResponse);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to retrieve category {ex.Message}");
                return BadRequest($"Failed to retrieve category {ex.Message}");
            }
        }


        [Route("All")]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<CategoryRetrieveDto>>> GetAllCategories()
        {
            try
            {
                var categoriesResponse = await _categoryRepository.GetAllCategories();

                return categoriesResponse == null ? NotFound() : Ok(categoriesResponse);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Failed to retrieve category {ex.Message}");
                return BadRequest($"Failed to retrieve category {ex.Message}");
            }
        }


        [HttpPost]
        public async Task<ActionResult<ServiceResponse<CategoryRetrieveDto>>> CreateCategory([FromBody] CategoryCreateDto categorydto)
        {
            try
            {
                var newCategoryReponse = await _categoryRepository.CreateCategory(categorydto);
                return (newCategoryReponse == null) ? BadRequest("Couldn't create Product") : Ok(newCategoryReponse);

            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't create product {ex.Message}");
            }
        }
    }
}
