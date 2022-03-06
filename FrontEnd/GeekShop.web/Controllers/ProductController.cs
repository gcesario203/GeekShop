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
        _service = service ?? throw new ArgumentException(nameof(service));
    }

    public async Task<IActionResult> ProductIndex()
    {
        var products = await _service.FindAll();
        return View(products);
    }
    public async Task<IActionResult> ProductCreate()
    {
        return View();
    }
    public async Task<IActionResult> ProductEdit(long id)
    {
        var product = await _service.FindById(id);

        if(product == null) return NoContent();

        return View(product);
    }

    public async Task<IActionResult> ProductDelete(long id)
    {
        var response = await _service.Delete(id);

        if(response) return RedirectToAction(nameof(ProductIndex));

        return NoContent();
    }

    [HttpPost]
    public async Task<IActionResult> ProductEdit(ProductModel model)
    {
        if(!ModelState.IsValid) return NoContent();

        var response = await _service.Update(model);

        if(response != null) return RedirectToAction(nameof(ProductIndex));

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ProductCreate(ProductModel model)
    {
        if(!ModelState.IsValid) return NoContent();

        var response = await _service.Create(model);

        if(response != null) return RedirectToAction(nameof(ProductIndex));

        return View(model);
    }
}