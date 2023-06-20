using API.DTOs;
using API.Helpers;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PagedList<CategoryDto>> GetCategoriesAsync(PaginationParams p);
    }
}