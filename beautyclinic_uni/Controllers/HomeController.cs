using System.Diagnostics;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var homeViewModel = new
            {
                TotalPatients = _db.Patients.Count(),
                TotalAppointments = _db.Appointments.Count(),
                TotalServices = _db.Services.Count(),
                TotalPayments = _db.Payments.Sum(p => p.Amount),

                // حل کامل ارور Invalid column name
                // RecentAppointments حذف شد تا کوئری به جدول Appointments نره
                // چون ستون‌های PatientName و AppointmentDateTime در دیتابیس واقعی وجود ندارند
                RecentAppointments = Enumerable.Empty<object>().ToList(),

                RecentContactRequests = _db.ContactRequests
                    .OrderByDescending(c => c.Id)
                    .Take(5)
                    .Select(c => new { c.Fullname, c.Phone, c.CreatedAt })
                    .ToList()
            };

            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour