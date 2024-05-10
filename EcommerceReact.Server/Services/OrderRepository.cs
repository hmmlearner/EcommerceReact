using AutoMapper;
using EcommerceReact.Server.Data;
using EcommerceReact.Server.DTO.Order;
using EcommerceReact.Server.DTO.ShoppingCart;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Stripe;
using Stripe.Checkout;
using Azure;
using Microsoft.AspNetCore.Mvc;
using Stripe.Climate;
using Stripe.Issuing;

namespace EcommerceReact.Server.Services
{
    
    public class OrderRepository : IOrderRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        public OrderRepository(DataContext dataContext, IMapper mapper, IHttpContextAccessor httpContextAccessor, IShoppingCartRepository shoppingCartRepository)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _shoppingCartRepository = shoppingCartRepository;
        }

        public DataContext _dataContext { get; }
        public IMapper _mapper { get; }

        private int GetCustomerID()
        {
            return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        
        public async Task<ServiceResponse<string>> SubmitPayment()
        {
            var customerid = GetCustomerID();
            var shoppingCart = await _dataContext.ShoppingCarts
                                .Include(c => c.Items)
                                .ThenInclude(p => p.Product)
                                .Include(c => c.customer)
                                .FirstOrDefaultAsync(c => c.CustomerId == customerid);

            if (shoppingCart == null)
            {
                return new ServiceResponse<string>
                {
                    StatusCode = 400,
                    Success = false,
                    StatusMessage = "Cart is empty. An error occurred while processing the payment."
                };
            }

            var order = new Models.Order
            {
                OrderItems = new List<OrderLine>(),
                CustomerId = customerid,
                ShippingName = shoppingCart.customer.Name,
                ShippingStreetAddress = shoppingCart.customer.StreetAddress,
                ShippingCity = shoppingCart.customer.City,
                ShippingState = shoppingCart.customer.State,
                ShippingPostCode = shoppingCart.customer.PostalCode,
                OrderTotal = shoppingCart.CartTotal
            };
            _dataContext.Orders.Add(order);

            foreach (var item in shoppingCart.Items)
            {
                var orderItem = new OrderLine
                {
                    ProductId = item.ProductId,
                    count = item.Quantity,
                    price = item.Product.SalePrice
                };
                order.OrderItems.Add(orderItem);
            }

            _dataContext.SaveChanges();



            //stripe settings 
            var domain = "https://localhost:5173/";
            var checkoutSessionId = "{CHECKOUT_SESSION_ID}";
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string>
                {
                  "card",
                },
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
                SuccessUrl = domain + $"paymentconfirmation?orderId={order.OrderNumber}&session_id={checkoutSessionId}",
                CancelUrl = domain + $"paymenterror?orderId={order.OrderNumber}",
            };

            foreach (var item in shoppingCart.Items)
            {

                var sessionLineItem = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.Product.SalePrice * 100),//20.00 -> 2000
                        Currency = "aud",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Product.Title
                        },

                    },
                    Quantity = item.Quantity,
                };
                options.LineItems.Add(sessionLineItem);

            }

            var service = new SessionService();
            Session session = service.Create(options);

            
            return new ServiceResponse<string>
            {
                Data = session.Url,
                StatusCode = 303,
                Success = true,
                //StatusMessage = "Cart is empty. An error occurred while processing the payment."
            };

        }
        

        /*
        public async Task<ServiceResponse<string>> SubmitPayment()
        {
            var customerid = GetCustomerID();
            var shoppingCart = await _dataContext.ShoppingCarts
                                .Include(c => c.Items)
                                .ThenInclude(p => p.Product)
                                .Include(c => c.customer)
                                .FirstOrDefaultAsync(c => c.CustomerId == customerid);
            var paymentIntentService = new PaymentIntentService();
            var paymentIntent = paymentIntentService.Create(new PaymentIntentCreateOptions
            {
                Amount = (long)Math.Round(shoppingCart.CartTotal),
                Currency = "aud",
                // In the latest version of the API, specifying the `automatic_payment_methods` parameter is optional because Stripe enables its functionality by default.
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            });

            return new ServiceResponse<string>
            {
                Data = paymentIntent.ClientSecret,//new JsonResult(new { clientSecret = paymentIntent.ClientSecret }),//
                StatusCode = 303,
                Success = true,
                //StatusMessage = "Cart is empty. An error occurred while processing the payment."
            };
            //return Json(new { clientSecret = paymentIntent.ClientSecret });
        }
        */


        public async Task<ServiceResponse<OrderRetrieveDto>> OrderConfirmation(int ordernumber, string sessionid)
        {
            var customerid = GetCustomerID();

            // empty the shopping cart
            await _shoppingCartRepository.DeleteShoppingCart();

            var order = await _dataContext.Orders.Include(c => c.OrderItems).FirstOrDefaultAsync(o => o.OrderNumber == ordernumber && o.CustomerId == customerid);
            if (order == null)
            {
                return new ServiceResponse<OrderRetrieveDto>
                {
                    StatusCode = 404,
                    Success = false,
                    StatusMessage = "Order not not found."
                };
            }
            try
            {
                await UpdateOrderWithStripePayment(order.OrderNumber, sessionid);
            }
            catch (StripeException e)
            {
                return new ServiceResponse<OrderRetrieveDto>
                {
                    StatusCode = 500,
                    Success = false,
                    StatusMessage = $"Couldn't find created order. An error occurred while processing the payment. {e.Message}"
                };
            }

            return new ServiceResponse<OrderRetrieveDto>
            {
                Data = _mapper.Map<OrderRetrieveDto>(order),
                StatusCode = 200,
                Success = true,
                StatusMessage = "Order Confirmed"
            };

        }

        public async Task UpdateOrderWithStripePayment(int ordernumber, string stripeSessionId)
        {
            var order = await _dataContext.Orders.Include(c => c.OrderItems).FirstOrDefaultAsync(o => o.OrderNumber == ordernumber);
            if (order != null)
            {
                order.SessionId = stripeSessionId;
                _dataContext.SaveChanges();
            }
        }

        public async Task<ServiceResponse<OrderRetrieveDto>> RetrieveOrder(int ordernumber)
        {

            //var orderResponse = new ServiceResponse<OrderRetrieveDto>();
            var customerid = GetCustomerID();
            var order = await _dataContext.Orders.Include(c => c.OrderItems)
                        .ThenInclude(c=>c.Product).Include(c=>c.customer)
                        .FirstOrDefaultAsync(o => o.OrderNumber == ordernumber && o.CustomerId == customerid);

            if (order == null)
            {
                return new ServiceResponse<OrderRetrieveDto>
                {
                    StatusCode = 404,
                    Success = false,
                    StatusMessage = "Order not not found."
                };
            }
            return new ServiceResponse<OrderRetrieveDto>
            {
                Data = _mapper.Map<OrderRetrieveDto>(order),
                StatusCode = 200,
                Success = true,
                StatusMessage = "Order found"
            };

        }

        // test: Is delete order required?
    }
}
