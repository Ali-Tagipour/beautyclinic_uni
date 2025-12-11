using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db;
        public DashboardController(ApplicationDbContext c) { db = c; }

        public IActionResult Index()
        {
            var vm = new
            {
                Patients = db.Patients.Count(),
                Appointments = db.Appointments.Count(),
                Payments = db.Payments.Sum(x => x.Amount),
                Services = db.Services.Count()
            };
            return View(vm);
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour