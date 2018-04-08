using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_IndustryInfo : AdminPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();
        }
    }

    public void DataBinds()
    {
        if (KeyID != 0)
        {
            Hi.Model.SYS_Industry industry = new Hi.BLL.SYS_Industry().GetModel(KeyID);

            lblname.InnerText = industry.InduName;
            lblsort.InnerText = industry.SortIndex;
            lblstate.InnerText = industry.IsEnabled == 1 ? "启用" : "禁用";
        }
        else
        {
            Response.Write("行业分类不存在。");
            Response.End();
        }
    }

    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.SYS_Industry industry = new Hi.BLL.SYS_Industry().GetModel(KeyID);
        if (industry != null)
        {
            industry.dr = 1;
            industry.ts = DateTime.Now;
            industry.modifyuser = UserID;
            if (new Hi.BLL.SYS_Industry().Update(industry))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='IndustryList.aspx'; }");
                Response.Redirect("IndustryList.aspx");
            }
        }
    }
}