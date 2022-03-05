using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GeekShop.web.Models;
using GeekShop.web.Services;

namespace GeekShop.web.Controllers;

public class ProductController : Controller
{

    private readonly ProductService _service;
    public ProductController(ProductService service)
    {
        _service = service;
    }

    public async Task<IActionResult> ProductIndex()
    {
        var teste = await _service.FindAll();
        return View();
    }
}
