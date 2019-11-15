using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreations.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;
using JPCreations.Models.ViewModels;
using JPCreations.ViewModels.OrderDTO;

namespace JPCreations.Controllers
{
    public class ModeratorsController : Controller
    {
        ApplicationDbContext context;
        public ModeratorsController()
        {
            context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var listofCustomers = context.Customers.Include(c => c.ApplicationUser).ToList();
            return View(listofCustomers);
        }

        // GET: Moderators/Details/5
        public ActionResult Details(int id)
        {
            var customer = context.Customers.Find(id);
            return View(customer);
        }

        // GET: Moderators/Create
        public ActionResult Create()
        {
            Moderator moderator = new Moderator();
            return View(moderator);
        }

        // POST: Moderators/Create
        [HttpPost]
        public ActionResult Create(Moderator moderator)
        {
            try
            {
                moderator.ApplicationId = User.Identity.GetUserId();
                context.Moderators.Add(moderator);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Moderators/Edit/5
        public ActionResult Edit(int id)
        {
            Moderator moderator = context.Moderators.Find(id);
            return View(moderator);
        }

        // POST: Moderators/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Moderator moderator)
        {
            try
            {
                Moderator moderatorEdit = context.Moderators.Find(id);
                moderatorEdit.FirstName = moderator.FirstName;
                moderatorEdit.LastName = moderator.LastName;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        
        public ActionResult ShippedEmail(int id)
        {
            Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == id).SingleOrDefault();
            return View(customer);
        }
        [HttpPost]
        public ActionResult ShippedEmail(int id, Customer customer)
        {

            var user = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == customer.Id).SingleOrDefault();
            var userEmail = user.ApplicationUser.Email;
            var moderatorList = context.Moderators.Include(m => m.ApplicationUser).ToList();
            List<string> ModeratorEmails = new List<string>();
            for (int i = 0; i < moderatorList.Count; i++)
            {
                var ModEmails = moderatorList[i].ApplicationUser.Email.ToString();
                ModeratorEmails.Add(ModEmails);
            }
            var senderEmail = new MailAddress(userEmail);
            var receiverEmail = new MailAddress(ModeratorEmails[1]);

            var password = "AbcPassword1!";
            var sub = "Your Package Has Shipped";
            var body = "Your Order for has Shipped.";
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
            var Id = customer.Id;
            return View();


            
        }
        public ActionResult Orders()
        {
            List<OrderViewModel> orders = context.Orders.Include(o => o.Customer).ToArray().Select(x => new OrderViewModel(x)).ToList();
            foreach(var order in orders)
            {
                Dictionary<string, int> productsandQty = new Dictionary<string, int>();
                double total = 0;
                List<OrderDetailsDTO> orderDetailsList = context.OrderDetails.Include(o => o.Customer).Include(o => o.OrderDTO).Include(o => o.Product).Where(x => x.OrderId == order.OrderId).ToList();
                Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == order.CustomerId).FirstOrDefault();
                string name = customer.FirstName + " " + customer.LastName;
                foreach(var orderdetials in orderDetailsList)
                {
                    Product product = context.Products.Include(p => p.Image).Where(p => p.Id == orderdetials.ProductId).FirstOrDefault();
                    double price = product.Price;
                    string productName = product.ProductName;
                    productsandQty.Add(productName, orderdetials.Quantity);
                    total += orderdetials.Quantity * price;
                }
                order.Name = name;
                order.ProductsAndQty = productsandQty;
                order.Total = total;
            }
            return View(orders);
        }
    
    }
}
