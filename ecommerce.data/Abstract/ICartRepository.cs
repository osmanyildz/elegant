using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.entity;

namespace ecommerce.data.Abstract
{
    public interface ICartRepository:IRepository<Cart>
    {
        void CreateFirstCart(string userId);
        Cart GetCartByUserId(string userId);
        void AddToCart(string userId, int productId, int quantity,string sizeType);
        void DeleteCartItem(CartItem cartItem);
        CartItem GetCartItemById(int cartItemId);
        void RemoveOne(int cartItemId);
        Cart GetByUserId(string id);
        void ClearCart(int cartId);
    }
}