using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.entity
{
    public class GenderCategory
    {
        public int GenderId { get; set; }
        public Gender Gender { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}