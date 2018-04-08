using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class financial : LoginPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        //操作日志统计开始
        Utils.WritePageLog(Request, "金融服务");
        //操作日志统计结束
    }
}