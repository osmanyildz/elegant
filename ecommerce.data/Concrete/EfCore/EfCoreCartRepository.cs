using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace ecommerce.data.Concrete.EfCore
{
    public class EfCoreCartRepository : EfCoreRepository<Cart, ECommerceContext>, ICartRepository
    {
        public void AddToCart(string userId, int productId, int quantity,string sizeType)
        {
            var cart = GetCartByUserId(userId);
            if(cart!=null){
                // System.Console.WriteLine("Burada1");
                // System.Console.WriteLine(userId);
                // System.Console.WriteLine(productId);
                // System.Console.WriteLine(quantity);
                // System.Console.WriteLine(sizeType);

                var index = cart.CartItems.FindIndex(i=>i.ProductId==productId);
                if(index<0){

                    cart.CartItems.Add(new CartItem(){
                    CartId=cart.Id,
                    ProductId=productId,
                    Quantity=quantity,
                    SizeType=sizeType
                });
                }else{
                    System.Console.WriteLine("Burada3");
                    cart.CartItems[index].Quantity+=quantity;
                }
                 UpdateCart(cart);
            }
        }

        public void CreateFirstCart(string userId)
        {
            var entity = new Cart(){UserId=userId};
            using (var context = new ECommerceContext())
            {
                context.Carts.Add(entity);
                context.SaveChanges();
            }
        }
        public void DeleteCartItem(CartItem cartItem)
        {
            using (var context = new ECommerceContext())
            {
                context.CartItems.Remove(cartItem);
                context.SaveChanges();
            }
        }

        public CartItem GetCartItemById(int cartItemId)
        {
            using (var context = new ECommerceContext())
            {
                var cartItem = context.CartItems.Where(i=>i.Id==cartItemId).FirstOrDefault();
                if(cartItem==null){
                    System.Console.WriteLine("cartItem null geldi 1");
                }
                return cartItem;
            }
        }

        public Cart GetCartByUserId(string id){
            using (var context = new ECommerceContext())
            {
                var cart = context.Carts
                .Include(i=>i.CartItems)
                .ThenInclude(p=>p.Product)
                .ThenInclude(o=>o.ImageUrls)
                
                .FirstOrDefault(i=>i.UserId==id);
                return cart;
            }
        }
        public void UpdateCart(Cart entity){
            using (var context = new ECommerceContext())
            {
                context.Carts.Update(entity);
                context.SaveChanges();
            }
        }
        public void UpdateCartItem(CartItem entity){
            using (var context = new ECommerceContext())
            {
                context.CartItems.Update(entity);
                context.SaveChanges();
            }
        }
        public void RemoveOne(int cartItemId){
            var cartItem = GetCartItemById(cartItemId);
            if(cartItem == null){
                System.Console.WriteLine("cartItem null geldi");
            }
            cartItem.Quantity-=1;
            UpdateCartItem(cartItem);
            if(cartItem.Quantity==0){
                DeleteCartItem(cartItem);
            }
        }

        public Cart GetByUserId(string id)
        {
            using (var context = new ECommerceContext())
            {
                return context.Carts
                .Include(i=>i.CartItems)
                .ThenInclude(p=>p.Product)
                .FirstOrDefault(k=>k.UserId==id);
                
            }
        }
    }
}