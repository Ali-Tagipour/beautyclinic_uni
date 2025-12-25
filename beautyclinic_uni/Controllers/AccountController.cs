using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace beautyclinic_uni.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Login
        [HttpGet]
        public IActionResult Login()
        {
            return View("~/Views/login&signup/login.cshtml");
        }

        // POST: Login - شماره تلفن + Password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string phone, string password)
        {
            if (string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "لطفاً شماره تلفن و رمز عبور را وارد کنید.";
                return View("~/Views/login&signup/login.cshtml");
            }

            var user = _context.Users
                .FirstOrDefault(u => u.Phone == phone && u.Password == password);

            if (user == null)
            {
                ViewBag.Error = "شماره تلفن یا رمز عبور اشتباه است.";
                return View("~/Views/login&signup/login.cshtml");
            }

            // لاگین موفق
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.Fullname ?? "کاربر");

            return RedirectToAction("Index", "Home");
        }

        // GET: Signup
        [HttpGet]
        public IActionResult Signup()
        {
            return View("~/Views/login&signup/signup.cshtml");
        }

        // POST: Signup - شامل Password
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Signup(User model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/login&signup/signup.cshtml", model);
            }

            // چک تکراری بودن شماره تلفن یا ایمیل
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

            TempData["SuccessMessage"] = "حساب شما با موفقیت ساخته شد. حالا می‌توانید وارد شوید.";

            return RedirectToAction("Login");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
