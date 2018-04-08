<%@ WebHandler Language="C#" Class="OrderChart" %>

using System;
using System.Web;
using System.Data;
using DBUtility;
using System.Web.SessionState;
using System.Text;

public class OrderChart : IHttpHandler, IReadOnlySessionState
{
    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string ActionType = context.Request["ActionType"] + "";
        switch (ActionType)
        {
            case "Month":
                context.Response.Write(OrderMonth(context));
                context.Response.End();
                break;
            case "YM":
                context.Response.Write(ChartCount(context, ActionType));
                context.Response.End();
                break;
            case "Y":
                context.Response.Write(ChartCount(context, ActionType));
                context.Response.End();
                break;
        }
    }

    //当月
    public string OrderMonth(HttpContext context)
    {
        string result = "";
        try
        {
            //获取当前时间
            DateTime date = DateTime.Now;
            //当月第一天
            DateTime day1 = new DateTime(date.Year, date.Month, 1);
            //当月最后一天
            DateTime day2 = day1.AddMonths(1).AddDays(-1);

            //当月天数
            int days = System.Threading.Thread.CurrentThread.CurrentUICulture.Calendar.GetDaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            
            StringBuilder yData = new StringBuilder();
            StringBuilder day = new StringBuilder(); ;
            yData.Append("[");
            day.Append("['");

            decimal MaxData = 0;

            //登录信息
            if (context.Session["UserModel"] is LoginModel)
            {
                LoginModel logUser = context.Session["UserModel"] as LoginModel;

                string sql = @"select CompID,Day([CreateDate]) sday,YEAR([CreateDate]) Years, Month([CreateDate]) smonth, SUM([AuditAmount]) as [TotalAmount],sum( CASE WHEN Otype=9 THEN AuditAmount ELSE 0 END) as [zdAmount]  from ( SELECT * FROM  [dbo].[DIS_Order] where compID=" + logUser.CompID + "  and CreateDate BETWEEN DATEADD(mm, DATEDIFF(mm,0,getdate()), 0) and dateadd(d,-day(getdate()),dateadd(m,1,getdate())) and OState in(2,3,4,5,7)) M where compID=" + logUser.CompID + " group by Day([CreateDate]),Month([CreateDate]),YEAR([CreateDate]),CompID order by sday asc";
                
                DataTable dtDay = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
                if (dtDay != null && dtDay.Rows.Count > 0)
                {
                    foreach (DataRow item in dtDay.Rows)
                    {
                        if (MaxData <= Convert.ToDecimal(item["TotalAmount"]))
                            MaxData = Convert.ToDecimal(item["TotalAmount"]);
                    }
                    for (int i = 0; i < days; i++)
                    {
                        DataRow[] dr = dtDay.Select(string.Format("sday='{0}'", day1.ToString("dd")));
                        if (dr.Length > 0)
                            yData.Append(Convert.ToDecimal(dr[0]["TotalAmount"]).ToString("0.00") + ",");
                        else
                            yData.Append("0,");

                        string daydate = day1.ToString("dd");
                        day.Append(daydate + "','");
                        day1 = day1.AddDays(1);
                    }
                }
                else
                {
                    for (int i = 0; i < days; i++)
                    {
                        yData.Append("0,");
                        string daydate = day1.ToString("dd");
                        day.Append(daydate + "','");
                        day1 = day1.AddDays(1);
                    }
                }
                //if (i < j)
                //{
                //    System.Threading.Tasks.Parallel.For(0, j - i, index =>
                //    {
                //        int t = yData.Length + index;
                //        yData += "0,";
                //    });
                //}
            }
            string sday = day.ToString().Substring(0, day.Length - 2) + "]";
            string syData = yData.ToString().Substring(0, yData.Length - 1) + "]";
            result = "{\"result\":\"true\",\"day\": \"" + sday + "\",\"yData\":\"" + syData + "\",\"MaxData\":\"" + MaxData + "\"}";
        }
        catch (Exception)
        {
            result = "{\"result\":\"false\",\"day\": \"\",\"yData\":\"\"}";
        }
        return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result);
    }

    public string ChartCount(HttpContext context,string type)
    {
        string result = "";
        try
        {
            decimal MaxData = 0;
            StringBuilder yData = new StringBuilder();
            StringBuilder day = new StringBuilder(); ;
            yData.Append("[");
            day.Append("['");
            //获取当前时间
            DateTime date = DateTime.Now;
            DateTime day1 = date;
            int j = 0;
            
            //登录信息
            if (context.Session["UserModel"] is LoginModel)
            {
                LoginModel logUser = context.Session["UserModel"] as LoginModel;

                string sql = "";
                if (type == "YM")
                {
                    sql = @"select CompID,YEAR([CreateDate]) Years, Month([CreateDate]) smonth,SUM([AuditAmount]) as [TotalAmount],sum( CASE WHEN Otype=9 THEN AuditAmount ELSE 0 END) as [zdAmount]  from ( SELECT * FROM  [dbo].[DIS_Order] where compID=" + logUser.CompID + "  and CreateDate BETWEEN dateadd(month,-6,getdate())  and getDate() and OState in(2,3,4,5,7)) M where compID=" + logUser.CompID + " group by Month([CreateDate]),YEAR([CreateDate]),CompID  order by  Years asc ,smonth asc ";
                    day1 = date.AddMonths(-6);
                    j = 6;
                }
                else if (type == "Y")
                {
                    sql = @"select CompID,YEAR([CreateDate]) Years, Month([CreateDate]) smonth,SUM([AuditAmount]) as [TotalAmount],sum( CASE WHEN Otype=9 THEN AuditAmount ELSE 0 END) as [zdAmount]  from ( SELECT * FROM  [dbo].[DIS_Order] where compID=" + logUser.CompID + "  and CreateDate BETWEEN dateadd(YEAR,-1,getdate())  and getDate() and OState in(2,3,4,5,7)) M where compID=" + logUser.CompID + " group by Month([CreateDate]),YEAR([CreateDate]),CompID  order by  Years asc ,smonth asc ";
                    day1 = date.AddYears(-1);
                    j = 12;
                }
                
                DataTable dtDay = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

                if (dtDay != null && dtDay.Rows.Count > 0)
                {
                    foreach (DataRow item in dtDay.Rows)
                    {
                        if (MaxData <= Convert.ToDecimal(item["TotalAmount"]))
                            MaxData = Convert.ToDecimal(item["TotalAmount"]);
                    }
                    for (int i = 0; i <= j; i++)
                    {
                        DataRow[] dr = dtDay.Select(string.Format("Years='{0}' and smonth='{1}'", day1.ToString("yyyy"), day1.ToString("MM")));
                        if (dr.Length > 0)
                            yData.Append(Convert.ToDecimal(dr[0]["TotalAmount"]).ToString("0.00") + ",");
                        else
                            yData.Append("0,");

                        string daydate = day1.ToString("yyyy-MM");
                        day.Append(daydate + "','");
                        day1 = day1.AddMonths(1);
                    }
                }
                else
                {
                    for (int i = 0; i <= j; i++)
                    {
                        yData.Append("0,");
                        string daydate = day1.ToString("yyyy-MM");
                        day.Append(daydate + "','");
                        day1 = day1.AddMonths(1);
                    }
                }
            }
            string sday = day.ToString().Substring(0, day.Length - 2) + "]";
            string syData = yData.ToString().Substring(0, yData.Length - 1) + "]";
            result = "{\"result\":\"true\",\"day\": \"" + sday + "\",\"yData\":\"" + syData + "\",\"MaxData\":\"" + MaxData + "\"}";
        }
        catch (Exception)
        {
            result = "{\"result\":\"false\",\"day\": \"\",\"yData\":\"\"}";
        }
        return new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(result);
    }
    
    public bool IsReusable {
        get {
            return false;
        }
    }

}