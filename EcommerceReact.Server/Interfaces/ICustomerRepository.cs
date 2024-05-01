using EcommerceReact.Server.DTO.Customer;
using EcommerceReact.Server.DTO.ShoppingCart;
using EcommerceReact.Server.Models;

namespace EcommerceReact.Server.Interfaces
{
    public interface ICustomerRepository
    {
        public Task<ServiceResponse<CustomerRetrieveDto>> GetCustomerById(int id);
        public Task<ServiceResponse<CustomerRetrieveDto>> GetCustomerByEmail(string email);
        public Task<ServiceResponse<CustomerRetrieveDto>> CreateCustomer(CustomerCreateDto customer);

        public Task<ServiceResponse<Customer>> UpdateCustomer(Customer customer);
        public Task<ServiceResponse<CustomerRetrieveDto>> CustomerLogin(string email, string password);

        //public void CustomerLogout();

        public Task<ServiceResponse<CustomerRetrieveDto>> RetrieveCustomer();
        public Task<bool> CustomerExists(string email);

    }
}
