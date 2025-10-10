using SelectPdf;

namespace PdfApi.SelectPDF;

internal class SelectPDFService : IPDFService
{
    public byte[] CreatePDF(string content, string? baseUrl)
    {

        // instantiate a html to pdf converter object
        SelectPdf.HtmlToPdf converter = new SelectPdf.HtmlToPdf();

        // create a new pdf document converting the html code
        SelectPdf.PdfDocument doc = converter.ConvertHtmlString(content, baseUrl);
        // PdfPage page = doc.AddPage();
        // // get the #PKCS12 certificate from file
        // PdfDigitalCertificatesCollection certificates =
        //     PdfDigitalCertificatesStore.GetCertificates(@"uc-client.p12", "test");
        // PdfDigitalCertificate certificate = certificates[0];

        // PdfRenderingResult result;
        // PdfFont font = doc.AddFont(PdfStandardFont.Helvetica);
        // font.Size = 20;
        // PdfTextElement txt = new PdfTextElement(0, 0, "This is a text element", font);
        // result = page.Add(txt);


        // // create the digital signature object
        // PdfDigitalSignatureElement signature =
        //     new PdfDigitalSignatureElement(result.PdfPageLastRectangle, certificate);
        // signature.Reason = "SelectPdf";
        // signature.ContactInfo = "SelectPdf";
        // signature.Location = "SelectPdf";
        // page.Add(signature);

        // save pdf document
        byte[] d = doc.Save();

        // close pdf document
        doc.Close();

        return d;


    }
}