using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_SysManager_RebateInfo : CompPageBase
{

    public string page = "1";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (KeyID > 0)
            {
                Databind();
                BindRebateDetail();
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
                    Response.Write("你无权限访问。");
                    Response.End();
                }
                
                txtDisID.InnerText = new Hi.BLL.BD_Distributor().GetModel(rebate.DisID).DisName;
                txtCode.InnerText = rebate.ReceiptNo;
                txtRebateAmount.InnerText = rebate.RebateAmount.ToString("0.00");
                //string type = rebate.RebateType == 1 ? "整单返利" : (rebate.RebateType == 2 ? "分摊返利" : "");
                //if (type != "")
                //    txtType.InnerText = type;
                //else
                //{
                //    Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                //    "<script>layerCommon.msg('返利信息已经失效！', IconOption.错误, 2000);</script>");
                //    return;
                //}

                txtStartDate.InnerText = rebate.StartDate.ToString("yyyy-MM-dd");
                txtEndDate.InnerText = rebate.EndDate.ToString("yyyy-MM-dd");
                txtRemark.InnerText = rebate.Remark;

                //add by hgh  0922
                if (rebate.RebateState != 1)
                {
                    //Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                    //"<script>layerCommon.msg('返利信息已经失效！', IconOption.哭脸, 2000);</script>");
                    //return;
                    this.libtnEdit.Visible = false; //返利失效后，不可编辑
                    lblQixian.InnerText = "（已失效）";
                }
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

    protected void btn_Del(object sender, EventArgs e)
    {
        Hi.Model.BD_Rebate rebate = new Hi.Model.BD_Rebate();

        if (KeyID != 0) //修改
        {
            rebate = new Hi.BLL.BD_Rebate().GetModel(KeyID);
            rebate.dr = 1;
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
        else //新增
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "Result",
                "<script>layerCommon.msg('新增失败！', IconOption.错误, 2000);</script>");
            return;
        }
    }

    public void BindRebateDetail()
    {
        int pageCount = 0;
        int Counts = 0;
        string JoinTableStr = "  BD_RebateDetail join Dis_order as Dorder   on Dorder.id=BD_RebateDetail.orderid ";
        DataTable LRobate = new Hi.BLL.BD_RebateDetail().GetList(Pager.PageSize, Pager.CurrentPageIndex, " BD_RebateDetail.createdate ", true, " ReceiptNo,BD_RebateDetail.Amount,BD_RebateDetail.CreateDate ", JoinTableStr, " and RebateID=" + KeyID + " and isnull(BD_RebateDetail.dr,0)=0  ", out pageCount, out Counts);
        this.Rpt_RobateDetail.DataSource = LRobate;
        this.Rpt_RobateDetail.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        if (LRobate.Rows.Count > 0)
        {
            libtnEdit.Visible = false;
            libtnDel.Visible = false;
        }
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        BindRebateDetail();
    }

}