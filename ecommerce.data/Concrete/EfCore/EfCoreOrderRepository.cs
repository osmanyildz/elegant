using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreRepository<Order, ECommerceContext>, IOrderRepository
    {
        public void AddOrder(Order entity)
        {
            if(entity!=null){
                using(var context = new ECommerceContext()){
                    context.Orders.Add(entity);
                    context.SaveChanges();
                }
            }
        }
        public List<Order> GetOrdersByUserId(string userId)
        {   
             using(var context = new ECommerceContext())
            {

                var orders = context.Orders
                                    .Include(i=>i.OrderItems)
                                    .ThenInclude(i=>i.Product)
                                    .ThenInclude(p=>p.ImageUrls)
                                    .AsQueryable();

                if(!string.IsNullOrEmpty(userId))
                {
                    orders = orders.Where(i=>i.UserId ==userId);
                }

                return orders.ToList();
            }
        }
        public List<Order> GetAllOrders(){
            using (var context = new ECommerceContext())
            {
                var orders = context.Orders.ToList();
                return orders;
            }
        }

        public Order GetOrderItemsByOrderId(int id)
        {
            using(var context =new  ECommerceContext()){
                var order = context.Orders.Where(p=>p.Id==id).Include(i=>i.OrderItems).ThenInclude(k=>k.Product).ThenInclude(o=>o.ImageUrls).FirstOrDefault();
                return order;
            }
        }
    }
}