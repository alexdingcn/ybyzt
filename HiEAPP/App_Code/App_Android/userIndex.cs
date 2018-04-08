using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DBUtility;
using LitJson;
using NPOI.SS.Formula.Functions;

/// <summary>
/// 经销商首页统计数据
/// </summary>
public class userIndex
{
	public userIndex()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public Hi.Model.SYS_Users user = null;
    public int payCount = 0;     //待支付订单数
    public int ReceiveCount = 0; //待收货订单数
    public int salesorder = 0;   //赊销订单
    public int message = 0;      //问题回复
    public string price = string.Empty;        //企业钱包金额
    public string disType = string.Empty;      //经销商分类
    public string disAreaID = string.Empty;    //经销商区域
    public int dayOrderCount = 0;    //当天订单数量
    public int orderCount = 0;    //本月订单数量
    public string DaySum = string.Empty;     //当天订购额
    public string DayPaymentSum = string.Empty;   //当天付款额
    public string MonthSum = string.Empty;     //本月订购额
    public string PaymentSum = string.Empty;   //本月付款额
    public string PayableSum = string.Empty;   //本月应款额
    public double sum = 0.00D;

    public Hi.Model.BD_Distributor dis = null;

    public ResultJSC GetDisNum(string JSon)
    {
        try
        {
            #region JSon取值

            string UserID = string.Empty;
            string DisID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["DisID"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                DisID = JInfo["DisID"].ToString();
            }
            else
            {
                return new ResultJSC() { Result = "F", Description = "参数异常" };
            }

            if (!new Common().IsLegitUser(int.Parse(UserID), out user, 0, int.Parse(DisID)))
                return new ResultJSC() { Result = "F", Description = "未找到用户信息" };
            
            dis = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(DisID));
            if (dis == null)
                return new ResultJSC() { Result = "F", Description = "未找到经销商信息" };

            #endregion

            //获取当前时间
            DateTime date = DateTime.Now;
            //当天0点0分
            DateTime day0 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            //当月第一天
            DateTime day1 = new DateTime(date.Year, date.Month, 1);
            //获取当前时间加一天
            DateTime Sday = date.AddDays(1);

            #region 本月订单

            List<Hi.Model.DIS_Order> orderl = new Hi.BLL.DIS_Order().GetList("",
                " isnull(dr,0)=0 and Otype!=9 and CompID=" + dis.CompID + " and DisID=" + dis.ID, "");
            if (orderl != null)
            {
                if (orderl.Count > 0)
                {
                    //待支付订单
                    string strwhere =
                        " (( Otype=1 and OState not in (-1,0,1)  and PayState in (0,1) ) or( Otype<>1 and OState in(2,4,5)   and PayState in (0,1) )) and OState<>6   and DisID='" +
                        dis.ID + "' and ReturnState in(0,1) and isnull(dr,0)=0";
                    List<Hi.Model.DIS_Order> ol = new Hi.BLL.DIS_Order().GetList("", strwhere, "");
                    if (ol != null && ol.Count > 0)
                        payCount = ol.Count;

                    //待收货订单
                    ReceiveCount = orderl.FindAll(p => p.OState == (int) Enums.OrderState.已发货).Count;

                    //赊销订单
                    salesorder = orderl.FindAll(p => p.Otype == (int) Enums.OType.赊销订单).Count;

                    //当天订单数
                    dayOrderCount =
                        orderl.FindAll(
                            p =>
                                p.ReturnState == (int)Enums.ReturnState.未退货 &&
                                p.CreateDate >= day0).Count;

                    //本月订单数
                    orderCount =
                        orderl.FindAll(
                            p =>
                                p.ReturnState == (int) Enums.ReturnState.未退货 &&
                                p.CreateDate >= Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM"))).Count;

                }
            }

            #endregion

            #region 当天订购额

            string daysql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where DisID=" +
                              dis.ID + " and CompID=" + dis.CompID + " and CreateDate>='" + day0 + "'";
            DataTable dayDt = SqlHelper.Query(SqlHelper.LocalSqlServer, daysql).Tables[0];
            if (dayDt != null)
            {
                if (dayDt.Rows.Count > 0)
                {
                    decimal sumAmount = dayDt.Rows[0]["sumAmount"].ToString() == ""
                        ? sum.ToString().ToDecimal()
                        : Convert.ToDecimal(dayDt.Rows[0]["sumAmount"]);
                    DaySum = (sumAmount/10000).ToString();
                }
            }

            #endregion

            #region 本月订购额

            string monthsql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where DisID=" +
                              dis.ID + " and CompID=" + dis.CompID + " and CreateDate>='" + day1 +
                              "' and CreateDate<='" + Sday + "'";
            DataTable monthDt = SqlHelper.Query(SqlHelper.LocalSqlServer, monthsql).Tables[0];
            if (monthDt != null)
            {
                if (monthDt.Rows.Count > 0)
                {
                    decimal sumAmount = monthDt.Rows[0]["sumAmount"].ToString() == ""
                        ? sum.ToString().ToDecimal()
                        : Convert.ToDecimal(monthDt.Rows[0]["sumAmount"]);
                    MonthSum = (sumAmount/10000).ToString();
                }
            }

            #endregion

            #region 当天付款额

            //付款额
            string daypaggersql = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where DisID=" + dis.ID +
                               "and CompID=" + dis.CompID + " and Date>='" + day0 + "' and Date<='" + Sday + "'";

            DataTable daypaggerdt = SqlHelper.Query(SqlHelper.LocalSqlServer, daypaggersql).Tables[0];
            if (daypaggerdt != null)
            {
                if (daypaggerdt.Rows.Count > 0)
                {
                    decimal Price = daypaggerdt.Rows[0]["Price"].ToString() == ""
                        ? sum.ToString().ToDecimal()
                        : Convert.ToDecimal(daypaggerdt.Rows[0]["Price"]);
                    DayPaymentSum = (Price/10000).ToString();
                }
            }

            #endregion

            #region 本月付款额

            //本月付款额
            string paggersql = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where DisID=" + dis.ID +
                               "and CompID=" + dis.CompID + " and Date>='" + day1 + "' and Date<='" + Sday + "'";

            DataTable paggerdt = SqlHelper.Query(SqlHelper.LocalSqlServer, paggersql).Tables[0];
            if (paggerdt != null)
            {
                if (paggerdt.Rows.Count > 0)
                {
                    decimal Price = paggerdt.Rows[0]["Price"].ToString() == ""
                        ? sum.ToString().ToDecimal()
                        : Convert.ToDecimal(paggerdt.Rows[0]["Price"]);
                    PaymentSum = (Price/10000).ToString();
                }
            }

            #endregion

            #region 本月应付额

            //本月应付额

            decimal AuditAmount = 0;
            decimal payAmount = 0;

            //赊销订单  未支付的
            //string ArrearageSql = "SELECT SUM(AuditAmount) as AuditAmount FROM [dbo].[ArrearageRpt_view] where DisID=" + user.DisID + "and CompID=" + user.CompID + " and CreateDate>='" + day1 + "' and CreateDate<='" + Sday + "'";

            //DataTable ArrearageDt = SqlHelper.Query(SqlHelper.LocalSqlServer, ArrearageSql).Tables[0];
            //if (ArrearageDt != null)
            //{
            //    if (ArrearageDt.Rows.Count > 0)
            //    {
            //        AuditAmount = ArrearageDt.Rows[0]["AuditAmount"].ToString() == "" ? sum.ToString().ToDecimal() : Convert.ToDecimal(ArrearageDt.Rows[0]["AuditAmount"]);
            //    }
            //}

            //未支付订单金额
            string paysql =
                "  select SUM(AuditAmount) as AuditAmount from DIS_Order where (( Otype=1 and OState not in (-1,0,1)  and PayState=0 ) or( Otype<>1 and OState=2   and PayState=0 )) and OState<>6 and ReturnState=0 and isnull(dr,0)=0 and CompID=" +
                dis.CompID + " and DisID=" + dis.ID + " and CreateDate>='" + day1 + "' and CreateDate<='" + Sday +
                "'";

            DataTable payDt = SqlHelper.Query(SqlHelper.LocalSqlServer, paysql).Tables[0];
            if (payDt != null && payDt.Rows.Count > 0)
            {
                payAmount = payDt.Rows[0]["AuditAmount"].ToString() == ""
                    ? 0
                    : payDt.Rows[0]["AuditAmount"].ToString().ToDecimal(0);
            }

            PayableSum = ((payAmount + AuditAmount)/10000).ToString();

            #endregion

            return new ResultJSC()
            {
                Result = "T", 
                Description = "返回正确",
                MonthSum = MonthSum,
                OrderCount = orderCount.ToString(),
                PaymentSum = PaymentSum,
                PayableSum = PayableSum,
                DayPaymentSum = DayPaymentSum,
                DaySum = DaySum,
                DayOrderCount = dayOrderCount.ToString()
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetDisNum:" + JSon);
            return new ResultJSC() { Result = "F", Description = "参数异常" };
        }
    }

    #region 返回

    /// <summary>
    /// 经销商
    /// </summary>
    public class ResultJSC
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String MonthSum { get; set; }//本月销售额
        public String OrderCount { get; set; }//本月订单数
        public String PaymentSum { get; set; }//本月付款额
        public String PayableSum { get; set; }//本月未付额
        public string DayPaymentSum { get; set; }//当天付额
        public string DaySum { get; set; }//当天销售额
        public String DayOrderCount { get; set; }//当天订单数
    }

    #endregion

}