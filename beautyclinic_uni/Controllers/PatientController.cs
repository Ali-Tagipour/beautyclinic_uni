using Accura.Models;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class PatientController : Controller
    {
        private readonly ApplicationDbContext db;
        public PatientController(ApplicationDbContext context) { db = context; }

        public IActionResult Index() => View(db.Patients.ToList());

        [HttpPost]
        public IActionResult Add(Patient p)
        {
            db.Patients.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var p = db.Patients.Find(id);
            if (p != null)
            {
                db.Patients.Remove(p);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(Patient p)
        {
            db.Patients.Update(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
