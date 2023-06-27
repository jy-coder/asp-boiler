using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API.Interfaces
{
    public interface ICategoryRepository
    {
        Task<PagedList<CategoryDto>> GetCategoriesAsync(PaginationParams p);

        Task<Category> GetCategoryByIdAsync(int categoryId);
    }
}