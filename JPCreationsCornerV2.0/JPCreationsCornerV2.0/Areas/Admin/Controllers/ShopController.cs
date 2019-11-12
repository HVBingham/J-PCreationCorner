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
        public ActionResult Categories()
        {
            List<CategoryViewModel> categoryViewModelList;
            using (Context context = new Context())
            {
                categoryViewModelList = context.Categories.ToArray().OrderBy(c => c.Sorting).Select(c => new CategoryViewModel(c)).ToList();
            }
            return View(categoryViewModelList);
        }

        [HttpPost]
        public string AddNewCategory(string catName)
        {
            string id;
            using (Context context = new Context())
            {
                if (context.Categories.Any(c => c.Name == catName))
                {
                    return "titletaken";
                }
                CategoryDTO dto = new CategoryDTO();
                dto.Name = catName;
                dto.Slug = catName.Replace(" ", "-").ToLower();
                dto.Sorting = 100;
                context.Categories.Add(dto);
                context.SaveChanges();
                id = dto.Id.ToString();
            }
            return id;
        }

        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (Context context = new Context())
            {
                int count = 1;
                CategoryDTO dto;
                foreach (var categoryId in id)
                {
                    dto = context.Categories.Find(categoryId);
                    dto.Sorting = count;
                    context.SaveChanges();
                    count++;
                }
            }
        }

        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            using (Context context = new Context())
            {
                CategoryDTO dto = context.Categories.Find(id);
                if (dto == null)
                {
                    return Content("The Page does not exist.");
                }
                context.Categories.Remove(dto);
                context.SaveChanges();
            }
            return RedirectToAction("Categories");
        }
        [HttpPost]
        public string RenameCategory(string  newCatName, int id)
        {
            using (Context context = new Context())
            {
                if (context.Categories.Any(c => c.Name == newCatName))
                    return "titletaken";
                CategoryDTO dto = context.Categories.Find(id);
                dto.Name = newCatName;
                dto.Slug = newCatName.Replace(" ", "-").ToLower();
                context.SaveChanges();
            }
            return "OK";
        }
    }

}