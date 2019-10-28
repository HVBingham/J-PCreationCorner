﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreations.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace JPCreations.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext context;
        public HomeController()
        {
            context = new ApplicationDbContext();
        }
        public ActionResult Index()
        {
            // var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            // var role = UserManager.GetRoles(User.Identity.GetUserId());
            // if (role[0].ToString() == RoleName.Customer)
            // {
            //     return RedirectToAction("Index", "Customers");
            // }
            //else
            //{
            //     return RedirectToAction("Index", "Moderators");
            //}
            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}