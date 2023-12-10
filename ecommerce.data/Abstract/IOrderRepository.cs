using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.data.Abstract
{
    public interface IOrderRepository:IRepository<Order>
    {
        void AddOrder(Order entity);
        List<Order> GetOrdersByUserId(string userId);

    }
}