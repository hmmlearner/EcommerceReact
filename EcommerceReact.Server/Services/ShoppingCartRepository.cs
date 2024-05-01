using EcommerceReact.Server.Data;
using EcommerceReact.Server.DTO.Product;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;

using AutoMapper;
using EcommerceReact.Server.DTO.ShoppingCart;
using System.Security.Claims;
using EcommerceReact.Server.Exceptions;
using Stripe;
//using Stripe;
//using Ecommerce.DTO.Order;
//using Stripe.Climate;

namespace EcommerceReact.Server.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCart shoppingCart { get; set; } = new ShoppingCart();

        public ShoppingCartRepository(DataContext dataContext, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }
        public ShoppingCart GetCart(int shoppingCartId)
        {
            ShoppingCart? cart = _dataContext.ShoppingCarts
                                .Include(c => c.Items)
                                .FirstOrDefault(c => c.Id == shoppingCartId);
            if (cart == null)
            {
                throw new ShoppingCartNotFoundException("Shopping cart not found");
            }

            return cart;
        }

        private int GetCustomerID()
        {
            return int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        }

        private async Task<ShoppingCart> ShoppingCart()
        {
            var customerId = GetCustomerID();
#pragma warning disable CS8601 // Possible null reference assignment.
            shoppingCart = await _dataContext.ShoppingCarts.Include(cartItem => cartItem.Items)
                             .ThenInclude(ci => ci.Product)
                             .Include(c => c.customer)
                             .FirstOrDefaultAsync(c => c.CustomerId == customerId);
            return shoppingCart;
#pragma warning restore CS8601 // Possible null reference assignment.

        }

        public async Task<ServiceResponse<ShoppingCartRetrieveDto>> RetrieveShoppingCart()
        {
            var customerId = GetCustomerID();
            //#pragma warning disable CS8601 // Possible null reference assignment.
            //shoppingCart = await _dataContext.ShoppingCarts.Include(cartItem => cartItem.Items)
            //                 .ThenInclude(ci => ci.Product)
            //                 .Include(c => c.customer)
            //                 .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            shoppingCart = await _dataContext.ShoppingCarts.Include(cartItem => cartItem.Items)
                                         .ThenInclude(ci => ci.Product)
                                         //.Include(c => c.customer)
                                         .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            //#pragma warning restore CS8601 // Possible null reference assignment.


            if (shoppingCart == null)
            {
                return new ServiceResponse<ShoppingCartRetrieveDto>
                {
                    StatusCode = 400,
                    Success = false,
                    StatusMessage = "Cart doesn't exists."
                };
            }

                return new ServiceResponse<ShoppingCartRetrieveDto>
                {
                    Data = _mapper.Map<ShoppingCartRetrieveDto>(shoppingCart),
                    StatusCode = 200,
                    Success = true,
                    StatusMessage = "Cart retrieved"
                };
            

        }


        public async Task<ServiceResponse<ShoppingCartRetrieveDto>> AddToShoppingCart(int productId, int quantity)
        {
            // dont have to check if the cart already exists for customer?
            //TODO: Need to do a check if productId is valid
            var customerId = GetCustomerID();
            var product = await _dataContext.Products.SingleOrDefaultAsync(x => x.Id == productId);
            if(product == null)
            {
                return new ServiceResponse<ShoppingCartRetrieveDto>
                {
                    StatusCode = 400,
                    Success = false,
                    StatusMessage = "Invalid product to add"
            };
            }

#pragma warning disable CS8601 // Possible null reference assignment.
            shoppingCart = await _dataContext.ShoppingCarts.Include(cartItem => cartItem.Items)
                    .ThenInclude(ci => ci.Product)
                    .Include(c => c.customer)
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);
#pragma warning restore CS8601 // Possible null reference assignment.

            if (shoppingCart == null) 
            {
                shoppingCart = new ShoppingCart { Items= new List<ShoppingCartItem>(), CustomerId = customerId };
                _dataContext.ShoppingCarts.Add(shoppingCart);
            }
            var shoppingCartItem = shoppingCart.Items.Find(c => c.ProductId == productId);
            if(shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem
                {
                    ProductId = productId,
                    Quantity = quantity
                };
                shoppingCart.Items.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Quantity += quantity;
            }
            _dataContext.SaveChanges();

            shoppingCart.CartTotal = CalculateTotal();
            _dataContext.ShoppingCarts.Update(shoppingCart);
            _dataContext.SaveChanges();

            var shoppingCartRetrievedto = _mapper.Map<ShoppingCartRetrieveDto>(shoppingCart);

            return new ServiceResponse<ShoppingCartRetrieveDto>
            {
                Data = shoppingCartRetrievedto,
                StatusCode = 200,
                Success = true,
                StatusMessage = "Product added to cart"
            };

        }

        public double CalculateTotal()
        {
            return shoppingCart.Items.Sum(item => item.Product.SalePrice * item.Quantity);
        }

        public async Task<ServiceResponse<ShoppingCartRetrieveDto>> DeleteFromShoppingCart(int productId, int quantity)
        {
            var customerId = GetCustomerID();
            //var serviceReponse = new ServiceResponse<ShoppingCartRetrieveDto>();
            shoppingCart = await _dataContext.ShoppingCarts.Include(cartItem => cartItem.Items)
                            .ThenInclude(ci => ci.Product)
                            .Include(c => c.customer)
                            .FirstOrDefaultAsync(c => c.CustomerId == customerId);

            if(shoppingCart == null)
            {
                return new ServiceResponse<ShoppingCartRetrieveDto>
                {
                    StatusCode = 400,
                    Success = false,
                    StatusMessage = "Cart doesn't exists."
                };
            }
            var shoppingCartItem = shoppingCart.Items.Find(c => c.ProductId == productId);
            var totalQuantity = shoppingCart.Items.Sum(item => item.Quantity);
            if (shoppingCartItem == null)
            {
                return new ServiceResponse<ShoppingCartRetrieveDto>
                {
                    StatusCode = 400,
                    Success = false,
                    StatusMessage = "Product not found in the cart invalid deletion"
            };
            }

            if (totalQuantity == quantity)
            {
                //delete item and cart
                _dataContext.ShoppingCartItems.Remove(shoppingCartItem);
                _dataContext.ShoppingCarts.Remove(shoppingCart);
                _dataContext.SaveChanges();
                return new ServiceResponse<ShoppingCartRetrieveDto>
                {
                    Data = _mapper.Map<ShoppingCartRetrieveDto>(shoppingCart),
                    StatusCode = 200,
                    Success = true,
                    StatusMessage = "Product and cart deleted"
                };
            }
            else if(shoppingCartItem.Quantity > quantity)
            {
                shoppingCartItem.Quantity -= quantity;
            }
            else
            {
                _dataContext.ShoppingCartItems.Remove(shoppingCartItem);
            }
            //var cartTotal = CalculateTotal();
            _dataContext.SaveChanges();
            return new ServiceResponse<ShoppingCartRetrieveDto>
            {
                Data = _mapper.Map<ShoppingCartRetrieveDto>(shoppingCart),
                StatusCode = 200,
                Success = true,
                StatusMessage = "Product deleted from cart"
            };
        }
       

        public async Task<ServiceResponse<string>> DeleteShoppingCart()
        {
            var customerId = GetCustomerID();
            var serviceResponse = new ServiceResponse<string>();

            try
            {
                shoppingCart = await _dataContext.ShoppingCarts
                    .Include(cartItem => cartItem.Items)
                    .ThenInclude(ci => ci.Product)
                    .Include(c => c.customer)
                    .FirstOrDefaultAsync(c => c.CustomerId == customerId);

                if (shoppingCart != null)
                {
                    _dataContext.ShoppingCartItems.RemoveRange(shoppingCart.Items);
                    _dataContext.ShoppingCarts.Remove(shoppingCart);
                    _dataContext.SaveChanges();

                    serviceResponse.StatusCode = 200;
                    serviceResponse.Success = true;
                    serviceResponse.StatusMessage = "Shopping cart deleted successfully.";
                }
                else
                {
                    serviceResponse.StatusCode = 404; // Not Found
                    serviceResponse.Success = false;
                    serviceResponse.StatusMessage = "Shopping cart not found.";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.StatusCode = 500; // Internal Server Error
                serviceResponse.Success = false;
                serviceResponse.StatusMessage = $"An error occurred: {ex.Message}";
            }

            return serviceResponse;
        }

    }
}
