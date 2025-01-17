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
    public class EfCoreCartRepository : EfCoreRepository<Cart>, ICartRepository
    {
        public EfCoreCartRepository(ECommerceContext context):base(context)
        {
            
        }
        private ECommerceContext commerceContext{
            get{return context as ECommerceContext;}
        }
        public void AddToCart(string userId, int productId, int quantity,string sizeType)
        {
            var cart = GetCartByUserId(userId);
            if(cart!=null){
                var index = cart.CartItems.FindIndex(i=>i.ProductId==productId && i.SizeType==sizeType);
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
            
                commerceContext.Carts.Add(entity);
                commerceContext.SaveChanges();
            
        }
        public void DeleteCartItem(CartItem cartItem)
        {
           
                commerceContext.CartItems.Remove(cartItem);
                commerceContext.SaveChanges();
            
        }

        public CartItem GetCartItemById(int cartItemId)
        {
            
                var cartItem = commerceContext.CartItems.Where(i=>i.Id==cartItemId).FirstOrDefault();
                if(cartItem==null){
                    System.Console.WriteLine("cartItem null geldi 1");
                }
                return cartItem;
            
        }

        public Cart GetCartByUserId(string id){
          
                var cart = commerceContext.Carts
                .Include(i=>i.CartItems)
                .ThenInclude(p=>p.Product)
                .ThenInclude(o=>o.ImageUrls)
                
                .FirstOrDefault(i=>i.UserId==id);
                return cart;
            
        }
        public void UpdateCart(Cart entity){
           
                commerceContext.Carts.Update(entity);
                commerceContext.SaveChanges();
            
        }
        public void UpdateCartItem(CartItem entity){
          
                commerceContext.CartItems.Update(entity);
                commerceContext.SaveChanges();
            
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
            
                return commerceContext.Carts
                .Include(i=>i.CartItems)
                .ThenInclude(p=>p.Product)
                .FirstOrDefault(k=>k.UserId==id);
                
            
        }

        public void ClearCart(int cartId)
        {
            
               var cartItems = commerceContext.CartItems.Where(p=>p.CartId==cartId).ToList();
                commerceContext.CartItems.RemoveRange(cartItems);
                commerceContext.SaveChanges();
            
        }
    }
}