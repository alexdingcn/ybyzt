using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Order_updateBill : CompPageBase
{
    Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(KeyID);

            if (OrderModel != null)
            {
                //this.txtBillNo.Value = OrderModel.BillNo;
                //if (OrderModel.IsBill == 1)
                //{
                //    this.txtBillNo.Attributes["readonly"] = "readonly";
                //    this.rdoIsBillNo.Attributes["disabled"] = "true";
                //    this.rdoIsBillOk.Attributes["disabled"] = "true";

                //    this.rdoIsBillOk.Checked = true;
                //    this.rdoIsBillNo.Checked = false;
                //    this.Save.Visible = false;
                //}
                //else
                //{
                //    this.rdoIsBillOk.Checked = false;
                //    this.rdoIsBillNo.Checked = true;
                //}
            }
        }
    }

     /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(KeyID);

        try
        {
            string BillNo = this.txtBillNo.Value.Trim();
            string IsBill = this.rdoIsBillOk.Checked ? "1" : "0";

            if (OrderModel != null)
            {
                //if (OrderModel.IsBill == 0)
                //{
                //    OrderModel.BillNo = BillNo;
                //    OrderModel.IsBill = IsBill.ToInt(0);

                //    if (OrderBll.Update(OrderModel))
                //    {
                //        Response.Write("<script language=\"javascript\">window.parent.Audit('" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "');</script>");
                //    }
                //}
                //else
                //    JScript.AlertMsgOne(this, "发票信息已完成，不能再保存!", JScript.IconOption.错误, 2500);
            }
            else
                JScript.AlertMsgOne(this, "数据不存在!", JScript.IconOption.错误, 2500);
        }
        catch (Exception ex)
        {
            JScript.AlertMsgOne(this, "保存失败!", JScript.IconOption.错误, 2500);
        }
    }
}