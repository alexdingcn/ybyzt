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
            LiteralJS.Text = " $.BindRegister(\"RegiComp\");" + (user == null ? "" : "$('#txt_Phone').trigger('blur');" )+ "";
            if (!string.IsNullOrEmpty(Request["Comid"]))
            {
                Hi.Model.BD_Company Comp = new Hi.BLL.BD_Company().GetModel(Request["Comid"].ToInt(0));
                if (Comp == null)
                {
                    return;
                }
                else if (Comp.dr == 1)
                {
                    return;
                }
                else if (Comp.AuditState != 2)
                {
                    return;
                }
                else if (Comp.IsEnabled != 1)
                {
                    return;
                }

                Compid = Comp.ID;
                HidCompid.Value = Request["Comid"];
                CompDisName.InnerHtml = "代理商名称：";
                Title_type.InnerHtml = Comp.CompName + " － 代理商加盟";
                //Title = ConfigurationManager.AppSettings["TitleName"].ToString() + " － 代理商加盟";
                LiteralJS.Text = " $.BindRegister(\"RegiDis\"," + Comp.ID + ");" + (user == null ? "" : "$('#txt_Phone').trigger('blur');") + "";
            }
            //操作日志统计开始
            Utils.WritePageLog(Request, "卖家注册");
            //操作日志统计结束
        }
    }
}