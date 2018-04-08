using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Order_OrderReturnList : CompPageBase
{
    public string page;
    Hi.BLL.DIS_OrderReturn OrderReturnBll = new Hi.BLL.DIS_OrderReturn();

    protected void Page_Load(object sender, EventArgs e)
    {
        //page = Request["page"] + "";
        //this.Pager.CurrentPageIndex = page.ToInt(0);

        if (!IsPostBack)
        {
            //string CreateDate = "0";
            //if (Common.GetCompService(CompID.ToString(), out CreateDate) == "0")
            //{
            //    Response.Redirect("../SysManager/Service.aspx", true);
            //}
            string type = Request.QueryString["type"] + "";
            if (type == "")
            {
                this.ddrState.Value = "1";
            }
            else
            {
                this.ddrState.Value = "2";
            }
            this.hidCompId.Value = this.CompID.ToString();
            this.txtPager.Value = Common.PageSize;
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        string IDlist = string.Empty;//销售经理下属 员工ID集合
        if (DisSalesManID != 0)
        {
            if (Common.GetDisSalesManType(DisSalesManID.ToString(), out IDlist))
            {
                //销售经理
                strwhere = "and DisID in(select ID from BD_Distributor where smid in(" + IDlist + "))";
            }
            else
            {
                strwhere = "and DisID in(select ID from BD_Distributor where smid = " + DisSalesManID + ")";
            }
        }

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }

        if (this.ddrState.Value != "-2")
        {
            strwhere += " and ReturnState=" +Common.NoHTML( this.ddrState.Value.Trim().ToString());
        }

        string type = Request.QueryString["type"] + "";
        if (type == "")
        {
            this.Ostate1.Attributes.Add("style", "display:block;");
            this.Ostate2.Attributes.Add("style", "display:block;");
            strwhere += "and CompID=" + this.CompID + " and (ReturnState>" + (int)Enums.AuditState.未提交 + " or ReturnState=" + (int)Enums.AuditState.退回 + ") and isnull(dr,0)=0"; //IsDel=1  订单已删除
        }
        else
        {
            this.Ostate1.Attributes.Add("style", "display:none;");
            this.Ostate2.Attributes.Add("style", "display:none;");
            strwhere += "and CompID=" + this.CompID + " and ReturnState>=" + (int)Enums.AuditState.已审 + " and OrderID in (select Id from DIS_Order where PayState=" + (int)Enums.PayState.已支付 + ") and isnull(dr,0)=0"; //IsDel=1  订单已删除
        }

        if (this.txtPager.Value.Trim().ToString() != "" && this.txtPager.Value.Trim().ToString() != "0")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPager.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }
        }

        List<Hi.Model.DIS_OrderReturn> l = OrderReturnBll.GetList(Pager.PageSize, Pager.CurrentPageIndex, "ReturnDate", false, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = l;
        this.rptOrder.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Common.NoHTML( Pager.CurrentPageIndex.ToString());
        ViewState["strwhere"] = Where();
        Bind();
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {

        ViewState["strwhere"] = Where();
        Bind();
    }

    public string Where() {
        string strWhere = string.Empty;

        if (this.txtReceiptNo.Value != "")
        {
            strWhere += "and ReceiptNo like '%" +Common.NoHTML( this.txtReceiptNo.Value.Trim().ToString().Replace("'", "''")) + "%'";
        }
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            strWhere += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" +Common.NoHTML( txtDisName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (this.ddrState.Value != "-2")
        {
            strWhere += " and ReturnState=" +Common.NoHTML( this.ddrState.Value.Trim().ToString());
        }
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and CreateDate>='" +this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            strWhere += "and CreateDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }
        if (this.txtAuditDate.Value != "")
        {
            strWhere += " and AuditDate>='" + this.txtAuditDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndAuditDate.Value != "")
        {
            strWhere += "and AuditDate<'" + this.txtEndAuditDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }
        return strWhere;
    }
}