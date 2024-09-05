using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Dotnet8LineRichMenu.Models;

namespace Dotnet8LineRichMenu.Controllers;

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

    public IActionResult Bank()
    {
        return View();
    }

    public IActionResult News()
    {
        return View();
    }

    public IActionResult Marketing()
    {
        return View();
    }

    public IActionResult SocialMedia()
    {
        return View();
    }

    public IActionResult SocialMediaUrl()
    {
        return View();
    }
    
    public IActionResult Markdown()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}