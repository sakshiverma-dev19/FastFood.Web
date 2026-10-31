using FastFood.Repository;
using Microsoft.AspNetCore.Mvc;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
  
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

    public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.TotalOrders = _db.OrderHeaders.Count();

            ViewBag.TotalItems = _db.Items.Count();

            ViewBag.TotalCategories = _db.Categories.Count();

            ViewBag.TotalSales = _db.OrderHeaders.Any()
                ? _db.OrderHeaders.Sum(x => x.OrderTotal)
                : 0;

            ViewBag.RecentOrders = _db.OrderHeaders
                .OrderByDescending(x => x.Id)
                .Take(5)
                .ToList();

            ViewBag.NewOrders = _db.OrderHeaders
                .Where(x => x.OrderStatus == "Pending")
                .OrderByDescending(x => x.Id)
                .Take(5)
                .ToList();

            ViewBag.NewOrderCount = _db.OrderHeaders
                .Count(x => x.OrderStatus == "Pending");

            return View();
        }
    }


}
