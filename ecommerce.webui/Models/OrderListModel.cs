using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.webui.Models
{
    public class OrderListModel
    {
        public int Id { get; set; }
        public string OrderDate { get; set; }
        public string OrderNumber { get; set; }
        public string CustomerId { get; set; }
        public string CustomerNameSurname { get; set; }
        public EnumOrderState orderState { get; set; }

    }
}