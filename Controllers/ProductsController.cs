
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;

using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {

        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ProductsController(IMapper mapper, IUnitOfWork uow)
        {
            _mapper = mapper;
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] ProductParams productParams)
        {

            if (!string.IsNullOrEmpty(Request.Query["categoryIds"]))
            {
                var categoryIdsString = Request.Query["categoryIds"].ToString();
                productParams.CategoryIds = categoryIdsString;
            }
            var products = await _uow.ProductRepository.GetProductsAsync(productParams);

            Response.AddPaginationHeader(new PaginationHeader(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages));
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            return await _uow.ProductRepository.GetProductDtoByIdAsync(id);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto updatedProductDto)
        {
            var existingProduct = await _uow.ProductRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var categoryIds = updatedProductDto.CategoryIds;
            await _uow.ProductRepository.UpdateProductCategoriesAsync(existingProduct, categoryIds);

            _mapper.Map(updatedProductDto, existingProduct);
            _uow.ProductRepository.Update(existingProduct);

            if (await _uow.Complete())
            {
                return NoContent();
            }

            return BadRequest("Failed to update product");
        }



    }
}