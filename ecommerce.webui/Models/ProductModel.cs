using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.webui.Models
{
    public class ProductModel
    {

         public string Name { get; set; }
        public string Url { get; set; }

        public double? Price { get; set; }
      
        public string Description { get; set; }

        public List<string>? ImageUrls { get; set; }

        public List<Category> Categories { get; set; }

        public int GenderId { get; set; }
 
        public int ParentCategoryId { get; set; }
        public int IsPopular { get; set; }
        
    }
}