using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.entity
{
    public class SizeType
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ParentCategory ParentCategory { get; set; }
        public int ParentCategoryId { get; set; }
        public List<ProductSizeType> ProductSizeTypes { get; set; }
    }
}