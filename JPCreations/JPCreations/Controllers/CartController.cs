
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using JPCreations.Models;
using JPCreations.ViewModels;
using JPCreations.ViewModels.OrderDTO;

namespace JPCreations.Controllers
{
    public class CartController : Controller
    {
        ApplicationDbContext context;
        public CartController()
        {
            context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var cart = Session["cart"] as List<CartViewModel> ?? new List<CartViewModel>();
            if(cart.Count==0|| Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }
            double total = 0;
            foreach(var item in cart)
            {
                total += item.Total;

            }
            ViewBag.GrandTotal = total;
            return View(cart);
        }
        public ActionResult CartPartial()
        {
            CartViewModel model = new CartViewModel();
            int qty = 0;
            double price = 0;
            if (Session["cart"] != null)
            {
                var list = (List<CartViewModel>)Session["cart"];
                foreach(var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity*item.Price;
                }
                model.Quantity = qty;
                model.Price = price;
            }
            else
            {
                model.Quantity = 0;
                model.Price = 0;
            }

            return PartialView(model); ;
        }
        public ActionResult AddToCartPartial(int id)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel> ?? new List<CartViewModel>();
            CartViewModel model = new CartViewModel();
            Product product = context.Products.Include(p => p.Image).Where(p => p.Id == id).SingleOrDefault(); ;
            var productInCart = cart.FirstOrDefault(x => x.ProductId == id);
            if (productInCart == null)
            {
                cart.Add(new CartViewModel()
                {
                    ProductId = product.Id,
                    ProductName = product.ProductName,
                    Quantity = 1,
                    Price = product.Price,
                    Image = product.Image,
                });                
            }
            else
            {
                productInCart.Quantity++;
            }
            int qty = 0;
            double price = 0;
            foreach( var item in cart)
            {
                qty += item.Quantity;
                price += item.Quantity * item.Price;
            }
            model.Quantity = qty;
            model.Price = price;
            Session["cart"] = cart;

            return PartialView(model);
        }
        [HttpGet]
        public JsonResult IncrementProduct(int productId)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;
            CartViewModel model = cart.FirstOrDefault(x => x.ProductId == productId);
            model.Quantity++;
            var result = new { qty = model.Quantity, price = model.Price };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public JsonResult DecrementProduct(int productId)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;
            CartViewModel model = cart.FirstOrDefault(x => x.ProductId == productId);
            if (model.Quantity > 1)
            {
                model.Quantity--;
            }
            else
            {
                model.Quantity = 0;
                cart.Remove(model);
            }
            var result = new { qty = model.Quantity, price = model.Price };
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public void RemoveProduct(int productId)
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;
            CartViewModel model = cart.FirstOrDefault(x => x.ProductId == productId);
            cart.Remove(model);
        }
        public ActionResult PayPalPartial()
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;

            return PartialView(cart);
        }
        [HttpPost]
        public void PlaceOrder()
        {
            List<CartViewModel> cart = Session["cart"] as List<CartViewModel>;
            string userName = User.Identity.Name;
            OrderDTO orderDTO = new OrderDTO();
            var q = context.Customers.Include(c => c.ApplicationUser).Where(c => c.ApplicationUser.UserName == userName).FirstOrDefault();
            int userId = q.Id;
            orderDTO.CustomerId = userId;
            orderDTO.OrderDate = DateTime.Now;
            context.Orders.Add(orderDTO);
            context.SaveChanges();
            int orderId = orderDTO.OrderId;
            OrderDetailsDTO orderDetailsDTO = new OrderDetailsDTO();
            foreach(var item in cart)
            {
                orderDetailsDTO.OrderId = orderId;
                orderDetailsDTO.CustomerId = userId;
                orderDetailsDTO.ProductId = item.ProductId;
                orderDetailsDTO.Quantity = item.Quantity;
                context.OrderDetails.Add(orderDetailsDTO);
                context.SaveChanges();
            }
            Customer customer = context.Customers.Where(c => c.Id == userId).SingleOrDefault();
            var user = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == customer.Id).SingleOrDefault();
            var userEmail = user.ApplicationUser.Email;

            var senderEmail = new MailAddress(userEmail);
            var moderatorList = context.Moderators.Include(m => m.ApplicationUser).ToList();
            List<string> ModeratorEmails = new List<string>();
            for (int i = 0; i < moderatorList.Count; i++)
            {
                var ModEmails = moderatorList[i].ApplicationUser.Email.ToString();
                ModeratorEmails.Add(ModEmails);
            }
            var receiverEmail = new MailAddress("SampleModerators2019k@gmail.com");
            var order = context.Orders.Where(o => o.OrderId == orderId).SingleOrDefault();
            var password = "AbcPassword1!";
            var sub = "New Order number " +orderId ;
            var body = "You have received a new order. Order Id number " +orderId +". The order was placed on " +order.OrderDate +"Please log in to view the order details.";
            var smtp = new SmtpClient()
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body
            })
            {
                smtp.Send(mess);
            }
        }
    }

}
