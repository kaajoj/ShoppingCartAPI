using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AspnetAPI.Models;
using AspnetAPI.Data;

namespace AspnetAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly AspnetAPIContext _context;

        public ProductsController(AspnetAPIContext context)
        {
            _context = context;
        }

        // GET: products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> getProduct()
        {
            return await _context.Product.ToListAsync();
        }

        // GET: products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> getProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: products/5
        [HttpPut("{id}")]
        public async Task<IActionResult> putProduct(int id, Product product)
        {
            if (id != product.Id)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!productExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: products
        [HttpPost]
        public async Task<ActionResult<Product>> postProduct(Product product)
        {
            _context.Product.Add(product);
            await _context.SaveChangesAsync();

            return CreatedAtAction("getProduct", new { id = product.Id }, product);
        }

        // DELETE: products/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> deleteProduct(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Product.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }

        private bool productExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
