using EcommerceReact.Server.DTO.Category;
using EcommerceReact.Server.Models;

namespace EcommerceReact.Server.Interfaces
{
    public interface ICategoryRepository
    {
        public Task<ServiceResponse<Category>> GetCategoryById(int id);
        public Task<ServiceResponse<CategoryRetrieveDto>> CreateCategory(CategoryCreateDto product);
        public Task<ServiceResponse<CategoryRetrieveDto>> GetCategoryByName(string categoryName);
        public Task<ServiceResponse<List<CategoryRetrieveDto>>> GetAllCategories();
    }
}
