using READERDEMO;

namespace RegulaReaderMinimalApi.Helpers;

public static class NamingHelper
{
    public static string ConvertToCamelCase(eVisualFieldType type)
    {
        var clean = type.ToString().Replace("ft_", "").ToLowerInvariant();
        var parts = clean.Split('_');
        return parts[0] + string.Concat(parts.Skip(1).Select(p => char.ToUpper(p[0]) + p[1..]));
    }
}