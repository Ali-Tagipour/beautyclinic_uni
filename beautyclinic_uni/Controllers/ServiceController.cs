using Accura.Models;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace beautyclinic_uni.Controllers
{
    public class ServiceController : Controller
    {
        private readonly ApplicationDbContext db;
        public ServiceController(ApplicationDbContext c) { db = c; }

        public IActionResult Index() => View(db.Services.ToList());

        [HttpPost]
        public IActionResult Add(ServiceItem s)
        {
            db.Services.Add(s);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
