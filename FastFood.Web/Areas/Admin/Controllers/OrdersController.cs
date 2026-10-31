using FastFood.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly ApplicationDbContext _db;

        public OrdersController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Details(int id)
        {
            var orderHeader = _db.OrderHeaders
                .FirstOrDefault(x => x.Id == id);

            if (orderHeader == null)
            {
                return NotFound();
            }

            ViewBag.Order = orderHeader;
            ViewBag.Status = orderHeader.OrderStatus;
            ViewBag.OrderId = orderHeader.Id;

            var details = _db.OrderDetails
                .Include(x => x.Item)
                .Where(x => x.OrderHeaderId == id)
                .ToList();

            return View(details);
        }

        public IActionResult Preparing(int id)
        {
            var order = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = "Preparing";
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult OutForDelivery(int id)
        {
            var order = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = "Out For Delivery";

            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        public IActionResult Completed(int id)
        {
            var order = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            order.OrderStatus = "Delivered";
            _db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Invoice(int id)
        {
            var order = _db.OrderHeaders
                .FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Order = order;
            ViewBag.OrderId = order.Id;

            var details = _db.OrderDetails
                .Include(x => x.Item)
                .Where(x => x.OrderHeaderId == id)
                .ToList();

            return View(details);
        }
        public IActionResult Index()
        {
            var orders = _db.OrderHeaders
      .OrderByDescending(x => x.Id)
      .ToList();

            return View(orders);
        }


       

}
}