using EcommerceReact.Server.DTO.Order;
using EcommerceReact.Server.DTO.ShoppingCart;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;
using EcommerceReact.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceReact.Server.Controllers
{
    
    ///<summary>
    /// Controller for managing orders.
    /// </summary>
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly ILogger _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderController(ILogger<Order> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        /// <summary>
        /// Endpoint for confirming an order.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <param name="sessionid">The session ID.</param>
        /// <returns>The response containing the order confirmation details.</returns>
        [Route("OrderConfirmation")]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<OrderRetrieveDto>>> OrderConfirmation(int orderNumber, string sessionid)
        {
            try
            {
                var orderConfirmationReponse = await _orderRepository.OrderConfirmation(orderNumber, sessionid);
                return (orderConfirmationReponse == null) ? BadRequest("Couldn't submit order") : Ok(orderConfirmationReponse);

            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't submit order {ex.InnerException}");
            }
        }

        /// <summary>
        /// Endpoint for submitting an order.
        /// </summary>
        /// <returns>The response containing the payment submission details.</returns>
        [Route("OrderSubmit")]
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<string>>> OrderSubmit()
        {
            try
            {
                var orderConfirmationReponse = await _orderRepository.SubmitPayment();
                if (orderConfirmationReponse.Success == true)
                {
                    HttpContext.Response.Headers.Add("Location", orderConfirmationReponse.Data.ToString());
                    return Ok(orderConfirmationReponse);
                }
                else
                {
                    return BadRequest(orderConfirmationReponse);
                }

            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't submit order {ex.InnerException}");
            }
        }

        /// <summary>
        /// Endpoint for retrieving an order.
        /// </summary>
        /// <param name="orderNumber">The order number.</param>
        /// <returns>The response containing the retrieved order details.</returns>
        [Route("RetrieveOrder")]
        [HttpGet]
        public async Task<ActionResult<ServiceResponse<OrderRetrieveDto>>> RetrieveOrder(int orderNumber)
        {
            try
            {
                var orderReponse = await _orderRepository.RetrieveOrder(orderNumber);
                return (orderReponse == null) ? BadRequest("Couldn't retrieve order") : Ok(orderReponse);

            }
            catch (Exception ex)
            {
                return BadRequest($"Couldn't retrieve order {ex.Message}");
            }
        }
    }
}
