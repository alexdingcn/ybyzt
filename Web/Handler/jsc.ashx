<%@ WebHandler Language="C#" Class="jsc" %>

using System;
using System.Web;
using System.Collections.Generic;
using System.Data;
using DBUtility;
using System.Web.SessionState;

public class jsc : IHttpHandler, IReadOnlySessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        context.Response.Write(Count(context));
        context.Response.End();
    }


    public string Count(HttpContext context)
    {
        ResultMsgJsc m = new ResultMsgJsc();
        double sum = 0.00D;
        try
        {
            //登录信息
            if (context.Session["UserModel"] is LoginModel)
            {
                LoginModel logUser = context.Session["UserModel"] as LoginModel;

                //获取当前时间
                DateTime date = DateTime.Now;
                //当天的0点0分
                DateTime day0 = new DateTime(date.Year, date.Month, date.Day, 0, 0, 0);
                //获取当前时间加一天
                DateTime Sday = day0.AddDays(1);

                //本周周一  
                DateTime startWeek = date.AddDays(1 - Convert.ToInt32(date.DayOfWeek.ToString("d")));
                startWeek = new DateTime(startWeek.Year, startWeek.Month, startWeek.Day, 0, 0, 0);
                //本周周末   周末多加一天  startWeek.AddDays(6)
                DateTime endWeek = startWeek.AddDays(7);

                //当月第一天
                DateTime day1 = new DateTime(date.Year, date.Month, 1);
                //本月  最后一天  多加一天
                DateTime mothday = day1.AddMonths(1);
                

                #region 销售订单
                Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
                List<Hi.Model.DIS_Order> orderl = OrderBll.GetList("", " isnull(dr,0)=0 and Otype!=9 and CompID=" + logUser.CompID, "");

                if (orderl != null && orderl.Count > 0)
                {
                    //待审核订单个数
                    m.NotCount = orderl.FindAll(p => p.OState == (int)Enums.OrderState.待审核).Count;
                    //待发货订单个数
                    m.DeliveryCount = orderl.FindAll(p => (p.OState == (int)Enums.OrderState.已审 || (p.OState == (int)Enums.OrderState.已发货 && (p.IsOutState == 0 || p.IsOutState == 1 || p.IsOutState == 2))) && (p.ReturnState == (int)Enums.ReturnState.未退货 || p.ReturnState == (int)Enums.ReturnState.新增退货)).Count;

                    //待收款订单
                    string strwhere = "1=1 and (( Otype=1 and OState not in (-1,0,1)  and PayState in(0,1) ) or( Otype<>1 and OState in (2,3,4,5) and PayState in(0,1))) and OState<>6   and CompID='" + logUser.CompID + "' and ReturnState=0 and Otype!=9 and isnull(dr,0)=0";
                    List<Hi.Model.DIS_Order> ol = new Hi.BLL.DIS_Order().GetList("", strwhere, "");
                    if (ol != null && ol.Count > 0)
                        m.PayOrder = ol.Count;
                   
                    
                    //当日订单数  2，3，4，5 状态  去掉待审核订单 order.OState == (int)Enums.OrderState.待审核 ||    不管退货状态 (order.ReturnState == (int)Enums.ReturnState.未退货 || order.ReturnState == (int)Enums.ReturnState.拒绝退货) &&
                    m.DayOrderCount = orderl.FindAll(order => ((order.OState == 2 || order.OState == 3 || order.OState == 4 || order.OState == 5 || order.OState == 7) &&
                                                         order.CreateDate >= day0 && order.CreateDate < Sday)).Count;
                    //本月订单数   去掉待审核订单 p.OState ==(int)Enums.OrderState.待审核 ||
                    m.OrderCount = orderl.FindAll(p => ((p.OState == 2 || p.OState == 3 || p.OState == 4 || p.OState == 5 || p.OState == 7) 
                        && p.CreateDate >= day1 && p.CreateDate < mothday)).Count;

                    //本周订单数   去掉待审核订单 p.OState == (int)Enums.OrderState.待审核 ||
                    m.WeekOrderCount = orderl.FindAll(p =>
                         (p.OState == 2 || p.OState == 3 || p.OState == 4 || p.OState == 5 || p.OState == 7) && p.CreateDate >= startWeek && p.CreateDate < endWeek).Count;

                }
                #endregion

                #region 退货订单
                Hi.BLL.DIS_OrderReturn OrderReturnBll = new Hi.BLL.DIS_OrderReturn();
                List<Hi.Model.DIS_OrderReturn> rOl = OrderReturnBll.GetList("", " isnull(dr,0)=0 and CompID=" + logUser.CompID, "");
                if (rOl != null)
                {
                    if (rOl.Count > 0)
                    {
                        //退货待审订单个数
                        //m.ReturnCount = rOl.FindAll(R => R.ReturnState == 1).Count;
                        m.ReturnMoney = rOl.FindAll(R => R.ReturnState == 2).Count;

                        m.ReturnCountDay = rOl.FindAll(R => R.AuditDate >= day0 && R.AuditDate < Sday && (R.ReturnState == 4)).Count;
                        m.ReturnCountWeek = rOl.FindAll(R => R.AuditDate >= startWeek && R.AuditDate < endWeek && (R.ReturnState == 4)).Count;
                        m.ReturnCountMonth = rOl.FindAll(R => R.AuditDate >= day1 && R.AuditDate < mothday && (R.ReturnState == 4)).Count;

                        //当天退款
                        List<Hi.Model.DIS_OrderReturn> rl = rOl.FindAll(R => R.AuditDate >= day0 && R.AuditDate < Sday && (R.ReturnState == 4));
                        decimal p = 0;
                        foreach (var item in rl)
                        {
                            List<Hi.Model.DIS_Order> rol = orderl.FindAll(S => S.ID == item.OrderID);
                            if (rol != null && rol.Count > 0)
                            {
                                foreach (var Pitem in rol)
                                {
                                    p += Convert.ToDecimal(Pitem.PayedAmount.ToString());
                                }
                            }
                        }
                        m.ReturnMoneyDay = p.ToString("N");

                        //本周退款
                        List<Hi.Model.DIS_OrderReturn> rwl = rOl.FindAll(R => R.AuditDate >= startWeek && R.AuditDate < endWeek && (R.ReturnState == 4));
                        decimal pw = 0;
                        foreach (var item in rwl)
                        {
                            List<Hi.Model.DIS_Order> rol = orderl.FindAll(S => S.ID == item.OrderID);
                            if (rol != null && rol.Count > 0)
                            {
                                foreach (var Pitem in rol)
                                {
                                    pw += Convert.ToDecimal(Pitem.PayedAmount.ToString());
                                }
                            }
                        }
                        m.ReturnMoneyWeek = pw.ToString("N");

                        //本月退款
                        List<Hi.Model.DIS_OrderReturn> rml = rOl.FindAll(R => R.AuditDate >= day1 && R.AuditDate < mothday && (R.ReturnState == 4));
                        decimal pm = 0;
                        foreach (var item in rml)
                        {
                            List<Hi.Model.DIS_Order> rol = orderl.FindAll(S => S.ID == item.OrderID);
                            if (rol != null && rol.Count > 0)
                            {
                                foreach (var Pitem in rol)
                                {
                                    pm += Convert.ToDecimal(Pitem.PayedAmount.ToString());
                                }
                            }
                        }
                        m.ReturnMoneyMonth = pm.ToString("N");
                    }
                }
                #endregion

                #region 代理商审核
                //Hi.BLL.BD_Distributor disBll = new Hi.BLL.BD_Distributor();
                //List<Hi.Model.BD_Distributor> dis = disBll.GetList("", " isnull(dr,0)=0 and CompID=" + logUser.CompID, "");

                //if (dis != null)
                //{
                //    if (dis.Count > 0)
                //    {
                //        //代理商待审核个数
                //        //m.disCount = dis.FindAll(D => D.AuditState == 0).Count;
                //        //总代理商总个数
                //        m.CountSum = dis.Count;
                //        //新增代理商个数
                //        //m.CountNew = dis.FindAll(D => D.CreateDate >= day1 && D.CreateDate < Sday && D.AuditState == 2).Count;
                //    }
                //}
                #endregion

                #region 商品信息
                List<Hi.Model.BD_GoodsInfo> goodsl = new Hi.BLL.BD_GoodsInfo().GetList("", " CompID=" + logUser.CompID + " and Isnull(dr,0)=0 and isnull(IsEnabled,0)=1", "");
                if (goodsl != null && goodsl.Count > 0)
                {
                    m.IsOffLineNO = goodsl.FindAll(p => p.IsOffline == 0).Count;
                    m.IsOffLineOk = goodsl.FindAll(p => p.IsOffline == 1).Count;
                }
                #endregion

                #region 促销信息
                //string prostr = "SELECT ID from BD_Goods where ID in (SELECT GoodsID from BD_PromotionDetail where ProID in(SELECT ID from BD_Promotion where compID=" + logUser.CompID + " and ProStartTime<=getdate() and ProEndTime >=getdate() and IsEnabled=1)) and compid= " + logUser.CompID + " and ISNULL(dr,0)=0 and IsEnabled = 1 and IsOffLine=1 ";
                //DataTable Prodt = SqlHelper.Query(SqlHelper.LocalSqlServer, prostr).Tables[0];
                //if (Prodt != null)
                //{
                //    m.proCount = Prodt.Rows.Count;
                //}
                #endregion

                #region 当天收款  当天销售额
                //收款    add by hgh  CompCollection_view 状态   and status!=3
                string strDay = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID in(select ID from Dis_Order where ISNULL(dr,0)=0 and (Otype<>9) and CompID=" + logUser.CompID + ")  and status!=3 and CompID=" + logUser.CompID +
                                   " and Date>='" + day0 + "' and Date<'" + Sday + "'  AND isnull(vedf9,0)=1 ";

                DataTable dtDay = SqlHelper.Query(SqlHelper.LocalSqlServer, strDay).Tables[0];
                if (dtDay != null)
                {
                    if (dtDay.Rows.Count > 0)
                    {
                        decimal Price = dtDay.Rows[0]["Price"].ToString() == ""
                            ? sum.ToString().ToDecimal()
                            : Convert.ToDecimal(dtDay.Rows[0]["Price"]);
                        m.dayPaggerSum = (Price).ToString("N");
                    }
                }
                //回款
                string daywherestr = "and CompID=" + logUser.CompID + " and CreateDate>='" + day0 + "' and CreateDate<'" + Sday + "' and OState in(2,3,4,5,7)";//包含已退订单金额
                string daysql = "SELECT Sum([AuditAmount]) as AuditAmount FROM  [dbo].[DIS_Order] " +
                    "where isnull(dr,0)=0 and Otype<>9 " + daywherestr;
                

                DataTable dayDt = SqlHelper.Query(SqlHelper.LocalSqlServer, daysql).Tables[0];
                if (dayDt != null)
                {
                    if (dayDt.Rows.Count > 0)
                    {
                        decimal sumAmount = dayDt.Rows[0]["AuditAmount"].ToString() == ""
                            ? sum.ToString().ToDecimal()
                            : Convert.ToDecimal(dayDt.Rows[0]["AuditAmount"]);

                        m.DaySum = (sumAmount).ToString("N");
                    }
                }
                
                //本周销售额
                string weekwherestr = "and CompID=" + logUser.CompID + " and CreateDate>='" + startWeek + "' and CreateDate<'" + endWeek + "' and OState in(2,3,4,5,7)";//包含已退订单金额
                string weeksql = "SELECT Sum([AuditAmount]) as AuditAmount FROM  [dbo].[DIS_Order] " +
                    "where isnull(dr,0)=0 and Otype<>9 " + weekwherestr;

                DataTable weekDt = SqlHelper.Query(SqlHelper.LocalSqlServer, weeksql).Tables[0];
                if (weekDt != null)
                {
                    if (weekDt.Rows.Count > 0)
                    {
                        decimal sumAmount = weekDt.Rows[0]["AuditAmount"].ToString() == ""
                            ? sum.ToString().ToDecimal()
                            : Convert.ToDecimal(weekDt.Rows[0]["AuditAmount"]);

                        m.WeekSum = (sumAmount).ToString("N");
                    }
                }
                //  add by hgh  CompCollection_view 状态   and status!=3
                string strweek = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID in(select ID from Dis_Order where ISNULL(dr,0)=0 and (Otype<>9) and CompID=" + logUser.CompID + ") and status!=3 and CompID=" + logUser.CompID +
                                   " and Date>='" + startWeek + "' and Date<'" + endWeek + "'  AND isnull(vedf9,0)=1 ";

                DataTable dtweek = SqlHelper.Query(SqlHelper.LocalSqlServer, strweek).Tables[0];
                if (dtweek != null)
                {
                    if (dtweek.Rows.Count > 0)
                    {
                        decimal Price = dtweek.Rows[0]["Price"].ToString() == ""
                            ? sum.ToString().ToDecimal()
                            : Convert.ToDecimal(dtweek.Rows[0]["Price"]);
                        m.WeekPaggerSum = (Price).ToString("N");
                    }
                }
                #endregion

                #region 本月收款  本月应收  本月销售额
                //本月收款   add by hgh  CompCollection_view 状态   and status!=3
                string paggersql = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where OrderID in(select ID from Dis_Order where ISNULL(dr,0)=0 and (Otype<>9 ) and CompID=" + logUser.CompID + ") and status!=3 and CompID=" + logUser.CompID + " and Date>='" + day1 + "' and Date<'" + mothday + "'  AND isnull(vedf9,0)=1 ";

                DataTable paggerdt = SqlHelper.Query(SqlHelper.LocalSqlServer, paggersql).Tables[0];
                if (paggerdt != null)
                {
                    if (paggerdt.Rows.Count > 0)
                    {
                        decimal Price = paggerdt.Rows[0]["Price"].ToString() == "" ? sum.ToString().ToDecimal() : Convert.ToDecimal(paggerdt.Rows[0]["Price"]);
                        m.paggerSum = (Price).ToString("N");
                    }
                }
                //本月收款额
                string wherestr = "and CompID=" + logUser.CompID + " and CreateDate>='" + day1 + "' and CreateDate<'" + mothday + "' and OState in (2,3,4,5,7)";//包含已退订单金额
                string monthsql = "SELECT Sum([AuditAmount]) as AuditAmount FROM  [dbo].[DIS_Order] " +
                    "where isnull(dr,0)=0 and Otype<>9 " + wherestr;


                DataTable monthDt = SqlHelper.Query(SqlHelper.LocalSqlServer, monthsql).Tables[0];
                if (monthDt != null)
                {
                    if (monthDt.Rows.Count > 0)
                    {
                        decimal sumAmount = monthDt.Rows[0]["AuditAmount"].ToString() == "" ? sum.ToString().ToDecimal() : Convert.ToDecimal(monthDt.Rows[0]["AuditAmount"]);

                        m.MonthSum = (sumAmount).ToString("N");
                    }
                }
                #endregion

                
                #region 待回复留言
                //List<Hi.Model.DIS_Suggest> LDis = new Hi.BLL.DIS_Suggest().GetList("", " isnull(dr,0)=0 and compid=" + logUser.CompID, "");
                //if (LDis.Count > 0)
                //{
                //    m.SuggectCount = LDis.FindAll(S => S.IsAnswer == 0).Count;
                //}
                #endregion
                #region 未读留言
                List<Hi.Model.BD_ShopMessage> shopl = new Hi.BLL.BD_ShopMessage().GetList("","isnull(dr,0)=0 and compid="+logUser.CompID+" and isread=0","");
                if (shopl.Count > 0) {
                    m.shopmsgCount = shopl.Count;
                }
                #endregion

                #region 合作待审
                //List<Hi.Model.YZT_FirstCamp> fcl = new Hi.BLL.YZT_FirstCamp().GetList("", "ISNULL(dr,0)=0 and State in (0,1) and CompID=" + logUser.CompID, "");
                string fclsql = @"select  distinct CMID  from YZT_FirstCamp where ISNULL(dr,0)=0 and State in (0,1) and CompID=" + logUser.CompID;
                DataTable fcl = SqlHelper.Query(SqlHelper.LocalSqlServer, fclsql).Tables[0];
                if (fcl != null && fcl.Rows.Count > 0)
                {
                    m.ReturnCount = fcl.Rows.Count;
                }

                string fcsql = @"select  distinct fc.DisID  from YZT_FCmaterials fc left join YZT_Annex an on fc.ID=an.fcID and an.fileAlias='4' and fc.type=2 where ISNULL(fc.dr,0)=0 and fc.CompID=" + logUser.CompID + " and an.validDate<='" + DateTime.Now.AddDays(15) + "'";
                DataTable fcDt = SqlHelper.Query(SqlHelper.LocalSqlServer, fcsql).Tables[0];
                if (fcDt != null && fcDt.Rows.Count > 0)
                {
                    m.disCount = fcDt.Rows.Count;
                }
                #endregion


                #region 招商

                string fmsql = @"select * from dbo.YZT_FirstCamp where State=2 and CompID=" + logUser.CompID;

                DataTable fmDt = SqlHelper.Query(SqlHelper.LocalSqlServer, fmsql).Tables[0];
                if (fmDt != null && fmDt.Rows.Count > 0)
                {
                    DataRow[] daydr = fmDt.Select("ts>='" + day0 + "' and ts<'" + Sday + "'");
                    DataRow[] weekdr = fmDt.Select("ts>='" + startWeek + "' and ts<'" + endWeek + "'");
                    DataRow[] MonthCMCountdr = fmDt.Select("ts>='" + day1 + "' and ts<'" + mothday + "'");

                    m.DayCMCount = daydr.Length;
                    m.WeekCMCount = weekdr.Length;
                    m.MonthCMCount = MonthCMCountdr.Length;
                }
                
                #endregion

                m.Result = true;
            }
        }
        catch (Exception ex)
        {
            m.Result = false;
            m.Msg = ex.Message;
        }
        return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(m);
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }
}

[Serializable]
public class ResultMsgJsc
{
    public ResultMsgJsc()
    {
        Result = false;
        Msg = "";
    }

    public bool Result;
    public string Msg;

    public int DayOrderCount = 0;    //当天订单数
    public string DaySum = "0";      //当日销售额
    public string dayPaggerSum = "0";//当日收款

    public int NotCount = 0;  //待审核订单个数
    public int DeliveryCount = 0;  //待发货订单个数
    public int OrderCount = 0;    //本月订单数
    public int PayOrder = 0;    //待收款订单

    public string billskSum = string.Empty; //本月账单收款
    public string billys = string.Empty; //本月账单应收

    public int ReturnCount = 0;  //退货待审订单个数
    public int ReturnMoney = 0;  //退款订单个数
    public int ReturnCountDay = 0; //今天退货个数
    public int ReturnCountWeek = 0; //本周退货个数
    public int ReturnCountMonth = 0; //本月退货个数

    public string ReturnMoneyDay = "0";  //当天退款
    public string ReturnMoneyWeek = "0"; //本周退款
    public string ReturnMoneyMonth = "0"; //本月退款

    public int PaymentCount = 0;  //企业钱包审核个数

    public int disCount = 0;  //代理商待审核个数
    public int CountSum = 0;  //总代理商总个数
    public int CountNew = 0;  //新增代理商个数

    public int SuggectCount = 0;  //待回复留言

    public int WeekOrderCount = 0; //本周订单数
    public string WeekSum = "";    //本周销售额
    public string WeekPaggerSum = "0";//当日收款

    public int DayCMCount = 0; //招商
    public int WeekCMCount = 0;    //招商
    public int MonthCMCount = 0;//招商
    
    //本月收款
    public string paggerSum = "";
    //本月应收
    public string ArrearageSum = "";
    //本月销售额
    public string MonthSum = "";

    public int IsOffLineNO = 0; //下架
    public int IsOffLineOk = 0;  //上架

    public int proCount = 0; //促销商品数量

    public int shopmsgCount = 0;//未读留言
}