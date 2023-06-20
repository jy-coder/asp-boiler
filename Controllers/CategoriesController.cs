
using API.DTOs;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CategoriesController : BaseApiController
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoriesController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] PaginationParams p)
        {
            var categories = await _categoryRepository.GetCategoriesAsync(p);

            Response.AddPaginationHeader(new PaginationHeader(categories.CurrentPage, categories.PageSize, categories.TotalCount, categories.TotalPages));
            return Ok(categories);
        }
    }
}