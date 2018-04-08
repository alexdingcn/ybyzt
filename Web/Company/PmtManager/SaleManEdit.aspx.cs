using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_SaleManEdit : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.BindSMParent(ddlSMParent, CompID, "请选择");
            Common.BindSMType(txtSMType);
            Databinds();
        }
    }
    public void Databinds()
    {
        Hi.Model.BD_DisSalesMan sale = new Hi.BLL.BD_DisSalesMan().GetModel(KeyID);
        if (sale != null)
        {
            try
            {
                if (sale.CompID != CompID)
                {
                    Response.Write("你无权限访问。");
                    Response.End();
                }
                if (sale.SalesType == ((int)Enums.DisSMType.业务员))
                {
                    ddlSMParent.SelectedValue = sale.ParentID.ToString();
                }
                txtSMType.SelectedValue = sale.SalesType.ToString();
                txtSaleName.Value = sale.SalesName;
                txtSaleCode.Value = sale.SalesCode;
                txtPhone.Value = sale.Phone;
                txtEmail.Value = sale.Email;
                txtRemark.Value = sale.Remark;
                int status = sale.IsEnabled;
                this.rdoStatus1.Checked = (status != 1);
                this.rdoStatus0.Checked = (status == 1);
            }
            catch (Exception ex)
            {

            }
        }
        else
        {
            if (Request["posttype"] == "1")
            {
                first.Visible = false;
                right.Style["margin-top"] = "0";
            }
        }

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        if (txtSMType.SelectedValue == ((int)Enums.DisSMType.业务员).ToString())
        {
            if (ddlSMParent.SelectedValue == "" || ddlSMParent.SelectedValue == "-1") {
                JScript.AlertMethod(this, "请选择业务经理！", JScript.IconOption.错误, "function (){ location.href=location.href; }");
                return;
            }
        }
        Hi.Model.BD_DisSalesMan sale = null;
        if (KeyID != 0)
        {
            sale = new Hi.BLL.BD_DisSalesMan().GetModel(KeyID);
            sale.SalesType = Convert.ToInt32(txtSMType.SelectedValue);
            if (txtSMType.SelectedValue == ((int)Enums.DisSMType.业务员).ToString())
            {
                sale.ParentID = ddlSMParent.SelectedValue.ToInt(0);
            }
            else
            {
                sale.ParentID = 0;
            }
            sale.SalesName =Common.NoHTML( txtSaleName.Value.Trim());
            sale.SalesCode = Common.NoHTML(txtSaleCode.Value.Trim());
            sale.Phone = Common.NoHTML(txtPhone.Value.Trim());
            sale.Email = Common.NoHTML(txtEmail.Value.Trim());
            sale.Remark = Common.NoHTML(txtRemark.Value.Trim());
            if (this.rdoStatus1.Checked)
                sale.IsEnabled = 0;
            else
                sale.IsEnabled = 1;

            sale.ts = DateTime.Now;
            sale.modifyuser = UserID;
            if (new Hi.BLL.BD_DisSalesMan().Update(sale))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='SaleManInfo.aspx?KeyID=" + KeyID + "'; }");
                Response.Redirect("SaleManInfo.aspx?KeyID=" + KeyID + "");
            }
        }
        else
        {
            sale = new Hi.Model.BD_DisSalesMan();  
            sale.SalesType = Convert.ToInt32(txtSMType.SelectedValue);
            if (txtSMType.SelectedValue == ((int)Enums.DisSMType.业务员).ToString())
            {
                sale.ParentID = ddlSMParent.SelectedValue.ToInt(0);
            }
            else
            {
                sale.ParentID = 0;
            }
            sale.CompID = CompID;
            sale.SalesName = Common.NoHTML(txtSaleName.Value.Trim());
            sale.SalesCode = Common.NoHTML(txtSaleCode.Value.Trim());
            sale.Phone = Common.NoHTML(txtPhone.Value.Trim());
            sale.Email = Common.NoHTML(txtEmail.Value.Trim());
            sale.Remark = Common.NoHTML(txtRemark.Value.Trim());
            if (this.rdoStatus1.Checked)
                sale.IsEnabled = 0;
            else
                sale.IsEnabled = 1;

            //标准参数
            sale.CreateDate = DateTime.Now;
            sale.CreateUserID = UserID;
            sale.ts = DateTime.Now;
            sale.modifyuser = UserID;
            int newuserid = 0;
            newuserid = new Hi.BLL.BD_DisSalesMan().Add(sale);
            if (newuserid > 0)
            {
                if (Request["posttype"] == "1")
                {
                    if (txtSMType.SelectedValue == ((int)Enums.DisSMType.业务员).ToString())
                    {
                        this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "<script>cancel('" + txtSaleName.Value.Trim() + " [业务员]',"+newuserid+")</script>");
                    }
                    else
                    {
                        this.Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "<script>cancel('" + txtSaleName.Value.Trim() + " [业务经理]',"+newuserid+")</script>");
                    }
                }
                else
                {
                    Response.Redirect("SaleManInfo.aspx?KeyID=" + newuserid);
                }
            }
        }
    }
}