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
            return RedirectToAction("CustomerCart", new { productId = id }) ;
        }
        public ActionResult CustomerCart(int productId)
        {
           
            CustomerCart customerCart = new CustomerCart();
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
                var Id = customer.Id;
                return RedirectToAction("Index", new { id = Id });
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
            Customer customer = context.Customers.Where(c => c.Id == id).SingleOrDefault();
            return View(customer);
        }
        [HttpPost]
        public ActionResult Email(Customer customer, string subject, string message)
        {
            
            
               


                  
                    var senderEmail = new MailAddress("SampleCustomers2019k@gmail.com");
                    var receiverEmail = new MailAddress("samplemoderator2019k@yahoo.com");
                    var password = "AbcPassword1!";
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
                     var Id = customer.Id;
                    return RedirectToAction("Index", "Customers", new { id=Id});
                
            
         
           
        }
      
    }
    

   
}
