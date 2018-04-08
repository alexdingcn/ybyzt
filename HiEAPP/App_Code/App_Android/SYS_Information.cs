using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DBUtility;
using LitJson;

/// <summary>
///SYS_Information 的摘要说明
/// </summary>
public class SYS_Information
{
	public SYS_Information()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public ResuletInfo GetNoticeList(string JSon)
    {
        try
        {
            #region JSon取值

            string UserID = string.Empty;
            string ResellerID = string.Empty;
            string ResellerType = string.Empty;
            string CreateDate = string.Empty;
            string IsRead = string.Empty;

            string sortType = string.Empty;
            string criticalOrderID = string.Empty;
            string sort = string.Empty;
            string getType = string.Empty;
            string rows = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["ResellerID"].ToString() != "" && JInfo["ResellerType"].ToString().Trim() != "" &&
                JInfo["CriticalOrderID"].ToString() != "" && JInfo["GetType"].ToString() != "" &&
                JInfo["Rows"].ToString() != "" && JInfo["SortType"].ToString() != "" &&
                JInfo["Sort"].ToString() != "")
            {
                UserID = JInfo["UserID"].ToString();
                ResellerID = JInfo["ResellerID"].ToString();
                ResellerType = JInfo["ResellerType"].ToString();
                criticalOrderID = JInfo["CriticalOrderID"].ToString();
                getType = JInfo["GetType"].ToString();
                rows = JInfo["Rows"].ToString();
                sortType = JInfo["SortType"].ToString();
                sort = JInfo["Sort"].ToString();
            }
            else
            {
                return new ResuletInfo() { Result = "F", Description = "参数为空异常" };
            }
            JsonData JSearch = JInfo["Search"];
            if (JSearch.Count > 0)
            {
                foreach (JsonData JSear in JSearch)
                {
                    if (JSear["CreateDate"].ToString() != "")
                        CreateDate = JSear["CreateDate"].ToString();
                    if (JSear["IsRead"].ToString() != "")
                        IsRead = JSear["IsRead"].ToString();
                }
            }

            Hi.Model.SYS_Users one = new Hi.Model.SYS_Users();

            if (ResellerType == "0")
            {
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, 0,int.Parse(ResellerID == "" ? "0" : ResellerID)))
                    return new ResuletInfo() {Result = "F", Description = "登录信息异常"};
            }
            else
            {
                if (!new Common().IsLegitUser(int.Parse(UserID), out one, int.Parse(ResellerID == "" ? "0" : ResellerID)))
                    return new ResuletInfo() { Result = "F", Description = "登录信息异常" };
            }

            #endregion

            string strWhere = string.Empty;
            strWhere += ResellerType == "0" ? " and disID='" + ResellerID + "'" : " and compID='" + ResellerID + "'";
            if (CreateDate != "")
                strWhere += " and CreateDate>'" + Convert.ToDateTime(CreateDate) + "' and  CreateDate<='" + Convert.ToDateTime(CreateDate).AddDays(1) + "'";
            if (IsRead != "")
                strWhere += " and IsRead ='" + IsRead + "'";
            if (JInfo["UserID"].ToString() != "")
                strWhere += " and userID='" + UserID + "'";
            strWhere += " and ISNULL(dr,0)=0";

            #region 模拟分页

            string tabName = " [dbo].[SYS_Information]"; //表名
            string strsql = string.Empty; //搜索sql

            if (sortType == "0")
            {
                sortType = "ID";
            }
            else if (sortType == "1") //日期·排序
            {
                sortType = "CreateDate";
            }
            else
            {
                sortType = "ID";
            }
            strsql = new Common().PageSqlString(criticalOrderID, "ID", tabName, sortType,
                sort, strWhere, getType, rows);
            if (strsql == "")
                return new ResuletInfo() { Result = "F", Description = "基础数据异常" };

            #endregion

            List<Info> list = new List<Info>();
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql);
            if (ds.Tables.Count == 0)
                return new ResuletInfo() { Result = "F", Description = "消息为空异常" };
            DataTable orderList = ds.Tables[0];
            if (orderList != null)
            {
                if (orderList.Rows.Count == 0)
                    return new ResuletInfo() { Result = "F", Description = "消息为空异常" };
                foreach (DataRow row in orderList.Rows)
                {
                    Info info = new Info();
                    info.ID = row["ID"].ToString();
                    info.IsRead = row["IsRead"].ToString();
                    info.Type = row["Type"].ToString();
                    info.Title = row["Title"].ToString();
                    info.Url = row["Url"].ToString();
                    info.Contents = row["Contents"].ToString();
                    info.CreateDate = row["CreateDate"].ToString();
                    list.Add(info);
                }
            }
            return new ResuletInfo()
            {
                Result = "T",
                Description = "成功",
                ListInfo = list
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetNoticeList：" + JSon);
            return new ResuletInfo() { Result = "F", Description = "参数为空异常" };
        }
    }

    public class ResuletInfo
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public List<Info> ListInfo { get; set; }
    }

    public class Info
    {
        public String ID { get; set; }
        public String Type { get; set; }
        public String Title { get; set; }
        public String Contents { get; set; }
        public String Url { get; set; }
        public String CreateDate { get; set; }
        public String IsRead { get; set; }
    }
}