using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.webui.Models
{
    public class CategoryModel
    {
        public string Name { get; set; }
        public int parentCategoryId { get; set; }
    }
}