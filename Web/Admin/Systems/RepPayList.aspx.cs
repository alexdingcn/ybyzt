using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Admin_Systems_RepPayList : AdminPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;
    public decimal tb = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        string Action = Request["Action"] + "";
        string OrgID = Request["OrgID"] + "";
        if (Action == "Action")
        {
            Response.Write(Common.getsaleman(OrgID));
            Response.End();
        }
        if (!IsPostBack)
        {
            //Common.BindOrgSale(Org, SaleMan, "全部");
            this.txtPager.Value = Common.PageSize;
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string strwhere = string.Empty;

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }

        if (this.txtPager.Value.Trim().ToString() != "")
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
        string sql = @"select bd_distributor.DisName '付款方', PAY_Payment.PayPrice '付款金额' ,PAY_Payment.guid '交易流水号',
dis_order.ReceiptNo '订单编号',
                        case
                         when Channel=1 then '快捷支付'  
                          when Channel=2 then '银联支付'  
                        when Channel=3 then '网银支付' 
                          when Channel=4 then 'B2B网银支付' end  as '付款方式',PayDate '支付时间',  
                           AccountName '收款方',bankcode '收款方帐号',PAY_PayLog.CreateDate '清算时间'
,case when PAY_PayLog.MarkName='40' then '成功'  when PAY_PayLog.MarkName='50' then '回冲'   when PAY_PayLog.MarkName='9999' then '已处理'  else '失败' end as '清算状态'
                            from PAY_Payment
                        join  BD_Distributor  on bd_distributor.id=pay_payment.disid 
                        join dis_order on dis_order.id=pay_payment.orderid 
                        join PAY_PayLog on PAY_PayLog.number=pay_payment.guid and Start=2000
                        where PAY_Payment.isaudit=1" + strwhere + " order by PayDate desc";

        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);

        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        for (int i = 0; i < ds.Rows.Count; i++)
        {
            ta += Convert.ToDecimal(ds.Rows[i]["付款金额"].ToString() == "" ? "0" : ds.Rows[i]["付款金额"].ToString());
            //tb += Convert.ToDecimal(((Convert.ToDecimal(ds.Rows[i]["AuditAmount"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount"].ToString()) - Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(ds.Rows[i]["DisID"].ToString().ToInt(0), Convert.ToInt32(ds.Rows[i]["CompID"].ToString())))) > 0 ? (Convert.ToDecimal(ds.Rows[i]["AuditAmount"].ToString() == "" ? "0" : ds.Rows[i]["AuditAmount"].ToString()) - Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(ds.Rows[i]["DisID"].ToString().ToInt(0), Convert.ToInt32(ds.Rows[i]["CompID"].ToString())))).ToString() : "0").ToString());
        }
        this.rptOrder.DataSource = dt;
        this.rptOrder.DataBind();

        page = Pager.CurrentPageIndex.ToString();
       // this.Org.SelectedValue = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        ViewState["strwhere"] = Where();
        Bind();
    }

    public string GetMess(string name,string guid) 
    {
        string str = string.Empty;
        if (name == "成功")
        {
            str = "成功";
        }
        else if (name == "已处理")
        {
            str = string.Format("<a style=' text-decoration:underline;color:green' href='javascript:void(0)' onclick='GoChuLi(\"{0}\",1)'>已处理</a>", guid);
       
        }
        else 
        {
            str = string.Format("<span style='color:red'>失败</span><a style=' text-decoration:underline;' href='javascript:void(0)' onclick='GoChuLi(\"{0}\")'>处理</a>", guid);
        }
        return str;
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
    protected string Where()
    {
        string strWhere = string.Empty;

        //if (this.txtDisName.Value.Trim() != "")
        //{
        //    string id = "0";
        //    string sql = " select * from BD_Distributor where ISNULL(dr,0)=0 and DisName like '%" + this.txtDisName.Value.Trim().ToString().Replace("'", "''") + "%'";
        //    DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql);
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in ds.Tables[0].Rows)
        //        {
        //            id += "," + dr["ID"].ToString();
        //        }
        //    }
        //    strWhere += " and DisID in (" + id + ") ";
        //}
        if (this.txtCreateDate.Value.Trim() != "")
        {
            strWhere += " and PayDate>='" + Convert.ToDateTime(this.txtCreateDate.Value.Trim()) + "'";
        }
        if (this.txtCreateDate1.Value.Trim() != "")
        {
            strWhere += " and PayDate<'" + Convert.ToDateTime(this.txtCreateDate1.Value.Trim()).AddDays(1) + "'";
        }
        //if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        //{
        //    strWhere += " and exists(select 1 from BD_Company  where id=CompID and  CompName like '%" + txtCompName.Value.Trim().Replace("'", "''") + "%')";
        //}
        //if (Org.SelectedValue != "-1")
        //{
        //    string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
        //    strWhere += " and exists(select 1 from BD_Company  where id=CompID and OrgID='" + org + "')";
        //}
        if (salemanid.Value != "-1" && salemanid.Value != "")
        {
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and SalesManID='" + Common.NoHTML(salemanid.Value) + "')";
        }
        if (SalesManID > 0 || OrgID > 0)
        {
            string whereIn = string.Empty;
            if (OrgID > 0)
            {
                whereIn += "  and OrgID=" + OrgID + "";
            }
            if (SalesManID > 0)
            {
                whereIn += " and SalesManID=" + SalesManID + "";
            }
            strWhere += " and exists(select 1 from BD_Company  where id=CompID and "+ whereIn + ")";
        }
        return strWhere;
    }
}