using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            // Set up HttpContext and HttpRequest for the controller
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

        // Test for GetProducts method
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

        // Test for GetProductById method
        [Fact]
        public async Task GetProductById_ReturnsOkObjectResult()
        {
            // Arrange
            var productId = 1;
            var productDto = new ProductDto { Id = productId, Name = "Product 1" };

            _mockUnitOfWork.Setup(u => u.ProductRepository.GetProductDtoByIdAsync(productId))
                .ReturnsAsync(productDto);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            // var okResult = Assert.IsType<OkObjectResult>(result.Result);
            // var returnedProduct = Assert.IsAssignableFrom<ProductDto>(okResult.Value);
            // Assert.Equal(productId, returnedProduct.Id);
            // Assert.Equal("Product 1", returnedProduct.Name);
        }
    }
}