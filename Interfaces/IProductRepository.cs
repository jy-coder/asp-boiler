using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface IProductRepository
    {
        void Update(Product product);
        void DeleteProduct(Product product);

        Task<Product> GetProductByIdAsync(int id);
        Task<PagedList<ProductDto>> GetProductsAsync(ProductParams userParams);
        Task<ProductDto> GetProductDtoByIdAsync(int id);
        Task<Product> GetProductCategory(int id);
        Task AddProductAsync(Product product);
    }
}