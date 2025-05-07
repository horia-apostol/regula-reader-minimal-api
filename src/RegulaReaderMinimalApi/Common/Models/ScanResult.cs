namespace RegulaReaderMinimalApi.Common.Models;

public sealed record ScanResult
{
    public Dictionary<string, Dictionary<string, string?>> Data { get; init; } = [];
    public Dictionary<string, string>? Images { get; init; }
    public Dictionary<string, string>? Links { get; init; }
}
