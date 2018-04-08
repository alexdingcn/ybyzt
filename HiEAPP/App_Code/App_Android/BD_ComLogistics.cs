using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;

/// <summary>
///BD_ComLogistics 的摘要说明
/// </summary>
public class BD_ComLogistics
{
	public BD_ComLogistics()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    public ResultLogistics GetLogisticsList(string JSon)
    {
        try
        {
            #region JSon取值

            string userID = string.Empty;
            string compID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["CompanyID"].ToString() != "" && JInfo["UserID"].ToString() != "")
            {
                userID = JInfo["UserID"].ToString();
                compID = JInfo["CompanyID"].ToString();
            }
            else
            {
                return new ResultLogistics { Result = "F", Description = "参数异常" };
            }

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user,int.Parse(compID)))
                return new ResultLogistics() { Result = "F", Description = "登录信息异常" };

            #endregion

            List<LogisticsChoice> LogisticsChoiceList = new List<LogisticsChoice>();
            List<Hi.Model.BD_ComLogistics> list = new Hi.BLL.BD_ComLogistics().GetList("", "CompID='"+compID+"' and Enabled=0 and dr=0", "");
            if (list != null && list.Count > 0)
            {
                foreach (var item in list)
                {
                    LogisticsChoice one = new LogisticsChoice();
                    one.LogisticsCode = item.LogisticsCode;
                    one.LogisticsName = item.LogisticsName;
                    LogisticsChoiceList.Add(one);
                }
                return new ResultLogistics
                {
                    Result = "T",
                    LogisticsChoiceList = LogisticsChoiceList
                };
            }
            return new ResultLogistics{ Result = "T",Description = "未找到绑定的物流公司" };
            
        }
        catch(Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetLogisticsList ：" + JSon);
            return new ResultLogistics() { Result = "F", Description = "异常" };
        }
    }

    public class ResultLogistics
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<LogisticsChoice> LogisticsChoiceList { get; set; }
    }
}