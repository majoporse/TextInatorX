using System.Diagnostics;
using Application.Services.ImageService.Handlers;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mvc.Models;
using Wolverine;

namespace Presentation.Mvc.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    IMessageBus bus) : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> UploadImage(ImageUploadModel? imageFile)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Model state is invalid.");
            return BadRequest(ModelState);
        }

        if (imageFile == null || imageFile.ImageData.Length == 0) return BadRequest();
        using var stream = new MemoryStream();
        await imageFile.ImageData.CopyToAsync(stream);

        await bus.InvokeAsync(new AddImageHandlerRequest(stream, imageFile.ImageData.FileName));
        return View("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}