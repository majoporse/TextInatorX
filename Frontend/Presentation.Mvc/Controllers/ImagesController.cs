using Contracts.Events;
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
            await bus.InvokeAsync<GetAllImagesRequestResult>(new GetAllImagesRequest(), timeout: 30.Seconds());
        if (!images.IsSuccess) return View("Error", new ErrorViewModel { RequestId = "Failed to load images" });

        var imagesOk = images.Value!;

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

        var res = await bus.InvokeAsync<GetImageRequestResult>(new GetImageRequest(id));
        if (!res.IsSuccess)
        {
            logger.LogError("Failed to get image.");
            return View("Error", new ErrorViewModel { RequestId = "Failed to get image" });
        }

        var image = res.Value!.ImageWithUrlDto;

        var textRes =
            await bus.InvokeAsync<GetImageTextRequestResult>(new GetImageTextRequest(id), timeout: 60.Seconds());
        if (!textRes.IsSuccess)
        {
            logger.LogError("Failed to retrieve image text: {ErrorMessage}", textRes.ErrorMessage);
            return View("Error", new ErrorViewModel { RequestId = "Failed to retrieve image text" });
        }

        var text = textRes.Value!;

        var model = new ImageDetailModel
        {
            CreatedAt = image.CreatedAt,
            Name = image.Name,
            ImageUrl = image.ImageUrl,
            Text = text.Text
        };

        return View(model);
    }
}