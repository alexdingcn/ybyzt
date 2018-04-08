using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_SaleManInfo : AdminPageBase
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
        Hi.Model.BD_SalesMan sale = new Hi.BLL.BD_SalesMan().GetModel(KeyID);
        if (sale != null)
        {
            if (UserType == 3 || UserType == 4) {
                if (sale.OrgID != OrgID) {
                    Response.Write("业务员不存在。");
                    Response.End();
                }
            }
            lblOrg.InnerText = GetOrgName(sale.OrgID);
            lblSaleName.InnerText = sale.SalesName;
            lblSaleCode.InnerText = sale.SalesCode;
            lblPhone.InnerText = sale.Phone;
            lblEmail.InnerText = sale.Email;
            lblRemark.InnerText = sale.Remark;
            lblstate.InnerHtml = sale.IsEnabled == 1 ? "启用" : "<i style='color:red'>禁用</i>";
        }
        else
        {
            Response.Write("业务员不存在。");
            Response.End();
        }
    }

    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.BD_SalesMan sale = new Hi.BLL.BD_SalesMan().GetModel(KeyID);
        if (sale != null)
        {
            sale.dr = 1;
            sale.ts = DateTime.Now;
            sale.modifyuser = UserID;
            if (new Hi.BLL.BD_SalesMan().Update(sale))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='SaleManList.aspx'; }");
                Response.Redirect("SaleManList.aspx");
            }
        }
    }

    public string GetOrgName(int OrgID)
    {
        Hi.Model.BD_Org org = new Hi.BLL.BD_Org().GetModel(OrgID);
        if (org != null)
        {
            return org.OrgName;
        }
        else
        {
            return "";
        }
    }
}