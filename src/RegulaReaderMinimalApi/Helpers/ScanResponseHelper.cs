using RegulaReaderMinimalApi.Common;
using RegulaReaderMinimalApi.Common.Models;

namespace RegulaReaderMinimalApi.Helpers;

public static class ScanResponseHelper
{
    public static object BuildResponse(
        ScanResult scanResult,
        string? fields,
        bool visual,
        bool mrz,
        string acceptHeader)
    {
        var includeLinks = ShouldIncludeLinks(acceptHeader);

        var filteredData = FilterFields(scanResult.Data, visual, mrz);

        if (!string.IsNullOrWhiteSpace(fields))
        {
            filteredData = ShapeData(filteredData, fields);
        }

        return AssembleResponse(filteredData, scanResult, includeLinks);
    }

    private static bool ShouldIncludeLinks(string acceptHeader) =>
        acceptHeader.Contains(CustomMediaTypes.ApplicationHateoasJsonV1, StringComparison.OrdinalIgnoreCase);

    private static Dictionary<string, object?> FilterFields(
        Dictionary<string, Dictionary<string, string?>> data,
        bool visual,
        bool mrz)
    {
        var result = new Dictionary<string, object?>();

        foreach (var (key, values) in data)
        {
            var field = new Dictionary<string, string?>();

            if (visual && values.TryGetValue(FieldNames.Visual, out var visualValue))
                field[FieldNames.Visual] = visualValue;

            if (mrz && values.TryGetValue(FieldNames.Mrz, out var mrzValue))
                field[FieldNames.Mrz] = mrzValue;

            if (field.Count > 0)
                result[key] = field;
        }

        return result;
    }

    private static Dictionary<string, object?> ShapeData(Dictionary<string, object?> data, string fields)
    {
        var shaped = new Dictionary<string, object?>();
        var requested = fields.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var field in requested)
        {
            if (data.TryGetValue(field, out var value))
            {
                shaped[field] = value;
            }
            else
            {
                throw new KeyNotFoundException(Errors.FieldNotFound(field));
            }
        }

        return shaped;
    }

    private static Dictionary<string, object?> AssembleResponse(
        Dictionary<string, object?> data,
        ScanResult result,
        bool includeLinks)
    {
        var response = new Dictionary<string, object?>
        {
            [ResponseKeys.Data] = data
        };

        if (includeLinks && result.Links is not null)
            response[ResponseKeys.Links] = result.Links;

        return response;
    }
}
