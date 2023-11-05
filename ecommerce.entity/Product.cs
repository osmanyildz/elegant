using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double? Price { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool IsPopular { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<Image> ImageUrls { get; set; }
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public ParentCategory ParentCategory { get; set; }
        public int ParentCategoryId { get; set; }
        public List<ProductSizeType> ProductSizeTypes { get; set; }
       
    }
}