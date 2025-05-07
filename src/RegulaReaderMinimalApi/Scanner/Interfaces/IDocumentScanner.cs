using RegulaReaderMinimalApi.Common.Models;

namespace RegulaReaderMinimalApi.Scanner.Interfaces
{
    public interface IDocumentScanner
    {
        Task ConnectAsync();
        Task DisconnectAsync();
        Task<ScanResult?> ReadAsync();
        byte[]? GetImage(string type);
    }
}
