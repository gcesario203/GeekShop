using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GeekShop.web.Models;
using Microsoft.AspNetCore.Authorization;
using GeekShop.web.Services;

namespace GeekShop.web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    private readonly ProductService _service;

    public HomeController(ILogger<HomeController> logger, ProductService service)
    {
        _logger = logger;

        _service = service ?? throw new ArgumentException(nameof(service));
    }
    
    [Authorize]
    public async Task<IActionResult> Details(long id)
    {
        var product = await _service.FindById(id);

        if (product == null) return NoContent();

        return View(product);
    }

    public async Task<IActionResult> Index()
    {
        var products = await _service.FindAll();
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        return RedirectToAction(nameof(Index));
    }
    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
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
