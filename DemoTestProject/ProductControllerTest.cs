using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Database.Entities;
using Moq;
using Solvex_Workshop1.Controllers;

namespace DemoTestProject
{
    [TestClass]
    public class ProductControllerTest
    {
        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnProduct_WhenProductExits()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Test",
                Details = "DetailTest",
                Price = 45
            };

            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(product));
            var controller = new ProductController(productRepository.Object);

            // Act
            var getProductById = controller.GetProductById(1);

            // Assert
            Assert.IsNotNull(getProductById);
        }

        [TestMethod]
        public async Task AddAsync_ShouldAddProduct_WhenProductIsValid()
        {
            // Arrange
            var product = new Product
            {
                Name = "Sopita",
                Details = "Sopita Maggi",
                Price = 20
            };

            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(x => x.AddAsync(It.IsAny<ProductDto>())).Returns(Task.FromResult(product));
            var controller = new ProductController(productRepository.Object);

            ProductDto pr = new();
            pr.Name = product.Name;
            pr.Details = product.Details;
            pr.Price = product.Price;

            // Act
            var addProduct = await controller.CreateProduct(pr);

            // Assert
            Assert.IsNotNull(addProduct);
        }


        [TestMethod]
        public async Task UpdateAsync_ShouldUpdateProduct_WhenProductIsValid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Sopita Maggi",
                Details = "Sazón Completo",
                Price = 40
            };

            var updatedProductDto = new ProductDto
            {
                Name = "Sopita Doña Gallina",
                Details = "Versión Clásica",
                Price = 25
            };

            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(product));
            productRepository.Setup(x => x.UpdateAsync(It.IsAny<int>(), It.IsAny<ProductDto>())).Callback((int id, ProductDto p) =>
            {
                product.Name = p.Name;
                product.Details = p.Details;
                product.Price = p.Price;
            });

            var controller = new ProductController(productRepository.Object);

            // Act
            await controller.UpdateProduct(product.Id, updatedProductDto);

            // Assert
            Assert.AreEqual(updatedProductDto.Name, product.Name);
            Assert.AreEqual(updatedProductDto.Details, product.Details);
            Assert.AreEqual(updatedProductDto.Price, product.Price);
        }


        [TestMethod]
        public async Task DeleteAsync_ShouldDeleteProduct_WhenIdIsValid()
        {
            // Arrange
            var product = new Product
            {
                Id = 1,
                Name = "Sopita",
                Details = "Sopita Maggi",
                Price = 20
            };

            var productRepository = new Mock<IProductRepository>();
            productRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(Task.FromResult(product));
            productRepository.Setup(x => x.DeleteAsync(It.IsAny<int>())).Returns(Task.CompletedTask);
            var controller = new ProductController(productRepository.Object);

            // Act
            await controller.DeleteProduct(product.Id);

            // Assert
            productRepository.Verify(x => x.DeleteAsync(It.IsAny<int>()), Times.Once());
        }

    }
}