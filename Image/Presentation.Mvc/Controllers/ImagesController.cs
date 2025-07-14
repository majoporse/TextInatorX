using Application.Services.ImageService.Handlers;
using Contracts.Events;
using ErrorOr;
using JasperFx.Core;
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

    [HttpGet("Images/Detail/{id}")]
    public async Task<IActionResult> Detail(Guid id)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Model state is invalid.");
            return BadRequest(ModelState);
        }

        var res = await bus.InvokeAsync<GetImageRequest.Result>(new GetImageRequest(id));
        var image = res.Image;
        var textRes = await bus.InvokeAsync<GetImageTextRequestResult>(new GetImageTextRequest(id), timeout: 60.Seconds());

        var model = new ImageDetailModel
        {
            CreatedAt = image.CreatedAt,
            Name = image.Name,
            ImageUrl = image.Url,
            Text = textRes.Text
        };

        return View(model);
    }
}