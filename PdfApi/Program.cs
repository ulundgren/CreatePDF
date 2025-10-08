using Microsoft.AspNetCore.Mvc;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements.Forms;
using ceTe.DynamicPDF.Text;
using ceTe.DynamicPDF.PageElements;
using System.Diagnostics;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IPDFService, PDFService>();

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
        [FromServices] IPDFService service,
        [FromQuery] string title,
        [FromBody] HtmlContent content
    ) =>
    {
        return Results.File(service.CreatePDF(title, content.content), "application/pdf", "document.pdf");
    })
    .WithName("CreatePDF");

app.MapPost(
    "/api/createpdfondisk",
    (
        [FromServices] IPDFService service,
        [FromQuery] string title,
        [FromBody] HtmlContent content,
        [FromQuery] int count = 10,
        [FromQuery] bool delete = false
    ) =>
    {
        long startTime = Stopwatch.GetTimestamp();
        int i = 0;
        for (i = 0; i < count; i++)
        {
            File.WriteAllBytes($"c:\\tmp\\document{i}.pdf", service.CreatePDF(title, content.content));
            if (delete)
            {
                File.Delete($"c:\\tmp\\document{i}.pdf");
            }
        }
        TimeSpan elapsed = Stopwatch.GetElapsedTime(startTime);
        return Results.Ok($"PDF created and deleted on disk {i} time(s) in {elapsed} seconds.");
    })
    .WithName("CreatePDFOnDisk");


app.Run();


public record HtmlContent(string content);


public interface IPDFService
{
    byte[] CreatePDF(string title, string content);
}

internal class PDFService : IPDFService
{
    public byte[] CreatePDF(string title, string content)
    {

        PageInfo layoutPage = new PageInfo(PageSize.A4, PageOrientation.Portrait);

        HtmlLayout html = new(text: content, pageInfo: layoutPage);

        html.Header.Center.Text = "<b><i>%%PR%%%%SP%% of %%ST%%</i></b>";
        html.Header.Center.HasPageNumbers = true;
        html.Header.Center.Width = 200;

        html.Footer.Center.Text = "%%PR%%%%SP(A)%% of %%ST(B)%%";
        html.Footer.Center.HasPageNumbers = true;
        html.Footer.Center.Width = 200;

        Document document = html.Layout();

        Page page = document.Pages[0];
        //Signature signature = new("signature", 50, 400, 450, 100);
        //page.Elements.Add(signature);

        GoogleFont googleFont = Font.Google("Roboto", false, false);
        Label lbl = new Label("A Google Font (Roboto) Example.", 10, 200, 150, 50, googleFont, 22);
        page.Elements.Add(lbl);

        Certificate cert = new(@"uc-client.p12", "test");
        document.Certify("signature", cert, CertifyingPermission.NoChangesAllowed);

        return document.Draw();
    }
}


public partial class Program { }