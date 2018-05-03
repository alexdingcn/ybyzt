using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using DBUtility;
using LitJson;

/// <summary>
///SYS_NewsNotice 的摘要说明
/// </summary>
public class SYS_NewsNotice
{
	public SYS_NewsNotice()
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
            string strWhere = string.Empty;
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo.Keys.Contains("NewsID"))
            {
                strWhere = string.IsNullOrEmpty(JInfo["NewsID"].ToString()) ? "" : " and id=" + JInfo["NewsID"].ToString();
            }
            #endregion

            #region 模拟分页

            string tabName = " [dbo].[SYS_NewsNotice]"; //表名
            string strsql = String.Format("SELECT * from {0} where IsEnabled=1 {1} order by ID desc", tabName, strWhere);

            #endregion

            List<Info> list = new List<Info>();
            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql);
            if (ds.Tables.Count == 0)
            {
                return new ResuletInfo() { Result = "F", Description = "消息为空异常" };
            }
                
            DataTable orderList = ds.Tables[0];
            if (orderList != null)
            {
                if (orderList.Rows.Count == 0)
                {
                    return new ResuletInfo() { Result = "F", Description = "消息为空异常" };
                }
                    
                foreach (DataRow row in orderList.Rows)
                {
                    Info info = new Info();
                    info.ID = row["ID"].ToString();
                    info.Type = row["NewsType"].ToString();
                    info.Title = row["NewsTitle"].ToString();
                    info.Contents = row["NewsContents"].ToString();
                    info.CreateDate = row["CreateDate"].ToString();
                    info.Keyword = row["KeyWords"].ToString();
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
        public String Keyword { get; set; }
    }
}