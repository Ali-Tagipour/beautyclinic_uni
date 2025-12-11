using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;

namespace beautyclinic_uni.Controllers
{
    public class ContactRequestController : Controller
    {
        private readonly ApplicationDbContext db;

        public ContactRequestController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            return View(db.ContactRequests.ToList());
        }

        [HttpPost]
        public IActionResult Submit(ContactRequest r)
        {
            r.CreatedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            db.ContactRequests.Add(r);
            db.SaveChanges();

            return Json(new { ok = true, msg = "درخواست شما ثبت شد." });
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour