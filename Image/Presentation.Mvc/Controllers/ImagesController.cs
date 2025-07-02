using Application.Services.ImageService.Handlers;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Presentation.Mvc.Models;
using Wolverine;

namespace Presentation.Mvc.Controllers;

public class ImagesController(IMessageBus bus, ILogger<ImagesController> logger) : Controller
{
    public async Task<IActionResult> Index()
    {
        var images =
            await bus.InvokeAsync<ErrorOr<GetAllImagesHandlerRequest.Result>>(new GetAllImagesHandlerRequest());
        if (images.IsError)
        {
            logger.LogError("Failed to retrieve images: {Errors}", images.Errors);
            return View("Error", new ErrorViewModel { RequestId = "Failed to load images" });
        }

        var imagesOk = images.Value;
        return View(new ImagesViewModel
        {
            Images = imagesOk.Images.ToList()
        });
    }
}