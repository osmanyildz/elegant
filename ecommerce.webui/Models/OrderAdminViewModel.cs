using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.webui.Models
{
    public class OrderAdminViewModel
    {
        public int OrderId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Note { get; set; }
        public int? OrderState { get; set; }
        public DateTime Date { get; set; }
        public List<OrderItemDetailModel> orderItemDetailModels { get; set; }
    }
}