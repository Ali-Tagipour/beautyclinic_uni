using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using System;

namespace beautyclinic_uni.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ContactController(ApplicationDbContext db)
        {
            _db = db;
        }

        // نمایش فرم تماس با ما
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // ثبت درخواست تماس
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ContactRequest request)
        {
            // چک ولیدیشن خودکار (Required, MaxLength و ...)
            if (!ModelState.IsValid)
            {
                return View(request); // برمی‌گرداند به فرم با نمایش خطاها
            }

            // چک دستی اضافی برای فیلدهای حیاتی
            if (string.IsNullOrWhiteSpace(request.Fullname) ||
                string.IsNullOrWhiteSpace(request.Phone) ||
                string.IsNullOrWhiteSpace(request.Message))
            {
                ModelState.AddModelError("", "لطفاً تمام فیلدهای اجباری را پر کنید.");
                return View(request);
            }

            try
            {
                // تنظیم تاریخ ثبت
                request.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                // اگر موضوع خالی بود، یک مقدار پیش‌فرض بگذار
                request.Subject ??= "تماس از وب‌سایت";

                // اضافه کردن به دیتابیس
                _db.ContactRequests.Add(request);
                _db.SaveChanges(); // ← اینجا واقعاً در جدول dbo.ContactRequests ذخیره می‌شه

                // پیام موفقیت (اختیاری: می‌تونی TempData استفاده کنی)
                TempData["SuccessMessage"] = "پیام شما با موفقیت ارسال شد. به زودی با شما تماس می‌گیریم.";

                return RedirectToAction("ThankYou");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "خطایی در ارسال پیام رخ داد. لطفاً مجدداً تلاش کنید.");
                return View(request);
            }
        }

        // صفحه تشکر بعد از ارسال موفق
        [HttpGet]
        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour