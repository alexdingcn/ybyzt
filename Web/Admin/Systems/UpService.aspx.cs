using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Systems_UpPhone : AdminPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            
            DataBinds();
            List<Hi.Model.Pay_Service> ServiceList = new Hi.BLL.Pay_Service().GetList(" top 5 *", "  isnull(dr,0)=0 and IsAudit=1 and CompID=" + KeyID + " ", " OutData desc");
            this.ServiceList.DataSource = ServiceList;
            this.ServiceList.DataBind();
        }
    }

    public void DataBinds()
    {
        //绑定服务日期
        //List<Hi.Model.Pay_Service> serviceord = new Hi.BLL.Pay_Service().GetList("*", " compid=" + KeyID + " and isaudit=1 and outofdata=0 ", " createdate desc");
        //if (serviceord.Count > 0)
        //{
            Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        txtCreateDate.Value = comp.EnabledStartDate.ToString("yyyy-MM-dd") == "0001-01-01" ? "" : comp.EnabledStartDate.ToString("yyyy-MM-dd");//存在有效服务
        txtEndCreateDate.Value = comp.EnabledEndDate.ToString("yyyy-MM-dd") == "0001-01-01" ? "" : comp.EnabledEndDate.ToString("yyyy-MM-dd");
        //}
    }

    protected void btnSubMit_Click(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(KeyID);
        if (comp!=null)
        {
            if (string.IsNullOrWhiteSpace(txtCreateDate.Value.Trim()))
            {
                JScript.AlertMsg(this, "起始日期不能为空");
                return;
            }
            if (string.IsNullOrWhiteSpace(txtEndCreateDate.Value.Trim()))
            {
                JScript.AlertMsg(this, "结束日期不能为空");
                return;
            }
            comp.EnabledStartDate =Convert.ToDateTime(txtCreateDate.Value);
            comp.EnabledEndDate = Convert.ToDateTime(txtEndCreateDate.Value);
            new Hi.BLL.BD_Company().Update(comp);
            ClientScript.RegisterStartupScript(this.GetType(), "MSG", "<script>window.parent.location.href=window.parent.location.href; </script>");
        }
       
    }
}