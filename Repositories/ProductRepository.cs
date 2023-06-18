

using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

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

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {

            return await _context.Products
            .Include(p => p.Photos)
            .Include(p => p.Categories)
            .AsSplitQuery()
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }
        public async Task<ProductDto> GetProductDtoByIdAsync(int id)
        {
            return await _context.Products
            .Include(p => p.Photos)
            .Include(p => p.Categories)
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