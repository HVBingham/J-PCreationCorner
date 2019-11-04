using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreations.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

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



            var userAuthentication = User.Identity.IsAuthenticated;

            if (!userAuthentication)
            {
                return RedirectToAction("IndexUser", "Products");
            }

            else
            { 
           
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var currentUser = UserManager.FindById(User.Identity.GetUserId());
                var role = UserManager.GetRoles(User.Identity.GetUserId());



                if (role[0].ToString() == RoleName.Moderator)
                {

                    context.SaveChanges();
                    return RedirectToAction("Index", "Moderators");
                }
                else
                { 

                    if (role[0].ToString() == RoleName.Customer)
                    {
                        Customer customer = context.Customers.Include(c => c.ApplicationUser).Where(c => c.ApplicationId == currentUser.Id).SingleOrDefault();
                            return RedirectToAction("Index", "Customers", new {Id=customer.Id});
                    }
                    else
                    {
                        return RedirectToAction("IndexUser", "Products");
                    }
                }
            }
            
            
                
            
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