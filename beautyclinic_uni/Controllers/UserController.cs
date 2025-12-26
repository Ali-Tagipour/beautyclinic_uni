using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using Microsoft.AspNetCore.Authorization; // اضافه شد برای امنیت
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    [Authorize] // فقط ادمین لاگین‌شده می‌تواند کاربران را مدیریت کند
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        // نمایش لیست کاربران
        public IActionResult Index()
        {
            var users = _db.Users
                           .OrderByDescending(u => u.Id)
                           .ToList();

            return View(users);
        }

        // افزودن کاربر جدید
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(User u)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", _db.Users.OrderByDescending(x => x.Id).ToList());
            }

            if (u == null || string.IsNullOrWhiteSpace(u.Phone))
            {
                TempData["ErrorMessage"] = "اطلاعات کاربر نامعتبر است.";
                return RedirectToAction("Index");
            }

            // چک تکراری بودن شماره تلفن یا ایمیل
            if (_db.Users.Any(x => x.Phone == u.Phone || (!string.IsNullOrEmpty(u.Email) && x.Email == u.Email)))
            {
                TempData["ErrorMessage"] = "شماره تلفن یا ایمیل قبلاً ثبت شده است.";
                return RedirectToAction("Index");
            }

            // TODO: در آینده حتماً Password را هش کنید (مثلاً با BCrypt)
            // u.Password = BCrypt.Net.BCrypt.HashPassword(u.Password);

            _db.Users.Add(u);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "کاربر با موفقیت اضافه شد.";
            return RedirectToAction("Index");
        }

        // ویرایش کاربر
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(User u)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", _db.Users.OrderByDescending(x => x.Id).ToList());
            }

            if (u == null || u.Id <= 0)
            {
                TempData["ErrorMessage"] = "اطلاعات کاربر نامعتبر است.";
                return RedirectToAction("Index");
            }

            var existingUser = _db.Users.Find(u.Id);
            if (existingUser == null)
            {
                TempData["ErrorMessage"] = "کاربر یافت نشد.";
                return RedirectToAction("Index");
            }

            // چک تکراری بودن شماره تلفن یا ایمیل (به جز خود کاربر)
            if (_db.Users.Any(x => x.Id != u.Id && (x.Phone == u.Phone || (!string.IsNullOrEmpty(u.Email) && x.Email == u.Email))))
            {
                TempData["ErrorMessage"] = "شماره تلفن یا ایمیل قبلاً توسط کاربر دیگری استفاده شده است.";
                return RedirectToAction("Index");
            }

            existingUser.Fullname = u.Fullname;
            existingUser.Phone = u.Phone;
            existingUser.Email = u.Email;

            // اگر رمز جدید وارد شده بود، آن را به‌روزرسانی کن
            if (!string.IsNullOrWhiteSpace(u.Password))
            {
                // TODO: هش کردن رمز
                existingUser.Password = u.Password;
            }

            _db.Users.Update(existingUser);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "اطلاعات کاربر با موفقیت ویرایش شد.";
            return RedirectToAction("Index");
        }

        // حذف کاربر
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
            {
                TempData["ErrorMessage"] = "کاربر یافت نشد.";
                return RedirectToAction("Index");
            }

            _db.Users.Remove(user);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "کاربر با موفقیت حذف شد.";
            return RedirectToAction("Index");
        }
    }
}
// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour