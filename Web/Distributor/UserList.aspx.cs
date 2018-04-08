using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Distributor_UserList : DisPageBase
{
    public string price =string.Empty;            //企业钱包金额
    public Hi.Model.SYS_Users user = null;
    public string page = "1";//默认初始页
    //进货订单
    Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
    public int payCount = 0;  //待支付订单数
    public int ReceiveCount = 0; //待收货订单数
    public int OrderCount = 0;  //本月订单数
    
    public double sum = 0.00D;

    public string MonthSum = string.Empty;  //本月订购额
    public string PaymentSum = string.Empty; //本月付款额
    public string PayableSum = string.Empty;  //本月应款额

    protected void Page_Load(object sender, EventArgs e)
    {
        user = new Hi.BLL.SYS_Users().GetModel(this.UserID);
        price = Convert.ToDecimal(new Hi.BLL.PAY_PrePayment().sums(user.DisID, user.CompID)).ToString("0.00");//(decimal)dt.Rows[0]["DisAccount"];
        if (user.IsFirst != 2)
        {
            if (user.IsFirst == 0)
            {
                if (user.UserPwd == Util.md5("123456"))
                {
                    JScript.AlertMethod(this, "经检测，您是第一次登录，为了您的账户安全，请先修改登录密码和支付密码！", JScript.IconOption.笑脸, "function (){ location.href = 'UserPWDEdit.aspx'; }");
                    return;
                }
                else
                {
                    user.IsFirst = 1;
                    user.modifyuser = user.ID;
                    user.ts = DateTime.Now;
                    new Hi.BLL.SYS_Users().Update(user);
                    JScript.AlertMethod(this, "经检测，您是第一次登录，为了您的账户安全，请修改支付密码！", JScript.IconOption.笑脸, "function (){ location.href = 'PayPWDEdit.aspx'; }");
                    return;
                }

            }
        }
        if (!IsPostBack) 
        {
            InBind();
            Bind();
        }
    }

    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }


    public void InBind()
    {
        //获取当前时间
        DateTime date = DateTime.Now;
        //当月第一天
        DateTime day1 = new DateTime(date.Year, date.Month, 1);
        //获取当前时间加一天
        DateTime Sday = date.AddDays(1);

        #region 进货订单
        List<Hi.Model.DIS_Order> orderl = new Hi.BLL.DIS_Order().GetList("", " isnull(dr,0)=0 and Otype!=9 and isnull(ReturnState,0)=0 and CompID=" + user.CompID + " and DisID=" + user.DisID, "");
        if (orderl != null)
        {
            if (orderl.Count > 0)
            {
                //待支付订单
                payCount = orderl.FindAll(p => ((p.OState == (int)Enums.OrderState.已审 || p.OState == (int)Enums.OrderState.已到货 || p.OState == (int)Enums.OrderState.已发货) && p.PayState == (int)Enums.PayState.未支付 && p.Otype == (int)Enums.OType.赊销订单) || (p.OState == (int)Enums.OrderState.已审 && p.PayState == (int)Enums.PayState.未支付 && p.Otype != (int)Enums.OType.赊销订单)).Count;
                //待支付订单
                payCount = orderl.FindAll(p => (p.OState == (int)Enums.OrderState.已审 || p.OState == (int)Enums.OrderState.已到货 || p.OState == (int)Enums.OrderState.已发货) && p.PayState == (int)Enums.PayState.未支付).Count;

                //待收货订单
                ReceiveCount = orderl.FindAll(p => p.OState == (int)Enums.OrderState.已发货).Count;
                //本月订单数
                OrderCount = orderl.FindAll(p => p.CreateDate >= day1 && p.CreateDate <= Sday).Count;
            }
        }
        #endregion

        #region  本月订购额  本月付款额   本月应付额
        //本月订购额
        string monthsql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where DisID=" + user.DisID + " and CompID=" + user.CompID + " and CreateDate>='" + day1 + "' and CreateDate<='" + Sday + "'";

        DataTable monthDt = SqlHelper.Query(SqlHelper.LocalSqlServer, monthsql).Tables[0];
        if (monthDt != null)
        {
            if (monthDt.Rows.Count > 0)
            {
                decimal sumAmount = monthDt.Rows[0]["sumAmount"].ToString() == "" ? sum.ToString().ToDecimal() : Convert.ToDecimal(monthDt.Rows[0]["sumAmount"]);
                MonthSum = (sumAmount).ToString("N");
            }
        }

        //本月付款额
        string paggersql = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where DisID=" + user.DisID + "and CompID=" + user.CompID + " and Date>='" + day1 + "' and Date<='" + Sday + "'";

        DataTable paggerdt = SqlHelper.Query(SqlHelper.LocalSqlServer, paggersql).Tables[0];
        if (paggerdt != null)
        {
            if (paggerdt.Rows.Count > 0)
            {
                decimal Price = paggerdt.Rows[0]["Price"].ToString() == "" ? sum.ToString().ToDecimal() : Convert.ToDecimal(paggerdt.Rows[0]["Price"]);
                PaymentSum = (Price).ToString("N");
            }
        }

        //本月应付额
        string ArrearageSql = "SELECT SUM(AuditAmount) as AuditAmount FROM [dbo].[ArrearageRpt_view] where DisID=" + user.DisID + "and CompID=" + user.CompID + " and CreateDate>='" + day1 + "' and CreateDate<='" + Sday + "'";

        DataTable ArrearageDt = SqlHelper.Query(SqlHelper.LocalSqlServer, ArrearageSql).Tables[0];
        if (ArrearageDt != null)
        {
            if (ArrearageDt.Rows.Count > 0)
            {
                decimal AuditAmount = ArrearageDt.Rows[0]["AuditAmount"].ToString() == "" ? sum.ToString().ToDecimal() : Convert.ToDecimal(ArrearageDt.Rows[0]["AuditAmount"]);
                PayableSum = (AuditAmount).ToString("N");
            }
        }

        #endregion

    }

    /// <summary>
    /// 代理商列表显示
    /// </summary>
    public void Bind()
    {
        //查询录入明细
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        //查询全部手动录入冲正
        //strwhere += " and PreType=3";
        //有效数据显示
       // strwhere += " and IsEnabled = 1";
        //审核状态是已审的
        //strwhere += " and AuditState = 2";
        //付款状态是成功的
        strwhere += " and Start=1";

        int disId = this.DisID;
        if (disId != 0)
        {
            strwhere += " and PreType in (1,2,3,4,5)  and DisID=" + disId + "";
        }
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
       
            Pager.PageSize =10;

            List<Hi.Model.PAY_PrePayment> pay = new Hi.BLL.PAY_PrePayment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreatDate", true, strwhere, out pageCount, out Counts);

        this.rptDis.DataSource = pay;
        this.rptDis.DataBind();


        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();

    }

    #region  作废代码
    /// <summary>
    /// 根据不同的状态，显示不同的操作按钮
    /// </summary>
    /// <param name="state"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    //public string Getmessage(int state,int id)
    //{
    //    string str = string.Empty;
    //    if (state == 2 || state == 3)
    //    {
    //        str = string.Format("<a href=\'javascript:void(0)\' onclick=\'pay({0})\' class=\"btnBl\">付款</a>", id);
    //    }
    //    else if(state==1)
    //    {
    //        str = "已支付";
    //    }
    //    else if (state == 4)
    //    {
    //        str = "已结算";
    //    }

    //    return str;

    //}
    #endregion
}