using Microsoft.AspNetCore.Mvc;

public class ClinicsController : Controller // Author: ehsanghiyasi
{
    public IActionResult Index()// Author: ehsanghiyasi
    {
        return View("~/Views/clinic/clinic.cshtml");// Author: ehsanghiyasi
    }
}