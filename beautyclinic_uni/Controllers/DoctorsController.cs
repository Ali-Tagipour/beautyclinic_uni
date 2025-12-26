using Microsoft.AspNetCore.Mvc;

namespace beautyclinic_uni.Controllers
{
    // صفحه‌ی لیست پزشکان (عمومی - نیازی به Authorize نیست مگر بخواهی)
    public class DoctorsController : Controller
    {
        // بازگرداندن View استاندارد در مسیر Views/Doctors/Index.cshtml
        public IActionResult Index()
        {
            return View("Index"); // MVC به صورت پیش‌فرض دنبال Views/Doctors/Index.cshtml می‌گردد
        }
    }
}
