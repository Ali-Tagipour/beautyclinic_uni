using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller// Author:  ehsanghiyasi 
{
    public IActionResult Login()// Author:  ehsanghiyasi
    {
        return View("~/Views/login&signup/login.cshtml");// Author:  ehsanghiyasi
    }

    public IActionResult Signup()// Author:  ehsanghiyasi
    {
        return View("~/Views/login&signup/signup.cshtml");// Author:  ehsanghiyasi
    }
}
// Project: BeautyClinic_Uni
// Author:  ehsanghiyasi until 2025/11/12