using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.entity
{
    public class ProductSizeType
    {
        public int SizeTypeId { get; set; }
        public SizeType SizeType { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}