using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Goods_ShowPic : System.Web.UI.Page
{
    public string pic = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            object img = Request.QueryString["pic"];
            if (img != null)
            {
                pic = img.ToString().Trim();
            }
        }
    }
}