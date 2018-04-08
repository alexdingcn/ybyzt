using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hi.Model;
using System.Data;

public partial class Company_SysManager_RebateModify : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (KeyID > 0)
            {
                Databind();
            }
            if (Request.QueryString["Type"]=="1")
            {
                btnAdd.Visible = false;
            }
        }
    }

    public void Databind()
    {
        try
        {
            Hi.Model.BD_Rebate rebate = new Hi.BLL.BD_Rebate().GetModel(KeyID);
            if (rebate != null)
            {
                if (rebate.CompID != CompID)
                {
                    Response.Write("信息异常。");
                    Response.End();
                }
                if (rebate.RebateState != 1)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    "<script>layerCommon.msg('返利信息已经失效！', IconOption.哭脸, 2000);</script>");
                    return;
                }
                if (IsFind()) {
                    return;
                }
                txtDisID.CompID = rebate.CompID.ToString();
                txtDisID.Disid = rebate.DisID.ToString();
                txtCode.Value = rebate.ReceiptNo;
                txtRebateAmount.Value = rebate.RebateAmount.ToString("0.00");
                //this.RdType1.Checked = (rebate.RebateType == 1);
                //this.RdType2.Checked = (rebate.RebateType == 2);
                txtStartDate.Value = rebate.StartDate.ToString("yyyy-MM-dd");
                txtEndDate.Value = rebate.EndDate.ToString("yyyy-MM-dd");
                txtRemark.InnerText = rebate.Remark;
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    "<script>layerCommon.msg('返利信息已经失效！', IconOption.错误, 2000);</script>");
                return;
            }
        }
        catch (Exception ex)
        {

        }
    }

    public bool IsFind()
    {
        int pageCount = 0;
        int Counts = 0;
        string JoinTableStr = "  BD_RebateDetail join Dis_order as Dorder   on Dorder.id=BD_RebateDetail.orderid ";
        DataTable LRobate = new Hi.BLL.BD_RebateDetail().GetList(1, 1, " BD_RebateDetail.createdate ", true, " ReceiptNo,BD_RebateDetail.Amount,BD_RebateDetail.CreateDate ", JoinTableStr, " and RebateID=" + KeyID + " and BD_RebateDetail.dr=0 ", out pageCount, out Counts);
        if (LRobate.Rows.Count > 0)
        {
            JScript.AlertMethod(this, "返利已被使用，不允许修改", JScript.IconOption.错误, "function(){ history.go(-1); }");
        }
        return LRobate.Rows.Count > 0;
    }

    protected void btnAdd_Click(object sender, EventArgs e)
    {
        Hi.Model.BD_Rebate rebate = new BD_Rebate();

        if (KeyID != 0)//修改
        {
            rebate = new Hi.BLL.BD_Rebate().GetModel(KeyID);
            if (IsFind())
            {
                return;
            }
            rebate.DisID = Convert.ToInt32(txtDisID.Disid);
            rebate.RebateAmount = Convert.ToDecimal(txtRebateAmount.Value);
            //可用返利金额
            rebate.EnableAmount = rebate.RebateAmount;
            rebate.ReceiptNo = Common.NoHTML(txtCode.Value);
            //if (this.RdType1.Checked)
            //{
            //    rebate.RebateType = 1;
            //}
            //else if (this.RdType2.Checked)
            //{
            //    rebate.RebateType = 2;
            //}
            rebate.RebateType = 0;
            rebate.StartDate = Convert.ToDateTime(txtStartDate.Value);
            rebate.EndDate = Convert.ToDateTime(txtEndDate.Value);
            rebate.Remark = Common.NoHTML(txtRemark.Value);

            rebate.ts = DateTime.Now;
            rebate.modifyuser = UserID;
            if (new Hi.BLL.BD_Rebate().Update(rebate))
            {
                Response.Redirect("RebateList.aspx");
                Response.End();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    "<script>layerCommon.msg('修改失败！', IconOption.错误, 2000);</script>");
                return;
            }
        }
        else//新增
        {
            rebate.DisID = Convert.ToInt32(txtDisID.Disid);
            rebate.CompID = new Hi.BLL.BD_Distributor().GetModel(rebate.DisID).CompID;
            rebate.RebateAmount = Convert.ToDecimal(txtRebateAmount.Value);
            rebate.UserdAmount = 0;
            rebate.EnableAmount = rebate.RebateAmount;
            rebate.ReceiptNo = Common.NoHTML(txtCode.Value);
            //if (this.RdType1.Checked)
            //{
            //    rebate.RebateType = 1;
            //}
            //else if (this.RdType2.Checked)
            //{
            //    rebate.RebateType = 2;
            //}
            rebate.RebateType =0;
            rebate.RebateState = 1;
            rebate.StartDate = Convert.ToDateTime(txtStartDate.Value);
            rebate.EndDate = Convert.ToDateTime(txtEndDate.Value);
            rebate.Remark = Common.NoHTML(txtRemark.Value);

            rebate.ts = DateTime.Now;
            rebate.modifyuser = UserID;
            
            int id = new Hi.BLL.BD_Rebate().Add(rebate);
            if (id > 0)
            {
                Response.Redirect("RebateList.aspx");
                Response.End();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    "<script>layerCommon.msg('新增失败！', IconOption.错误, 2000);</script>");
                return;
            }
        }
    }
}