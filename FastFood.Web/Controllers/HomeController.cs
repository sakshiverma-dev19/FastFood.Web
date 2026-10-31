using System.Diagnostics;
using FastFood.Models;
using FastFood.Repository;
using FastFood.Web.Models;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FastFood.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(
            ILogger<HomeController> logger,
            ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index(string search, int? categoryId)
        {
            ViewBag.Categories = _db.Categories
                         .AsNoTracking()
                         .ToList();

            var items = _db.Items
                           .AsNoTracking()
                           .Include(x => x.Category)
                           .Include(x => x.SubCategory)
                           .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim();

                items = items.Where(x => x.Name.Contains(search));
            }

            if (categoryId.HasValue)
            {
                items = items.Where(x => x.CategoryId == categoryId);
            }

            return View(items.ToList());
        }
        public IActionResult Details(int id)
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
        public IActionResult AddToCart(int id)
        {
            var cartItem = _db.Carts
                .FirstOrDefault(x => x.ItemId == id);

            if (cartItem == null)
            {
                Cart cart = new Cart()
                {
                    ItemId = id,
                    Quantity = 1
                };

                _db.Carts.Add(cart);
            }
            else
            {
                cartItem.Quantity++;
            }

            _db.SaveChanges();

            return RedirectToAction("Cart");
        }
        public IActionResult Cart()
        {
            var cartItems = _db.Carts
                .Include(x => x.Item)
                .ToList();

            return View(cartItems);
        }

        public IActionResult RemoveFromCart(int id)
        {
            var cart = _db.Carts.Find(id);

            if (cart != null)
            {
                _db.Carts.Remove(cart);
                _db.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        public IActionResult Preparing(int id)
        {
            var order = _db.OrderHeaders.Find(id);

            if (order != null)
            {
                order.OrderStatus = "Preparing";
                _db.SaveChanges();
            }

            return RedirectToAction("AllOrders");
        }

        public IActionResult Completed(int id)
        {
            var order = _db.OrderHeaders.Find(id);

            if (order != null)
            {
                order.OrderStatus = "Completed";
                _db.SaveChanges();
            }

            return RedirectToAction("AllOrders");
        }
        public IActionResult IncreaseQuantity(int id)
        {
            var cart = _db.Carts.Find(id);

            if (cart != null)
            {
                cart.Quantity++;
                _db.SaveChanges();
            }

            return RedirectToAction("Cart");
        }

        public IActionResult DecreaseQuantity(int id)
        {
            var cart = _db.Carts.Find(id);

            if (cart != null && cart.Quantity > 1)
            {
                cart.Quantity--;
                _db.SaveChanges();
            }

            return RedirectToAction("Cart");
        }
        public IActionResult Checkout()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["Name"] = model.Name;
            TempData["Phone"] = model.Phone;
            TempData["HouseNo"] = model.HouseNo;
            TempData["Street"] = model.Street;
            TempData["City"] = model.City;
            TempData["Pincode"] = model.Pincode;
            TempData["Landmark"] = model.Landmark;

            return RedirectToAction("Payment");
        }

        public IActionResult Payment(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }

        [HttpPost]
        public IActionResult Payment(int orderId, string paymentMethod)
        {
            var order = _db.OrderHeaders.Find(orderId);

            if (order != null)
            {
                order.PaymentMethod = paymentMethod;

                if (paymentMethod == "Cash On Delivery")
                {
                    order.Paymentstatus = "Pending";
                }
                else
                {
                    order.Paymentstatus = "Paid";
                }

                _db.SaveChanges();
            }

            return RedirectToAction("OrderSuccess");
        }
        public IActionResult AllOrders()
        {
            var orders = _db.OrderHeaders
                .OrderByDescending(x => x.Id)
                .ToList();

            return View(orders);
        }
        public IActionResult OrderSuccess()
        {
            return View();
        }

        public IActionResult Orders()
        {
            var orders = _db.OrderHeaders
                .OrderByDescending(x => x.OrderDate)
                .ToList();

            return View(orders);
        }

        public IActionResult CancelOrder(int id)
        {
            var order = _db.OrderHeaders.FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            // Sirf Pending order cancel ho
            if (order.OrderStatus == "Pending")
            {
                order.OrderStatus = "Cancelled";
                _db.SaveChanges();
            }

            return RedirectToAction(nameof(OrderDetails), new { id = id });
        }
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult OrderDetails(int id)
        {
            var order = _db.OrderHeaders
                .FirstOrDefault(x => x.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            ViewBag.Order = order;

            var orderDetails = _db.OrderDetails
                .Include(x => x.Item)
                .Where(x => x.OrderHeaderId == id)
                .ToList();

            return View(orderDetails);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}