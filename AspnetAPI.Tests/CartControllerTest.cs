using System.Collections.Generic;
using System.Linq;
using AspnetAPI.Controllers;
using AspnetAPI.Data;
using AspnetAPI.Models;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AspnetAPI.Tests
{
    public class CartControllerTest
    {
        // Test AddToCart() method
        [Fact]
        public async void AddToCart()
        {
            #region Arrange
            var options = new DbContextOptionsBuilder<AspnetAPIContext>()
                .UseInMemoryDatabase(databaseName: "AspnetAPI")
                .Options;

            await using (var context = new AspnetAPIContext(options))
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

            await using (var context = new AspnetAPIContext(options))
            {
                var controller = new CartController(context);
                await controller.AddToCart(1, 20);
                cartEntryCreated = await context.Cart.FindAsync(1);
            }
            #endregion

            #region Assert
            Assert.NotNull(cartEntryCreated);
            Assert.Equal(1, cartEntryCreated.ProductId);
            Assert.Equal(20, cartEntryCreated.Quantity);
            #endregion
        }

        // Test DeleteFromCart() method
        [Fact]
        public async void DeleteFromCart()
        {
            #region Arrange
            var options = new DbContextOptionsBuilder<AspnetAPIContext>()
                .UseInMemoryDatabase(databaseName: "AspnetAPI")
                .Options;

            await using (var context = new AspnetAPIContext(options))
            {
                context.Add(new Cart()
                {
                    Quantity = 40,
                    Product = new Product()
                    {
                        Name = "Test",
                        Price = 0
                    }
                });
                context.SaveChanges();
            }

            Cart removedFromCart = null;
            int cartSize = 1;
            #endregion

            #region Act

            await using (var context = new AspnetAPIContext(options))
            {
                var controller = new CartController(context);
                removedFromCart = (await controller.DeleteFromCart(1)).Value;
                cartSize = await context.Cart.CountAsync();
            }
            #endregion

            #region Assert
            Assert.Equal(0,cartSize);
            Assert.Equal(1, removedFromCart.ProductId);
            Assert.Equal(40, removedFromCart.Quantity);
            #endregion
        }

        // Test GetCart() method
        [Fact]
        public async void GetCart()
        {
            #region Arrange
            var options = new DbContextOptionsBuilder<AspnetAPIContext>()
                .UseInMemoryDatabase(databaseName: "AspnetAPI")
                .Options;
            await using (var context = new AspnetAPIContext(options))
            {
                context.Add(new Cart()
                {
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

            await using (var context = new AspnetAPIContext(options))
            {
                var controller = new CartController(context);
                cart = await controller.GetCart();
            }
            #endregion

            #region Assert
            Assert.NotNull(cart);
            Assert.Single(cart);
            Assert.True(cart.ContainsKey("1"));
            Assert.True(cart.ContainsValue(5));
            #endregion
        }

        // Test GetCartValue() method
        [Fact]
        public async void GetCartValue()
        {
            #region Arrange
            var options = new DbContextOptionsBuilder<AspnetAPIContext>()
                .UseInMemoryDatabase(databaseName: "AspnetAPI")
                .Options;
            await using (var context = new AspnetAPIContext(options))
            {
                context.Add(new Cart()
                {
                    Quantity = 5,
                    Product = new Product()
                    {
                        Name = "Test1",
                        Price = 0
                    }
                });
                context.Add(new Cart()
                {
                    Quantity = 20,
                    Product = new Product()
                    {
                        Name = "Test2",
                        Price = 0
                    }
                });
                context.SaveChanges();
            }

            decimal cartTotalValue;
            #endregion

            #region Act

            await using (var context = new AspnetAPIContext(options))
            {
                var controller = new CartController(context);
                cartTotalValue = await controller.GetCartValue();
            }
            #endregion

            #region Assert
            Assert.Equal(225,cartTotalValue);
            #endregion
        }


    }
}
