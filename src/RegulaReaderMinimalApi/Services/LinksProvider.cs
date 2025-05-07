using RegulaReaderMinimalApi.Common;
using RegulaReaderMinimalApi.Services.Interfaces;

namespace RegulaReaderMinimalApi.Services;

public sealed class LinksProvider(LinkGenerator linkGenerator, IHttpContextAccessor accessor) : ILinksProvider
{
    private readonly LinkGenerator _linkGenerator = linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor = accessor;

    public string GetImageLink(string type)
    {
        var context = _httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException(Errors.NoHttpContext);

        return _linkGenerator.GetUriByName(
            context,
            RouteNames.GetScannerImage,
            new { type }
        ) ?? throw new InvalidOperationException(Errors.LinkGenerationFailed);
    }
}
