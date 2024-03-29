﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using JPCreations.Models;
using JPCreations.Models.ViewModels;
using JPCreations.ViewModels.OrderDTO;
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
            List<Product> productsList = context.Products.Include(p => p.Image).Where(p => p.IsActive == true & p.Quantity >= 1).ToList();
            Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == id).SingleOrDefault();
            ProductsViewModel productsView = new ProductsViewModel();
            productsView.Customer = customer;
            for (int i = 0; i < productsList.Count; i++)
            {
                productsView.products.Add(productsList[i]);
            }

            return View(productsView);
        }
        public ActionResult ProductDetails(int id, int custId)
        {
            ProductsViewModel productsView = new ProductsViewModel();
            productsView.products = new List<Product>();
            productsView.Customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == custId).SingleOrDefault();
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
            var receiverEmail = new MailAddress("SampleModerators2019k@gmail.com");

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
            return RedirectToAction("Index", "Customers", new { id = Id });
        }


        public ActionResult Orders(int id)
        {
            List<OrderViewModel> orders = context.Orders.Include(o => o.Customer).ToArray().Select(x => new OrderViewModel(x)).ToList();
            foreach (var order in orders)
            {
                Dictionary<string, int> productsandQty = new Dictionary<string, int>();
                double total = 0;
                List<OrderDetailsDTO> orderDetailsList = context.OrderDetails.Include(o => o.Customer).Include(o => o.OrderDTO).Include(o => o.Product).Where(x => x.OrderId == order.OrderId).ToList();
                Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == id).FirstOrDefault();
                string name = customer.FirstName + " " + customer.LastName;
                foreach (var orderdetials in orderDetailsList)
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
        public ActionResult SuggestedItems(int id)
        {
            SuggestedItemsViewModel itemsViewModel = new SuggestedItemsViewModel();
            List<OrderViewModel> orders = context.Orders.Include(o => o.Customer).ToArray().Select(x => new OrderViewModel(x)).ToList();
            foreach (var order in orders)
            {
                Dictionary<string, int> productsandQty = new Dictionary<string, int>();
                double total = 0;
                List<OrderDetailsDTO> orderDetailsList = context.OrderDetails.Include(o => o.Customer).Include(o => o.OrderDTO).Include(o => o.Product).Where(x => x.OrderId == order.OrderId).ToList();
                Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == id).FirstOrDefault();
                string name = customer.FirstName + " " + customer.LastName;
                foreach (var orderdetials in orderDetailsList)
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
                Dictionary<string, int> Products2 = new Dictionary<string, int>();
                foreach (var orderdetails in orderDetailsList)
                {
                    string productName = "";
                    Product product1 = context.Products.Include(p => p.Image).Where(p => p.Category == orderdetails.Product.Category).FirstOrDefault();
                    for (int i = 0; i < Products2.Count; i++)
                    {
                         productName = product1.ProductName +i;
                    }
                    Products2.Add(productName,product1.Quantity);
                }
               
                itemsViewModel.products = Products2;
            }
            return View(itemsViewModel);
        }
    }
}

