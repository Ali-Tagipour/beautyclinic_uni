using Accura.Models;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext db;
        public AppointmentController(ApplicationDbContext c) { db = c; }

        public IActionResult Index() => View(db.Appointments.ToList());

        [HttpPost]
        public IActionResult Add(Appointment ap)
        {
            db.Appointments.Add(ap);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult ChangeStatus(int id, string status)
        {
            var ap = db.Appointments.Find(id);
            if (ap != null)
            {
                ap.Status = status;
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var ap = db.Appointments.Find(id);
            if (ap != null)
            {
                db.Appointments.Remove(ap);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour