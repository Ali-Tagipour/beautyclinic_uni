using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;

namespace beautyclinic_uni.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext db;

        public UserController(ApplicationDbContext context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            var list = db.Users.ToList();
            return View(list);
        }

        [HttpPost]
        public IActionResult Add(User u)
        {
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Edit(User u)
        {
            db.Users.Update(u);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var u = db.Users.Find(id);
            if (u != null)
            {
                db.Users.Remove(u);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}
