using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreations.Models;

namespace JPCreations.Controllers
{
    public class ProductsController : Controller
    {
        ApplicationDbContext context;


        public ProductsController()
        {
            context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var listofproducts = context.Products.ToList();
            return View(listofproducts);
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            var product = context.Products.Find(id);
            return View(product);
        }

        // GET: Products/Create
        public ActionResult Create()
        {
            Product product = new Product();
            return View(product);
        }

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                context.Products.Add(product);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Edit/5
        public ActionResult Edit(int id)
        {
            Product product = context.Products.Find(id);
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                Product editProduct = context.Products.Find(id);
                editProduct.ProductName = product.ProductName;
                editProduct.Price = product.Price;
                editProduct.Description = editProduct.Description;
                editProduct.Category = product.Category;
                editProduct.Images = product.Images;

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Products/Delete/5
        public ActionResult Delete(int id)
        {
            Product product = context.Products.Find(id);
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, Product product)
        {
            try
            {
                Product removeProduct = context.Products.Find(id);
                removeProduct = product;
                context.Products.Remove(removeProduct);
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
