
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts([FromQuery] UserParams userParams)
        {
            var products = await _productRepository.GetProductsAsync(userParams);

            Response.AddPaginationHeader(new PaginationHeader(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages));
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            return await _productRepository.GetProductByIdAsync(id);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct(int id, [FromBody] ProductUpdateDto updatedProductDto)
        {
            var existingProduct = await _productRepository.GetProductByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            _mapper.Map(updatedProductDto, existingProduct);

            if (await _productRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to update product");
        }



    }
}