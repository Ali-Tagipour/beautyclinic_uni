using Microsoft.AspNetCore.Mvc;

namespace beautyclinic_uni.Controllers
{
    public class AuthController : Controller
    {
        // صفحه انتخاب نقش
        public IActionResult Role()
        {
            return View();
        }

        // ورود کاربر (ادمین / منشی)
        public IActionResult UserLogin()
        {
            return View();
        }

        // ورود پزشک
        public IActionResult DoctorLogin()
        {
            return View();
        }
    }
}

// Project: BeautyClinic_Uni
// Author: Ali Tagipour

// Project: BeautyClinic_Uni
// Author: Ali Tagipour
