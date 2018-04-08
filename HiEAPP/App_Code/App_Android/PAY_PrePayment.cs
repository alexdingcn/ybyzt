using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.UI.WebControls;
using DBUtility;
using LitJson;


public class PAY_PrePayment
{
    public PAY_PrePayment()
    {
    }

    public ResultPre WXGetPrePaymentList(string JSon)
    {
        List<PrePayment> ListPrePayment = new List<PrePayment>();
        string disID = string.Empty;
        string criticalOrderID = string.Empty; //当前列表最临界点产品ID:初始-1
        string getType = string.Empty; //方向
        string rows = string.Empty;
        string sortType = string.Empty;
        string sort = string.Empty;
        string start = string.Empty;

        try
        {
            #region  JSon 取值

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count == 0 || JInfo["ResellerID"].ToString() == "" ||
                JInfo["CriticalOrderID"].ToString() == "" || JInfo["GetType"].ToString() == "" ||
                JInfo["Rows"].ToString() == "" || JInfo["SortType"].ToString() == "" ||
                JInfo["Sort"].ToString() == "")
            {
                return new ResultPre() {Result = "F", Description = "参数异常"};
            }
            disID = JInfo["ResellerID"].ToString();
            criticalOrderID = JInfo["CriticalOrderID"].ToString();
            getType = JInfo["GetType"].ToString();
            rows = JInfo["Rows"].ToString();
            sortType = JInfo["SortType"].ToString();
            sort = JInfo["Sort"].ToString();

            #endregion

            #region 模拟分页

            string tabName = " [dbo].[PAY_PrePayment]"; //表名
            string strsql = string.Empty; //搜索sql
            sortType = sortType == "1" ? "CreateDate" : "ID";
            string strWhere = string.Empty;
            strWhere = " and start != 2 and disID='" + disID + "'";
            strsql = new Common().PageSqlString(criticalOrderID, "ID", tabName, sortType,
                sort, strWhere, getType, rows);
            if (strsql == "")
                return new ResultPre() {Result = "F", Description = "基础数据异常"};

            #endregion

            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql);
            if (ds.Tables.Count == 0)
                return new ResultPre() {Result = "F", Description = "没有更多数据"};
            DataTable List = ds.Tables[0];
            if (List != null)
            {
                if (List.Rows.Count == 0)
                    return new ResultPre() {Result = "F", Description = "没有更多数据"};
                foreach (DataRow row in List.Rows)
                {
                    PrePayment PrePayment = new PrePayment();

                    PrePayment.ID = row["ID"].ToString();
                    PrePayment.CompID = row["CompID"].ToString();
                    PrePayment.DisID = row["DisID"].ToString();
                    Hi.Model.BD_Distributor dis =
                        new Hi.BLL.BD_Distributor().GetModel(int.Parse(row["DisID"].ToString()));
                    if (dis == null)
                        return new ResultPre() {Result = "F", Description = "未找到加盟商"};
                    PrePayment.DisName = dis.DisName;
                    PrePayment.DisCode = dis.DisCode;
                    PrePayment.Start = row["Start"].ToString();
                    PrePayment.AuditState = row["AuditState"].ToString();
                    PrePayment.PreType = row["PreType"].ToString();
                    PrePayment.Price = new Hi.BLL.PAY_PrePayment().sums(dis.ID, dis.CompID).ToString();
                    PrePayment.OldId = row["OldId"].ToString();
                    PrePayment.Paytime = row["Paytime"].ToString();
                    PrePayment.CreatDate = row["CreatDate"].ToString();
                    //PrePayment.CrateUserID = row["CrateUser"].ToString();
                    PrePayment.CrateUserName = row["CrateUser"].ToString();
                    PrePayment.AuditUser = row["AuditUser"].ToString();
                    PrePayment.IsEnabled = row["IsEnabled"].ToString();
                    PrePayment.AuditDate = row["AuditDate"].ToString();
                    PrePayment.vdef1 = row["vdef1"].ToString();

                    ListPrePayment.Add(PrePayment);
                }
            }

            return new ResultPre() {Result = "T", Description = "操作成功", ListPrePayment = ListPrePayment};
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "WXGetPrePaymentList：" + JSon);
            return new ResultPre() {Result = "F", Description = "参数异常"};
        }
    }

    #region 返回

    public class ResultPre
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<PrePayment> ListPrePayment { get; set; }
    }

    public class PrePayment
    {
        public string ID { get; set; }
        public string CompID { get; set; }
        public string DisID { get; set; }
        public string DisName { get; set; }
        public string DisCode { get; set; }
        public string Start { get; set; }
        public string AuditState { get; set; }
        public string PreType { get; set; }
        public string Price { get; set; }
        public string OldId { get; set; }
        public string Paytime { get; set; }
        public string CreatDate { get; set; }
        public string CrateUserID { get; set; }
        public string CrateUserName { get; set; }
        public string AuditUser { get; set; }
        public string IsEnabled { get; set; }
        public string AuditDate { get; set; }
        public string vdef1 { get; set; }
    }

    #endregion

}

