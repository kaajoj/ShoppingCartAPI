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
                    ProductId = 2,
                    Quantity = 6,
                    Product = new Product()
                    {
                        Name = "Test",
                        Price = 0
                    }
                });
                context.SaveChanges();
            }

            Dictionary<string, float> cart;
            // Cart cart = null;
            // var cartEntryKey;
            // var cartEntryValue;
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

        // Test DeleteCart() method

    }
}
