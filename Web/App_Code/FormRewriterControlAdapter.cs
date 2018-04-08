using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

namespace Sample.Web.UI.Adapters
{
    public class FormRewriterControlAdapter :
        System.Web.UI.Adapters.ControlAdapter
    {
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(new RewriteFormHtmlTextWriter(writer));
        }
    }

    public class RewriteFormHtmlTextWriter : HtmlTextWriter
    {
        public RewriteFormHtmlTextWriter(HtmlTextWriter writer) : base(writer)
        {
            this.InnerWriter = writer.InnerWriter;
        }

        public RewriteFormHtmlTextWriter(TextWriter writer)
            : base(writer)
        {
            this.InnerWriter = writer;
        }

        public override void WriteAttribute(string name, string value, bool fEncode)
        {
            if (name == "action")
            {
                HttpContext context = HttpContext.Current;

                if (context.Items["ActionAlreadyWritten"] == null)
                {
                    value = context.Request.RawUrl;
                    context.Items["ActionAlreadyWritten"] = true;
                }
            }

            base.WriteAttribute(name, value, fEncode);
        }
    }
}
