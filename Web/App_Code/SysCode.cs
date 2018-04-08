using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DBUtility;
using System.Configuration;

/// <summary>
///SysCode 的摘要说明  Add by hgh  获取系统自动生成编码
/// </summary>
public class SysCode
{
	public SysCode()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    /// <summary>
    /// 获得最新Code
    /// </summary>
    /// <param name="Name"></param>
    /// <returns></returns>
    public static string GetNewCode(string strName)
    {
        string returnstr = "";
        try
        {
            List<Hi.Model.SYS_SysName> NameModel = new Hi.BLL.SYS_SysName().GetList("", "CompID=0 and Name='" + strName + "'", "");
            if (NameModel != null)
            {
                //string OrgCode = ConfigurationManager.AppSettings["OrgCode"] == null ? "" : ConfigurationManager.AppSettings["OrgCode"].ToString().Trim() + "-";
                //string value = NameModel[0].Value;
                string yyyy = DateTime.Today.Year.ToString().PadLeft(4, '0');
                string mm = DateTime.Today.Month.ToString().PadLeft(2, '0');
                string dd = DateTime.Today.Day.ToString().PadLeft(2, '0');

                string codeName = string.Empty;
                
                codeName = yyyy + mm + dd;

                List<Hi.Model.SYS_SysCode> CodeModel = new Hi.BLL.SYS_SysCode().GetList("", "CompID=0 and CodeName='" + codeName + "'", "");
                if (CodeModel.Count > 0)
                {
                    int codeValue = Convert.ToInt32(CodeModel[0].CodeValue);
                    int newCode = codeValue + 1;
                    string newCodeStr = "";
                    
                    newCodeStr = codeName + "-" + newCode.ToString().PadLeft(6, '0');
                    //returnstr = OrgCode + value + "-" + newCodeStr;
                    returnstr = "MD-" + newCodeStr;

                    //修改最新值
                    SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, "update SYS_SysCode set CodeValue=" + newCode + " where CodeName='" + codeName + "'");

                }
                else
                {
                    string newCodeStr = "";
                    newCodeStr = codeName + "-000001";

                    //returnstr = OrgCode + value + "-" + newCodeStr;
                    returnstr = "MD-" + newCodeStr;
                    //插入数据
                    SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, "insert into SYS_SysCode(CompID,CodeName,CodeValue,ts,modifyuser) values(0,'" + codeName + "',1,'" + DateTime.Now + "',0)");
                }
            }
        }
        catch { }
        return returnstr;
    }

    /// <summary>
    ///  发货、退单编号
    /// </summary>
    /// <param name="strName">单据</param>
    /// <param name="OrderId">订单ID</param>
    /// <returns></returns>
    public static string GetCode(string strName,string OrderId)
    {
        string returnstr = "";
        try
        {
            int sort = 0;
            returnstr += "-";
            if (strName == "发货单")
            {
                List<Hi.Model.DIS_OrderOut> outl = new Hi.BLL.DIS_OrderOut().GetList("", " OrderID=" + OrderId + " and dr=0", "");
                sort = outl.Count;
                returnstr += "F";
            }
            else if (strName == "退单")
            {
                List<Hi.Model.DIS_OrderReturn> rl = new Hi.BLL.DIS_OrderReturn().GetList("", " OrderID=" + OrderId + " and dr=0", "");
                sort = rl.Count;
                returnstr += "T";
            }

            sort++;
            if (sort < 10)
                returnstr += "0" + sort;
            else
                returnstr += sort;
        }
        catch (Exception)
        {}
        return returnstr;
    }

    /// <summary>
    /// 获得账单最新Code
    /// </summary>
    /// <param name="strName">获取要那个对象的编号</param>
    /// <param name="number">对应sys_syscode表的序列</param>
    /// <returns></returns>
    public static string GetZD_NewCode(string strName,int number)
    {
        string returnstr = "";
        try
        {
            List<Hi.Model.SYS_SysName> NameModel = new Hi.BLL.SYS_SysName().GetList("", "CompID=" + number + " and Name='" + strName + "'", "");
            if (NameModel != null)
            {
                //string OrgCode = ConfigurationManager.AppSettings["OrgCode"] == null ? "" : ConfigurationManager.AppSettings["OrgCode"].ToString().Trim();
                //string value = NameModel[0].Value;
                string yyyy = DateTime.Today.Year.ToString().PadLeft(4, '0');
                string mm = DateTime.Today.Month.ToString().PadLeft(2, '0');
                string dd = DateTime.Today.Day.ToString().PadLeft(2, '0');

                string codeName = string.Empty;
                
                codeName = yyyy + mm + dd;

                List<Hi.Model.SYS_SysCode> CodeModel = new Hi.BLL.SYS_SysCode().GetList("", "CompID=" + number + "  and CodeName='" + codeName + "'", "");
                if (CodeModel.Count > 0)
                {
                    int codeValue = Convert.ToInt32(CodeModel[0].CodeValue);
                    int newCode = codeValue + 1;
                    string newCodeStr = "";
                   
                    newCodeStr = codeName + "-" + newCode.ToString().PadLeft(6, '0');
                    //returnstr = OrgCode + value + "-" + newCodeStr;
                    returnstr = "MZ-" + newCodeStr;

                    //修改最新值
                    SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, "update SYS_SysCode set CodeValue=" + newCode + " where CodeName='" + codeName + "'");

                }
                else
                {
                    string newCodeStr = "";
                    newCodeStr = codeName + "-000001";
                    
                    //returnstr = OrgCode + value + "-" + newCodeStr;
                    returnstr = "MZ-" + newCodeStr;

                    SqlHelper.ExecuteSql(SqlHelper.LocalSqlServer, "insert into SYS_SysCode(CompID,CodeName,CodeValue,ts,modifyuser) values(" + number + ",'" + codeName + "',1,'" + DateTime.Now + "',0)");
                }
            }
        }
        catch { }
        return returnstr;
    }

}