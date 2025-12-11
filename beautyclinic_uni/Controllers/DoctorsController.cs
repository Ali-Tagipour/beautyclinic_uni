using Microsoft.AspNetCore.Mvc;

public class DoctorsController : Controller // Author: ehsanghiyasi
{
    public IActionResult Index() // Author: ehsanghiyasi
    {
        return View("~/Views/doctors/doctors.cshtml");
    }
}
