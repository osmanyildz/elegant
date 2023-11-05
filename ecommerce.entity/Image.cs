using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.entity
{
    public class Image
    {
        public int Id { get; set; }
        public string ImageUrl{ get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } 
    }
}