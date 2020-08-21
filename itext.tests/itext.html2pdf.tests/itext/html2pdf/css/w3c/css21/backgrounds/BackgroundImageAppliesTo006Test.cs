using System;
using iText.Html2pdf.Css.W3c;

namespace iText.Html2pdf.Css.W3c.Css21.Backgrounds {
    // TODO DEVSIX-2445. Display property (table-column) lacking support
    public class BackgroundImageAppliesTo006Test : W3CCssTest {
        protected internal override String GetHtmlFileName() {
            return "background-image-applies-to-006.xht";
        }
    }
}
