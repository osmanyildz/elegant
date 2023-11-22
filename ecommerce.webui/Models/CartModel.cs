using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.webui.Models
{
    public class CartModel
    {
        public int Id { get; set; }
        public List<CartItemModel> CartItems { get; set; }
        public double? TotalPrice(){
            // var totalPrice = 29.99;
            var totalPrice=0.0;
            foreach (var item in CartItems)
            {
                if(item.Price==null){
                    continue;
                }else{
                    totalPrice+=item.Quantity*(double)item.Price;
                }
            }
            // totalPrice +=(double) CartItems.Sum(i=>i.Quantity * i.Price);

            return totalPrice;
        }

    }
    public class CartItemModel
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string ImageUrl { get; set; }
        public double? Price { get; set; }
        public string SizeType { get; set; }
        
    }
}