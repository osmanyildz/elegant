using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.webui.Models
{
    public class CategoryEditModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public List<int?> genderIds { get; set; }
        public int ParentCategoryId { get; set; }
    }
}