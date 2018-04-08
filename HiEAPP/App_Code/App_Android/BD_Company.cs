using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using LitJson;
using Microsoft.SqlServer.Server;

public class BD_Company
{
    public BD_Company()
    {
    }

    /// <summary>
    /// 通过经销商ID获取绑定的企业
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultList GetResellerCompany(string JSon)
    {
        try
        {
            string userID = string.Empty;
            string disID = string.Empty;
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "" && JInfo["ResellerID"].ToString() != "")
            {
                if (JInfo["UserID"].ToString() != "")
                {
                    userID = JInfo["UserID"].ToString();
                }
                if (JInfo["ResellerID"].ToString() != "")
                {
                    disID = JInfo["ResellerID"].ToString();
                }
            }
            else
            {
                return new ResultList() {Result = "F", Description = "参数异常"};
            }
            
            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
            if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                return new ResultList() {Result = "F", Description = "经销商信息异常"};

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user,0, int.Parse(disID)))
                return new ResultList() { Result = "F", Description = "登录信息异常" };

            string strWhere = " ID = '" + dis.CompID + "' and ISNULL(dr,0)=0 and IsEnabled = 1 ";
            List<Hi.Model.BD_Company> list = new Hi.BLL.BD_Company().GetList("", strWhere, "");
            if (list != null && list.Count > 0)
            {
                List<Company> compList = new List<Company>();
                foreach (Hi.Model.BD_Company comp in list)
                {
                    Company com = new Company();
                    com.CompanyID = comp.ID;
                    com.CompanyName = comp.CompName;
                    compList.Add(com);
                }
                return new ResultList()
                {
                    Result = "T",
                    Description = "获取成功",
                    CompanyList = compList
                };
            }
            else
            {
                return new ResultList() {Result = "T", Description = "未找到绑定企业"};
            }
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerCompany ：" + JSon);
            return new ResultList() {Result = "F", Description = "异常"};
        }
    }

      /// <summary>
    /// 通过经销商ID获取绑定的企业
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultList GetUserCompany(string JSon)
    {
        try
        {

            string userID = string.Empty;
           
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString() != "")
            {
                if (JInfo["UserID"].ToString() != "")
                {
                    userID = JInfo["UserID"].ToString();
                }
            }
            else
            {
                return new ResultList() { Result = "F", Description = "参数异常" };
            }

            List<Company> compList = new List<Company>();

            List<Hi.Model.BD_Company> ListComp = new List<Hi.Model.BD_Company>();

            List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("*", " dr=0 and  Userid=" + userID + " and IsAudit=2  ", " createdate ");

            string Compid = string.Empty;
            if (ListCompUser != null && ListCompUser.Count > 0) {
                foreach (var item in ListCompUser)
                {
                    if (item.CType == 2) {
                        Compid += Compid == "" ? item.CompID.ToString() : "," + item.CompID;
                    }
                }
            }

            string[] CompDis = Compid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            if (Compid != "")
            {
                ListComp = new Hi.BLL.BD_Company().GetList("ID,CompName", " isnull(dr,0)=0 and AuditState=2 and IsEnabled=1  and ID in(" + Compid + ")", "createdate");

                if (ListComp != null && ListComp.Count > 0)
                {
                    foreach (Hi.Model.BD_Company comp in ListComp)
                    {
                        Company com = new Company();
                        com.CompanyID = comp.ID;
                        com.CompanyName = comp.CompName;
                        compList.Add(com);
                    }
                }
            }

            return new ResultList()
            {
                Result = "T",
                Description = "获取成功",
                CompanyList = compList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetUserCompany ：" + JSon);
            return new ResultList() { Result = "F", Description = "异常" };
        }
    }

    /// <summary>
    /// 获取app版本
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultVersion GetVersion()
    {
        bool result = false;
        string description = string.Empty;
        string version = string.Empty;
        string url = string.Empty;
        string VerName = string.Empty;
        string VerLog = string.Empty;
        string VIU = string.Empty;
        string PackageSize = string.Empty;
        try
        {
            string pathxml = HttpRuntime.AppDomainAppPath.ToString() + "APP\\VerControl.xml";
            if (File.Exists(pathxml))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(pathxml);
                //读取Alipay节点下的数据。SelectSingleNode匹配第一个Alipay节点
                XmlNode root = doc.SelectSingleNode("Version");

                if (root != null)
                {
                    result = true;
                    if (root.SelectSingleNode("CompControl") != null)
                        version = (root.SelectSingleNode("CompControl")).InnerText;
                    if (root.SelectSingleNode("CompURL") != null)
                        url = (root.SelectSingleNode("CompURL")).InnerText;
                    if (root.SelectSingleNode("CompVerName") != null)
                        VerName = (root.SelectSingleNode("CompVerName")).InnerText;
                    if (root.SelectSingleNode("CompVerLog") != null)
                        VerLog = (root.SelectSingleNode("CompVerLog")).InnerText;
                    if (root.SelectSingleNode("VIU") != null)
                        VIU = (root.SelectSingleNode("VIU")).InnerText;
                    if (root.SelectSingleNode("PackageSize") != null)
                        PackageSize = (root.SelectSingleNode("PackageSize")).InnerText;
                    return new ResultVersion()
                    {
                        Result = result ? "T" : "F",
                        Description = description,
                        Version = version,
                        Url = url,
                        VerName = VerName,
                        VerLog = VerLog,
                        VIU = VIU,
                        PackageSize = PackageSize
                    };
                }
            }
            else
            {
                return new ResultVersion() {Result = "F", Description = "版本配置文件没找到!"};
            }
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetVersion");
            return new ResultVersion() {Result = "F", Description = "版本配置文件异常"};
        }

        return new ResultVersion() {Result = "F", Description = "异常"};
    }

    #region 返回

    public class ResultList
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<Company> CompanyList { get; set; }
    }

    public class Company
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
    }

    public class ResultVersion
    {
        public string Result;
        public string Description;
        public string Version;
        public string Url;
        public string VerName;
        public string VerLog;
        public string VIU;
        public string PackageSize;
    }

    #endregion
}

