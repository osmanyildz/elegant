using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ecommerce.entity
{
    public class Order
    {
           public int Id { get; set; }
        public string OrderNumber { get; set; } //Benzersiz bir string order oluşturulurken random oluşturulur
        public DateTime OrderDate { get; set; }
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }
        public string PaymentId { get; set; }
        public string ConversationId { get; set; }
        public EnumOrderState OrderState { get; set; }
        public List<OrderItem> OrderItems { get; set; }
    }
    public enum EnumOrderState{
        received=0,
        cancelled=1,
        inCargo=2,
        completed=3
    }
    }