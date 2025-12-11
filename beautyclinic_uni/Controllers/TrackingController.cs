using Accura.Models;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class TrackingController : Controller
    {
        private readonly ApplicationDbContext db;
        public TrackingController(ApplicationDbContext c) { db = c; }

        public IActionResult Index() => View(db.Tracking.ToList());

        [HttpPost]
        public IActionResult Add(TrackingSession t)
        {
            db.Tracking.Add(t);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour