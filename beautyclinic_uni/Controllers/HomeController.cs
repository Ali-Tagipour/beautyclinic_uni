using Microsoft.AspNetCore.Mvc;

namespace beautyclinic_uni.Controllers
{
    public class HomeController : Controller
    {
        // ✅ این اکشن حیات صفحه‌ست
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // ✅ فقط برای فرم تماس (بدون دیتابیس، بدون ریدایرکت)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SubmitContact(
            string FullName,
            string Phone,
            string Message)
        {
            if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(Message))
            {
                return Json(new
                {
                    ok = false,
                    msg = "لطفاً نام، شماره تماس و پیام را وارد کنید."
                });
            }

            // ❌ دیتابیس نداریم
            // ✅ فقط پیام کاربرپسند

            return Json(new
            {
                ok = true,
                msg = "درخواست شما ثبت شد 🌸 به‌زودی با شما تماس می‌گیریم."
            });
        }
    }
}

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour

// Project: BeautyClinic_Uni
// Developer: Ali Tagipour