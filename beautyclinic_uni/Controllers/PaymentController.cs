using Accura.Models;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext db;
        public PaymentController(ApplicationDbContext c) { db = c; }

        public IActionResult Index() => View(db.Payments.ToList());

        [HttpPost]
        public IActionResult Add(Payment p)
        {
            db.Payments.Add(p);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
