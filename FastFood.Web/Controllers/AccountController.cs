using FastFood.Models;
using FastFood.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace FastFood.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AccountController(
       UserManager<ApplicationUser> userManager,
       SignInManager<ApplicationUser> signInManager,
       IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _webHostEnvironment = webHostEnvironment;
        }
        // Register GET
        public IActionResult Register()
        {
            return View();
        }









        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            ApplicationUser user = new ApplicationUser
            {
                UserName = vm.Email,
                Email = vm.Email
            };

            var result = await _userManager.CreateAsync(user, vm.Password);

            if (result.Succeeded)
            {
                TempData["success"] = "Registration Successful";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(vm);
        }











        // Profile GET
        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            var model = new ProfileViewModel
            {
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                City = user.City,
                Address = user.Address,
                PostalCode = user.PostalCode,
                ProfileImage = user.ProfileImage
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Profile(ProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Login");
            }

            user.Name = model.Name;
            user.PhoneNumber = model.PhoneNumber;
            user.City = model.City;
            user.Address = model.Address;
            user.PostalCode = model.PostalCode;

            if (model.ImageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() +
                                  Path.GetExtension(model.ImageFile.FileName);

                string uploadPath = Path.Combine(
                    _webHostEnvironment.WebRootPath,
                    "ProfileImages");

                if (!Directory.Exists(uploadPath))
                {
                    Directory.CreateDirectory(uploadPath);
                }

                using (var stream = new FileStream(
                    Path.Combine(uploadPath, fileName),
                    FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }

                user.ProfileImage = fileName;
            }

            await _userManager.UpdateAsync(user);

            TempData["success"] = "Profile Updated Successfully.";

            return RedirectToAction(nameof(Profile));
        }
        // Login GET
        public IActionResult Login()
        {
            return View();
        }

        // Login POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    vm.Email,
                    vm.Password,
                    false,
                    false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(vm.Email);

                    if (user != null && user.Email == "admin@gmail.com")
                    {
                        return RedirectToAction(
                            "Index",
                            "Dashboard",
                            new { area = "Admin" });
                    }

                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError("", "Invalid Email or Password");
            }

            return View(vm);
        }

        // Logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}