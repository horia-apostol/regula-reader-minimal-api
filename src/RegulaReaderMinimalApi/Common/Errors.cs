namespace RegulaReaderMinimalApi.Common;

public static class Errors
{
    public const string NoOcrResult = "No OCR result available.";
    public const string MissingDataField = "Missing 'data' field in scan response.";
    public const string ImageNotFound = "Image not found.";
    public const string NoHttpContext = "No HttpContext available.";
    public const string LinkGenerationFailed = "Could not generate image link.";
    public const string UnsupportedImageType = "Unsupported image type.";
    public static string FieldNotFound(string field) => $"Field '{field}' not found.";
}
