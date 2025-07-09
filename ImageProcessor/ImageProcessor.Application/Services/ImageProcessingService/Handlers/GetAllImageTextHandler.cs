using ErrorOr;
using ImageProcessor.Application.Interfaces;
using Wolverine.Attributes;

namespace ImageProcessor.Application.Services.ImageProcessingService.Handlers;

public record GetAllImageTextRequest
{
    public record TextResponse(
        Guid Id,
        string Text,
        Guid ImageId,
        DateTime CreatedAt
    );

    public record Response(
        ErrorOr<IEnumerable<TextResponse>> Texts
    );
}

[WolverineHandler]
public class GetAllImageTextHandler(IImageTextRepository repository)
{
    public async Task<GetAllImageTextRequest.Response> HandleAsync(GetAllImageTextRequest request,
        CancellationToken cancellationToken = default)
    {
        var texts = await repository.GetAll(cancellationToken);
        return new GetAllImageTextRequest.Response(
            texts.Then(val =>
                val.Select(e => new GetAllImageTextRequest.TextResponse(e.Id, e.Text, e.ImageId, e.CreatedAt))
            ));
    }
}