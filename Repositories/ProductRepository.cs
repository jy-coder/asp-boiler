

using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace API.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PagedList<ProductDto>> GetProductsAsync([FromQuery] ProductParams productParams)
        {

            var query = _context.Products
            .AsSplitQuery()
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .AsNoTracking();

            if (!string.IsNullOrEmpty(productParams.CategoryIds))
            {
                var categoryIds = productParams.CategoryIds.Split(',').Select(int.Parse).ToList();

                query = query.Where(p => p.Categories.Any(c => categoryIds.Contains(c.Id)));
            }

            return await PagedList<ProductDto>.CreateAsync(query, productParams.PageNumber, productParams.PageSize);
        }

        public async Task<ProductDto> GetProductDtoByIdAsync(int id)
        {
            return await _context.Products
            .Where(x => x.Id == id)
            .AsSplitQuery()
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }


        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }


        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }
    }
}