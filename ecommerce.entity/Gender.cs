using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.entity
{
    public class Gender
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<GenderCategory> GenderCategories { get; set; }
    }
}