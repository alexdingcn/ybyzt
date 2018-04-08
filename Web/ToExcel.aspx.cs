using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ToExcel : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    public void btn_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = false;
        Response.ContentEncoding = System.Text.Encoding.UTF8;
        Response.AppendHeader("Content-Disposition", string.Format("attachment;filename={0}.{1}", DateTime.Today.ToString("yyyy-MM-dd"),"xls"));
        Response.ContentType = "application/vnd.ms-excel";
        this.EnableViewState = false;
        System.IO.StringWriter oStringWriter = new System.IO.StringWriter();
        System.Web.UI.HtmlTextWriter oHtmlTextWriter = new System.Web.UI.HtmlTextWriter(oStringWriter);
        this.RenderControl(oHtmlTextWriter);
        //强制输出bom 这样避免excel打开时乱码
        Response.BinaryWrite(new byte[] { 0xEF, 0xBB, 0xBF });
        Response.Output.Write(this.tbl.Value); 
        Response.Flush();
        Response.End();
    }
}