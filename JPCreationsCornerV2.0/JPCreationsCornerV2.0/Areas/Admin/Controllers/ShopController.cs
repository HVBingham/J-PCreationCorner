using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using JPCreationsCornerV2._0.Models.Data;
using JPCreationsCornerV2._0.Models.ViewModels.Shop;
using PagedList;

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
        [HttpGet]
        public ActionResult AddProduct()
        {
            ProductViewModel model = new ProductViewModel();
            using (Context context = new Context())
            {
                model.Categories = new SelectList(context.Categories.ToList(), "Id", "Name");
            }
                return View(model);
        }
        [HttpPost]
        public ActionResult AddProduct(ProductViewModel model, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                using (Context contex = new Context())
                {
                    model.Categories = new SelectList(contex.Categories.ToList(), "Id", "Name");
                    return View(model);
                }
            }
            using (Context context =new Context())
            {
                if (context.Products.Any(p => p.Name == model.Name))
                {
                    model.Categories = new SelectList(context.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("", "That product name is taken!");
                    return View(model);
                }
            }
            int id;
            using (Context context = new Context())
            {
                ProductDTO product = new ProductDTO();
                product.Name = model.Name;
                product.Slug = model.Name.Replace(" ", "-").ToLower();
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                CategoryDTO catDTO = context.Categories.FirstOrDefault(c => c.Id == model.CategoryId);
                product.CategoryName = catDTO.Name;
                context.Products.Add(product);
                context.SaveChanges();
                id = product.Id;
            }
            TempData["SM"] = "You have added a product!";
            #region Upload Image
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            var pathString1 = Path.Combine(originalDirectory.ToString(), "Products");
            var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\"+id.ToString());
            var pathString3 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
            var pathString4 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
            var pathString5 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");
            if (!Directory.Exists(pathString1))
                 Directory.CreateDirectory(pathString1);
                if (!Directory.Exists(pathString2))
                    Directory.CreateDirectory(pathString2);
                if (!Directory.Exists(pathString3))
                    Directory.CreateDirectory(pathString3);
                if (!Directory.Exists(pathString4))
                    Directory.CreateDirectory(pathString4);
                if (!Directory.Exists(pathString5))
                    Directory.CreateDirectory(pathString5);

            if (file != null && file.ContentLength > 0)
            {
                string ext = file.ContentType.ToLower();
                    if (ext != "image/jpg" &&
                        ext != "image/jpeg" &&
                        ext != "image/pjeg" &&
                        ext != "image/gif" &&
                        ext != "image/x-png" &&
                        ext != "image/png")
                    {
                        using (Context context = new Context())
                        {
                            model.Categories = new SelectList(context.Categories.ToList(), "Id", "Name");
                            ModelState.AddModelError("", "The image was not uploaded - wrong image extenstion.");
                            return View(model);
                        }
                    }
                    string imageName = file.FileName;
                    using (Context context = new Context())
                    {
                        ProductDTO dto = context.Products.Find(id);
                        dto.ImageName = imageName;
                        context.SaveChanges();
                    }
                    var path = string.Format("{0}\\{1}", pathString2, imageName);
                    var path2 = string.Format("{0}\\{1}", pathString3, imageName);
                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
            }

            #endregion
            return RedirectToAction("AddProduct");
        }
        public ActionResult Products(int? page, int? catId)
        {
            List<ProductViewModel> listOfProductViewModel;
            var pageNumber = page ?? 1;
            using (Context context = new Context())
            {
                listOfProductViewModel = context.Products.ToArray()
                    .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                    .Select(x => new ProductViewModel(x))
                    .ToList();
                ViewBag.Categories = new SelectList(context.Categories.ToList(), "Id", "Name");
                ViewBag.SelectedCat = catId.ToString();
            }
            var onePageofProducts = listOfProductViewModel.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfProducts = onePageofProducts;
            return View(listOfProductViewModel);
        }
        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            ProductViewModel model;
            using(Context context = new Context())
            {
                ProductDTO dto = context.Products.Find(id);
                if(dto== null)
                {
                    return Content("That product does not exist.");
                }
                model = new ProductViewModel(dto);
                model.Categories = new SelectList(context.Categories.ToList(), "Id", "Name");
                model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                    .Select(f=>Path.GetFileName(f));
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult EditProduct(ProductViewModel model, HttpPostedFileBase file)
        {
            int id = model.Id;
            using(Context context = new Context())
            {
                model.Categories = new SelectList(context.Categories.ToList(), "Id", "Name");
            }
            model.GalleryImages = Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id +"/Gallery/Thumbs"))
                .Select(fn => Path.GetFileName(fn));
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using(Context context1 = new Context())
            {
                if (context1.Products.Where(p => p.Id!= id).Any(p=>p.Name == model.Name))
                {
                    ModelState.AddModelError(" ", "That product name is taken.");
                    return View(model);
                }
            }
            using(Context context2 = new Context())
            {
                ProductDTO dto = context2.Products.Find(id);
                dto.Name = model.Name;
                dto.Slug = model.Name.Replace(" ", "-").ToLower();
                dto.Price = model.Price;
                dto.Description = model.Description;
                dto.CategoryId = model.CategoryId;
                dto.ImageName = model.ImageName;
                CategoryDTO categoryDTO = context2.Categories.FirstOrDefault(c => c.Id == model.CategoryId);
                dto.CategoryName = categoryDTO.Name;
                context2.SaveChanges();
            }
            TempData["SM"] = "You have edited the product!";
            #region Image Upload
            if(file != null && file.ContentLength > 0)
            {
                string ext = file.ContentType.ToLower();
                if(ext != "image/jpg"&&
                    ext != "image/jpeg" &&
                    ext != "image/pjpeg"&&
                    ext != "image/gif"&&
                    ext != "image/x-png"&&
                    ext!= "image/png")
                {
                    using(Context context = new Context())
                    { 
                        ModelState.AddModelError(" ", "The image was not uploaded - wrong image extention.");
                        return View(model);
                    }
                }
                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                var pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
                var pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Thumbs");
                DirectoryInfo di1 = new DirectoryInfo(pathString1);
                DirectoryInfo di2 = new DirectoryInfo(pathString2);
                foreach (FileInfo file2 in di1.GetFiles())
                    file2.Delete();
                foreach (FileInfo file3 in di2.GetFiles())
                    file3.Delete();
                string imageName = file.FileName;
                using(Context context1 = new Context())
                {
                    ProductDTO dto = context1.Products.Find(id);
                    dto.ImageName = imageName;
                    context1.SaveChanges();
                }
                var path = string.Format("{0}\\{1}", pathString1, imageName);
                var path2 = string.Format("{0}\\{1}", pathString2, imageName);
                file.SaveAs(path);
                WebImage img = new WebImage(file.InputStream);
                img.Resize(200, 200);
                img.Save(path2);

            }
            #endregion

            return RedirectToAction("EditProduct");
        }
        public ActionResult DeleteProduct(int id)
        {
            using(Context context = new Context())
            {
                ProductDTO dto = context.Products.Find(id);
                context.Products.Remove(dto);
                context.SaveChanges();
            }
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
            string pathString = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString());
            if (Directory.Exists(pathString))
                Directory.Delete(pathString, true);
            return RedirectToAction("Products");
        }
        [HttpPost]
        public void SaveGalleryImages(int  id)
        {
            foreach(string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];
                if (file != null && file.ContentLength > 0)
                {
                    var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                    string pathString1 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery");
                    string pathString2 = Path.Combine(originalDirectory.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");
                    var path = string.Format("{0}\\{1}", pathString1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", pathString2, file.FileName);
                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);
                }
            }
            
        }
    }
    


}