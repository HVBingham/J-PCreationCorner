using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using JPCreations.Models;
using JPCreations.ViewModels;

namespace JPCreations.Controllers
{
    public class CartController : Controller
    {
        public ActionResult Index()
        {
            var cart = Session["cart"] as List<CartViewModel> ?? new List<CartViewModel>();
            if(cart.Count==0|| Session["cart"] == null)
            {
                ViewBag.Message = "Your cart is empty.";
                return View();
            }
            decimal total = 0m;
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
            decimal price = 0m;
            if (Session["cart"] != null)
            {
                var list = (List<CartViewModel>)Session["cart"];
                foreach(var item in list)
                {
                    qty += item.Quantity;
                    price += item.Quantity*item.Price;
                }
            }
            else
            {
                model.Quantity = 0;
                model.Price = 0m;
            }

            return PartialView(model); ;
        }
        //ApplicationDbContext context;
        //ProductsViewModel productsView;
        //Customer customer;
        //public CartController()
        //{
        //    context = new ApplicationDbContext();
        //    productsView = new ProductsViewModel();
        //    ProductAdd();
        //}
        
        //public ActionResult Index(int id)
        //{
        //    FindCustomer(id);
        //    return View(customer);
        //}

        // GET: Cart/Details/5
       // public ActionResult Buy(int id, int custId)
       // {
       //     FindCustomer(custId);
       //     productsView.Customer = customer;

       //     if (customer.Items.Count == 0)
       //     {
       //         customer.Items.Add(new Item { Product = productsView.find(id), Quantity = 1 });
       //         context.SaveChanges();
       //     }
       //     else
       //     {
       //         for (int i = 0; i < customer.Items.Count; i++)
       //         {
       //             if (customer.Items[i].Product.Id == id)
       //             {
       //                 customer.Items[i].Quantity++;
       //                 context.SaveChanges();
       //             }
       //             else
       //             {
       //                 customer.Items.Add(new Item { Product = productsView.find(id), Quantity = 1 });
       //                 context.SaveChanges();
       //             }
       //         }
       //     }
          
       //     return RedirectToAction("Index", "Cart", new { id = custId });
       // }
       // public void FindCustomer(int id)
       // {
       //   customer= context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == id).SingleOrDefault();
       //     if (customer.Items == null)
       //     {
       //         customer.Items = new List<Item>();
       //     }
       // }
       // public void ProductAdd()
       // {
       //     List<Product> productsList = context.Products.Include(p => p.Image).Where(p => p.IsActive == true & p.Quantity >= 1).ToList();
       //     for(int i=0; i < productsList.Count; i++)
       //     {
       //         productsView.products.Add(productsList[i]);
       //     }
         
       // }
       //public ActionResult Remove(int id, int custId)
       // {
       //     Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.Id == custId).SingleOrDefault();
       //     productsView.Customer = customer;
       //     var cart = customer.Items;

       //     for (int i = 0; i < customer.Items.Count; i++)
       //     {
       //         if (customer.Items[i].Product.Id == id)
       //         {
       //             customer.Items.RemoveAt(i);
       //         }

       //     }
       //     return RedirectToAction("Index");
       // }

       
    }
}
