using System.Xml;
using READERDEMO;
using RegulaReaderMinimalApi.Common;
using RegulaReaderMinimalApi.Common.Models;
using RegulaReaderMinimalApi.Helpers;
using RegulaReaderMinimalApi.Scanner.Interfaces;
using RegulaReaderMinimalApi.Services.Interfaces;

namespace RegulaReaderMinimalApi.Scanner;

public sealed class RegulaScanner(ILinksProvider linksProvider) : IDocumentScanner
{
    private readonly RegulaReader _reader = new();
    private readonly ILinksProvider _linksProvider = linksProvider;

    public Task ConnectAsync() => Task.Run(() => _reader.Connect());

    public Task DisconnectAsync() => Task.Run(() => _reader.Disconnect());

    public async Task<ScanResult?> ReadAsync()
    {
        return await Task.Run(() =>
        {
            _reader.MultiPageProcessing = true;

            if (_reader.IsReaderResultTypeAvailable((int)eRPRM_ResultType.RPRM_ResultType_OCRLexicalAnalyze) <= 0)
                return null;

            var xml = _reader.CheckReaderResultXML((int)eRPRM_ResultType.RPRM_ResultType_OCRLexicalAnalyze, 0, 0);
            var xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xml);

            var data = new Dictionary<string, Dictionary<string, string?>>();
            var nodeList = xmlDoc.GetElementsByTagName(XmlConstants.DocumentFieldAnalysisInfo);

            foreach (XmlElement item in nodeList)
            {
                var typeText = item[XmlConstants.Type]?.InnerText;
                if (!int.TryParse(typeText, out int typeValue)) continue;

                var fieldType = (eVisualFieldType)typeValue;
                var key = NamingHelper.ConvertToCamelCase(fieldType);

                if (data.ContainsKey(key))
                    continue;

                var visual = item[XmlConstants.FieldVisual]?.InnerText;
                var mrz = item[XmlConstants.FieldMrz]?.InnerText;

                data[key] = new Dictionary<string, string?>
                {
                    [FieldNames.Visual] = visual,
                    [FieldNames.Mrz] = mrz
                };
            }

            var links = new Dictionary<string, string?>
            {
                [ImageTypes.Portrait] = _linksProvider.GetImageLink(ImageTypes.Portrait),
                [ImageTypes.Full] = _linksProvider.GetImageLink(ImageTypes.Full),
                [ImageTypes.Bw] = _linksProvider.GetImageLink(ImageTypes.Bw),
                [ImageTypes.Uv] = _linksProvider.GetImageLink(ImageTypes.Uv)
            };

            return new ScanResult
            {
                Data = data,
                Links = links!
            };
        });
    }

    public byte[]? GetImage(string type)
    {
        if (!ImageTypes.Supported.Contains(type.ToLowerInvariant()))
            return null;

        return type.ToLowerInvariant() switch
        {
            ImageTypes.Portrait => _reader.GetReaderGraphicsFileImageByFieldType(201) as byte[],
            ImageTypes.Full => _reader.GetReaderFileImage(1) as byte[],
            ImageTypes.Bw => _reader.GetReaderFileImage(0) as byte[],
            ImageTypes.Uv => _reader.GetReaderFileImage(2) as byte[],
            _ => null
        };
    }
}
