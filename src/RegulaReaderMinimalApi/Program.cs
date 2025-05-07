using RegulaReaderMinimalApi.Endpoints;
using RegulaReaderMinimalApi.Scanner;
using RegulaReaderMinimalApi.Scanner.Interfaces;
using RegulaReaderMinimalApi.Services;
using RegulaReaderMinimalApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IDocumentScanner, RegulaScanner>();
builder.Services.AddSingleton<ILinksProvider, LinksProvider>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.MapScannerEndpoints();

app.Run();

