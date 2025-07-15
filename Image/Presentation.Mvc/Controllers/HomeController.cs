using System.Diagnostics;
using Application.Services.ImageService.Handlers;
using Contracts.Events;
using JasperFx.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Presentation.Mvc.Hubs;
using Presentation.Mvc.Models;
using Wolverine;

namespace Presentation.Mvc.Controllers;

public class HomeController(
    ILogger<HomeController> logger,
    IHubContext<ImageUploadHub> hubContext,
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
    public async Task<ActionResult> UploadImage(ImageUploadModel? imageFile,
        [FromHeader(Name = "X-SignalR-Connection-Id")]
        string connectionId)
    {
        if (!ModelState.IsValid)
        {
            logger.LogError("Model state is invalid.");
            return BadRequest(ModelState);
        }

        if (imageFile == null || imageFile.ImageData.Length == 0) return BadRequest();

        using var stream = new MemoryStream();
        await imageFile.ImageData.CopyToAsync(stream);

        var res = await bus.InvokeAsync<AddImageHandlerRequest.Result>(
            new AddImageHandlerRequest(stream, imageFile.ImageData.FileName));

        if (!res.IsSuccess) return BadRequest(res.ErrorMessage);

        var image = res.Value;

        Task.Run(async () =>
        {
            var text = await bus.InvokeAsync<ImageUploadedEventResult>(new ImageUploadedEvent(image.Image.Id,
                image.ImageUrl), timeout: 30.Seconds());

            await hubContext.Clients.Client(connectionId).SendAsync(ImageUploadHub.RecieveImageDataEvent, new
            {
                ImageId = image.Image.Id,
                image.ImageUrl,
                ImageName = image.Image.Name,
                text = text.Value.Text
            });
        });

        return Ok(new { image.ImageUrl, ImageName = image.Image.Name });
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}