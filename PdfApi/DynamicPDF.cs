using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements.Forms;
using ceTe.DynamicPDF.Text;
using ceTe.DynamicPDF.PageElements;

namespace PdfApi.DynamicPDF;

internal class DynamicPDFService : IPDFService
{
    public byte[] CreatePDF(string content, string? baseUrl)
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