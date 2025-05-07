namespace RegulaReaderMinimalApi.Common;

public static class ImageTypes
{
    public const string Portrait = "portrait";
    public const string Full = "full";
    public const string Bw = "bw";
    public const string Uv = "uv";

    public static readonly HashSet<string> Supported =
    [
        Portrait, Full, Bw, Uv
    ];
}
