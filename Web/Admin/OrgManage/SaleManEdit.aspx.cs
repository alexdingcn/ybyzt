using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_SaleManEdit : AdminPageBase
{
    public DropDownList allSaleMan = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Common.BindOrgSale(txtOrg, allSaleMan, "请选择");
            Databinds();
        }
    }
    public void Databinds()
    {
        Hi.Model.BD_SalesMan sale = new Hi.BLL.BD_SalesMan().GetModel(KeyID);
        if (sale != null)
        {
            try
            {
                if (UserType == 3 || UserType == 4)
                {
                    if (sale.OrgID != OrgID)
                    {
                        Response.Write("业务员不存在。");
                        Response.End();
                    }
                }
                txtOrg.SelectedValue = sale.OrgID.ToString();
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

    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Hi.Model.BD_SalesMan sale = null;
        if (KeyID != 0)
        {
            sale = new Hi.BLL.BD_SalesMan().GetModel(KeyID);
            sale.SalesName = Common.NoHTML(txtSaleName.Value.Trim());
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
            if (new Hi.BLL.BD_SalesMan().Update(sale))
            {
                //JScript.AlertMsgMo(this, "操作成功", "function(){ window.location.href='SaleManInfo.aspx?KeyID=" + KeyID + "'; }");
                Response.Redirect("SaleManInfo.aspx?KeyID=" + KeyID + "");
            }
        }
        else
        {
            sale = new Hi.Model.BD_SalesMan();
            sale.OrgID = Convert.ToInt32(txtOrg.SelectedValue);
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
            sale.ts = DateTime.Now;
            sale.modifyuser = UserID;
            int newuserid = 0;
            newuserid = new Hi.BLL.BD_SalesMan().Add(sale);
            if (newuserid > 0)
            {
                Response.Redirect("SaleManInfo.aspx?KeyID=" + newuserid);
            }
        }
    }
}