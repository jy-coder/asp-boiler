
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

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public CategoriesController(IMapper mapper, IUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }



        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetCategories([FromQuery] PaginationParams p)
        {
            var categories = await _uow.CategoryRepository.GetCategoriesAsync(p);

            Response.AddPaginationHeader(new PaginationHeader(categories.CurrentPage, categories.PageSize, categories.TotalCount, categories.TotalPages));
            return Ok(categories);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(int id)
        {
            var category = await _uow.CategoryRepository.GetCategoryByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return Ok(categoryDto);
        }

    }
}