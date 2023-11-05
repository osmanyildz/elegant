using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.webui.Models
{
    public class CategoryViewModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public ParentCategory parentCategories { get; set; }
        public List<GenderCategory> genderCategories { get; set; }
        
    }
}