using RegulaReaderMinimalApi.Common;

namespace RegulaReaderMinimalApi.Helpers;

public static class LinksHelper
{
    public static string GetImageLink(LinkGenerator linkGenerator, IHttpContextAccessor accessor, string type)
    {
        var context = accessor.HttpContext
            ?? throw new InvalidOperationException(Errors.NoHttpContext);

        return linkGenerator.GetUriByName(
            context,
            RouteNames.GetScannerImage,
            new { type }
        ) ?? throw new InvalidOperationException(Errors.LinkGenerationFailed);
    }
}
