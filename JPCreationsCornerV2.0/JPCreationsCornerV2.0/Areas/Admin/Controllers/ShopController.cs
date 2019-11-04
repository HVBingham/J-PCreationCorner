using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JPCreationsCornerV2._0.Models.Data;
using JPCreationsCornerV2._0.Models.ViewModels.Shop;

namespace JPCreationsCornerV2._0.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop
        public ActionResult Index()
        {
            List<CategoryViewModel> categoryViewModelList;
            using(Context context = new Context())
            {
                categoryViewModelList = context.Categories.ToArray().OrderBy(c => c.Sorting).Select(c => new CategoryViewModel(c)).ToList();
            }
            return View(categoryViewModelList);
        }
      
        [HttpPost]
        public ActionResult AddNewCategory(string caName )
        {
            //if (ModelState.IsValid)
            //{
            //    return View(model);
            //}
            //using(Context context = new Context())
            //{
            //    string slug;
            //    CategoryDTO dto = new CategoryDTO();
            //    dto.Name = model.Name;
            //    if (string.IsNullOrWhiteSpace(model.Slug))
            //    {
            //        slug = model.Name.Replace("", "-").ToLower();
            //    }
            //    else
            //    {
            //        slug = model.Slug.Replace("", "-").ToLower();
            //    }
            //    if(context.Categories.Any(c=>c.Name==model.Name)|| context.Categories.Any(c => c.Slug == model.Slug))
            //    {
            //        ModelState.AddModelError("", "That title or slug already exists.");
            //        return View(model);
            //    }
            //    dto.Slug = slug;
            //    dto.Sorting = 100;
            //    context.Categories.Add(dto);
            //    context.SaveChanges();
            //}
            //TempData["SM"] = "You have added a new category!";
            return RedirectToAction("AddNewCategory");
        }
    }

}