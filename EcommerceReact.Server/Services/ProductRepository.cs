﻿using AutoMapper;
using EcommerceReact.Server.Data;
using EcommerceReact.Server.DTO.Category;
using EcommerceReact.Server.DTO.Product;
using EcommerceReact.Server.Interfaces;
using EcommerceReact.Server.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace EcommerceReact.Server.Services
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public ProductRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<ProductRetrieveDto>> CreateProduct(ProductCreateDto productDto)
        {
            var serviceReponse = new ServiceResponse<ProductRetrieveDto>();
            var product = _mapper.Map<Product>(productDto);
            _dataContext.Products.Add(product);    
            await _dataContext.SaveChangesAsync();
            var newProduct = await _dataContext.Products.SingleOrDefaultAsync(x=>x.Id== product.Id);
            serviceReponse.Data = _mapper.Map<ProductRetrieveDto>(newProduct);
            return serviceReponse;
        }

        public async Task<ServiceResponse<List<ProductRetrieveDto>>> GetAllProducts()
        {
            var serviceResponse = new ServiceResponse<List<ProductRetrieveDto>>();
            var products = await _dataContext.Products.ToListAsync();
            serviceResponse.Data = products.Select(x => _mapper.Map<ProductRetrieveDto>(x)).ToList();
            return serviceResponse;

        }
        public async Task<ServiceResponse<ProductRetrieveDto>> GetProductById(int id)
        {
            var serviceResponse = new ServiceResponse<ProductRetrieveDto>();
           var product = await _dataContext.Products.SingleOrDefaultAsync(x => x.Id == id);
            serviceResponse.Data = _mapper.Map<ProductRetrieveDto>(product);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ProductRetrieveDto>>> GetProductsByCategory(int categoryId)
        {
            var serviceResponse = new ServiceResponse<List<ProductRetrieveDto>>();
            var products = await _dataContext.Products.Where(x => x.CategoryId == categoryId).ToListAsync();
            serviceResponse.Data = products.Select(x => _mapper.Map<ProductRetrieveDto>(x)).ToList();
            return serviceResponse;
        }

    }
}
