using FastFood.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.ViewComponents
{
    public class AdminNotificationViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _db;

        public AdminNotificationViewComponent(ApplicationDbContext db)
        {
            _db = db;
        }

        public IViewComponentResult Invoke()
        {
            var orders = _db.OrderHeaders
                .Where(x => x.OrderStatus == "Pending")
                .OrderByDescending(x => x.Id)
                .Take(5)
                .ToList();

            ViewBag.Count = orders.Count;

            return View(orders);
        }
    }
}