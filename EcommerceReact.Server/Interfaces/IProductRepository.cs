using EcommerceReact.Server.DTO.Category;
using EcommerceReact.Server.DTO.Product;
using EcommerceReact.Server.Models;
namespace EcommerceReact.Server.Interfaces
{
    public interface IProductRepository
    {
        public Task<ServiceResponse<ProductRetrieveDto>> GetProductById(int id);
        public Task<ServiceResponse<ProductRetrieveDto>> CreateProduct(ProductCreateDto product);

        public Task<ServiceResponse<List<ProductRetrieveDto>>> GetAllProducts();

        public Task<ServiceResponse<List<ProductRetrieveDto>>> GetProductsByCategory(int categoryId);

        //public Task<ServiceResponse<Product>> UpdateProduct(Product product);
        //public Task<ServiceResponse<Product>> DeleteProduct(int id);


    }
}
