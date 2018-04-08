using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class help_help :LoginPageBase
{
    public string index = "";
    public string K = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //操作日志统计开始
            //Utils.WritePageLog(Request, "帮助中心");
            //K = Request["K"] + "";
            //操作日志统计结束
        }

        if (!string.IsNullOrEmpty(Request["Index"]))
            {
                index= Request["Index"];
            }
            else
            {
                index= "0";
            }
    }
   
}