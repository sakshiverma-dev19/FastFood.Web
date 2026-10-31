using FastFood.Models;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ItemsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ItemsController(
            ApplicationDbContext db,
            IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index(string search, int? categoryId)
        { ViewBag.Categories = _db.Categories.ToList(); var items = _db.Items.Include(x => x.Category).Include(x => x.SubCategory).AsQueryable(); if (!string.IsNullOrWhiteSpace(search)) { search = search.Trim().ToLower(); items = items.Where(x => x.Name.ToLower().Contains(search)); } if (categoryId.HasValue) { items = items.Where(x => x.CategoryId == categoryId.Value); } return View(items.ToList()); }
        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(
                _db.Categories,
                "Id",
                "Title"
            );

            ViewBag.SubCategoryList = new SelectList(
                _db.SubCategories,
                "Id",
                "Title"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ItemViewModel vm)
        {
            if (ModelState.IsValid)
            {
                string? fileName = null;

                if (vm.ImageFile != null)
                {
                    fileName = Guid.NewGuid().ToString() +
                               Path.GetExtension(vm.ImageFile.FileName);

                    string uploadFolder = Path.Combine(
                        _webHostEnvironment.WebRootPath,
                        "images",
                        "items"
                    );

                    string filePath = Path.Combine(uploadFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        vm.ImageFile.CopyTo(stream);
                    }
                }

                Item item = new Item()
                {
                    Name = vm.Name,
                    Description = vm.Description,
                    Price = vm.Price,
                    ImageUrl = fileName,
                    CategoryId = vm.CategoryId,
                    SubCategoryId = vm.SubCategoryId
                };

                _db.Items.Add(item);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryList = new SelectList(
                _db.Categories,
                "Id",
                "Title"
            );

            ViewBag.SubCategoryList = new SelectList(
                _db.SubCategories,
                "Id",
                "Title"
            );

            return View(vm);
        }

        // GET Edit
        public IActionResult Edit(int id)
        {
            var item = _db.Items.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            ItemViewModel vm = new ItemViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                CategoryId = item.CategoryId,
                SubCategoryId = item.SubCategoryId
            };

            ViewBag.CategoryList = new SelectList(
                _db.Categories,
                "Id",
                "Title",
                vm.CategoryId
            );

            ViewBag.SubCategoryList = new SelectList(
                _db.SubCategories,
                "Id",
                "Title",
                vm.SubCategoryId
            );

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ItemViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                foreach (var item in ModelState)
                {
                    foreach (var error in item.Value.Errors)
                    {
                        Console.WriteLine(item.Key + " : " + error.ErrorMessage);
                    }
                }

                ViewBag.CategoryList = new SelectList(_db.Categories, "Id", "Title", vm.CategoryId);
                ViewBag.SubCategoryList = new SelectList(_db.SubCategories, "Id", "Title", vm.SubCategoryId);

                return View(vm);
            }

            var data = _db.Items.Find(vm.Id);

            if (data == null)
            {
                return NotFound();
            }

            data.Name = vm.Name;
            data.Description = vm.Description;
            data.Price = vm.Price;
            data.CategoryId = vm.CategoryId;
            data.SubCategoryId = vm.SubCategoryId;

            if (vm.ImageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(vm.ImageFile.FileName);

                string folder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "items");

                if (!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }

                string path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    vm.ImageFile.CopyTo(stream);
                }

                data.ImageUrl = fileName;
            }

            _db.Update(data);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
        // GET Delete
        public IActionResult Delete(int id)
        {
            var item = _db.Items
                .Include(x => x.Category)
                .Include(x => x.SubCategory)
                .FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var item = _db.Items.Find(id);

            if (item == null)
            {
                return NotFound();
            }

            _db.Items.Remove(item);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
