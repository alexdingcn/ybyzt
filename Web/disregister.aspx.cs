using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;

public partial class CompRegister : System.Web.UI.Page
{
    public int Compid = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Hi.Model.SYS_Users user = LoginModel.IsLoginAllUser();
            if (user != null)
            {
                txt_Phone.Value = user.Phone;
            }
                Compid = 0;
                HidCompid.Value = Request["Comid"];
                //Title = ConfigurationManager.AppSettings["TitleName"].ToString() + " － 代理商加盟";
                LiteralJS.Text = " $.BindRegister(\"RegiDis\"," + 0 + ");" + (user == null ? "" : "$('#txt_Phone').trigger('blur');") + "";
            //操作日志统计开始
            Utils.WritePageLog(Request, "卖家注册");
            //操作日志统计结束
        }
    }
}