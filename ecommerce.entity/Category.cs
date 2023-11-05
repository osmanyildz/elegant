using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.entity
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<GenderCategory> GenderCategories{ get; set; }
        public ParentCategory ParentCategory { get; set; }      
        public int ParentCategoryId{ get; set; }
    }
}