using AutoMapper;
using EcommerceReact.Server.Data;
using EcommerceReact.Server.DTO.Category;
using EcommerceReact.Server.DTO.Product;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;

namespace EcommerceReact.Server.Services
{
    /// <summary>
    /// Represents a repository for managing categories.
    /// </summary>
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryRepository"/> class.
        /// </summary>
        /// <param name="dataContext">The data context.</param>
        /// <param name="mapper">The mapper.</param>
        public CategoryRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new category.
        /// </summary>
        /// <param name="product">The category create DTO.</param>
        /// <returns>A service response containing the created category.</returns>
        public async Task<ServiceResponse<CategoryRetrieveDto>> CreateCategory(CategoryCreateDto product)
        {
            var serviceReponse = new ServiceResponse<CategoryRetrieveDto>();
            var category = _mapper.Map<Category>(product);
            _dataContext.Categories.Add(category);
            await _dataContext.SaveChangesAsync();
            var newCategory = await _dataContext.Categories.SingleOrDefaultAsync(x => x.Id == category.Id);
            serviceReponse.Data = _mapper.Map<CategoryRetrieveDto>(newCategory);
            return serviceReponse;
        }

        /// <summary>
        /// Gets all categories.
        /// </summary>
        /// <returns>A service response containing a list of all categories.</returns>
        public async Task<ServiceResponse<List<CategoryRetrieveDto>>> GetAllCategories()
        {
            var serviceResponse = new ServiceResponse<List<CategoryRetrieveDto>>();
            var categories = await _dataContext.Categories.ToListAsync();
            serviceResponse.Data = categories.Select(x => _mapper.Map<CategoryRetrieveDto>(x)).ToList();
            return serviceResponse;
        }

        /// <summary>
        /// Gets a category by its ID.
        /// </summary>
        /// <param name="id">The category ID.</param>
        /// <returns>A service response containing the category.</returns>
        public async Task<ServiceResponse<Category>> GetCategoryById(int id)
        {
            var serviceResponse = new ServiceResponse<Category>();
            var category = await _dataContext.Categories.SingleOrDefaultAsync(x => x.Id == id);
            serviceResponse.Data = category;
            return serviceResponse;
        }

        /// <summary>
        /// Gets a category by its name.
        /// </summary>
        /// <param name="categoryName">The category name.</param>
        /// <returns>A service response containing the category.</returns>
        public async Task<ServiceResponse<CategoryRetrieveDto>> GetCategoryByName(string categoryName)
        {
            var serviceResponse = new ServiceResponse<CategoryRetrieveDto>();
            var category = await _dataContext.Categories.SingleOrDefaultAsync(x => x.Name == categoryName);
            serviceResponse.Data = _mapper.Map<CategoryRetrieveDto>(category);
            return serviceResponse;
        }
    }
}
