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

namespace JPCreations.Controllers
{
    public class CustomersController : Controller
    {
        public ApplicationDbContext context;
        public CustomersController()
        {
            context = new ApplicationDbContext();
            
        }
        // GET: Customers
        public ActionResult Index()
        {
         
            var listofProducts = context.Products.Include(p => p.Image).Where(p=>p.Quantity>=1 & p.IsActive==true).ToList();
            
            return View(listofProducts);
        }
        public ActionResult ProductDetails(int id)
        {
            ProductsViewModel productsView = new ProductsViewModel();
            productsView.Product = new Product();
            Product product = context.Products.Include(p => p.Image).Where(p => p.Id == id).SingleOrDefault();
            productsView.Product = product;
            return View(productsView);
        }
        [HttpPost]
        public ActionResult ProductDetails(int id,Product product)
        {
            var quantity = context.Products.Find(id);
            quantity.PurchaseQuantity = product.PurchaseQuantity;
            context.SaveChanges();
            return RedirectToAction("CustomerCart", new { Id = id });
        }
        public ActionResult CustomerCart(int productId, int custyId)
        {
           
            CustomerCart customerCart = new CustomerCart();
            customerCart.Products = new List<Product>();
            Customer customer = context.Customers.Find(custyId);
            var productAdd = context.Products.Include(p => p.Image).Where(p => p.Id == productId).SingleOrDefault();
            customerCart.Products.Add(productAdd);
            for(int i = 0; i<customerCart.Products.Count; i++)
            {
               
                customerCart.Products[i].Quantity-=customerCart.Products[i].PurchaseQuantity;
                var price = customerCart.Products[i].Price * (Convert.ToDouble(customerCart.Products[i].PurchaseQuantity));
                customerCart.Total += price;
                
                context.SaveChanges();

               
            }

            customer.customerCart = customerCart;
            
            return View(customerCart);
        }
        [HttpPost]
        public ActionResult CustomerCart(CustomerCart customerCart)
        {

            return View();
        }
        public ActionResult Details(int id)
        {
            Customer detailsOfCustomer = context.Customers.Find(id);
            return View(detailsOfCustomer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            Customer newCustomer = new Customer();
            return View(newCustomer);
        }

        // POST: Customers/Create
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                customer.ApplicationId = User.Identity.GetUserId();
                context.Customers.Add(customer);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int id)
        {
            Customer editCustomer = context.Customers.Find(id);

            return View(editCustomer);
        }

        // POST: Customers/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Customer customer)
        {
            try
            {

                var editCustomer = context.Customers.Find(id);
                editCustomer.FirstName = customer.FirstName;
                editCustomer.LastName = customer.LastName;
                editCustomer.StreetAddress = customer.StreetAddress;
                editCustomer.City = customer.City;
                editCustomer.State = customer.State;
                editCustomer.ZipCode = customer.ZipCode;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int id)
        {
            Customer removeCustomer = context.Customers.Find(id);
            return View(removeCustomer);
        }

        // POST: Customers/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Customer customer)
        {
            try
            {
                Customer removeCustomer = context.Customers.Find(id);
                removeCustomer = customer;
                context.Customers.Remove(removeCustomer);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Email(int id)
        {
            Customer customer = context.Customers.Find(id);
            return View(customer);
        }
        [HttpPost]
        public ActionResult Email(Customer customer,string subject, string message)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    
                    var userEmail = customer.ApplicationUser.Email;
                    var userName = customer.FirstName;
                    var moderatorsEmail = ModEmail();
                    var senderEmail = new MailAddress(userEmail, userName);
                    var receiverEmail = new MailAddress(moderatorsEmail);
                    var password = "Your Email Password here";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
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
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }
        public string ModEmail()
        {
            var moderators = context.Moderators.ToList();
            for (int i = 0; i < moderators.Count; i++)
            {
                var modEmail = moderators[i].ApplicationUser.Email.ToString();
                return modEmail;
            }
            return null;

        }
    }
    

   
}
