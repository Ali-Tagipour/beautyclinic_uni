using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using System;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ContactController(ApplicationDbContext db)
        {
            _db = db;
        }

        // POST: /Contact/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(string FullName, string Phone, string Email, string Message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullName) && Request?.Form != null && Request.Form.Keys.Count > 0)
                {
                    FullName = Request.Form["FullName"].FirstOrDefault();
                    Phone = Request.Form["Phone"].FirstOrDefault();
                    Email = Request.Form["Email"].FirstOrDefault();
                    Message = Request.Form["Message"].FirstOrDefault();
                }

                if (string.IsNullOrWhiteSpace(FullName) ||
                    string.IsNullOrWhiteSpace(Phone) ||
                    string.IsNullOrWhiteSpace(Message))
                {
                    TempData["ErrorMessage"] = "لطفاً نام، شماره تماس و پیام را وارد کنید.";
                    return Redirect($"{Url.Action("Index", "Home")}#contact");
                }

                var r = new ContactRequest
                {
                    FullName = FullName.Trim(),
                    Phone = Phone.Trim(),
                    Email = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
                    Message = Message.Trim(),
                    CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                };

                _db.ContactRequests.Add(r);
                _db.SaveChanges();

                TempData["SuccessMessage"] = "درخواست شما ثبت شد. متشکریم.";
                return Redirect($"{Url.Action("Index", "Home")}#contact");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "خطا در ثبت درخواست. لطفاً دوباره تلاش کنید.";
                TempData["ContactException"] = ex.Message;
                return Redirect($"{Url.Action("Index", "Home")}#contact");
            }
        }

        // تست اتصال به DB
        [HttpGet]
        public IActionResult TestSave()
        {
            try
            {
                var r = new ContactRequest
                {
                    FullName = "TEST SAVE",
                    Phone = "09120000000",
                    Email = "test@example.local",
                    Message = "تست ذخیره",
                    CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                };
                _db.ContactRequests.Add(r);
                var rows = _db.SaveChanges();
                return Content($"TestSave: rows affected = {rows}");
            }
            catch (Exception ex)
            {
                return Content("TestSave error: " + ex.Message);
            }
        }

        // وضعیت تعداد رکوردها
        [HttpGet]
        public IActionResult Status()
        {
            try
            {
                var count = _db.ContactRequests.Count();
                return Content($"ContactRequests rows = {count}");
            }
            catch (Exception ex)
            {
                return Content("Status error: " + ex.Message);
            }
        }
    }
}
