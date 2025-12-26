using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using beautyclinic_uni.ViewModels;
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

            var model = new DashboardViewModel
            {
                TotalPatients = _db.Patients.Count(),

                TotalAppointments = _db.Appointments.Count(),

                TotalPayments = _db.Payments.Any()
                    ? _db.Payments.Sum(x => x.Amount)
                    : 0,

                TotalServices = _db.Services.Count(),

                PendingAppointments = _db.Appointments
                    .Count(a => a.Status == "در انتظار" || a.Status == "Pending"),

                TodayAppointments = _db.Appointments
                    .Count(a =>
                        !string.IsNullOrEmpty(a.AppointmentDateTime) &&
                        a.AppointmentDateTime.Contains(todayString)
                    ),

                RecentContactRequests = _db.ContactRequests
                    .OrderByDescending(c => c.Id)
                    .Take(5)
                    .Select(c => new ContactRequestItem
                    {
                        FullName = c.FullName,
                        Phone = c.Phone,
                        CreatedAt = c.CreatedAt
                    })
                    .ToList()
            };

            // 🔴 مسیر View به‌صورت صریح (حل قطعی خطا)
            return View("~/Views/dashboard/dashboard.cshtml", model);
        }
    }
}
