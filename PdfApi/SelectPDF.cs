using SelectPdf;

namespace PdfApi.SelectPDF;

internal class SelectPDFService : IPDFService
{
    public byte[] CreatePDF(string content, string? baseUrl)
    {


        // instantiate a html to pdf converter object
        HtmlToPdf converter = new HtmlToPdf();

        // create a new pdf document converting the html code
        PdfDocument doc = converter.ConvertHtmlString(content, baseUrl);






        // save pdf document
        byte[] d = doc.Save();

        // close pdf document
        doc.Close();

        return d;


    }
}