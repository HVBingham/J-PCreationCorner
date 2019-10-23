using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreations.Models;
using Microsoft.AspNet.Identity;

namespace JPCreations.Controllers
{
    public class ModeratorsController : Controller
    {
        ApplicationDbContext context;
        public ModeratorsController()
        {
            context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            var listofCustomers = context.Customers.ToList();
            return View(listofCustomers);
        }

        // GET: Moderators/Details/5
        public ActionResult Details(int id)
        {
            var customer = context.Customers.Find(id);
            return View(customer);
        }

        // GET: Moderators/Create
        public ActionResult Create()
        {
            Moderator moderator = new Moderator();
            return View(moderator);
        }

        // POST: Moderators/Create
        [HttpPost]
        public ActionResult Create(Moderator moderator)
        {
            try
            {
                moderator.ApplicationId = User.Identity.GetUserId();
                context.Moderators.Add(moderator);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Moderators/Edit/5
        public ActionResult Edit(int id)
        {
            Moderator moderator = context.Moderators.Find(id);
            return View(moderator);
        }

        // POST: Moderators/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, Moderator moderator)
        {
            try
            {
                Moderator moderatorEdit = context.Moderators.Find(id);
                moderatorEdit.FirstName = moderator.FirstName;
                moderatorEdit.LastName = moderator.LastName;
                context.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Moderators/Delete/5
        //public ActionResult Delete(int id)
        //{
           
        //    return View();
        //}

        //// POST: Moderators/Delete/5
        //[HttpPost]
        //public ActionResult Delete(int id, FormCollection collection)
        //{
        //    try
        //    {
        //        // TODO: Add delete logic here

        //        return RedirectToAction("Index");
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
