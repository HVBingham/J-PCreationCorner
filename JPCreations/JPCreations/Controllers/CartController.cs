using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreations.Models;
using System.Data.Entity;

namespace JPCreations.Controllers
{
    public class CartController : Controller
    {
        ApplicationDbContext context;
        ProductsViewModel productsView;
        List<Item> cart;
        public CartController()
        {
            context = new ApplicationDbContext();
            productsView = new ProductsViewModel();
            ProductAdd();
            cart = new List<Item>();
        }
        
        public ActionResult Index(int id)
        {
            Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == id).SingleOrDefault();
            return View(customer);
        }

        // GET: Cart/Details/5
        public ActionResult Buy(int id, int custId)
        {
            Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == custId).SingleOrDefault();
            productsView.Customer = customer;
            customer.Cart=cart;
            if (cart.Count == 0)
            {
                cart.Add(new Item { Product = productsView.find(id), Quantity = 1 });
            }
            else
            {
                for (int i = 0; i < cart.Count; i++)
                {
                    if (cart[i].Product.Id == id)
                    {
                        cart[i].Quantity++;
                    }
                    else
                    {
                        cart.Add(new Item { Product = productsView.find(id), Quantity = 1 });
                    }
                }
            }
            return RedirectToAction("Index","Cart", new { id = customer.Id });
        }
        public void ProductAdd()
        {
            List<Product> productsList = context.Products.Include(p => p.Image).Where(p => p.IsActive == true & p.Quantity >= 1).ToList();
            for (int i = 0; i < productsList.Count; i++)
            {
                productsView.products.Add(productsList[i]);
            }
        }
       public ActionResult Remove(int id)
        {
            for(int i = 0; i < cart.Count; i++)
            {
                if (cart[i].Product.Id == id)
                {
                    cart.RemoveAt(i);
                }

            }
            return RedirectToAction("Index");
        }

       
    }
}
