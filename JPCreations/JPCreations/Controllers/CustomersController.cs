using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using JPCreations.Models;
using Microsoft.AspNet.Identity;

namespace JPCreations.Controllers
{
    public class CustomersController : Controller
    {
        public ApplicationDbContext context;
        public CustomersController()
        {
            context = new ApplicationDbContext();
            
        }
        public ActionResult Index(int id)
        {
           var listOfProducts = context.Products.Include(p => p.Image).Where(p=>p.Quantity>=1 & p.IsActive==true).ToList();
            ProductsIndexView products = new ProductsIndexView();
            products.Products = new List<Product>();
            products.Customer = context.Customers.Where(c=>c.Id==id).SingleOrDefault();
            for(int i=0; i<listOfProducts.Count; i++)
            {
                var productAdd = listOfProducts[i];
                products.Products.Add(productAdd);
            }
            return View(products);
        }
        public ActionResult ProductDetails(int id,int custId)
        {
            ProductsViewModel productsView = new ProductsViewModel();
            productsView.Product = new Product();
            productsView.Customer = context.Customers.Include(c=>c.ApplicationUser).Where(c => c.Id == custId).SingleOrDefault();
            Product product = context.Products.Include(p => p.Image).Where(p => p.Id == id).SingleOrDefault();
            productsView.Product = product;

            return View(productsView);
        }
        [HttpPost]
        public ActionResult ProductDetails(int id,int custid,ProductsViewModel productsView)
        {
            productsView.Product = context.Products.Include(p => p.Image).Where(p => p.Id == id).SingleOrDefault();
            productsView.Customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == custid).SingleOrDefault();
            var quantity = context.Products.Include(p=>p.Image).Where(p => p.Id == id).SingleOrDefault();
            quantity.PurchaseQuantity = productsView.Product.PurchaseQuantity;
            context.SaveChanges();
            return RedirectToAction("CustomerCart", new { productId = id, custId=productsView.Customer.Id }) ;
        }
        public ActionResult CustomerCart(int productId,int custId)
        {
            var customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == custId).SingleOrDefault();
            CustomerCart customerCart = customer.customerCart;
            customerCart.Products = new List<Product>();
            var productAdd = context.Products.Include(p => p.Image).Where(p => p.Id == productId).SingleOrDefault();
            customerCart.Products.Add(productAdd);
            for(int i = 0; i<customerCart.Products.Count; i++)
            {
                customerCart.Products[i].Quantity-=customerCart.Products[i].PurchaseQuantity;
                var price = customerCart.Products[i].Price * (Convert.ToDouble(customerCart.Products[i].PurchaseQuantity));
                customerCart.Total += price;
                context.SaveChanges();  
            }            
            return View(customer);
        }
        [HttpPost]
        public ActionResult CustomerCart(Customer customer)
        { 
            return View();
        }
        public ActionResult Details(int id)
        {
            Customer detailsOfCustomer = context.Customers.Find(id);
            return View(detailsOfCustomer);
        }
        public ActionResult Create()
        {
            Customer newCustomer = new Customer();
            return View(newCustomer);
        }
        [HttpPost]
        public ActionResult Create(Customer customer)
        {
            try
            {
                customer.ApplicationId = User.Identity.GetUserId();
                context.Customers.Add(customer);
                context.SaveChanges();
                var Id = customer.Id;
                return RedirectToAction("Index", new { id = Id });
            }
            catch
            {
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            Customer editCustomer = context.Customers.Find(id);

            return View(editCustomer);
        }
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
        public ActionResult Delete(int id)
        {
            Customer removeCustomer = context.Customers.Find(id);
            return View(removeCustomer);
        }
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
            Customer customer = context.Customers.Include(c=>c.ApplicationUser).Where(c=>c.Id==id).SingleOrDefault();
            return View(customer);
        }
        [HttpPost]
        public ActionResult Email(Customer customer, string subject, string message)
        {
            var user = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == customer.Id).SingleOrDefault();
            var userEmail = user.ApplicationUser.Email;
            var moderatorList = context.Moderators.Include(m=>m.ApplicationUser).ToList();
            List<string> ModeratorEmails = new List<string>();
            for (int i =0; i<moderatorList.Count; i++)
            {
              var  ModEmails = moderatorList[i].ApplicationUser.Email.ToString();
                ModeratorEmails.Add(ModEmails);
            }
            var senderEmail = new MailAddress(userEmail);
            var receiverEmail = new MailAddress(ModeratorEmails[1]);

            var password = "AbcPassword1!";
            var sub = subject;
            var body = message;
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
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(mess);
            }
                var Id = customer.Id;
            return RedirectToAction("Index", "Customers", new { id=Id});
        }
    }
}
