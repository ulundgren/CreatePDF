using Microsoft.AspNetCore.Mvc;
using PdfApi.DynamicPDF;
using PdfApi.SelectPDF;
using PdfApi.IronPDF;
using System.Diagnostics;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<DynamicPDFService>();
builder.Services.AddSingleton<SelectPDFService>();
builder.Services.AddSingleton<IronPDFService>();
builder.Services.AddSingleton<Func<string, IPDFService>>(sp => key =>
{
    return key switch
    {
        "dynamic" => sp.GetRequiredService<DynamicPDFService>(),
        "select" => sp.GetRequiredService<SelectPDFService>(),
        "iron" => sp.GetRequiredService<IronPDFService>(),
        _ => sp.GetRequiredService<DynamicPDFService>() // default
    };
});


// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapPost(
    "/api/createpdf",
    (
        [FromServices] Func<string, IPDFService> pdfServiceFactory,
        [FromQuery] string title,
        [FromBody] HtmlContent content,
        [FromQuery] string type = "dynamic",
        [FromQuery] string? baseUrl = null
    ) =>
    {
        var service = pdfServiceFactory(type);
        return Results.File(service.CreatePDF(content.content, baseUrl), "application/pdf", "document.pdf");
    })
    .WithName("CreatePDF");

app.MapPost(
    "/api/createpdfondisk",
    (
        [FromServices] Func<string, IPDFService> pdfServiceFactory,
        [FromQuery] string title,
        [FromBody] HtmlContent content,
        [FromQuery] int count = 10,
        [FromQuery] bool delete = false,
        [FromQuery] string type = "dynamic",
        [FromQuery] string? baseUrl = null
    ) =>
    {
        var service = pdfServiceFactory(type);
        long startTime = Stopwatch.GetTimestamp();
        int i = 0;
        for (i = 0; i < count; i++)
        {
            File.WriteAllBytes($"c:\\tmp\\document{i}.pdf", service.CreatePDF(content.content, baseUrl));
            if (delete)
            {
                File.Delete($"c:\\tmp\\document{i}.pdf");
            }
        }
        TimeSpan elapsed = Stopwatch.GetElapsedTime(startTime);
        return Results.Ok($"PDF created and deleted on disk {i} time(s) in {elapsed} seconds.");
    })
    .WithName("CreatePDFOnDisk");

app.MapPost(
"/api/createandtimepdf",
(
    [FromServices] Func<string, IPDFService> pdfServiceFactory,
    [FromQuery] string title,
    [FromBody] HtmlContent content,
    [FromQuery] int count = 0,
    [FromQuery] bool delete = false,
    [FromQuery] string type = "dynamic",
    [FromQuery] string? baseUrl = null
) =>
{
    var service = pdfServiceFactory(type);

    int i = 0;
    if (count != 0)
    {
        long startTime = Stopwatch.GetTimestamp();
        for (i = 0; i < count; i++)
        {
            service.CreatePDF(content.content, baseUrl);
        }
        TimeSpan elapsed = Stopwatch.GetElapsedTime(startTime);
        return Results.Ok($"PDF created and deleted on disk {i} time(s) in {elapsed} seconds.");
    }
    else
    {
        var ret = new System.Text.StringBuilder();

        long startTime = Stopwatch.GetTimestamp();
        service.CreatePDF(content.content, baseUrl);
        TimeSpan elapsed = Stopwatch.GetElapsedTime(startTime);

        ret.AppendLine($"1 PDF created in {elapsed} seconds.");

        startTime = Stopwatch.GetTimestamp();
        for (i = 0; i < 10; i++)
        {
            service.CreatePDF(content.content, baseUrl);
        }
        elapsed = Stopwatch.GetElapsedTime(startTime);
        ret.AppendLine($"10 PDFs created in {elapsed} seconds.");

        startTime = Stopwatch.GetTimestamp();
        for (i = 0; i < 100; i++)
        {
            service.CreatePDF(content.content, baseUrl);
        }
        elapsed = Stopwatch.GetElapsedTime(startTime);
        ret.AppendLine($"100 PDFs created in {elapsed} seconds.");

        startTime = Stopwatch.GetTimestamp();
        for (i = 0; i < 1000; i++)
        {
            service.CreatePDF(content.content, baseUrl);
        }
        elapsed = Stopwatch.GetElapsedTime(startTime);
        ret.AppendLine($"1000 PDFs created in {elapsed} seconds.");

        return Results.Ok(ret.ToString());
    }


})
.WithName("CreateAndTimePdf");


app.Run();


public record HtmlContent(string content);


public interface IPDFService
{
    byte[] CreatePDF(string content, string? baseUrl);
}




public partial class Program { }