using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_CompCtinfo : CompPageBase
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
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID);
        if (comp != null)
        {
            txtCustomInfo.Text = comp.CustomCompinfo;
            div_CustomInfo.InnerHtml = comp.CustomCompinfo;
        }
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(CompID);
        if (comp != null)
        {  
            comp.CustomCompinfo = txtCustomInfo.Text;
            if (new Hi.BLL.BD_Company().Update(comp))
            JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href=window.location.href; }");
        }
    } 
    
    protected void btnEdit_Click(object sender, EventArgs e)
    {
        txtCustomInfo.Attributes.Add("style", "display:block");
        div_CustomInfo.Attributes.Add("style", "display:none");
        btnEdit.Visible = false;
        btnback.Visible = true;
        btnSave.Visible = true;
        ClientScript.RegisterStartupScript(this.GetType(), "result", "<script>Load();</script>");
    }
}