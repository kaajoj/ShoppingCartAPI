using System.Collections.Generic;
using AspnetAPI.Controllers;
using AspnetAPI.Data;
using AspnetAPI.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AspnetAPI.Tests
{
    public class CartControllerTest
    {
        // Test GetCart() method
        [Fact]
        public async void GetCart()
        {
            #region Arrange
            var options = new DbContextOptionsBuilder<AspnetAPIContext>()
                .UseInMemoryDatabase(databaseName: "AspnetAPI")
                .Options;
            using (var context = new AspnetAPIContext(options))
            {
                context.Add(new Cart()
                {
                    ProductId = 1,
                    Quantity = 5,
                    Product = new Product()
                    {
                        Name = "Test",
                        Price = 0
                    }
                });
                context.SaveChanges();
            }

            Dictionary<string, float> cart;
            #endregion

            #region Act
            using (var context = new AspnetAPIContext(options))
            {
                var controller = new CartController(context);
                cart = await controller.GetCart();
            }
            #endregion

            #region Assert
            Assert.NotNull(cart);
            Assert.True(cart.ContainsKey("1"));
            Assert.True(cart.ContainsValue(5));
            #endregion
        }

        // Test GetCartValue() method


        // Test AddToCart() method
        [Fact]
        public async void AddToCart()
        {
            #region Arrange
            var options = new DbContextOptionsBuilder<AspnetAPIContext>()
                .UseInMemoryDatabase(databaseName: "AspnetAPI")
                .Options;

            using (var context = new AspnetAPIContext(options))
            {
                context.Add(new Product()
                {
                    Name = "Test",
                    Price = 0
                });
                context.SaveChanges();
            }

            Cart cartEntryCreated = null;
            #endregion

            #region Act
            using (var context = new AspnetAPIContext(options))
            {
                var controller = new CartController(context);
                await controller.AddToCart(1, 20);
                cartEntryCreated = context.Cart.Find(1);
            }
            #endregion

            #region Assert
            Assert.NotNull(cartEntryCreated);
            Assert.Equal(1,cartEntryCreated.ProductId);
            Assert.Equal(20, cartEntryCreated.Quantity);
            #endregion
        }

        // Test DeleteCart() method

    }
}
