using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AspnetAPI.Controllers;
using AspnetAPI.Data;
using AspnetAPI.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AspnetAPI.Tests
{
    public class ProductsControllerTest
    {
        // Test GetProduct() method
        [Fact]
        public async void GetProduct()
        {
            #region Arrange
            var options = new DbContextOptionsBuilder<AspnetAPIContext>()
                .UseInMemoryDatabase(databaseName: "AspnetAPI")
                .Options;
            using (var context = new AspnetAPIContext(options))
            {
                context.Add(new Product()
                {
                    Name = "Test1",
                    Price = 0
                });
                context.SaveChanges();
            }

            Product productExist = null;
            Product productNotExist = null;
            #endregion

            #region Act
            using (var context = new AspnetAPIContext(options))
            {
                var controller = new ProductsController(context);
                productExist = (await controller.GetProduct(1)).Value;
                productNotExist = (await controller.GetProduct(2)).Value;
            }
            #endregion

            #region Assert

            Assert.True(
                productExist != null
                && productNotExist == null
            );

            Assert.Same("Test1", productExist.Name);
            #endregion

        }

        // Test PostProduct() method
        [Fact]
        public async void PostProduct()
        {
            #region Arrange
            var options = new DbContextOptionsBuilder<AspnetAPIContext>()
                .UseInMemoryDatabase(databaseName: "AspnetAPI")
                .Options;

            Product product = new Product()
            {
                Name = "Test2",
                Price = 0
            };

            Product productCreated = null;
            #endregion

            #region Act
            using (var context = new AspnetAPIContext(options))
            {
                var controller = new ProductsController(context);
                await controller.PostProduct(product);
                productCreated = context.Product.Find(product.Id);
            }
            #endregion

            #region Assert
            Assert.True(productCreated != null);
            #endregion

        }
    }
}
