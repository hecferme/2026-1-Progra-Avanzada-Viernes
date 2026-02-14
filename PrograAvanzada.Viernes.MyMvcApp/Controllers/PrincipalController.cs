using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PrograAvanzada.Viernes.MyMvcApp.Models;

namespace PrograAvanzada.Viernes.MyMvcApp.Controllers;

public class PrincipalController : Controller
{
    private readonly ILogger<PrincipalController> _logger;

    public PrincipalController(ILogger<PrincipalController> logger)
    {
        _logger = logger;
    }

    public IActionResult EjemploFecha()
    {
        return View();
    }

    public IActionResult FechaPersonalizada(int? hoursOffset)
    {
        int offset = hoursOffset ?? 0;
        if (offset < -24) offset = -24;
        if (offset > 24) offset = 24;

        DateTime now = DateTime.Now;
        DateTime customizedDate = now.AddHours(offset);

        ViewBag.Now = now;
        ViewBag.CustomizedDate = customizedDate;
        ViewBag.Offset = offset;

        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
