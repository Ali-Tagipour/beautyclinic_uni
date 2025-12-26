using Accura.Models;
using beautyclinic_uni.Data;
using Microsoft.AspNetCore.Authorization; // اضافه شد برای امنیت
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    [Authorize] // فقط کاربران لاگین‌شده (ادمین یا بیمار) می‌توانند به پیگیری درمان دسترسی داشته باشند
    public class TrackingController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TrackingController(ApplicationDbContext db)
        {
            _db = db;
        }

        // نمایش لیست جلسات پیگیری درمان
        public IActionResult Index()
        {
            var trackingSessions = _db.Tracking
                                      .OrderByDescending(t => t.Id)
                                      .ToList();

            return View(trackingSessions);
        }

        // افزودن جلسه پیگیری جدید
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(TrackingSession t)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", _db.Tracking.OrderByDescending(x => x.Id).ToList());
            }

            if (t == null || t.Session <= 0)
            {
                TempData["ErrorMessage"] = "اطلاعات جلسه پیگیری نامعتبر است.";
                return RedirectToAction("Index");
            }

            // تنظیم تاریخ اگر خالی بود
            if (string.IsNullOrWhiteSpace(t.Date))
            {
                t.Date = DateTime.Now.ToString("yyyy-MM-dd");
            }

            // وضعیت پیش‌فرض اگر خالی بود
            t.Status ??= "در حال انجام";

            _db.Tracking.Add(t);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "جلسه پیگیری با موفقیت اضافه شد.";
            return RedirectToAction("Index");
        }

        // حذف جلسه پیگیری
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var session = _db.Tracking.Find(id);
            if (session == null)
            {
                TempData["ErrorMessage"] = "جلسه پیگیری یافت نشد.";
                return RedirectToAction("Index");
            }

            _db.Tracking.Remove(session);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "جلسه پیگیری با موفقیت حذف شد.";
            return RedirectToAction("Index");
        }

        // ویرایش جلسه پیگیری
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TrackingSession t)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", _db.Tracking.OrderByDescending(x => x.Id).ToList());
            }

            if (t == null || t.Id <= 0)
            {
                TempData["ErrorMessage"] = "اطلاعات جلسه نامعتبر است.";
                return RedirectToAction("Index");
            }

            var existingSession = _db.Tracking.Find(t.Id);
            if (existingSession == null)
            {
                TempData["ErrorMessage"] = "جلسه پیگیری یافت نشد.";
                return RedirectToAction("Index");
            }

            existingSession.Session = t.Session;
            existingSession.Date = t.Date;
            existingSession.Status = t.Status;

            _db.Tracking.Update(existingSession);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "جلسه پیگیری با موفقیت ویرایش شد.";
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
