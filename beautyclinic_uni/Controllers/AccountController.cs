using Microsoft.AspNetCore.Mvc;

public class AccountController : Controller
{
    public IActionResult Login()
    {
        return View("~/Views/login&signup/login.cshtml");
    }

    public IActionResult Signup()
    {
        return View("~/Views/login&signup/signup.cshtml");
    }
}
