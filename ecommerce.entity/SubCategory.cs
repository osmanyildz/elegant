using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.entity
{
    public class SubCategory
    {
        public int Id { get; set; }
        public string subCategoryName { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}