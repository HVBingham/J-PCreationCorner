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
    }
}