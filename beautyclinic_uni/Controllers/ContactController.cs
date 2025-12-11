using Microsoft.AspNetCore.Mvc;
using beautyclinic_uni.Data;
using beautyclinic_uni.Models;

namespace beautyclinic_uni.Controllers
{
    public class ContactController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ContactController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Contact/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contact/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContactRequest request)
        {
            if (ModelState.IsValid)
            {
                _context.ContactRequests.Add(request);
                await _context.SaveChangesAsync();
                return RedirectToAction("ThankYou");
            }
            return View(request);
        }

        public IActionResult ThankYou()
        {
            return View();
        }
    }
}
// Project: BeautyClinic_Uni
// Author: Ali Tagipour