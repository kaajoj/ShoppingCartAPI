using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspnetAPI.Models;

namespace AspnetAPI
{
    public class CartOperations
    {

        public static decimal calculateCartValue(List<Cart> cartProducts)
            {
                decimal totalValue = 0;
                foreach (var cartProduct in cartProducts)
                {
                    if (cartProduct.Quantity > 10)
                    {
                        cartProduct.Product.Price = 10;
                    } else if (cartProduct.Quantity <= 10)    // question to business - no clear requirement what if quantity equals exactly 10
                    {
                        cartProduct.Product.Price = 5;
                    }
                    totalValue += (decimal) (cartProduct.Quantity * cartProduct.Product.Price);
                }

                return totalValue;
            }
    }
}
