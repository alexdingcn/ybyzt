using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DBUtility;
using LitJson;
using NPOI.SS.Formula.Functions;

/// <summary>
///查询经销商订单统计列表的方法
/// </summary>
public class GetResellerOrderStaList
{
	public GetResellerOrderStaList()//构造函数
	{
        
	}
    public class ReturnOrderStaList//返回的字段
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String OrderDayNum { get; set; }
        public String OrderDaySum { get; set; }
        public List<OrderYearList> OrderYearList { get; set; }
    }
    public class OrderYearList//返回的年度
    {
        public String Year { get; set; }
        public List<OrderMonthList> OrderMonthList { get; set; }
    }
    public class OrderMonthList//返回的月度订单，退货单的数量与金额
    {
        public String OrderMonth { get; set; }
        public String OrderMonthNum { get; set; }
        public String OrderMonthSum { get; set; }
        public String OrderOutMonthNum { get; set; }
        public String OrderOutMonthSum { get; set; }
    }
    public ReturnOrderStaList GetOrderStaList(string JSon)
    {
        try
        {
            string OrderDayNum = string.Empty;
            string OrderDaySum = string.Empty;
            string UserID = string.Empty;
            string ResellerID = string.Empty;
            string BeginDate = string.Empty;
            string EndDate = string.Empty;
            string date_now = string.Empty;
            string nextdate = string.Empty;
            string strsql = string.Empty;
            string CompID = string.Empty;
            int fordate_start_month = 0;
            int fordate_end_month = 0;
            //ReturnOrderStaList returnorderstalist = new ReturnOrderStaList();
            List<OrderYearList> orderyearlist = new List<OrderYearList>();
            #region 取出JSon的值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            #endregion
            //判断传入的参数是否异常
            if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["ResellerID"].ToString() == ""
                || JInfo["BeginDate"].ToString() == "" || JInfo["EndDate"].ToString() == "")
            
                return new ReturnOrderStaList() { Result = "F", Description = "参数异常" };
            
            UserID = JInfo["UserID"].ToString();
            ResellerID = JInfo["ResellerID"].ToString();
            BeginDate = JInfo["BeginDate"].ToString();
            EndDate = JInfo["EndDate"].ToString();
            date_now = DateTime.Now.ToString("yyyy-MM-dd");
            strsql = "select UserID from SYS_CompUser where CType = '2' and DisID = '" + ResellerID + "' and UserID = '" + UserID + "'";
            strsql += " and isnull(dr,0) = 0";
            UserID = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            if(UserID == "")//判断用户是否存在
                return new ReturnOrderStaList() { Result = "F", Description = "未找到用户信息" };
            strsql = "select CompID from BD_Distributor where ID = '"+ResellerID+"' and isnull(dr,0) = 0";
            CompID = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            if(CompID == "")//判断此经销商是否存在
                return new ReturnOrderStaList() { Result = "F", Description = "未找到经销商信息" };

            nextdate = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
            //今日订单数
            strsql = "select count(*) from DIS_Order where otype <> '9' and isnull(dr,0) = 0 and CompID = '"+CompID +"' ";
            strsql += "and DisID = '" + ResellerID + "' and OState in (2,4,5) and createdate >= '"+date_now+"'";
            strsql += " and createdate < '"+nextdate+"'";
            OrderDayNum  = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
            //今日订单总额
            strsql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where DisID=" +
                              ResellerID + " and CompID=" + CompID + " and CreateDate>='" + date_now + "'";//之前版本的sql

            //重新自己写得SQL
            //strsql = "select sum(TotalAmount) from DIS_Order where otype <> '9' and isnull(dr,0) = 0 and CompID = '" + CompID + "' ";
            //strsql += "and DisID = '" + ResellerID + "' and OState in (2,4,5) and createdate >= '" + date_now + "'";
            OrderDaySum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer),"0.00");
            //考虑跨年的情况
            DateTime date_start = DateTime.Parse(BeginDate);
            int year_begin = date_start.Year;
            DateTime date_end = DateTime.Parse(EndDate);
            int year_end = date_end.Year;
            for (int i = year_begin; i <= year_end; i++)
            {
                OrderYearList orderyear = new OrderYearList();
                orderyear.Year = i.ToString();
                if (year_begin  == year_end)//没跨年获取每月数据的循环开始月与结束月
                {
                    fordate_start_month = date_start.Month;
                    fordate_end_month = date_end.Month;
                }
                else if (i == year_begin)//跨年且年度等于开始时间的年度，循环开始月份是开始时间的月份，结束月份是12月
                {
                    fordate_start_month = date_start.Month;
                    fordate_end_month = 12;
                }
                else if (i != year_end)//跨年且年度不等于开始和结束时间的年度，循环开始月份是1月，结束月份是12月
                {
                    fordate_start_month = 1;
                    fordate_end_month = 12;
                }
                else//跨年且年度等于结束时间年度，循环开始月份是1月，结束月份是结束时间的月份
                {
                    fordate_start_month = 1;
                    fordate_end_month = date_end.Month;
                }
                List<OrderMonthList> ordermonthlist = new List<OrderMonthList>();
                for (int month = fordate_start_month; month <= fordate_end_month; month++)
                {
                    OrderMonthList ordermonth = new OrderMonthList();
                    ordermonth.OrderMonth = month.ToString();
                    DateTime month_fristday = new DateTime(i, month, 1);//当月第一天日期
                    //DateTime month_lastday = month_fristday.AddMonths(1).AddDays(-1);//当月最后一天
                    DateTime month_lastday = month_fristday.AddMonths(1);//下个月第一天
                    //本月订单数
                    strsql = "select count(*) from DIS_Order where otype <> '9' and isnull(dr,0) = 0 and CompID = '" + CompID + "' ";
                    strsql += "and DisID = '" + ResellerID + "' and OState in (2,4,5) and createdate >= '" + month_fristday.ToString("yyyy-MM-dd") + "'";
                    strsql += " and createdate < '"+month_lastday.ToString("yyyy-MM-dd")+"' ";
                    ordermonth.OrderMonthNum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
                    //本月订单总额
                    strsql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where DisID=" +
                        ResellerID + " and CompID=" + CompID + " and CreateDate>='" + month_fristday.ToString("yyyy-MM-dd") + "' ";
                    strsql += " and createdate < '"+month_lastday.ToString("yyyy-MM-dd")+"'";
                    ordermonth.OrderMonthSum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer),"0.00");
                    //本月退款单
                    strsql = "select count(*) from dis_order a inner join dis_orderreturn b on ";
                    strsql += " a.id = b.orderid where a.otype <>'9' and isnull(a.dr,0) = 0 and isnull(b.dr,0) = 0";
                    strsql += " and a.ostate = '7' and b.auditdate>= '"+month_fristday.ToString("yyyy-MM-dd")+"'";
                    strsql += " and b.auditdate < '"+month_lastday.ToString("yyyy-MM-dd")+"' and a.CompID = '"+CompID+"'";
                    strsql += " and a.disid = '"+ResellerID+"'";
                    ordermonth.OrderOutMonthNum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer),"0");
                    
                    //本月退款额
                    strsql = "select sum(a.PayedAmount) from dis_order a inner join dis_orderreturn b on ";
                    strsql += " a.id = b.orderid where a.otype <>'9' and isnull(a.dr,0) = 0 and isnull(b.dr,0) = 0";
                    strsql += " and a.ostate = '7' and b.auditdate>= '"+month_fristday.ToString("yyyy-MM-dd")+"'";
                    strsql += " and b.auditdate < '"+month_lastday.ToString("yyyy-MM-dd")+"' and a.CompID = '"+CompID+"'";
                    strsql += " and a.disid = '"+ResellerID+"'";
                    ordermonth.OrderOutMonthSum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer),"0.00");
                    ordermonthlist.Add(ordermonth);
                }
                orderyear.OrderMonthList = ordermonthlist;
                orderyearlist.Add(orderyear);
            }
            return new ReturnOrderStaList()
            {
                Result = "T", 
                Description = "返回正确",
                OrderDayNum = OrderDayNum,
                OrderDaySum = OrderDaySum,
                OrderYearList = orderyearlist
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerOrderStaList:" + JSon);
            return new ReturnOrderStaList() { Result = "F", Description = "参数异常" };
        }
    }
}