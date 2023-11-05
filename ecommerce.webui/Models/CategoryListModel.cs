using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.webui.Models
{
    public class CategoryListModel
    {
        // public List<Category> Categories { get; set; }
        public List<Category> ManCategories { get; set; }
        public List<Category> WomanCategories { get; set; }
    }
}