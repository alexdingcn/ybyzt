using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_newOrder_Code39 : System.Web.UI.Page
{
    public string KeyID ="";
    protected void Page_Load(object sender, EventArgs e)
    {
     
        if (!string.IsNullOrEmpty(Request["KeyID"]))
            KeyID = Request["KeyID"] + "";
        //DataTable lo = new Hi.BLL.DIS_OrderOut().GetList("", " isnull(o.dr,0)=0 and o.IsAudit<>3 and o.ID=" + KeyID);
        Code39 _Code39 = new Code39();
        _Code39.Height = 60;
        _Code39.Magnify =0.5;
      //_Code39.ViewFont = new Font("微软雅黑", 11);
        System.Drawing.Image _CodeImage = _Code39.GetCodeImage(KeyID, Code39.Code39Model.Code39Normal, true, true);
        System.IO.MemoryStream _Stream = new System.IO.MemoryStream();
        _CodeImage.Save(_Stream, System.Drawing.Imaging.ImageFormat.Jpeg);
        Response.ContentType = "image/jpeg";
        Response.Clear();
        Response.BufferOutput = true;
        Response.BinaryWrite(_Stream.GetBuffer());
        Response.Flush();
    }
}