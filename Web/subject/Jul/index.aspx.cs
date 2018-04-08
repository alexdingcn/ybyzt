using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class subject_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //操作日志统计开始
            Utils.WritePageLog(Request, "7月精选店铺");
            //操作日志统计结束
        }
    }
}