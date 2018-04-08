using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DBUtility;
using LitJson;
using NPOI.SS.Formula.Functions;
/// <summary>
///查询经销商订单统计的方法
/// </summary>
public class GetResellerOrderSta
{
	public GetResellerOrderSta()
	{
	}
    public class ReturnOrderSta//返回的参数
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String BeginDate { get; set; }
        public String EndDate { get; set; }
        public String OrderNum { get; set; }
        public String OrderSum { get; set; }
        public String OrderOutNum { get; set; }
        public String OrderOutSum { get; set; }
    }
    public ReturnOrderSta GetOrderSta(string JSon)
    {
        try
        {
            string UserID = string.Empty;
            string ResellerID = string.Empty;
            string BeginDate = string.Empty;
            string EndDate = string.Empty;
            string BeginDate_Return = string.Empty;
            string EndDate_Return = string.Empty;
            string OrderNum = string.Empty;
            string OrderSum = string.Empty;
            string OrderOutNum = string.Empty;
            string OrderOutSum = string.Empty;
            string CompID = string.Empty;
            string strsql = string.Empty;
            #region 取出JSon的值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            #endregion
            //判断传入的参数是否异常
            if (JInfo.Count <= 0 || JInfo["UserID"].ToString() == "" || JInfo["ResellerID"].ToString() == ""
                || JInfo["BeginDate"].ToString() == "" || JInfo["EndDate"].ToString() == "")
                return new ReturnOrderSta() { Result = "F", Description = "参数异常" };
            //传入的参数赋值
            UserID = JInfo["UserID"].ToString();
            ResellerID = JInfo["ResellerID"].ToString();
            BeginDate = JInfo["BeginDate"].ToString();
            EndDate = JInfo["EndDate"].ToString();
            strsql = "select UserID from SYS_CompUser where CType = '2' and DisID = '" + ResellerID + "' and UserID = '" + UserID + "'";
            strsql += " and isnull(dr,0) = 0";
            UserID = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            if (UserID == "")//判断用户是否存在
                return new ReturnOrderSta() { Result = "F", Description = "未找到用户信息" };
            strsql = "select CompID from BD_Distributor where ID = '" + ResellerID + "' and isnull(dr,0) = 0";
            CompID = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            if (CompID == "")//判断此经销商是否存在
                return new ReturnOrderSta() { Result = "F", Description = "未找到经销商信息" };
            //查询期间的订单总数
            DateTime date_end = DateTime.Parse(EndDate);
            date_end = date_end.AddDays(1);
            strsql = "select count(*) from DIS_Order where createdate >= '" + BeginDate + "' and createdate < '" + date_end.ToString("yyyy-MM-dd") + "'  ";
            strsql += " and DisID = '" + ResellerID + "'  and CompID = '" + CompID + "' and OState in (2,4,5) ";
            strsql += " and isnull(dr,0) = 0 and otype <> '9'";
            OrderNum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");
            //查询期间的订单总额
            strsql = "SELECT SUM(sumAmount) as sumAmount FROM [dbo].[MonthSaleRpt_view] where CreateDate>='" + BeginDate + "' and ";
            strsql += "  createdate < '" + date_end.ToString("yyyy-MM-dd") + "' and DisID=" +
                       ResellerID + " and CompID=" + CompID + " and OState <>7";
            //strsql += " and createdate < '" + date_end.ToString("yyyy-MM-dd") + "'";
            OrderSum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer),"0.00");
            //查询期间退货单总数
            strsql = "select count(*) from dis_order a inner join dis_orderreturn b on ";
            strsql += " a.id = b.orderid where b.auditdate>= '" + BeginDate + "' ";
            strsql += "  and b.auditdate < '" + date_end.ToString("yyyy-MM-dd") + "' and a.disid = '" + ResellerID + "'  ";
            strsql += "  and a.ostate = '7' and a.CompID = '" + CompID + "'";
            strsql += "  and isnull(a.dr,0) = 0  and isnull(b.dr,0) = 0 and a.otype <>'9'";
            OrderOutNum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer),"0");
            //查询期间退款总额
            strsql = "select sum(a.PayedAmount) from dis_order a inner join dis_orderreturn b on ";
            strsql += " a.id = b.orderid where b.auditdate>= '" + BeginDate + "' ";
            strsql += " and b.auditdate < '" + date_end.ToString("yyyy-MM-dd") + "' and a.disid = '" + ResellerID + "' ";
            strsql += "  and a.ostate = '7' and a.CompID = '" + CompID + "'";
            strsql += "  and isnull(a.dr,0) = 0 and isnull(b.dr,0) = 0 and a.otype <>'9' ";
            OrderOutSum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql,SqlHelper.LocalSqlServer),"0.00");
            return new ReturnOrderSta()//返回参数
            {
                Result = "T",
                Description = "返回正确",
                BeginDate = BeginDate,
                EndDate = EndDate,
                OrderNum = OrderNum,
                OrderSum = OrderSum,
                OrderOutNum = OrderOutNum,
                OrderOutSum = OrderOutSum
            };

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerOrderSta:" + JSon);
            return new ReturnOrderSta() { Result = "F", Description = "参数异常" };
        }
    }
}