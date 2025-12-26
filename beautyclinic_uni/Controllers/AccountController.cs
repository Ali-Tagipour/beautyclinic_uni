using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace beautyclinic_uni.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ======================
        // GET: Login
        // ======================
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/login&signup/login.cshtml");
        }

        // ======================
        // POST: Login
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string phone, string password)
        {
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "لطفاً شماره تلفن و رمز عبور را وارد کنید.";
                return View("~/Views/login&signup/login.cshtml");
            }

            var user = _context.Users.FirstOrDefault(u => u.Phone == phone);

            if (user == null || user.Password != password)
            {
                ViewBag.Error = "شماره تلفن یا رمز عبور اشتباه است.";
                return View("~/Views/login&signup/login.cshtml");
            }

            // ===== ساخت Cookie Auth =====
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Fullname ?? "کاربر"),
                new Claim(ClaimTypes.MobilePhone, user.Phone ?? "")
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal
            );

            // ✅ بعد از لاگین → داشبورد
            return RedirectToAction("Index", "Dashboard");
        }
        // Project: BeautyClinic_Uni
        // Author: Ali Tagipour

        // ======================
        // GET: Signup
        // ======================
        [HttpGet]
        public IActionResult Signup()
        {
            return View("~/Views/login&signup/signup.cshtml");
        }

        // Project: BeautyClinic_Uni
        // Developer: Ali Tagipour


        // ======================
        // POST: Signup
        // ======================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(User model)
        {
            if (!ModelState.IsValid)
                return View("~/Views/login&signup/signup.cshtml", model);

            bool exists = _context.Users.Any(u =>
                u.Phone == model.Phone ||
                (!string.IsNullOrEmpty(u.Email) && u.Email == model.Email));

            if (exists)
            {
                ViewBag.Error = "این شماره تلفن یا ایمیل قبلاً ثبت شده است.";
                return View("~/Views/login&signup/signup.cshtml", model);
            }

            _context.Users.Add(model);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "ثبت‌نام با موفقیت انجام شد. لطفاً وارد شوید.";
            return RedirectToAction("Login");
        }

        // ======================
        // Logout
        // ======================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return RedirectToAction("Login");
        }
    }
}
// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour


