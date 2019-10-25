using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreations.Models;

namespace JPCreations.Controllers
{
    public class ImagesController : Controller
    {
        ApplicationDbContext context;
        public ImagesController()
        {
            context = new ApplicationDbContext();
        }
        [HttpGet]
        public ActionResult Add()
        {
            Image image = new Image();
            return View(image);
        }
        [HttpPost]
        public ActionResult Add(Image image)
        {
            string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
            string extension = Path.GetExtension(image.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff")+extension;
            image.ImagePath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/") + fileName);
            image.ImageFile.SaveAs(fileName);
            context.Images.Add(image);
            context.SaveChanges();
            return View();
        }
    } 
}