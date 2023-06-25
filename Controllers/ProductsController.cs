
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
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper;
            _productRepository = productRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] ProductParams productParams)
        {

            if (!string.IsNullOrEmpty(Request.Query["categoryIds"]))
            {
                var categoryIdsString = Request.Query["categoryIds"].ToString();
                productParams.CategoryIds = categoryIdsString;
            }
            var products = await _productRepository.GetProductsAsync(productParams);

            Response.AddPaginationHeader(new PaginationHeader(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages));
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(int id)
        {
            return await _productRepository.GetProductDtoByIdAsync(id);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto updatedProductDto)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);
            if (existingProduct == null)
            {
                return NotFound();
            }

            var categoryIds = updatedProductDto.CategoryIds;
            await _productRepository.UpdateProductCategoriesAsync(existingProduct, categoryIds);

            _mapper.Map(updatedProductDto, existingProduct);
            _productRepository.Update(existingProduct);

            if (await _productRepository.SaveAllAsync())
            {
                return NoContent();
            }

            return BadRequest("Failed to update product");
        }



    }
}