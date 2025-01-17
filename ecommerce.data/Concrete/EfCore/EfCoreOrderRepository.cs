using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreOrderRepository : EfCoreRepository<Order>, IOrderRepository
    {
        public EfCoreOrderRepository(ECommerceContext context):base(context)
        {
            
        }
        private ECommerceContext ECommerceContext{
            get{return context as ECommerceContext;}
        }
        public void AddOrder(Order entity)
        {
            if(entity!=null){

                    ECommerceContext.Orders.Add(entity);
                    ECommerceContext.SaveChanges();
                
            }
        }
        public List<Order> GetOrdersByUserId(string userId)
        {   
            

                var orders = ECommerceContext.Orders
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
        public List<Order> GetAllOrders(){
          
                var orders = ECommerceContext.Orders.ToList();
                return orders;
            
        }

        public Order GetOrderItemsByOrderId(int id)
        {

                var order = ECommerceContext.Orders.Where(p=>p.Id==id).Include(i=>i.OrderItems).ThenInclude(k=>k.Product).ThenInclude(o=>o.ImageUrls).FirstOrDefault();
                return order;
            
        }
    }
}