using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.webui.Models
{
    public class SubCategoryModel
    {
        public string Name { get; set; }
        public int ParentCategoryId { get; set; }
        public List<Category> Categories { get; set; }
    }
}