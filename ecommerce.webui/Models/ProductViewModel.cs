using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.webui.Models
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Image> ImageUrls{ get; set; }
        public double? Price { get; set; }
        public bool IsPopular { get; set; }
        public string Url { get; set; }
        public int GenderId { get; set; }
        public List<ProductCategory> Categories { get; set; }
        public List<ProductSizeType> ProductSizeTypes { get; set; }
        public List<int> SelectedCategories { get; set; }
        public int ParentCategoryId { get; set; }
        public List<IFormFile> formFiles { get; set; }
    }
}