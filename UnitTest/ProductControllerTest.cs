
using API.Controllers;
using API.DTOs;
using API.DTOs.Product;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace API.UnitTest
{
    public class ProductsControllerTests
    {
        private readonly ProductsController _controller;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;

        public ProductsControllerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            var httpContext = new DefaultHttpContext();
            httpContext.Request.QueryString = new QueryString("?categoryIds=1,3,5");
            _controller = new ProductsController(_mockMapper.Object, _mockUnitOfWork.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = httpContext
                }
            };
        }

        [Fact]
        public async Task GetProducts_ReturnsOkObjectResult()
        {
            // Arrange
            ProductParams productParams = new ProductParams();
            var products = new PagedList<ProductDto>(
                new List<ProductDto> { new ProductDto { Id = 1, Name = "Product 1" } },
                1,
                1,
                1
            );

            _mockUnitOfWork.Setup(u => u.ProductRepository.GetProductsAsync(productParams))
                .ReturnsAsync(products);

            // Act
            var result = await _controller.GetProducts(productParams);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var returnedProducts = Assert.IsAssignableFrom<PagedList<ProductDto>>(okResult.Value);
            Assert.Single(returnedProducts);
            Assert.Equal(1, returnedProducts.FirstOrDefault()?.Id);
            Assert.Equal("Product 1", returnedProducts.FirstOrDefault()?.Name);
        }

        [Fact]
        public async Task GetProductById_ReturnsOkObjectResult()
        {
            // Arrange
            var productId = 1;
            var productDto = new ProductDto { Id = productId, Name = "Product 1" };

            _mockUnitOfWork.Setup(u => u.ProductRepository.GetProductDtoByIdAsync(productId))
                .ReturnsAsync(productDto);

            // Act
            var actionResult = await _controller.GetProductById(productId);
            var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);

            // Assert
            var returnedProduct = Assert.IsAssignableFrom<ProductDto>(okResult.Value);
            Assert.Equal(productId, returnedProduct.Id);
            Assert.Equal("Product 1", returnedProduct.Name);
        }


        [Fact]
        public async Task DeleteProduct_ReturnsNotFoundResult()
        {
            // Arrange
            var productId = 1;
            Product nullProduct = null;

            _mockUnitOfWork.Setup(u => u.ProductRepository.GetProductByIdAsync(productId))
                .ReturnsAsync(nullProduct);

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(404, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsNoContentResult()
        {
            // Arrange
            var productId = 1;
            var updatedProductDto = new ProductUpdateDto
            {
                CategoryIds = new List<int> { 1, 2, 3 }
            };
            var existingProduct = new Product
            {
                Id = productId,
                Name = "Product 1",
                ProductCategories = new List<ProductCategory>
                {
                    new ProductCategory { CategoryId = 1 },
                    new ProductCategory { CategoryId = 4 }
                }
            };

            _mockUnitOfWork.Setup(u => u.ProductRepository.GetProductByIdAsync(productId))
                .ReturnsAsync(existingProduct);
            _mockUnitOfWork.Setup(u => u.Complete())
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateProduct(productId, updatedProductDto);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(204, noContentResult.StatusCode);
        }


        [Fact]
        public async Task CreateProduct_ReturnsOkObjectResult()
        {
            // Arrange
            var productCreateDto = new ProductCreateDto
            {
                Name = "New Product",
                Price = 9.99M,
                Description = "Product description",
                CategoryIds = new List<int> { 1, 2, 3 }
            };
            var product = new Product
            {
                Id = 1,
                Name = productCreateDto.Name,
                Price = productCreateDto.Price,
                Description = productCreateDto.Description,
                ProductCategories = new List<ProductCategory>(),
                PhotoUrl = "https://example.com/photos/1.jpg"
            };

            _mockMapper.Setup(m => m.Map<Product>(productCreateDto)).Returns(product);
            _mockUnitOfWork.Setup(u => u.CategoryRepository.GetCategoryByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((int categoryId) => new Category { Id = categoryId });

            _mockUnitOfWork.Setup(u => u.ProductRepository.AddProductAsync(product))
                .Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.Complete())
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CreateProduct(productCreateDto);

            // // Assert
            var okResult = Assert.IsType<ActionResult<ProductDto>>(result);

        }
    }
}