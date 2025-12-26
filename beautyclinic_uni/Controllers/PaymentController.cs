using Accura.Models;
using beautyclinic_uni.Data;
using Microsoft.AspNetCore.Authorization; // اضافه شد برای امنیت
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    [Authorize] // فقط کاربران لاگین‌شده (ادمین) می‌توانند به مدیریت پرداخت‌ها دسترسی داشته باشند
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PaymentController(ApplicationDbContext db)
        {
            _db = db;
        }

        // نمایش لیست پرداخت‌ها
        public IActionResult Index()
        {
            var payments = _db.Payments
                              .OrderByDescending(p => p.Id)
                              .ToList();

            // محاسبه جمع کل پرداختی‌ها برای نمایش در View
            ViewBag.TotalPayments = payments.Sum(p => p.Amount);

            return View(payments);
        }

        // افزودن پرداخت جدید
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Payment p)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.TotalPayments = _db.Payments.Sum(x => x.Amount);
                return View("Index", _db.Payments.OrderByDescending(x => x.Id).ToList());
            }

            if (p == null || p.Amount <= 0)
            {
                TempData["ErrorMessage"] = "اطلاعات پرداخت نامعتبر است.";
                return RedirectToAction("Index");
            }

            // تنظیم تاریخ اگر خالی بود
            if (string.IsNullOrWhiteSpace(p.Date))
            {
                p.Date = DateTime.Now.ToString("yyyy-MM-dd");
            }

            _db.Payments.Add(p);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "پرداخت با موفقیت ثبت شد.";
            return RedirectToAction("Index");
        }

        // حذف پرداخت (اختیاری – اگر نیاز داری)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var payment = _db.Payments.Find(id);
            if (payment == null)
            {
                TempData["ErrorMessage"] = "پرداخت یافت نشد.";
                return RedirectToAction("Index");
            }

            _db.Payments.Remove(payment);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "پرداخت با موفقیت حذف شد.";
            return RedirectToAction("Index");
        }
    }
}
// Project: BeautyClinic_Uni
// Developer: Ali Tagipour