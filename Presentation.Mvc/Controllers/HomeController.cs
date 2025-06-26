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
    public ActionResult UploadImage(ImageUploadModel imageFile)
    {
        if (imageFile != null && imageFile.ImageData.Length > 0)
        {
            bus.SendAsync(new AddImageHandlerRequest("TODO change", new MemoryStream(imageFile.ImageData)));
        }
        else
        {
        }
        return View("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}