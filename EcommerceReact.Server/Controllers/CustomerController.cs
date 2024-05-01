using EcommerceReact.Server.DTO.Customer;
using EcommerceReact.Server.DTO.ShoppingCart;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;
using EcommerceReact.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace EcommerceReact.Server.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/customer")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<Customer> _logger;
        private readonly ICustomerRepository _customerRepository;

        public CustomerController(ILogger<Customer> logger, ICustomerRepository customerRepository)  
        { 
            _logger = logger;
            _customerRepository = customerRepository;
        }

        [HttpPost]
        [Route("CreateCustomer")]
        public async Task<ActionResult<ServiceResponse<CustomerRetrieveDto>>> CreateCustomer([FromBody] CustomerCreateDto customer)
        {
            try
            {
                var newCustomerReponse = await _customerRepository.CreateCustomer(customer);
                return (newCustomerReponse == null) ? BadRequest("Couldn't create customer") : Ok(newCustomerReponse);

            }
            catch (Exception ex) {
                return BadRequest($"Couldn't create customer {ex.Message}");
            }
        }

        [HttpPost]
        [Route("Login")]
        //prathimac@gmail.com, Test_12
        //public async Task<ActionResult<ServiceResponse<CustomerRetrieveDto>>> CustomerLogin(string username, string password)
        public async Task<ActionResult<ServiceResponse<CustomerRetrieveDto>>> CustomerLogin([FromBody] LoginFormDto m)
        {
            if(!m.IsValid())
            {
                return BadRequest("Invalid login data");
            }
            try
            {
                string username = m.Username;
                string password = m.Password;
                var loginCustomerReponse = await _customerRepository.CustomerLogin(username, password);
                return (loginCustomerReponse == null) ? Unauthorized("Invalid Credentials") : Ok(loginCustomerReponse);

            }
            catch (Exception ex)
            {
                return Unauthorized($"Invalid Credentials { ex.Message}");
            }
        }
        [Route("retrievecustomer")]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<CustomerRetrieveDto>>> RetrieveCustomer()
        {
            try
            {
                var customerReponse = await _customerRepository.RetrieveCustomer();
                return (customerReponse == null) ? BadRequest("Couldn't retrieve cart") : Ok(customerReponse);

            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't retrieve cart {ex.Message}");
            }
        }

        ////Create CustomerLogout API method
        //[HttpPost]
        //[Route("Logout")]
        //public async IActionResult CustomerLogout()
        //{
        //    try
        //    {
        //        var logoutCustomerReponse = await _customerRepository.CustomerLogout();
        //        return Ok(new { message = "Logout successful" });

        //    }
        //    catch (Exception ex)
        //    {
        //        return Unauthorized($"Invalid Credentials { ex.Message}");
        //    }
        //}

    }
}
