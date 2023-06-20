using API.Data;
using API.DTOs;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace API.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<PagedList<CategoryDto>> GetCategoriesAsync(PaginationParams p)
        {
            var query = _context.Categories.ProjectTo<CategoryDto>(_mapper.ConfigurationProvider).AsQueryable();
            return await PagedList<CategoryDto>.CreateAsync(query, p.PageNumber, p.PageSize);
        }
    }
}