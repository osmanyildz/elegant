using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.webui.Models
{
    public class OrderItemDetailModel
    {
        public int Id { get; set; }
        public double? Price { get; set; }
        public string Name { get; set; }
        public string SizeType { get; set; }
        public int? Quantity { get; set; }
        public string ImageUrl { get; set; }
        public int? CancelledState { get; set; }
    }
}