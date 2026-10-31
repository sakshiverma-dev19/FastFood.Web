using FastFood.Models;
using FastFood.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoriesController(ApplicationDbContext db)
        {
            _db = db;
        }

        // List
        public IActionResult Index()
        {
            var categories = _db.Categories
                                .OrderBy(x => x.Title)
                                .ToList();

            return View(categories);
        }

        // GET Create
        public IActionResult Create()
        {
            return View();
        }

        // POST Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();

                TempData["success"] = "Category Created Successfully";
                return RedirectToAction("Index");
            }

            return View(obj);
        }

        // GET Edit
        public IActionResult Edit(int id)
        {
            var category = _db.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (!ModelState.IsValid)
            {
                return View(obj);
            }

            _db.Categories.Update(obj);
            _db.SaveChanges();

            TempData["success"] = "Updated Successfully";

            return RedirectToAction(nameof(Index));
        }
        // GET Delete
        public IActionResult Delete(int id)
        {
            var category = _db.Categories.Find(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Category obj)
        {
            var data = _db.Categories.Find(obj.Id);

            if (data == null)
                return NotFound();

            _db.Categories.Remove(data);
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}