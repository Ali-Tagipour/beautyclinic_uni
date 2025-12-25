using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using System;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var todayString = DateTime.Today.ToString("yyyy-MM-dd");

            var dashboardViewModel = new
            {
                TotalPatients = _db.Patients.Count(),
                TotalAppointments = _db.Appointments.Count(),
                TotalPayments = _db.Payments.Sum(x => x.Amount),
                TotalServices = _db.Services.Count(),

                // نوبت‌های در انتظار
                PendingAppointments = _db.Appointments
                    .Count(a => a.Status == "در انتظار" || a.Status == "Pending"),

                // نوبت‌های امروز - با فیلد جدید AppointmentDateTime
                TodayAppointments = _db.Appointments
                    .Count(a => a.AppointmentDateTime != null && a.AppointmentDateTime.Contains(todayString)),

                // درخواست‌های تماس اخیر
                RecentContactRequests = _db.ContactRequests
                    .OrderByDescending(c => c.Id)
                    .Take(5)
                    .Select(c => new { c.FullName, c.Phone, c.CreatedAt })
                    .ToList()
            };

            return View(dashboardViewModel);
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour