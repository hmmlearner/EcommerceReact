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

    /// <summary>
    /// Represents a repository for managing shopping carts.
    /// </summary>
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

    /// <summary>
    /// Retrieves a shopping cart by its ID.
    /// </summary>
    /// <param name="shoppingCartId">The ID of the shopping cart to retrieve.</param>
    /// <returns>The retrieved shopping cart.</returns>
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

    private async Task<ShoppingCart> GetShoppingCart()
    {
        var customerId = GetCustomerID();
        shoppingCart = await _dataContext.ShoppingCarts.Include(cartItem => cartItem.Items)
                         .ThenInclude(ci => ci.Product)
                         .Include(c => c.customer)
                         .FirstOrDefaultAsync(c => c.CustomerId == customerId);
        return shoppingCart;
    }

    /// <summary>
    /// Retrieves the shopping cart for the current customer.
    /// </summary>
    /// <returns>A service response containing the retrieved shopping cart.</returns>
    public async Task<ServiceResponse<ShoppingCartRetrieveDto>> RetrieveShoppingCart()
    {
        var customerId = GetCustomerID();
        shoppingCart = await _dataContext.ShoppingCarts.Include(cartItem => cartItem.Items)
                                     .ThenInclude(ci => ci.Product)
                                     //.Include(c => c.customer)
                                     .FirstOrDefaultAsync(c => c.CustomerId == customerId);

        if (shoppingCart == null)
        {
            return new ServiceResponse<ShoppingCartRetrieveDto>
            {
                StatusCode = 400,
                Success = false,
                StatusMessage = "Cart doesn't exist."
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

    /// <summary>
    /// Adds a product to the shopping cart.
    /// </summary>
    /// <param name="productId">The ID of the product to add.</param>
    /// <param name="quantity">The quantity of the product to add.</param>
    /// <returns>A service response containing the updated shopping cart.</returns>
    public async Task<ServiceResponse<ShoppingCartRetrieveDto>> AddToShoppingCart(int productId, int quantity)
    {
        var customerId = GetCustomerID();
        var product = await _dataContext.Products.SingleOrDefaultAsync(x => x.Id == productId);
        if (product == null)
        {
            return new ServiceResponse<ShoppingCartRetrieveDto>
            {
                StatusCode = 400,
                Success = false,
                StatusMessage = "Invalid product to add"
            };
        }

        shoppingCart = await GetShoppingCart();

        if (shoppingCart == null)
        {
            shoppingCart = new ShoppingCart { Items = new List<ShoppingCartItem>(), CustomerId = customerId };
            _dataContext.ShoppingCarts.Add(shoppingCart);
        }

        var shoppingCartItem = shoppingCart.Items.Find(c => c.ProductId == productId);
        if (shoppingCartItem == null)
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

        var shoppingCartRetrieveDto = _mapper.Map<ShoppingCartRetrieveDto>(shoppingCart);

        return new ServiceResponse<ShoppingCartRetrieveDto>
        {
            Data = shoppingCartRetrieveDto,
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
        shoppingCart = await GetShoppingCart();

        if (shoppingCart == null)
        {
            return new ServiceResponse<ShoppingCartRetrieveDto>
            {
                StatusCode = 400,
                Success = false,
                StatusMessage = "Cart doesn't exist."
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
                StatusMessage = "Product not found in the cart. Invalid deletion."
            };
        }

        if (totalQuantity == quantity)
        {
            // Delete item and cart
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
        else if (shoppingCartItem.Quantity > quantity)
        {
            shoppingCartItem.Quantity -= quantity;
        }
        else
        {
            _dataContext.ShoppingCartItems.Remove(shoppingCartItem);
        }

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