using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AspnetAPI.Models;

namespace AspnetAPI.Data
{
    public class AspnetAPIContext : DbContext
    {
        public AspnetAPIContext (DbContextOptions<AspnetAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }

        public DbSet<Cart> Cart { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Cart>()
                .HasIndex(u => u.ProductId)
                .IsUnique();
        }
    }
}
