using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreations.Models;
using System.Data.Entity;

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
            var listofproducts = context.Products.Include(p => p.Image).Where(p=>p.IsActive==true).ToList();
            return View(listofproducts);
        }
        public ActionResult Deactivated()
        {
            var listofDeactivated = context.Products.Include(p => p.Image).Where(p => p.IsActive == false).ToList();
            return View(listofDeactivated);
        }

        // GET: Products/Details/5
        public ActionResult Details(int id)
        {
            var product = context.Products.Include(p => p.Image).SingleOrDefault(p => p.Id == id);
            return View(product);
        }

   
        // GET: Products/Create
        public ActionResult Create()
        {
            var images = context.Images.ToList();
            var product = new Product()
            {
                Images = images
            };
            //ViewBag.Products = new SelectList(context.Images.ToList(), "Title", "Title");
            return View(product);
        }

        // POST: Products/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            try
            {
                //ViewBag.Products = new SelectList(context.Images.ToList(), "Title", "Title");
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
            var images = context.Images.ToList();
            Product product = context.Products.Include(p=>p.Image).Where(p => p.Id == id).SingleOrDefault();
            //ViewBag.Images = new SelectList(context.Images.ToList(), "Id", "Title");
            product.Images = images;
            return View(product);
        }

        // POST: Products/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Product product)
        {
            
            try
            {
                Product editProduct = context.Products.Include(p => p.Image).Where(p => p.Id == id).SingleOrDefault();
                editProduct.ProductName = product.ProductName;
                editProduct.Price = product.Price;
                editProduct.Description = editProduct.Description;
                editProduct.Category = product.Category;
                editProduct.ImageId = product.ImageId;
                editProduct.IsActive = product.IsActive;
                editProduct.Quantity = product.Quantity;
                context.SaveChanges();
                //ViewBag.Images = new SelectList(context.Images.ToList(), "Id", "Title");
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
