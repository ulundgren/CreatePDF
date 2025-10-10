using System.Drawing;
using IronPdf;
using IronPdf.Signing;

namespace PdfApi.IronPDF;

internal class IronPDFService : IPDFService
{
    public byte[] CreatePDF(string content, string? baseUrl)
    {

        var renderer = new ChromePdfRenderer();
        var pdf = renderer.RenderHtmlAsPdf(content);

        // Create a PdfSignature object directly from the certificate file and password.
        var signature = new PdfSignature("uc-client.p12", "test")
        {
            SignatureDate = DateTime.Now
            // SigningContact = "IronPDF",
            // SigningLocation = "IronPDF",
            // SigningReason = "IronPDF",
            // TimeStampUrl = "http://timestamp.digicert.com",
            // TimestampHashAlgorithm = TimestampHashAlgorithms.SHA256,
            // SignatureImage = new PdfSignatureImage("sign.png", 0,
            //     new Rectangle(350, 750, 200, 100))
        };

        pdf.Sign(signature);

        return pdf.BinaryData;
    }
}