using AutoMapper;
using EcommerceReact.Server.DTO.Category;
using EcommerceReact.Server.DTO.Customer;
using EcommerceReact.Server.DTO.Order;
using EcommerceReact.Server.DTO.Product;
using EcommerceReact.Server.DTO.ShoppingCart;
using EcommerceReact.Server.Models;

namespace EcommerceReact.Server
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Customer, CustomerCreateDto>();
            CreateMap<CustomerCreateDto, Customer>()
                                .ForMember(
                                dest => dest.Password,
                                t => t.Ignore()
                                );
            CreateMap<Customer, CustomerRetrieveDto>().ReverseMap();
            CreateMap<Customer, CustomerUpdateDto>().ReverseMap();

            CreateMap<Product, ProductRetrieveDto>();

            CreateMap<ProductCreateDto, Product>().ReverseMap();
            CreateMap<Category, CategoryRetrieveDto>();
            CreateMap<CategoryCreateDto, Category>();

            CreateMap<ShoppingCartItem, ShoppingCartItemRetrieveDto>()
                .ForMember(dest => dest.ShoppingCartItemId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Product.Id))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Title))
                .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.Product.SKU))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Product.Price))
                .ForMember(dest => dest.SalePrice, opt => opt.MapFrom(src => src.Product.SalePrice))
                .ForMember(dest => dest.WasPrice, opt => opt.MapFrom(src => src.Product.WasPrice))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.Product.ImageUrl));

            CreateMap<ShoppingCart, ShoppingCartRetrieveDto>()
                //.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.customer.Email))
                //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.customer.Name))
                //.ForMember(dest => dest.IsAdmin, opt => opt.MapFrom(src => src.customer.IsAdmin))
                //.ForMember(dest => dest.StreetAddress, opt => opt.MapFrom(src => src.customer.StreetAddress))
                //.ForMember(dest => dest.City, opt => opt.MapFrom(src => src.customer.City))
                //.ForMember(dest => dest.State, opt => opt.MapFrom(src => src.customer.State))
                //.ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.customer.PostalCode))
                .ForMember(dest => dest.ShoppingCartId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ProductTotal, opt => opt.MapFrom(src => src.Items.Sum(item => item.Quantity)))
                .ForMember(dest => dest.CartTotal, opt => opt.MapFrom(src => src.CartTotal))
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items));


            CreateMap<Order, OrderRetrieveDto>().
                ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.OrderItems));
        }
    }
}
