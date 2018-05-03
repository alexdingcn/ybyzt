using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using LitJson;
using Microsoft.SqlServer.Server;
using DBUtility;
using System.Data;

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
                return new ResultList() { Result = "F", Description = "参数异常" };
            }

            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
            if (dis == null || dis.dr == 1 || dis.IsEnabled == 0)
                return new ResultList() { Result = "F", Description = "经销商信息异常" };

            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(userID), out user, 0, int.Parse(disID)))
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
                return new ResultList() { Result = "T", Description = "未找到绑定企业" };
            }
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetResellerCompany ：" + JSon);
            return new ResultList() { Result = "F", Description = "异常" };
        }
    }


    /// <summary>
    /// 获得企业列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public ResultList GetCompanyList(string JSon)
    {
        try
        {
            string userID = string.Empty;

            JsonData JInfo = JsonMapper.ToObject(JSon);

            List<Company> compList = new List<Company>();

            List<Hi.Model.BD_Company> ListComp = new Hi.BLL.BD_Company().GetList("*", " isnull(dr,0)=0 and AuditState=2 and IsEnabled=1", "ID desc");

            if (ListComp != null)
            {
                foreach (Hi.Model.BD_Company comp in ListComp)
                {
                    Company com = new Company();
                    com.CompanyID = comp.ID;
                    com.CompanyName = comp.CompName;
                    com.CompLogo = comp.CompLogo;
                    com.Contact = comp.Principal;
                    com.ContactPhone = comp.Phone;
                    com.Address = comp.CompAddr + comp.Address;
                    compList.Add(com);
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
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetCompanyList ：" + JSon);
            return new ResultList() { Result = "F", Description = "异常" };
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
            if (ListCompUser != null && ListCompUser.Count > 0)
            {
                foreach (var item in ListCompUser)
                {
                    if (item.CType == 2)
                    {
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
    /// 获取招商列表
    /// </summary>
    /// <param name="JSon"></param>
    /// <returns></returns>
    public FCMerchantList GetMerchantList(string JSon)
    {
        try
        {
            string userID = string.Empty;
            string fcId = string.Empty;
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0)
            {
                fcId = JInfo.Keys.Contains("FcID") ? JInfo["FcID"].ToString() : "";
            }

            string gtypeids = string.Empty;
            string strsql = @"select yc.ID, bc.ShortName, bc.CompLogo, g.CategoryID, yc.CMName, yc.GoodsName,  yc.Remark, yc.ValueInfo,
	                   yc.GoodsCode, yc.ForceDate, yc.InvalidDate, yc.ProvideData, g.ID as GoodsID, yc.CompID
                from YZT_CMerchants yc
                    left join BD_Company bc on bc.ID = yc.CompID
                    left join BD_GoodsInfo info on info.ID=yc.GoodsID
                    left join BD_Goods g on g.ID=info.GoodsID 
                    left join SYS_GType gt on gt.ID=g.CategoryID
                where isnuLL(gt.dr,0)=0 and isnuLL(gt.IsEnabled,0)=1 
                    and yc.IsEnabled = 1 and isnull(yc.dr, 0) = 0
                    and yc.ForceDate < getdate() and yc.InvalidDate >= getdate()";

            if (!string.IsNullOrEmpty(fcId)) {
                strsql += " and yc.ID= " + fcId;
            }

            strsql += "order by yc.ID desc";

            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strsql);
            List<Campaign> campaignList = new List<Campaign>();

            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        Campaign campaign = new Campaign();
                        campaign.CompID = Convert.ToInt32(row["CompID"]);
                        campaign.CampaignID = Convert.ToInt32(row["ID"]);
                        campaign.CompanyName = Convert.ToString(row["ShortName"]);
                        campaign.CompLogoUrl = Convert.ToString(row["CompLogo"]);
                        campaign.CategoryID = Convert.ToInt32(row["CategoryID"]);
                        campaign.CMName = Convert.ToString(row["CMName"]);
                        campaign.GoodsName = Convert.ToString(row["GoodsName"]);
                        campaign.Remark = Convert.ToString(row["Remark"]);
                        campaign.ValueInfo = Convert.ToString(row["ValueInfo"]);
                        campaign.GoodsCode = Convert.ToString(row["GoodsCode"]);
                        campaign.StartDate = Convert.ToDateTime(row["ForceDate"]).ToString("yyyy-MM-dd");
                        campaign.EndDate = Convert.ToDateTime(row["InvalidDate"]).ToString("yyyy-MM-dd");
                        campaign.Province = Convert.ToString(row["ProvideData"]);
                        campaign.GoodsID = Convert.ToInt32(row["GoodsID"]);
                        campaignList.Add(campaign);
                    }
                }
            }


            return new FCMerchantList()
            {
                Result = "T",
                Description = "获取成功",
                CampaignList = campaignList
            };
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetMerchantList ：" + JSon);
            return new FCMerchantList() { Result = "F", Description = "异常" };
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
                return new ResultVersion() { Result = "F", Description = "版本配置文件没找到!" };
            }
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetVersion");
            return new ResultVersion() { Result = "F", Description = "版本配置文件异常" };
        }

        return new ResultVersion() { Result = "F", Description = "异常" };
    }

    #region 返回

    public class ResultList
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<Company> CompanyList { get; set; }
    }

    public class FCMerchantList
    {
        public string Result { get; set; }
        public string Description { get; set; }
        public List<Campaign> CampaignList { get; set; }
    }

    public class Company
    {
        public int CompanyID { get; set; }
        public string CompanyName { get; set; }
        public string CompLogo { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public string ContactPhone { get; set; }

    }


    public class Campaign
    {
        public int CampaignID { get; set; }
        public string CompanyName { get; set; }
        public string CompLogoUrl { get; set; }
        public string GoodsName { get; set; }
        public string CMName { get; set; }
        public string GoodsCode { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int CategoryID { get; set; }
        public string Remark { get; set; }
        public string ValueInfo { get; set; }
        public int CompID { get; set; }
        public int GoodsID { get; set; }
        public string Province { get; set; }
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

