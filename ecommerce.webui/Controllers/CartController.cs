using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using ecommerce.data.Abstract;
using ecommerce.entity;
using ecommerce.webui.Identity;
using ecommerce.webui.Models;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ecommerce.webui.Controllers
{
    public class CartController : Controller
    {
        private UserManager<User> _userManager;
        private ICartRepository _cartRepository;
        private IOrderRepository _orderRepository;
        public CartController(UserManager<User> userManager, ICartRepository cartRepository, IOrderRepository orderRepository)
        {
            _cartRepository = cartRepository;
            _userManager = userManager;
            _orderRepository = orderRepository;
        }

        public IActionResult Index()
        {
            var cart = _cartRepository.GetCartByUserId(_userManager.GetUserId(User));
            return View(new CartModel()
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = (double)i.Product.Price,
                    SizeType = i.SizeType,
                    ImageUrl = i.Product.ImageUrls.ElementAt(0).ImageUrl,
                    Quantity = i.Quantity
                }).ToList()
            });
        }
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity, string sizeType)
        {
            _cartRepository.AddToCart(_userManager.GetUserId(User), productId, quantity, sizeType);
            // System.Console.WriteLine(productId+"   " +quantity);

            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteCartItem(int cartItemId)
        {
            var cartItem = _cartRepository.GetCartItemById(cartItemId);
            if (cartItem != null)
            {
                _cartRepository.DeleteCartItem(cartItem);
                return Redirect("Index");
            }
            else
            {
                System.Console.WriteLine("cartItem null geldi 2");
                return Redirect("Index");
            }
        }
        [HttpPost]
        public IActionResult AddOne(int cartItemId)
        {
            var cartItem = _cartRepository.GetCartItemById(cartItemId);
            if (cartItem != null)
            {

                _cartRepository.AddToCart(_userManager.GetUserId(User), cartItem.ProductId, 1, cartItem.SizeType);
            }
            else
            {
                System.Console.WriteLine("cartItemId null geldi");
            }
            return Redirect("Index");
        }
        [HttpPost]
        public IActionResult RemoveOne(int cartItemId)
        {
            _cartRepository.RemoveOne(cartItemId);
            return Redirect("Index");
        }
        public IActionResult Checkout()
        {
            var cart = _cartRepository.GetCartByUserId(_userManager.GetUserId(User));
            if (cart == null)
            {
                System.Console.WriteLine("cart null geldi");
            }
            var orderModel = new OrderModel();

            orderModel.CartModel = new CartModel()
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    ProductId = i.ProductId,
                    CartItemId = i.Id,
                    Name = i.Product.Name,
                    Price = i.Product.Price,
                    Quantity = i.Quantity,
                    SizeType = i.SizeType,
                    ImageUrl = i.Product.ImageUrls.ElementAt(0).ImageUrl
                }).ToList()
            };
            return View(orderModel);
        }
        [HttpPost]
        public IActionResult Checkout(OrderModel orderModel)
        {
            var userId = _userManager.GetUserId(User);
            var cart = _cartRepository.GetByUserId(userId);

            orderModel.CartModel = new CartModel()
            {
                Id = cart.Id,
                CartItems = cart.CartItems.Select(i => new CartItemModel()
                {
                    CartItemId = i.Id,
                    ProductId = i.ProductId,
                    Quantity = i.Quantity,
                    Price = i.Product.Price,
                    Name = i.Product.Name,
                    ImageUrl = "",
                }).ToList()
            };

            var payment = PaymentProcess(orderModel);
            if (orderModel.CartModel.TotalPrice() < 750)
            {
                var shippingPayment = ShippingPayment(orderModel);
                if (shippingPayment.Status == "success")
                {
                    System.Console.WriteLine("Kargo ücreti alındı");
                }
                else
                {
                    System.Console.WriteLine("Kargo ücreti alınamadı");
                }
                System.Console.WriteLine(shippingPayment.ErrorMessage);
            }

            // System.Console.WriteLine(payment.ErrorMessage);
            if (payment.Status == "success")
            {
                _cartRepository.ClearCart(cart.Id);
                System.Console.WriteLine("Sipariş alındı");
                _orderRepository.AddOrder(new Order()
                {
                    OrderNumber = Guid.NewGuid().ToString(),
                    OrderDate = DateTime.Now,
                    UserId = _userManager.GetUserId(User),
                    FirstName = orderModel.FirstName,
                    LastName = orderModel.LastName,
                    Address = orderModel.Address,
                    City = orderModel.City,
                    Phone = orderModel.Phone,
                    Email = orderModel.Email,
                    Note = "",
                    PaymentId = payment.PaymentId, //Null olabilir dikkat
                    ConversationId = payment.ConversationId, //dikkat
                    OrderState = EnumOrderState.received,
                    OrderItems = cart.CartItems.Select(i => new entity.OrderItem()
                    {
                        ProductId = i.ProductId,
                        Price = (double)i.Product.Price,
                        Quantity = i.Quantity,
                        SizeType=i.SizeType
                    }).ToList()
                });

            }
            else
            {
                System.Console.WriteLine(payment.ErrorMessage);
                System.Console.WriteLine("Sipariş Alınamadı");
            }

            return RedirectToAction("OrderList");
        }
        public IActionResult OrderList()
        {
            var orders = _orderRepository.GetOrdersByUserId(_userManager.GetUserId(User));
            var orderItemList = new List<OrderItemModel>();
            var dict = new Dictionary<EnumOrderState, string>();
            dict.Add(EnumOrderState.received, "Hazırlanıyor");
            dict.Add(EnumOrderState.completed, "Teslim Edildi");
            dict.Add(EnumOrderState.inCargo, "Kargoda");
            dict.Add(EnumOrderState.cancelled, "İptal Edildi");
            ViewBag.OrderStates = dict;

            if (orders.Count() == 0)
            {
                System.Console.WriteLine("orderslar null geldi");
                return View(orderItemList);
            }
            else
            {
                System.Console.WriteLine("orders null değil");
                var date = new DateTime();
                foreach (var item in orders)
                {
                    foreach (var k in item.OrderItems)
                    {


                        orderItemList.Add(new OrderItemModel()
                        {
                            Name = k.Product.Name,
                            Quantity = k.Quantity,
                            Price = k.Price,
                            ImageUrl = k.Product.ImageUrls.ElementAt(0).ImageUrl,
                            OrderItemId = k.Id,
                            orderState = k.Order.OrderState,
                            OrderDate = item.OrderDate.Date.ToString().Substring(0, 10)
                        });
                    }

                }

                return View(orderItemList);
            }
        }

        private Payment PaymentProcess(OrderModel orderModel)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-CAcVWk93tJaAvrGgBYHiDpJoTC5S9qyM";
            options.SecretKey = "sandbox-uhWadoWXJR09rSRXufBLr7AMOzJmQqZI";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            System.Console.WriteLine(orderModel.CartModel.TotalPrice().ToString());


            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111111, 999999999).ToString();
            request.Price = orderModel.CartModel.TotalPrice().ToString();
            request.PaidPrice = orderModel.CartModel.TotalPrice().ToString();
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = orderModel.CartModel.Id.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();


            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = orderModel.CardName;
            paymentCard.CardNumber = orderModel.CardNumber; // 5528790000000008
            paymentCard.ExpireMonth = orderModel.ExpirationMonth; // 12
            paymentCard.ExpireYear = orderModel.ExpirationYear; // 2030
            paymentCard.Cvc = orderModel.Cvc; // 123
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = orderModel.FirstName;
            buyer.Surname = orderModel.LastName;
            buyer.GsmNumber = orderModel.Phone;
            buyer.Email = orderModel.Email;
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = orderModel.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";

            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;


            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem basketItem;

            foreach (var item in orderModel.CartModel.CartItems)
            {
                basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Name;
                basketItem.Category1 = "Telefon";

                basketItem.Price = (item.Price * item.Quantity).ToString();


                basketItem.Price = (item.Price * item.Quantity).ToString();


                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
                basketItems.Add(basketItem);
            }

            foreach (var item in basketItems)
            {
                System.Console.WriteLine(item.Price);
            }

            request.BasketItems = basketItems;
            return Payment.Create(request, options);

        }
        private Payment ShippingPayment(OrderModel orderModel)
        {
            Options options = new Options();
            options.ApiKey = "sandbox-CAcVWk93tJaAvrGgBYHiDpJoTC5S9qyM";
            options.SecretKey = "sandbox-uhWadoWXJR09rSRXufBLr7AMOzJmQqZI";
            options.BaseUrl = "https://sandbox-api.iyzipay.com";

            CreatePaymentRequest request = new CreatePaymentRequest();
            request.Locale = Locale.TR.ToString();
            request.ConversationId = new Random().Next(111111111, 999999999).ToString();
            request.Price = "29.99";
            request.PaidPrice = "29.99";
            request.Currency = Currency.TRY.ToString();
            request.Installment = 1;
            request.BasketId = orderModel.CartModel.Id.ToString();
            request.PaymentChannel = PaymentChannel.WEB.ToString();
            request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

            PaymentCard paymentCard = new PaymentCard();
            paymentCard.CardHolderName = orderModel.CardName;
            paymentCard.CardNumber = orderModel.CardNumber;
            paymentCard.ExpireMonth = orderModel.ExpirationMonth;
            paymentCard.ExpireYear = orderModel.ExpirationYear;
            paymentCard.Cvc = orderModel.Cvc;
            paymentCard.RegisterCard = 0;
            request.PaymentCard = paymentCard;

            Buyer buyer = new Buyer();
            buyer.Id = "BY789";
            buyer.Name = orderModel.FirstName;
            buyer.Surname = orderModel.LastName;
            buyer.GsmNumber = orderModel.Phone;
            buyer.Email = orderModel.Email;
            buyer.IdentityNumber = "74300864791";
            buyer.LastLoginDate = "2015-10-05 12:43:35";
            buyer.RegistrationDate = "2013-04-21 15:12:09";
            buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            buyer.Ip = "85.34.78.112";
            buyer.City = orderModel.City;
            buyer.Country = "Turkey";
            buyer.ZipCode = "34732";
            request.Buyer = buyer;

            Address shippingAddress = new Address();
            shippingAddress.ContactName = "Jane Doe";
            shippingAddress.City = "Istanbul";
            shippingAddress.Country = "Turkey";
            shippingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            shippingAddress.ZipCode = "34742";
            request.ShippingAddress = shippingAddress;

            Address billingAddress = new Address();
            billingAddress.ContactName = "Jane Doe";
            billingAddress.City = "Istanbul";
            billingAddress.Country = "Turkey";
            billingAddress.Description = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
            billingAddress.ZipCode = "34742";
            request.BillingAddress = billingAddress;

            List<BasketItem> basketItems = new List<BasketItem>();
            BasketItem firstBasketItem = new BasketItem();
            firstBasketItem.Id = orderModel.CartModel.Id.ToString();
            firstBasketItem.Name = "Shipping";
            firstBasketItem.Category1 = "Shipping";
            firstBasketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            firstBasketItem.Price = "29.99";
            basketItems.Add(firstBasketItem);
            request.BasketItems = basketItems;

            Payment payment = Payment.Create(request, options);
            return payment;
        }
    }
}