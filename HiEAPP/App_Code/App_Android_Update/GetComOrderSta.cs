using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DBUtility;
using LitJson;
using NPOI.SS.Formula.Functions;

/// <summary>
///查询核心企业统计的方法
/// </summary>
public class GetComOrderSta
{
    public GetComOrderSta()
    {
    }
    public class ReturnComOrderSta
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String BeginDate { get; set; }
        public String EndDate { get; set; }
        public String DayValue { get; set; }
        public String WeekValue { get; set; }
        public String MonthValue { get; set; }
        public List<String> DayList { get; set; }
        public List<String> valueList { get; set; }
    }
        //public static enum DayofWeek
        //{
        //    monday = 1,
        //    tuesday = 2,
        //    wednesday = 3,
        //    thursday = 4,
        //    friday = 5,
        //    saturday =6,
        //    sunday =7

        //}
        public ReturnComOrderSta GetComOrder(string JSon)
        {
            try
            {
                string UserID = string.Empty;
                string CompID = string.Empty;
                string BeginDate = string.Empty;
                string EndDate = string.Empty;
                string StaType = string.Empty;
                string DayValue = string.Empty;
                string WeekValue = string.Empty;
                string MonthValue = string.Empty;
                string strsql = string.Empty;
                List<String> DayList = new List<String>();
                List<String> valueList = new List<String>();
                DateTime datenow = DateTime.Now;//当前时间
                DateTime datenext = datenow.AddDays(1);//明天
                #region 取出JSon的值
                JsonData JInfo = JsonMapper.ToObject(JSon);
                #endregion
                //判断传入的参数是否异常
                if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["CompanyID"].ToString() == ""
                    || JInfo["BeginDate"].ToString() == "" || JInfo["EndDate"].ToString() == "" || JInfo["StaType"].ToString() == "")

                    return new ReturnComOrderSta() { Result = "F", Description = "参数异常" };

                UserID = JInfo["UserID"].ToString();
                CompID = JInfo["CompanyID"].ToString();
                BeginDate = JInfo["BeginDate"].ToString();
                EndDate = JInfo["EndDate"].ToString();
                StaType = JInfo["StaType"].ToString();
                strsql = "select UserID from SYS_CompUser where CType = '1' and CompID = '" + CompID + "' and UserID = '" + UserID + "'";
                strsql += " and isnull(dr,0) = 0";
                UserID = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                if (UserID == "")//判断用户是否存在
                    return new ReturnComOrderSta() { Result = "F", Description = "未找到用户信息" };
                strsql = "select ID from BD_Company where ID = '" + CompID + "' and isnull(dr,0) = 0";
                CompID = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
                if (CompID == "")//判断此核心企业是否存在
                    return new ReturnComOrderSta() { Result = "F", Description = "未找到核心企业信息" };
                //算出本周周一的日期
                string week_num = datenow.DayOfWeek.ToString().ToLower();
                int day_now = 0;
                switch (week_num)
                {
                    case "monday":
                        day_now = 1;
                        break;
                    case "tuesday":
                        day_now = 2;
                        break;
                    case "wednesday":
                        day_now = 3;
                        break;
                    case "thursday":
                        day_now = 4;
                        break;
                    case "friday":
                        day_now = 5;
                        break;
                    case "saturday":
                        day_now = 6;
                        break;
                    case "sunday":
                        day_now = 7;
                        break;

                }
                DateTime week_start = datenow.AddDays(1 - day_now);
                DateTime week_end = datenow.AddDays(8 - day_now);//下周一的日期
                //算出本月第一天的时间
                int year_now = datenow.Year;
                int month_now = datenow.Month;
                DateTime month_start = new DateTime(year_now, month_now, 1);//这个月一号
                DateTime month_end = month_start.AddMonths(1);//下个月一号
                DateTime fordate = DateTime.Parse(BeginDate);
                DateTime forenddate = DateTime.Parse(EndDate);
                
                //根据statype取不同的值
                switch (StaType)
                {
                    case "0"://订单数
                        strsql = "select count(*) from DIS_Order where createdate >= '" + datenow.ToString("yyyy-MM-dd") + "'  and CompID = '" + CompID + "' ";
                        strsql += " and OState in (2,4,5) and isnull(dr,0) = 0 and otype <> '9'";
                        strsql += " and createdate < '" + datenext.ToString("yyyy-MM-dd") + "'";//今日订单数
                        DayValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                        //本周订单数
                        strsql = "select count(*) from DIS_Order where createdate >= '" + week_start.ToString("yyyy-MM-dd") + "'";
                        strsql += " and CompID = '" + CompID + "' and OState in (2,4,5) and isnull(dr,0) = 0";
                        strsql += " and otype <> '9' and createdate < '" + week_end.ToString("yyyy-MM-dd") + "'";
                        WeekValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                        //本月订单数
                        strsql = "select count(*) from DIS_Order where createdate >= '" + month_start.ToString("yyyy-MM-dd") + "'";
                        strsql += " and CompID = '" + CompID + "' and OState in (2,4,5) and isnull(dr,0) = 0";
                        strsql += " and otype <> '9' and createdate < '" + month_end.ToString("yyyy-MM-dd") + "'";
                        MonthValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                        //根据开始日期结束结束取每一天的值（传过来的开始日期30天前的日期，结束日期是今天的日期
                        while(fordate <= forenddate )
                        {
                            DayList.Add(fordate.ToString("yyyy-MM-dd").Substring(8,2));
                            //当天订单数
                            strsql = "select count(*) from DIS_Order where createdate >= '" + fordate.ToString("yyyy-MM-dd") + "'";
                            strsql += " and createdate < '" + fordate.AddDays(1).ToString("yyyy-MM-dd") + "'  and CompID = '" + CompID + "'";
                            strsql += "  and OState in (2,4,5) and isnull(dr,0) = 0 and otype <> '9'";
                            string ordernum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                            valueList.Add(ordernum);
                            fordate = fordate.AddDays(1);
                        }
                        break;
                    case "1"://订单金额
                        //今日订单金额
                        strsql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where CreateDate>='" + datenow.ToString("yyyy-MM-dd") + "' ";
                        strsql += "and CompID=" + CompID + " and OState in (2,4,5) and CreateDate <'" + datenext.ToString("yyyy-MM-dd") + "' ";
                        DayValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                        DayValue = double.Parse(DayValue).ToString("F2");
                        //本周订单金额
                        strsql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where CreateDate>='" + week_start.ToString("yyyy-MM-dd") + "' ";
                        strsql += "and CompID=" + CompID + " and OState in (2,4,5) and CreateDate <'" + week_end.ToString("yyyy-MM-dd") + "' ";
                        WeekValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                        WeekValue = double.Parse(WeekValue).ToString("F2");
                        //本月订单金额
                        strsql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where CreateDate>='" + month_start.ToString("yyyy-MM-dd") + "' ";
                        strsql += "and CompID=" + CompID + " and OState in (2,4,5) and CreateDate <'" + month_end.ToString("yyyy-MM-dd") + "' ";
                        MonthValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                        MonthValue = double.Parse(MonthValue).ToString("F2");
                        //根据开始日期结束结束取每一天的值（传过来的开始日期30天前的日期，结束日期是今天的日期）
                        while (fordate <= forenddate)
                        {
                            DayList.Add(fordate.ToString("yyyy-MM-dd").Substring(8,2));
                            strsql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where ";
                            strsql += "CreateDate>='" + fordate.ToString("yyyy-MM-dd") + "' ";
                            strsql += "and CreateDate <'" + fordate.AddDays(1).ToString("yyyy-MM-dd") + "'";
                            strsql += "and CompID=" + CompID + " and OState in (2,4,5)  ";
                            string ordersum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                            ordersum = double.Parse(ordersum).ToString("F2");
                            valueList.Add(ordersum);
                            fordate = fordate.AddDays(1);
                        }

                        break;
                    case "2"://退货单数
                        //今日退货单数
                        strsql = "select count(*) from dis_order a inner join dis_orderreturn b on ";
                        strsql += " a.id = b.orderid where b.auditdate>= '" + datenow.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and a.ostate = '7' and a.CompID = '" + CompID + "'";
                        strsql += "  and b.auditdate < '" + datenext.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and isnull(a.dr,0) = 0  and isnull(b.dr,0) = 0 and a.otype <>'9'";
                        DayValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                        //本周退货单数
                        strsql = "select count(*) from dis_order a inner join dis_orderreturn b on ";
                        strsql += " a.id = b.orderid where b.auditdate>= '" + week_start.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and a.ostate = '7' and a.CompID = '" + CompID + "'";
                        strsql += "  and b.auditdate < '" + week_end.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and isnull(a.dr,0) = 0  and isnull(b.dr,0) = 0 and a.otype <>'9'";
                        WeekValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                        //本月退货单数
                        strsql = "select count(*) from dis_order a inner join dis_orderreturn b on ";
                        strsql += " a.id = b.orderid where b.auditdate>= '" + month_start.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and a.ostate = '7' and a.CompID = '" + CompID + "'";
                        strsql += "  and b.auditdate < '" + month_end.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and isnull(a.dr,0) = 0  and isnull(b.dr,0) = 0 and a.otype <>'9'";
                        MonthValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                        //根据开始日期结束结束取每一天的值（传过来的开始日期30天前的日期，结束日期是今天的日期）
                        while (fordate <= forenddate)
                        {
                            DayList.Add(fordate.ToString("yyyy-MM-dd").Substring(8,2));
                            strsql = "select count(*) from dis_order a inner join dis_orderreturn b on ";
                            strsql += " a.id = b.orderid where b.auditdate>= '" + fordate.ToString("yyyy-MM-dd") + "' ";
                            strsql += "  and b.auditdate < '" + fordate.AddDays(1).ToString("yyyy-MM-dd") + "' ";
                            strsql += "  and a.ostate = '7' and a.CompID = '" + CompID + "'";
                            strsql += "  and isnull(a.dr,0) = 0  and isnull(b.dr,0) = 0 and a.otype <>'9'";
                            string orderreturnnum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                            valueList.Add(orderreturnnum);
                            fordate = fordate.AddDays(1);
                        }
                        break;


                    case "3"://退货单金额
                        //今日退货单金额
                        strsql = "select sum(a.PayedAmount) from dis_order a inner join dis_orderreturn b on ";
                        strsql += " a.id = b.orderid where b.auditdate>= '" + datenow.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and a.ostate = '7' and a.CompID = '" + CompID + "'";
                        strsql += " and b.auditdate < '" + datenext.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and isnull(a.dr,0) = 0 and isnull(b.dr,0) = 0 and a.otype <>'9' ";
                        DayValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                        //本周退货单金额
                        strsql = "select sum(a.PayedAmount) from dis_order a inner join dis_orderreturn b on ";
                        strsql += " a.id = b.orderid where b.auditdate>= '" + week_start.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and a.ostate = '7' and a.CompID = '" + CompID + "'";
                        strsql += " and b.auditdate < '" + week_end.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and isnull(a.dr,0) = 0 and isnull(b.dr,0) = 0 and a.otype <>'9' ";
                        WeekValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                        //本月退货单金额
                        strsql = "select sum(a.PayedAmount) from dis_order a inner join dis_orderreturn b on ";
                        strsql += " a.id = b.orderid where b.auditdate>= '" + month_start.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and a.ostate = '7' and a.CompID = '" + CompID + "'";
                        strsql += " and b.auditdate < '" + month_end.ToString("yyyy-MM-dd") + "' ";
                        strsql += "  and isnull(a.dr,0) = 0 and isnull(b.dr,0) = 0 and a.otype <>'9' ";
                        MonthValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                        //根据开始日期结束结束取每一天的值（传过来的开始日期30天前的日期，结束日期是今天的日期）
                        while (fordate <= forenddate)
                        {
                            DayList.Add(fordate.ToString("yyyy-MM-dd").Substring(8,2));
                            strsql = "select sum(a.PayedAmount) from dis_order a inner join dis_orderreturn b on ";
                            strsql += " a.id = b.orderid where b.auditdate>= '" + fordate.ToString("yyyy-MM-dd") + "' ";
                            strsql += " and b.auditdate < '" + fordate.AddDays(1).ToString("yyyy-MM-dd") + "' ";
                            strsql += "  and a.ostate = '7' and a.CompID = '" + CompID + "'";
                            strsql += "  and isnull(a.dr,0) = 0 and isnull(b.dr,0) = 0 and a.otype <>'9' ";
                            string orderreturnsum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                            valueList.Add(orderreturnsum);
                            fordate = fordate.AddDays(1);
                        }
                        break;
                    case "4"://订单已收款
                        //今日订单已收款
                        strsql = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where Date>='" + datenow.ToString("yyyy-MM-dd") + "'";
                        strsql += " and CompID=" + CompID + " and Date < '" + datenext.ToString("yyyy-MM-dd") + "' and ";
                        strsql += " OrderID not in(select ID from Dis_Order where CompID=" + CompID + "  and ISNULL(dr,0)=0 and Otype=9 )";
                        DayValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                        //本周订单已收款
                        strsql = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where Date>='" + week_start.ToString("yyyy-MM-dd") + "'";
                        strsql += " and CompID=" + CompID + " and Date < '" + week_end.ToString("yyyy-MM-dd") + "' and ";
                        strsql += " OrderID not in(select ID from Dis_Order where CompID=" + CompID + "  and ISNULL(dr,0)=0 and Otype=9 )";
                        WeekValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                        //本月订单已收款
                        strsql = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where Date>='" + month_start.ToString("yyyy-MM-dd") + "'";
                        strsql += " and CompID=" + CompID + " and Date < '" + month_end.ToString("yyyy-MM-dd") + "' and ";
                        strsql += " OrderID not in(select ID from Dis_Order where CompID=" + CompID + "  and ISNULL(dr,0)=0 and Otype=9 )";
                        MonthValue = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                        //根据开始日期结束结束取每一天的值（传过来的开始日期30天前的日期，结束日期是今天的日期）
                        while (fordate <= forenddate)
                        {
                            DayList.Add(fordate.ToString("yyyy-MM-dd").Substring(8,2));
                            strsql = "SELECT SUM(Price) as Price FROM [dbo].[CompCollection_view] where Date>='" + fordate.ToString("yyyy-MM-dd") + "'";
                            strsql += " and Date < '" + fordate.AddDays(1).ToString("yyyy-MM-dd") + "' and CompID=" + CompID + "  and ";
                            strsql += " OrderID not in(select ID from Dis_Order where CompID=" + CompID + "  and ISNULL(dr,0)=0 and Otype=9 )";
                            string sum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0.00");
                            valueList.Add(sum);
                            fordate = fordate.AddDays(1);
                        }
                        break;


                }
                return new ReturnComOrderSta()
                {
                    Result = "T",
                    Description = "获取成功",
                    BeginDate = BeginDate,
                    EndDate = EndDate,
                    DayValue = DayValue,
                    WeekValue = WeekValue,
                    MonthValue = MonthValue,
                    DayList = DayList,
                    valueList = valueList
                };
            }
            catch (Exception ex)
            {
                Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetComOrderSta:" + JSon);
                return new ReturnComOrderSta() { Result = "F", Description = "参数异常" };
            }
        }
    }
