using Accura.Models;
using beautyclinic_uni.Data;
using Microsoft.AspNetCore.Authorization; // اضافه شد برای امنیت
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    [Authorize] // فقط کاربران لاگین‌شده (ادمین) می‌توانند به مدیریت خدمات دسترسی داشته باشند
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ServiceController(ApplicationDbContext db)
        {
            _db = db;
        }

        // نمایش لیست خدمات
        public IActionResult Index()
        {
            var services = _db.Services
                              .OrderByDescending(s => s.Id)
                              .ToList();

            return View(services);
        }

        // افزودن خدمت جدید
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(ServiceItem s)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", _db.Services.OrderByDescending(x => x.Id).ToList());
            }

            if (s == null || string.IsNullOrWhiteSpace(s.Title) || s.Price < 0)
            {
                TempData["ErrorMessage"] = "اطلاعات خدمت نامعتبر است.";
                return RedirectToAction("Index");
            }

            _db.Services.Add(s);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "خدمت با موفقیت اضافه شد.";
            return RedirectToAction("Index");
        }

        // حذف خدمت
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var service = _db.Services.Find(id);
            if (service == null)
            {
                TempData["ErrorMessage"] = "خدمت یافت نشد.";
                return RedirectToAction("Index");
            }

            _db.Services.Remove(service);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "خدمت با موفقیت حذف شد.";
            return RedirectToAction("Index");
        }

        // ویرایش خدمت
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ServiceItem s)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", _db.Services.OrderByDescending(x => x.Id).ToList());
            }

            if (s == null || s.Id <= 0)
            {
                TempData["ErrorMessage"] = "اطلاعات خدمت نامعتبر است.";
                return RedirectToAction("Index");
            }

            var existingService = _db.Services.Find(s.Id);
            if (existingService == null)
            {
                TempData["ErrorMessage"] = "خدمت یافت نشد.";
                return RedirectToAction("Index");
            }

            existingService.Title = s.Title;
            existingService.Desc = s.Desc;
            existingService.Price = s.Price;

            _db.Services.Update(existingService);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "خدمت با موفقیت ویرایش شد.";
            return RedirectToAction("Index");
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour