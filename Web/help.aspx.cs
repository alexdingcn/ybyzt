using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class help : LoginPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //操作日志统计开始
            Utils.WritePageLog(Request, "帮助中心");
            //操作日志统计结束
        }
    }
}