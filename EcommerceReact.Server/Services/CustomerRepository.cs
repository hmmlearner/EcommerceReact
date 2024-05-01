using AutoMapper;
using EcommerceReact.Server.Data;
using EcommerceReact.Server.DTO.Customer;
using EcommerceReact.Server.DTO.ShoppingCart;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceReact.Server.Services
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomerRepository( DataContext dataContext, IMapper mapper, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<ServiceResponse<CustomerRetrieveDto>> CreateCustomer(CustomerCreateDto customerdto)
        {
            var serviceReponse = new ServiceResponse<CustomerRetrieveDto>();
            if (await CustomerExists(customerdto.Email))
            {
                serviceReponse.Success = false;
                serviceReponse.StatusCode = 400;
                serviceReponse.StatusMessage = "Email already exists";
                return serviceReponse;
            }
            string password = customerdto.Password;
            var customer = _mapper.Map<Customer>(customerdto);
            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            customer.Password = passwordHash;
            customer.Saltkey = passwordSalt;
        
            _dataContext.Customers.Add(customer);
            await _dataContext.SaveChangesAsync();
            var newCustomer = await _dataContext.Customers.SingleOrDefaultAsync(x => x.Id == customer.Id);
            serviceReponse.Data = _mapper.Map<CustomerRetrieveDto>(newCustomer);
            serviceReponse.Data.Token = CreateToken(newCustomer);
            return serviceReponse;
        }

        public async Task<bool> CustomerExists(string email)
        {
            if(await _dataContext.Customers.AnyAsync(x => x.Email == email))
            {
                return true;
            }
            return false;
        }

        public async Task<ServiceResponse<CustomerRetrieveDto>> CustomerLogin(string email, string password)
        {
            var serviceReponse = new ServiceResponse<CustomerRetrieveDto>();
            var customer = await _dataContext.Customers.SingleOrDefaultAsync(x => x.Email == email);
            if(customer == null) 
            {
                serviceReponse.StatusCode = 401;
                serviceReponse.Success = false;
                serviceReponse.StatusMessage = "User Not Found";// user not found
                return serviceReponse;
            }
            if(VerifyPassword(password, customer.Password, customer.Saltkey))
            {
                serviceReponse.Data = _mapper.Map<CustomerRetrieveDto>(customer);
                serviceReponse.Data.Token = CreateToken(customer);
                serviceReponse.Success = true;
                return serviceReponse;
            }
            serviceReponse.StatusCode = 401;
            serviceReponse.Success = false;
            serviceReponse.StatusMessage = "Invalid Login";
            return serviceReponse;
        }

        public async Task<ServiceResponse<CustomerRetrieveDto>> GetCustomerByEmail(string email)
        {
            var serviceResponse = new ServiceResponse<CustomerRetrieveDto>();
            var customer = await _dataContext.Customers.SingleOrDefaultAsync(x => x.Email == email);
            serviceResponse.Data = _mapper.Map<CustomerRetrieveDto>(customer);
            return serviceResponse;
        }

        public async Task<ServiceResponse<CustomerRetrieveDto>> GetCustomerById(int id)
        {
            var serviceResponse = new ServiceResponse<CustomerRetrieveDto>();
            var customer = await _dataContext.Customers.SingleOrDefaultAsync(x => x.Id == id);
            serviceResponse.Data = _mapper.Map<CustomerRetrieveDto>(customer);
            return serviceResponse;
        }

        public Task<ServiceResponse<Customer>> UpdateCustomer(Customer customer)
        {
            throw new NotImplementedException();
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(Customer customer)
        {

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.Name),
                new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                new Claim(ClaimTypes.Role, customer.IsAdmin.ToString())
             };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var token = new JwtSecurityToken(
                claims:claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
                );



            return new JwtSecurityTokenHandler().WriteToken(token);
            //_configuration
        }


        private int GetCustomerID()
        {
            return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<ServiceResponse<CustomerRetrieveDto>> RetrieveCustomer()
        {
            var customerId = GetCustomerID();
            var serviceResponse = new ServiceResponse<CustomerRetrieveDto>();
            var customer = await _dataContext.Customers.SingleOrDefaultAsync(x => x.Id == customerId);
            serviceResponse.Data = _mapper.Map<CustomerRetrieveDto>(customer);
            return serviceResponse;
        }

        //public void CustomerLogout()
        //{
        //    //code to logout customer

        //}
    }
}
