using FastFood.Models;
using FastFood.Repository;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SubCategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SubCategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var data = _db.SubCategories
                          .Include(x => x.Category)
                          .ToList();

            return View(data);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryList = new SelectList(
                _db.Categories,
                "Id",
                "Title"
            );

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SubCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                SubCategory obj = new SubCategory()
                {
                    Title = vm.Title,
                    CategoryId = vm.CategoryId
                };

                _db.SubCategories.Add(obj);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryList = new SelectList(
                _db.Categories,
                "Id",
                "Title"
            );

            return View(vm);
        }

        // GET Edit
        public IActionResult Edit(int id)
        {
            var data = _db.SubCategories.Find(id);

            if (data == null)
            {
                return NotFound();
            }

            SubCategoryViewModel vm = new()
            {
                Id = data.Id,
                Title = data.Title,
                CategoryId = data.CategoryId
            };

            ViewBag.CategoryList = new SelectList(
                _db.Categories,
                "Id",
                "Title",
                vm.CategoryId
            );

            return View(vm);
        }

        // POST Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(SubCategoryViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var data = _db.SubCategories.Find(vm.Id);

                if (data == null)
                {
                    return NotFound();
                }

                data.Title = vm.Title;
                data.CategoryId = vm.CategoryId;

                _db.SaveChanges();

                return RedirectToAction("Index");
            }

            ViewBag.CategoryList = new SelectList(
                _db.Categories,
                "Id",
                "Title",
                vm.CategoryId
            );

            return View(vm);
        }
        // GET Delete
        public IActionResult Delete(int id)
        {
            var data = _db.SubCategories
                          .Include(x => x.Category)
                          .FirstOrDefault(x => x.Id == id);

            if (data == null)
            {
                return NotFound();
            }

            return View(data);
        }

        // POST Delete
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var data = _db.SubCategories.Find(id);

            if (data == null)
            {
                return NotFound();
            }

            _db.SubCategories.Remove(data);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

    }
}