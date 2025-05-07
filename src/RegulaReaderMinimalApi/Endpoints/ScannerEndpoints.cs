using Microsoft.AspNetCore.Mvc;
using RegulaReaderMinimalApi.Common;
using RegulaReaderMinimalApi.Helpers;
using RegulaReaderMinimalApi.Scanner.Interfaces;
using System.Net.Mime;

namespace RegulaReaderMinimalApi.Endpoints;

public static class ScannerEndpoints
{
    public static void MapScannerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/scanner").WithTags("Scanner");

        group.MapPost("/connect", async ([FromServices] IDocumentScanner scanner) =>
        {
            await scanner.ConnectAsync();
            return Results.Ok(Messages.ScannerConnected);
        })
        .WithName(RouteNames.ConnectScanner)
        .WithSummary("Connects to the Regula scanner.")
        .WithDescription("Initializes a connection to the scanner device.")
        .Produces(StatusCodes.Status200OK);

        group.MapPost("/disconnect", async ([FromServices] IDocumentScanner scanner) =>
        {
            await scanner.DisconnectAsync();
            return Results.Ok(Messages.ScannerDisconnected);
        })
        .WithName(RouteNames.DisconnectScanner)
        .WithSummary("Disconnects from the Regula scanner.")
        .WithDescription("Closes the connection to the scanner device.")
        .Produces(StatusCodes.Status200OK);

        group.MapPost("/read", async (
            HttpRequest request,
            [FromServices] IDocumentScanner scanner,
            [FromQuery] string? fields,
            [FromQuery] bool visual = true,
            [FromQuery] bool mrz = true
        ) =>
        {
            var result = await scanner.ReadAsync();
            if (result is null)
                return Results.NotFound(Errors.NoOcrResult);

            var response = ScanResponseHelper.BuildResponse(result, fields, visual, mrz, request.Headers.Accept.ToString());
            return Results.Json(response);
        })
        .WithName(RouteNames.ReadScannerData)
        .WithSummary("Reads OCR data from the scanner.")
        .WithDescription("Retrieves the data that was previously scanned.")
        .Produces<object>(
            StatusCodes.Status200OK,
            MediaTypeNames.Application.Json,
            CustomMediaTypes.ApplicationHateoasJsonV1, 
            CustomMediaTypes.ApplicationJsonV1)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status400BadRequest);
        
        group.MapGet("/image/{type}", ([FromRoute] string type, [FromServices] IDocumentScanner scanner) =>
        {
            if (!ImageTypes.Supported.Contains(type.ToLowerInvariant()))
                return Results.BadRequest(Errors.UnsupportedImageType);
            var bytes = scanner.GetImage(type);
            if (bytes is not null)
                return Results.File(bytes, "image/jpeg");
            return Results.NotFound(Errors.ImageNotFound);
        })
        .WithName(RouteNames.GetScannerImage)
        .WithSummary("Retrieves a scanned image by type.")
        .WithDescription("Supported types include: " +
        $"{ImageTypes.Portrait}, " +
        $"{ImageTypes.Full}, " +
        $"{ImageTypes.Bw}, " +
        $"{ImageTypes.Uv}.")
        .Produces<object>(
            StatusCodes.Status200OK,
            MediaTypeNames.Application.Json,
            CustomMediaTypes.ApplicationJsonV1)
        .Produces(StatusCodes.Status404NotFound);
    }
}
