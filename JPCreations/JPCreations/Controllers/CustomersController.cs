using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using JPCreations.Models;
using Microsoft.AspNet.Identity;
using SendGrid;
using SendGrid.Helpers.Mail;

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
            List<Product> productsList = context.Products.Include(p=>p.Image).Where(p=>p.IsActive==true & p.Quantity>=1).ToList();
            Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == id).SingleOrDefault();
            ProductsViewModel productsView = new ProductsViewModel();
            productsView.Customer = customer;
            for(int i =0; i<productsList.Count; i++)
            {
                productsView.products.Add(productsList[i]);
            }
            
            return View(productsView);
        }
        public ActionResult ProductDetails(int id,int custId)
        {
            ProductsViewModel productsView = new ProductsViewModel();
            productsView.products = new List<Product>();
            productsView.Customer = context.Customers.Include(c=>c.ApplicationUser).Where(c => c.Id == custId).SingleOrDefault();
            Product product = context.Products.Include(p => p.Image).Where(p => p.Id == id).SingleOrDefault();
            productsView.products.Add(product);

            return View(productsView);
        }
        [HttpPost]
        public ActionResult ProductDetails()
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
            Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == id).SingleOrDefault();
            return View(customer);
        }
        [HttpPost]
        public ActionResult Email(Customer customer, string subject, string message, string Password)
        {
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
            var receiverEmail = new MailAddress(ModeratorEmails[1]);

            ////var password = "AbcPassword1!";
            var password = Password;
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
            return RedirectToAction("Index", "Customers", new { id = Id });
        }

        //public ActionResult Email(int id)
        //{

        //    return View(id);
        //}
        //[HttpPost]
        //public async Task Email(int id, string subject, string message)
        //{
        //    APIKey apiKey = new APIKey();
        //    var key = apiKey.ApiKey;
        //    var user = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == id).SingleOrDefault();
        //    var name = user.FirstName + " "+ user.LastName;
        //    var userEmail = user.ApplicationUser.Email;
        //    var moderatorList = context.Moderators.Include(m => m.ApplicationUser).ToList();
        //    List<string> ModeratorEmails = new List<string>();
        //    for (int i = 0; i < moderatorList.Count; i++)
        //    {
        //        var ModEmails = moderatorList[i].ApplicationUser.Email.ToString();
        //        ModeratorEmails.Add(ModEmails);
        //    }
        //    var client = new SendGridClient(key);
        //    var from = new EmailAddress(userEmail, name);
        //    var sub = subject ;
        //    var to = new EmailAddress(ModeratorEmails[1]);
        //    var plainTextContent = message;
        //    var htmlContent = message;
        //    var msg = MailHelper.CreateSingleEmail(from, to, sub, plainTextContent, htmlContent);
        //    var response = await client.SendEmailAsync(msg);


        //}

    }
}
