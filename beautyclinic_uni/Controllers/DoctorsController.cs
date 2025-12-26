using Microsoft.AspNetCore.Mvc;

namespace beautyclinic_uni.Controllers
{
    // صفحه لیست پزشکان - عمومی (بدون نیاز به لاگین)
    public class DoctorsController : Controller
    {
        public IActionResult Index()
        {
            // مسیر کامل و صریح به فایل موجودت
            return View("~/Views/doctors/doctors.cshtml");
        }
    }
}