using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspnetAPI.Models
{
    public class Cart
    { 
        public int Id { get; set; }

        [ForeignKey("ProductID")]
        public int ProductId { get; set; }

        public float Quantity { get; set; }

        public Product Product { get; set; }
    }
}
