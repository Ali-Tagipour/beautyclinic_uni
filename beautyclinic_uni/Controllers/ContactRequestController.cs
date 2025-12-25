using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using System;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class ContactRequestController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ContactRequestController(ApplicationDbContext db)
        {
            _db = db;
        }

        // لیست تمام درخواست‌های تماس (پنل ادمین)
        public IActionResult Index()
        {
            var list = _db.ContactRequests
                          .OrderByDescending(x => x.Id)
                          .ToList();

            return View(list);
        }

        // ثبت درخواست تماس از طریق AJAX (فرم عمومی سایت)
        [HttpPost]
        [ValidateAntiForgeryToken]  // امنیت بیشتر حتی در AJAX
        public IActionResult Submit(ContactRequest r)
        {
            // چک نال و ولیدیشن
            if (r == null)
                return Json(new { ok = false, msg = "اطلاعات ارسال نشده است." });

            if (string.IsNullOrWhiteSpace(r.Fullname) ||
                string.IsNullOrWhiteSpace(r.Phone) ||
                string.IsNullOrWhiteSpace(r.Message))
            {
                return Json(new { ok = false, msg = "لطفاً نام، شماره تماس و پیام را وارد کنید." });
            }

            // تنظیم تاریخ ثبت
            r.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

            // تنظیم Subject اگر خالی بود
            r.Subject ??= "بدون موضوع";

            try
            {
                _db.ContactRequests.Add(r);
                _db.SaveChanges();  // اینجا واقعاً به دیتابیس BeautyClinicDB ذخیره می‌شه

                return Json(new { ok = true, msg = "درخواست شما با موفقیت ثبت شد. به زودی با شما تماس می‌گیریم." });
            }
            catch (Exception ex)
            {
                // اگر خطای دیتابیس بود (مثل تکراری بودن یا مشکل اتصال)
                return Json(new { ok = false, msg = "خطا در ثبت درخواست. لطفاً مجدد تلاش کنید." });
            }
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour