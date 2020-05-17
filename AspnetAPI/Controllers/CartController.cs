using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspnetAPI.Models;
using AspnetAPI.Data;

namespace AspnetAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AspnetAPIContext _context;

        public CartController(AspnetAPIContext context)
        {
            _context = context;
        }

        // PobierzKoszyk() - GET: /cart
        [HttpGet]
        public async Task<Dictionary<string, float>> GetCart()
        {
            return await _context.Cart.ToDictionaryAsync(cart => cart.ProductId.ToString(), cart => cart.Quantity);
        }

        // GET: cart/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> GetCart(int id)
        {
            var cart = await _context.Cart.FirstAsync(p => p.ProductId == id);

            if (cart == null)
            {
                return NotFound();
            }

            cart.Product = await _context.Product.FindAsync(id);

            return cart;
        }

        // PobierzKoszykWartosc() - GET: /cart/GetCartValue
        [HttpGet("GetCartValue")]
        public async Task<decimal> GetCartValue()
        {
            List<Cart> cartProducts = await _context.Cart.ToListAsync();
            foreach (var cartEntry in cartProducts)
            {
                cartEntry.Product = await _context.Product.FindAsync(cartEntry.ProductId);
            }
            var cartTotalValue = CartOperations.calculateCartValue(cartProducts);

            return cartTotalValue;
        }

        // DodajDoKoszyka() - POST: /cart/id/quantity
        [HttpPost("{id}/{quantity}")]
        public async Task<ActionResult<Cart>> AddToCart(int id, float quantity)
        {
            Cart cart = new Cart {ProductId = id, Quantity = quantity, Product = await _context.Product.FindAsync(id)};
            _context.Cart.Add(cart);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCart", new { id = cart.Id }, cart);
        }

        // UsunZKoszyka() - DELETE: /cart/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Cart>> DeleteFromCart(int id)
        {
            var cart = await _context.Cart.FirstOrDefaultAsync(p => p.ProductId == id);
            if (cart == null)
            {
                return NotFound();
            }
            _context.Cart.Remove(cart);
            await _context.SaveChangesAsync();

            return cart;
        }

    }
}
