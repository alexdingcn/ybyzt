using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DBUtility;
using LitJson;
using NPOI.SS.Formula.Functions;


/// <summary>
///GetRebate 的摘要说明
/// </summary>
public class GetRebate
{
	public GetRebate()//构造函数
	{

	}
    public class GetRebateResult
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String RebateSum { get; set; }//返利总金额
        public List<Rebate> RebateList { get; set; }
    }
    public class Rebate
    {
        public String RebateNo { get; set; }//返利单号
        public String RebateType { get; set; }//返利类型
        public String RebateMoney { get; set; }//返利金额
        public String UseredMoney { get; set; }//已使用金额
        public String EnableMoney { get; set; }//可用返利金额
        public String StartDate { get; set; }//有效期开始日期
        public String EndDate { get; set; }//有效期结束日期
        public String Remark { get; set; }//备注
    }
    public GetRebateResult GetRebateList(string JSon,string version)
    {
        string disid = string.Empty;
        string strsql = string.Empty;
        string RebateSum = string.Empty;
        string CompId = string.Empty;
        JsonData JInfo = JsonMapper.ToObject(JSon);//JSon取值
        
        if(JInfo.Count >0 &&JInfo["ResellerID"].ToString() != "")
        {
            disid = JInfo["ResellerID"].ToString();
        }
        else 
        {
            return new GetRebateResult() { Result = "F", Description = "参数异常" };
        }
        //GetRebateResult getrebateresult = new GetRebateResult();
        //判断经销商对应的核心企业是否存在
        try
        {
            strsql = "select CompID from BD_Distributor where id = " + disid + " ";
            strsql += " and isnull(AuditState,0) = 2 and ";
            strsql += " isnull(IsEnabled,1) = 1 and isnull(dr,0) = 0";
            CompId = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
            if (CompId == "")
                return new GetRebateResult() { Result = "F", Description = "参数异常" };
            //取出此经销商所有可用的返利的总金额,此金额为返利总金额

            //strsql = "select sum(EnableAmount) as RebateSum from BD_Rebate where DisID = '" + disid + "' ";
            //strsql += " and getdate() between StartDate and dateadd(day,1,EndDate)";
            //strsql += " and isnull(RebateState,1) = 1";
            //strsql += " and isnull(dr,0) = 0 and compid = '" + CompId + "'";
            //RebateSum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "0");

            //版本6，及以上版本会在修改订单时查询可用返利，所以需要传入orderid,没有传空
            if (version.ToLower()=="ios"||version.ToLower()=="android"||float.Parse(version)<6)
                RebateSum = Common.GetRebate(0,Int32.Parse(disid));                
            else
                RebateSum = Common.GetRebate(JInfo["OrderID"].ToString() == "" ? 0 : Int32.Parse(JInfo["OrderID"].ToString()), Int32.Parse(disid));
            RebateSum = double.Parse(RebateSum).ToString("F2");
            //getrebateresult.RebateSum = RebateSum;
            //取出List<Rebate>中需要的数据，注意有效期的限制
            strsql = "select ReceiptNo,RebateType,RebateAmount,UserdAmount,EnableAmount,StartDate,EndDate,Remark";
            strsql += " from BD_Rebate where DisID = " + disid + " ";
            strsql += " and getdate() between StartDate and dateadd(day,1,EndDate) and isnull(RebateState,1) = 1";
            strsql += " and isnull(dr,0) = 0 and compid = " + CompId + "";
            DataTable dt_rebate = SqlAccess.ExecuteSqlDataTable(strsql, SqlHelper.LocalSqlServer);
            List<Rebate> list_rebate = new List<Rebate>();
            //将每行数据循环赋值到List<Rebate>
            DateTime startDate, endDate;
            for (int i = 0; i < dt_rebate.Rows.Count; i++)
            {
                Rebate rebate = new Rebate();//Rebate实例
                rebate.RebateNo = ClsSystem.gnvl(dt_rebate.Rows[i]["ReceiptNo"], "");
                rebate.RebateType = ClsSystem.gnvl(dt_rebate.Rows[i]["RebateType"], "");
                rebate.RebateMoney = double.Parse(ClsSystem.gnvl(dt_rebate.Rows[i]["RebateAmount"], "0")).ToString("F2");
                rebate.UseredMoney = double.Parse(ClsSystem.gnvl(dt_rebate.Rows[i]["UserdAmount"], "0")).ToString("F2");
                rebate.EnableMoney = double.Parse(ClsSystem.gnvl(dt_rebate.Rows[i]["EnableAmount"], "0")).ToString("F2");
                //rebate.StartDate = ClsSystem.gnvl(dt_rebate.Rows[i]["StartDate"], "");
                if (ClsSystem.gnvl(dt_rebate.Rows[i]["StartDate"], "") != "")
                    rebate.StartDate = DateTime.Parse(ClsSystem.gnvl(dt_rebate.Rows[i]["StartDate"], "")).ToString("yyyy/MM/dd");
                else
                    rebate.StartDate = "";
                if (ClsSystem.gnvl(dt_rebate.Rows[i]["EndDate"], "") != "")
                    rebate.EndDate = DateTime.Parse(ClsSystem.gnvl(dt_rebate.Rows[i]["EndDate"], "")).ToString("yyyy/MM/dd");
                else
                    rebate.EndDate = "";
                //rebate.EndDate = ClsSystem.gnvl(dt_rebate.Rows[i]["EndDate"], "");
                rebate.Remark = ClsSystem.gnvl(dt_rebate.Rows[i]["Remark"], "");
                list_rebate.Add(rebate);
            }
            return new GetRebateResult()
            {
                Result = "T",
                Description = "获取成功",
                RebateSum = RebateSum,
                RebateList = list_rebate

            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetRebate:" + JSon);
            return new GetRebateResult() { Result = "F", Description = "参数异常" };
        }

    }
}