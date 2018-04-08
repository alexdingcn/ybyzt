using System;
using System.Activities.Debugger;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;

public partial class Company_Report_CompOrderRpt : CompPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;
    public decimal tb = 0;
    public int pageCount = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        page = Request["page"] + "";
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";

            if (Request.QueryString["type"] == null)
            {

                this.txtCreateDate.Value = DateTime.Now.AddDays(1 - DateTime.Now.Day).ToString("yyyy-MM-dd");
                this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            }
            else
            {
                if (Request.QueryString["type"] + "" == "1")
                {
                    this.txtCreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                    this.txtECreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
                }
            }
            ViewState["strwhere"] = Where();
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        int Counts = 0;
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
        strwhere += "and Otype!=9 and isnull(dr,0)=0 and OState in(2,4,5) and compID=" + this.CompID;

        List<Hi.Model.DIS_Order> l = OrderInfoBLL.GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);
        List<Hi.Model.DIS_Order> dis = OrderInfoBLL.GetList("", "Otype!=9 and isnull(dr,0)=0 and OState in(2,4,5) " + strwhere, "");
        for (int i = 0; i < dis.Count; i++)
        {
            ta += dis[i].AuditAmount;
            tb += dis[i].PayedAmount;
        }
        this.rptOrder.DataSource = l;
        this.rptOrder.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
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

        if (this.txtDisName.Value.Trim() != "")
        {
            string id = "0";
            string sql = " select * from BD_Distributor where ISNULL(dr,0)=0 and DisName like '%" + this.txtDisName.Value.Trim().ToString().Replace("'", "''") + "%'";
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer,sql);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    id += "," + dr["ID"].ToString();
                }
            }
            strWhere += " and DisID in (" + id + ") ";
        }
        if (this.ddrPayState.Value != "-1")
        {
            strWhere += " and PayState=" + this.ddrPayState.Value;
        }
        if (this.txtReceiptNo.Value.Trim() != "")
        {
            strWhere += " and ReceiptNo like '%" + this.txtReceiptNo.Value.Trim().ToString().Replace("'", "''") + "%'";
        }
        if (this.txtTotalAmount1.Value.Trim() != "")
        {
            strWhere += " and TotalAmount>=" + this.txtTotalAmount1.Value.Trim();
        }
        if (this.txtTotalAmount2.Value.Trim() != "")
        {
            strWhere += " and TotalAmount<" + this.txtTotalAmount2.Value.Trim();
        }
        if (this.txtCreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate>='" + Convert.ToDateTime(this.txtCreateDate.Value.Trim()) + "'";
        }
        if (this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and CreateDate<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        if (ddrOState.Value != "-2")
        {
            strWhere += " and OState=" + ddrOState.Value.Trim();
        }
        //if (ddrOState.Value == "9")
        //{
        //    strWhere += " and OState=" + (int)Enums.OrderState.已到货 + " and ReturnState not in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
        //}
        //if (ddrOState.Value == "5")
        //{
        //    strWhere += " and OState=" + (int)Enums.OrderState.已到货 + " and ReturnState in (" + (int)Enums.ReturnState.未退货 + "," + (int)Enums.ReturnState.新增退货 + ")";
        //}
        return strWhere;
    }

    /// <summary>
    /// 支付状态
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected string GetPayState(string id)
    {
        string payState = id;
        string state = string.Empty;
        switch (payState)
        { 
            case "1":
                state = "部分支付";
                break;
            case "2":
                state = "已支付";
                break;
            case "0":
                state = "未支付";
                break;
        }
        return state;
    }

    /// <summary>
    /// 支付状态
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    protected string GetOState(string id)
    {
        string payState = Common.GetDis(id.ToInt(0), "OState");
        string state = string.Empty;
        switch (payState)
        {
            case "0":
                state = "";
                break;
            case "1":
                state = "";
                break;
            case "2":
                state = "";
                break;
        }
        return state;
    }
}