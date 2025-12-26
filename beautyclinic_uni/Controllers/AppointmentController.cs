using Accura.Models;
using beautyclinic_uni.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AppointmentController(ApplicationDbContext db)
        {
            _db = db;
        }

        // نمایش لیست نوبت‌ها
        public IActionResult Index()
        {
            var list = _db.Appointments
                          .OrderByDescending(a => a.Id)
                          .ToList();

            return View(list);
        }

        // افزودن نوبت جدید
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(Appointment ap)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Index");

            if (ap == null)
                return RedirectToAction("Index");

            // مقداردهی امن
            ap.Status ??= "در انتظار";

            _db.Appointments.Add(ap);
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // تغییر وضعیت نوبت
        [HttpPost]
        public IActionResult ChangeStatus(int id, string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return RedirectToAction("Index");

            var ap = _db.Appointments.FirstOrDefault(a => a.Id == id);
            if (ap == null)
                return RedirectToAction("Index");

            ap.Status = status;
            _db.SaveChanges();

            return RedirectToAction("Index");
        }

        // حذف نوبت
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var ap = _db.Appointments.FirstOrDefault(a => a.Id == id);
            if (ap == null)
                return RedirectToAction("Index");

            _db.Appointments.Remove(ap);
            _db.SaveChanges();

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



