using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DBUtility;
using System.IO;
using System.Web.UI.HtmlControls;

public partial class Company_Report_CompCollection : CompPageBase
{
    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public decimal ta = 0;
    public int paymentid = 0;//支付记录ID

    public int pretype = 0;//支付类型

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string CreateDate = "0";
            if (Common.GetCompService(CompID.ToString(), out CreateDate) == "0")
            {
                Response.Redirect("../SysManager/Service.aspx", true);
            }
            this.txtPager.Value = Common.PageSize;

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
            //修改支付流程新增详情页面   start
            if (Request["Paymnetid"] != null)
                paymentid = Convert.ToInt32(Common.DesDecrypt(Request["Paymnetid"].ToString(), Common.EncryptKey));

            if (Request["pretype"] != null)
                pretype = Convert.ToInt32(Common.DesDecrypt(Request["pretype"].ToString(), Common.EncryptKey));
            //修改支付流程新增详情页面   end

            //不排除管理员的权限验证
            if (!Common.HasRightAll(this.CompID, this.UserID, "1030"))
                this.DelAll.Visible = false;

            ViewState["strwhere"] = Where();
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
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

        string sql = string.Empty;
        if (paymentid > 0)
        {
            sql = "SELECT case sxf when '' then null else sxf end as sxf1,isnull(Price,0) as Price1,* FROM [dbo].[CompCollection_view] where pretype=" + pretype + " and  paymentid=" + paymentid + " and status=1 order by Date  desc";
        }
        else
            sql = "SELECT case sxf when '' then null else sxf end as sxf1,isnull(Price,0) as Price1,* FROM [dbo].[CompCollection_view] where OrderID in(select ID from Dis_Order where ISNULL(dr,0)=0 and  (Otype<>9) and CompID=" + this.CompID + ")   and status=1 and CompID=" + this.CompID + strwhere 
            + " order by Date  desc";

        Pagger pagger = new Pagger(sql);

        Pager.RecordCount = pagger.GetDataCount(sql);

        DataTable dt = pagger.getData(Pager.PageSize, Pager.StartRecordIndex - 1);
        this.rptOrder.DataSource = dt;
        this.rptOrder.DataBind();

        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page =Common.NoHTML( Pager.CurrentPageIndex.ToString());
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
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim()))
        {
            strWhere += " and disid  in (select id from BD_Distributor  where ISNULL(dr,0)=0 and CompID=" + this.CompID + "  and DisName like '%" +Common.NoHTML( txtDisName.Value.Trim().Replace("'", "")) + "%')";
        }
        if (this.txtCreateDate.Value.Trim() != "")
        {
            strWhere += " and Date>='" + Convert.ToDateTime(this.txtCreateDate.Value.Trim()) + "'";
        }
        if (this.txtECreateDate.Value.Trim() != "")
        {
            strWhere += " and Date<'" + Convert.ToDateTime(this.txtECreateDate.Value.Trim()).AddDays(1) + "'";
        }
        if (this.ddrPayType.Value.Trim() != "-1")
        {
            strWhere += " and Source='" + GetUserType(this.ddrPayType.Value.Trim()) + "'";
        }
        if (this.orderid.Value.Trim() != "")
        {
            strWhere += " and orderID='" + GetOrderID(this.orderid.Value.Trim().Replace("'","")) + "'";
        }
        return strWhere;        
    }

    public string GetStr(string str)
    {
        if (str.Length == 0)
        {
            str = "无";
        }
        else if (str.Length >7)
        {
            str = str.Substring(0, 7) + "...";
        }
        return str;
    }

    public string GetUserType(string str)
    {
        switch (str)
        {
            case "1":
                str = "快捷支付";
                break;
            case "2":
                str = "银联支付";
                break;
            case "3":
                str = "网银支付";
                break;
            case "4":
                str = "B2B网银支付";
                break;
            case "5":
                str = "线下支付";
                break;
            case "6":
                str = "支付宝支付";
                break;
            case "7":
                str = "微信支付";
                break;
            case "8":
                str = "企业钱包支付";
                break;
        }
        return str;
    }

    public string GetBankName(string BankID)
    {
        if (BankID != "")
            return new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(BankID);
        return "预收款收款";
    }
    public string GetOrderID(string Num)
    {
        List<Hi.Model.DIS_Order> order = new Hi.BLL.DIS_Order().GetList("", "ISNULL(dr,0)=0 and compid=" + CompID + " and ReceiptNo='" + Num + "'", "");
        if (order.Count > 0)
        {
            return order[0].ID.ToString();
        }
        else
        {
            return "";
        }
    }
}