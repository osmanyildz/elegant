using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.entity
{
    public class ParentCategory
    {
        public int Id { get; set; }
        public string Name { get; set; } // üst giyim, alt giyim
        public List<Category> Categories { get; set; }
        public List<Product> Products { get; set; }
        public List<SizeType> SizeTypes { get; set; }
    }
}