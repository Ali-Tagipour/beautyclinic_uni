using Accura.Models;
using beautyclinic_uni.Data;
using Microsoft.AspNetCore.Authorization; // اضافه شد برای امنیت
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    [Authorize] // فقط کاربران لاگین‌شده (ادمین) می‌توانند به مدیریت بیماران دسترسی داشته باشند
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext _db;

        public PatientController(ApplicationDbContext db)
        {
            _db = db;
        }

        // نمایش لیست بیماران
        public IActionResult Index()
        {
            var patients = _db.Patients
                              .OrderByDescending(p => p.Id)
                              .ToList();
            return View(patients);
        }

        // افزودن بیمار جدید
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Patient p)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", _db.Patients.ToList());
            }

            if (p == null)
            {
                return RedirectToAction("Index");
            }

            _db.Patients.Add(p);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "بیمار با موفقیت اضافه شد.";
            return RedirectToAction("Index");
        }

        // حذف بیمار
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var patient = _db.Patients.Find(id);
            if (patient == null)
            {
                TempData["ErrorMessage"] = "بیمار یافت نشد.";
                return RedirectToAction("Index");
            }

            _db.Patients.Remove(patient);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "بیمار با موفقیت حذف شد.";
            return RedirectToAction("Index");
        }

        // ویرایش بیمار
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Patient p)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", _db.Patients.ToList());
            }

            if (p == null || p.Id <= 0)
            {
                TempData["ErrorMessage"] = "اطلاعات بیمار نامعتبر است.";
                return RedirectToAction("Index");
            }

            var existingPatient = _db.Patients.Find(p.Id);
            if (existingPatient == null)
            {
                TempData["ErrorMessage"] = "بیمار یافت نشد.";
                return RedirectToAction("Index");
            }

            // به‌روزرسانی فیلدها
            existingPatient.Name = p.Name;
            existingPatient.Age = p.Age;
            existingPatient.Service = p.Service;
            existingPatient.Status = p.Status;
            existingPatient.Details = p.Details;

            _db.Patients.Update(existingPatient);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "اطلاعات بیمار با موفقیت ویرایش شد.";
            return RedirectToAction("Index");
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour