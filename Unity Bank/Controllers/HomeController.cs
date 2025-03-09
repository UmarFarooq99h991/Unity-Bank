using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Unity_Bank.Models;

namespace Unity_Bank.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    //public IActionResult Error()
    //{
    //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    //}
    public IActionResult Error()
    {
        var errorMessage = TempData["Error"] as string ?? "An unexpected error occurred."; // ? Ensure a default message
        var errorModel = new ErrorViewModel
        {
            Message = errorMessage,
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier // ? Keep the RequestId
        };
        return View(errorModel);
    }


}
