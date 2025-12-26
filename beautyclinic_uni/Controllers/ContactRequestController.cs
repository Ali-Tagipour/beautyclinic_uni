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
        [ValidateAntiForgeryToken]
        public IActionResult Submit(ContactRequest r)
        {
            if (r == null)
                return Json(new { ok = false, msg = "اطلاعات ارسال نشده است." });

            if (string.IsNullOrWhiteSpace(r.FullName) ||
                string.IsNullOrWhiteSpace(r.Phone) ||
                string.IsNullOrWhiteSpace(r.Message))
            {
                return Json(new { ok = false, msg = "لطفاً نام، شماره تماس و پیام را وارد کنید." });
            }

            try
            {
                r.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm");

                _db.ContactRequests.Add(r);
                _db.SaveChanges();

                return Json(new { ok = true, msg = "درخواست شما با موفقیت ثبت شد. به زودی با شما تماس می‌گیریم." });
            }
            catch (Exception)
            {
                return Json(new { ok = false, msg = "خطا در ثبت درخواست. لطفاً مجدد تلاش کنید." });
            }
        }
    }
}

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

