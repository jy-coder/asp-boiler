
using API.DTOs;
using API.DTOs.Product;
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
            existingProduct.ProductCategories.RemoveAll(pc => !categoryIds.Contains(pc.CategoryId));
            var newCategoryIds = categoryIds.Except(existingProduct.ProductCategories.Select(pc => pc.CategoryId));
            foreach (var categoryId in newCategoryIds)
            {
                existingProduct.ProductCategories.Add(new ProductCategory { CategoryId = categoryId });
            }
            var updatedProduct = _mapper.Map<ProductUpdateDto, Product>(updatedProductDto, existingProduct);
            _uow.ProductRepository.Update(updatedProduct);

            if (await _uow.Complete())
            {
                return NoContent();
            }

            return BadRequest("Failed to update product");
        }



        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] ProductCreateDto productCreateDto)
        {
            var categoryIds = productCreateDto.CategoryIds;

            var product = _mapper.Map<Product>(productCreateDto);

            foreach (var categoryId in categoryIds)
            {
                var category = await _uow.CategoryRepository.GetCategoryByIdAsync(categoryId);
                if (category != null)
                {
                    product.ProductCategories.Add(new ProductCategory { Category = category });
                }
            }

            await _uow.ProductRepository.AddProductAsync(product);
            if (await _uow.Complete()) return Ok(_mapper.Map<ProductDto>(product));
            return BadRequest("Failed to send message");
        }



    }
}