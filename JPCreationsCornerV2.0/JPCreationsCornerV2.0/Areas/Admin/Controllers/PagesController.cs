﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreationsCornerV2._0.Models.Data;
using JPCreationsCornerV2._0.Models.ViewModels.Pages;
namespace JPCreationsCornerV2._0.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
       
        public ActionResult Index()
        {
            List<PageViewModel> pagesList;
            using (Context context = new Context())
            {
                pagesList = context.Pages.ToArray().OrderBy(p => p.Sorting).Select(p => new PageViewModel(p)).ToList();
            }

            return View(pagesList);
        }
        [HttpGet]
        public ActionResult AddPage()
        {
           
            return View();
        }
        [HttpPost]
        public ActionResult AddPage(PageViewModel model)
        {
             if (!ModelState.IsValid)
             {
                return View(model);
             }
            using (Context context = new Context())
            {
                string slug;
                PageDTO dto = new PageDTO();
                dto.Title = model.Title;
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }
                if (context.Pages.Any(p => p.Title == model.Title) || context.Pages.Any(p => p.Slug == slug))
                {
                    ModelState.AddModelError(" ", "That title or slug already exists.");
                    return View(model);
                }
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;
                context.Pages.Add(dto);
                context.SaveChanges();
            }
            TempData["SM"] = "You have added a new Page!";
                return RedirectToAction("AddPage");
        }
    }
}