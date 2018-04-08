using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_Systems_IndustryModify : AdminPageBase
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Databind();
        }
    }

    public void Databind()
    {
        if (KeyID != 0)
        {
            Hi.Model.SYS_Industry industry = new Hi.BLL.SYS_Industry().GetModel(KeyID);
            try
            {
                txtInduName.Value = industry.InduName;
                txtSort.Value = industry.SortIndex;
                int status = industry.IsEnabled;
                this.rdoStatus1.Checked = (status != 1);
                this.rdoStatus0.Checked = (status == 1);
            }
            catch (Exception ex)
            {

            }
        }
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Hi.Model.SYS_Industry industry = null;

        if (string.IsNullOrEmpty(txtInduName.Value.Trim()))
        {
            JScript.AlertMsg(this, "行业分类名称不能为空!");
            return;
        }
        if (KeyID != 0)
        {
            industry = new Hi.BLL.SYS_Industry().GetModel(KeyID);
            industry.InduName = Common.NoHTML(txtInduName.Value.Trim());
            industry.SortIndex = Common.NoHTML(txtSort.Value.Trim());
            if (this.rdoStatus1.Checked)
                industry.IsEnabled = 0;
            else
                industry.IsEnabled = 1;

            industry.InduCode = "0";
            industry.ts = DateTime.Now;
            industry.modifyuser = UserID;

            if (new Hi.BLL.SYS_Industry().Update(industry))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='IndustryInfo.aspx?KeyID=" + KeyID + "'; }");
                Response.Redirect("IndustryInfo.aspx?KeyID=" + KeyID);
            }
        }
        else
        {
            industry = new Hi.Model.SYS_Industry();
            industry.InduName = Common.NoHTML(txtInduName.Value.Trim());

            industry.SortIndex = Common.NoHTML(txtSort.Value.Trim());
            if (this.rdoStatus1.Checked)
                industry.IsEnabled = 0;
            else
                industry.IsEnabled = 1;

            //标准参数
            industry.CreateDate = DateTime.Now;
            industry.CreateUserID = UserID;
            industry.ts = DateTime.Now;
            industry.modifyuser = UserID;
            int newuserid = 0;
            newuserid = new Hi.BLL.SYS_Industry().Add(industry);
            if (newuserid > 0)
            {
                Response.Redirect("IndustryInfo.aspx?KeyID=" + newuserid);
            }
        }
    }

}