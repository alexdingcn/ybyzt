using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DBUtility;
using LitJson;

/// <summary>
/// 企业首页统计数据
/// </summary>
public class jsc
{
	public jsc()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    //销售订单
    Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
    public int NotCount = 0;  //待审核订单个数
    public int DeliveryCount = 0;  //待发货订单个数
    public int DayOrderCount = 0;    //当天订单数
    public int OrderCount = 0;    //本月订单数
    public int PayOrder = 0;    //待收款订单

    //退货订单
    Hi.BLL.DIS_OrderReturn OrderReturnBll = new Hi.BLL.DIS_OrderReturn();
    public int ReturnCount = 0;  //退货待审订单个数
    public int ReturnMoney = 0;  //退款订单个数

    //预收款审核
    Hi.BLL.PAY_PrePayment PaymentBLL = new Hi.BLL.PAY_PrePayment();
    public int PaymentCount = 0;  //企业钱包审核个数

    //经销商审核
    Hi.BLL.BD_Distributor disBll = new Hi.BLL.BD_Distributor();
    public int disCount = 0;  //经销商待审核个数
    public int CountSum = 0;  //总经销商总个数
    public int CountNew = 0;  //新增经销商个数

    //留言回复
    Hi.BLL.DIS_Suggest SuggestBll = new Hi.BLL.DIS_Suggest();
    public int SuggectCount = 0;  //待回复留言

    public double sum = 0.00D;
    //当日收款
    public string dayPaggerSum = "0";
    //当日销售额
    public string DaySum = "0";
    //本月收款
    public string paggerSum = "0";
    //本月应收
    public string ArrearageSum = "0";
    //本月销售额
    public string MonthSum = "0";

    public Hi.Model.SYS_Users user = null;
    Hi.Model.BD_Company comp = null;
    Hi.Model.BD_Distributor dis = null;

    public string billskSum = string.Empty; //本月账单收款
    public string billys = string.Empty; //本月账单应收

    /// <summary>
    /// 企业统计数据
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultJSC GetCompNum(string JSon)
    {
        try
        {
            #region JSon取值

            string UserID = string.Empty;
            string CompID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["CompID"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompID"].ToString();
            }
            else
            {
                return new ResultJSC() {Result = "F", Description = "参数异常"};
            }
            
            if (!new Common().IsLegitUser(int.Parse(UserID), out user, int.Parse(CompID)))
                return new ResultJSC() { Result = "F", Description = "未找到用户信息" };

            comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(CompID));
            if (comp == null)
                return new ResultJSC() {Result = "F", Description = "未找到企业信息"};


            #endregion

            //获取当前时间
            DateTime date = DateTime.Now;
            //当天的0点0分
            DateTime day0 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
            //当月第一天
            DateTime day1 = new DateTime(date.Year, date.Month, 1);
            //获取当前时间加一天
            DateTime Sday = date.AddDays(1);

            #region 销售订单

            //销售订单
            List<Hi.Model.DIS_Order> orderl = OrderBll.GetList("", " isnull(dr,0)=0 and Otype!=9 and CompID=" + comp.ID,
                "");
            if (orderl != null)
            {
                if (orderl.Count > 0)
                {
                    //待审核订单个数
                    NotCount = orderl.FindAll(p => p.OState == (int) Enums.OrderState.待审核).Count;

                    //待发货订单个数
                    DeliveryCount =
                        orderl.FindAll(
                            p =>
                                p.OState == (int) Enums.OrderState.已审 && p.ReturnState == (int) Enums.ReturnState.未退货 &&
                                (p.PayState == (int) Enums.PayState.已支付 || p.PayState == (int) Enums.PayState.部分支付 ||
                                 (p.PayState == (int) Enums.PayState.未支付 && p.Otype == (int) Enums.OType.赊销订单))).Count;

                    //当日订单数
                    DayOrderCount = orderl.FindAll(order =>
                        (order.ReturnState == (int) Enums.ReturnState.未退货 &&
                         order.OState >= (int) Enums.OrderState.待审核 &&
                         order.CreateDate >= day0)
                        ).Count;

                    //本月订单数
                    OrderCount =
                        orderl.FindAll(
                            p =>
                                p.ReturnState == (int) Enums.ReturnState.未退货 &&
                                (p.OState == (int) Enums.OrderState.已审 || p.OState == (int) Enums.OrderState.已发货 ||
                                 p.OState == (int) Enums.OrderState.已到货) && p.CreateDate >= day1 && p.CreateDate <= Sday)
                            .Count;
                    //待收款订单
                    //PayOrder = orderl.FindAll(p => (p.OState == (int)Enums.OrderState.已审 || p.OState == (int)Enums.OrderState.已到货 || p.OState == (int)Enums.OrderState.已发货) && p.PayState == (int)Enums.PayState.未支付).Count;

                    PayOrder =
                        orderl.FindAll(
                            p =>
                                ((p.OState == (int) Enums.OrderState.已审 || p.OState == (int) Enums.OrderState.已到货 ||
                                  p.OState == (int) Enums.OrderState.已发货) &&
                                 (p.PayState == (int) Enums.PayState.未支付 || p.PayState == (int) Enums.PayState.部分支付) &&
                                 p.Otype == (int) Enums.OType.赊销订单) ||
                                ((p.OState == (int) Enums.OrderState.已审 || p.OState == (int) Enums.OrderState.已到货 ||
                                  p.OState == (int) Enums.OrderState.已发货) &&
                                 (p.PayState == (int) Enums.PayState.未支付 || p.PayState == (int) Enums.PayState.部分支付) &&
                                 p.Otype != (int) Enums.OType.赊销订单)).Count;
                }
            }

            #endregion

            #region 退货订单

            List<Hi.Model.DIS_OrderReturn> rOl = OrderReturnBll.GetList("", " isnull(dr,0)=0 and CompID=" + comp.ID,
                "");
            if (rOl != null)
            {
                if (rOl.Count > 0)
                {
                    //退货待审订单个数
                    ReturnCount = rOl.FindAll(R => R.ReturnState == 1).Count;
                    ReturnMoney = rOl.FindAll(R => R.ReturnState == 2).Count;
                }
            }

            #endregion

            #region 企业钱包审核

            List<Hi.Model.PAY_PrePayment> Exa = PaymentBLL.GetList("", " isnull(dr,0)=0 and CompID=" + comp.ID, "");
            if (Exa != null)
            {
                if (Exa.Count > 0)
                {
                    PaymentCount = Exa.FindAll(E => E.AuditState == 0 && E.Start == 1).Count;
                }
            }

            #endregion

            #region 经销商审核

            List<Hi.Model.BD_Distributor> disList = disBll.GetList("", " isnull(dr,0)=0 and CompID=" + comp.ID, "");

            if (disList != null)
            {
                if (disList.Count > 0)
                {
                    //经销商待审核个数
                    disCount = disList.FindAll(D => D.AuditState == 0).Count;

                    //总经销商总个数
                    CountSum = disList.Count;

                    //新增经销商个数
                    CountNew =
                        disList.FindAll(D => D.CreateDate >= day1 && D.CreateDate <= Sday && D.AuditState == 2).Count;
                }
            }

            #endregion

            #region 当天收款  当天销售额

            //收款
            string strDay =
                "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID not in(select ID from Dis_Order where ISNULL(dr,0)=0 and Otype=9 and CompID=" +
                comp.ID + ") and CompID=" + comp.ID +
                " and Date>='" + day0 + "' and Date<='" + Sday + "'";

            DataTable dtDay = SqlHelper.Query(SqlHelper.LocalSqlServer, strDay).Tables[0];
            if (dtDay != null)
            {
                if (dtDay.Rows.Count > 0)
                {
                    decimal Price = dtDay.Rows[0]["Price"].ToString() == ""
                        ? sum.ToString().ToDecimal()
                        : Convert.ToDecimal(dtDay.Rows[0]["Price"]);
                    dayPaggerSum = (Price/10000).ToString();
                }
            }

            //销售额
            string daysql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where CompID=" +
                            comp.ID + " and CreateDate>='" + day0 + "' and CreateDate<='" + Sday +
                            "' and OState in (2,4,5)";

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

            #region 本月收款  本月应收  本月销售额

            //本月收款
            string paggersql =
                "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID not in(select ID from Dis_Order where ISNULL(dr,0)=0 and Otype=9 and CompID=" +
                comp.ID + ") and CompID=" + comp.ID +
                " and Date>='" + day1 + "' and Date<='" + Sday + "'";

            DataTable paggerdt = SqlHelper.Query(SqlHelper.LocalSqlServer, paggersql).Tables[0];
            if (paggerdt != null)
            {
                if (paggerdt.Rows.Count > 0)
                {
                    decimal Price = paggerdt.Rows[0]["Price"].ToString() == ""
                        ? sum.ToString().ToDecimal()
                        : Convert.ToDecimal(paggerdt.Rows[0]["Price"]);
                    paggerSum = (Price/10000).ToString();
                }
            }

            //本月应收
            string ArrearageSql =
                "SELECT SUM(AuditAmount-PayedAmount) as AuditAmount FROM [dbo].[ArrearageRpt_view] where Otype!=9 and CompID=" +
                comp.ID +
                " and CreateDate>='" + day1 + "' and CreateDate<='" + Sday + "'";

            DataTable ArrearageDt = SqlHelper.Query(SqlHelper.LocalSqlServer, ArrearageSql).Tables[0];
            if (ArrearageDt != null)
            {
                if (ArrearageDt.Rows.Count > 0)
                {
                    decimal AuditAmount = ArrearageDt.Rows[0]["AuditAmount"].ToString() == ""
                        ? sum.ToString().ToDecimal()
                        : Convert.ToDecimal(ArrearageDt.Rows[0]["AuditAmount"]);
                    ArrearageSum = (AuditAmount/10000).ToString();
                }
            }

            //本月销售额
            string monthsql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where  CompID=" +
                              comp.ID + " and CreateDate>='" + day1 + "' and CreateDate<='" + Sday +
                              "' and OState in(2,4,5)";

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

            #region 待回复留言

            List<Hi.Model.DIS_Suggest> LDis = new Hi.BLL.DIS_Suggest().GetList("",
                " isnull(dr,0)=0 and compid=" + comp.ID, "");

            if (LDis.Count > 0)
            {
                SuggectCount = LDis.FindAll(S => S.IsAnswer == 0).Count;
            }

            #endregion

            #region 本月账单收款，本月账单应收

            //账单收款
            string billsksql = string.Format(@"SELECT sum(Price)  as price FROM [dbo].[CompCollection_view] 
                             where OrderID  in(select ID from Dis_Order 
                             where ISNULL(dr,0)=0 and Otype=9 and CompID={0}) and CompID={0}", comp.ID);

            DataTable billskdt = SqlHelper.Query(SqlHelper.LocalSqlServer, billsksql).Tables[0];
            if (billskdt != null)
            {
                if (billskdt.Rows.Count > 0)
                {
                    decimal Price = billskdt.Rows[0]["Price"].ToString() == ""
                        ? sum.ToString().ToDecimal()
                        : Convert.ToDecimal(billskdt.Rows[0]["Price"]);
                    billskSum = (Price/10000).ToString("N");
                }
            }
            //账单应收
            string billyssql =
                string.Format(
                    @"select sum(AuditAmount-PayedAmount) AuditAmount from(select * from [dbo].[ArrearageRpt_view] where  CompID={0} and Otype=9 )M  where compID={0}",
                    comp.ID);
            DataTable billysdt = SqlHelper.Query(SqlHelper.LocalSqlServer, billyssql).Tables[0];
            if (billysdt != null)
            {
                if (billysdt.Rows.Count > 0)
                {
                    decimal Price = billysdt.Rows[0]["AuditAmount"].ToString() == ""
                        ? sum.ToString().ToDecimal()
                        : Convert.ToDecimal(billysdt.Rows[0]["AuditAmount"]);
                    billys = (Price/10000).ToString("N");
                }
            }

            #endregion

            return new ResultJSC()
            {
                Result = "T",
                Description = "返回正确",
                MonthSum = MonthSum,
                OrderCount = OrderCount.ToString(),
                PaggerSum = paggerSum,
                ArrearageSum = ArrearageSum,
                CountSum = CountSum.ToString(),
                CountNew = CountNew.ToString(),
                DayPaggerSum = dayPaggerSum,
                DaySum = DaySum,
                DayCount = DayOrderCount.ToString(),
                billskSum = billskSum,
                billys = billys
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCompNum：" + JSon);
            return null;
        }
    }

    #region 返回

    /// <summary>
    /// 企业
    /// </summary>
    public class ResultJSC
    {
        public String Result { get; set; }
        public String Description { get; set; }

        public String DaySum { get; set; }//当天销售额
        public String MonthSum { get; set; }//本月销售额

        public String DayCount { get; set; }//当天订单数
        public String OrderCount { get; set; }//本月订单数

        public String DayPaggerSum { get; set; }//当天收款
        public String PaggerSum { get; set; }//本月收款

        public String ArrearageSum { get; set; }//本月应收
        public String CountSum { get; set; }//总经销商总个数
        public String CountNew { get; set; }//新增经销商个数

        public string billskSum { get; set; } //本月账单收款
        public string billys { get; set; }//本月账单应收
    }

    #endregion
}