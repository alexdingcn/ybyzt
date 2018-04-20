using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using Hi.SQLServerDAL;
using DBUtility;
using System.Data;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Reflection;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using NPOI;
using NPOI.OpenXml4Net;
using NPOI.OpenXmlFormats;
using NPOI.XSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.SS.Formula.Eval;
using System.Data.SqlClient;
using System.Web.Configuration;
//using Test801;
using WebReference;
using MY.Client;
using System.Text.RegularExpressions;

/// <summary>
///Common 的摘要说明
/// </summary>
public class Common
{
    static string e = string.Empty;
    public static string aredIdList = string.Empty;
    public static string typeIdList = string.Empty;
    public static string cateIdList = string.Empty;
    public static string EncryptKey = "HaiYuSE9SFOT";
    public Common()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 是否启用代理商价格维护  by szj 2015-09-14
    /// </summary>
    public static string IsDisPrice
    {
        get
        {
            string IsDisPrice = string.Empty;
            IsDisPrice = ConfigurationManager.AppSettings["IsDisPrice"] != null ? ConfigurationManager.AppSettings["IsDisPrice"].ToString() : "0";
            return IsDisPrice;
        }
    }

    /// <summary>
    /// 获取代理商的值
    /// </summary>
    /// <param name="Id">代理商Id</param>
    /// <param name="Con">列名</param>
    /// <returns></returns>
    public static string GetDis(int Id, string Column)
    {
        string dis = string.Empty;

        string sql = "select * from BD_Distributor where  Id=" + Id;  //ISNULL(dr,0)=0 and   edit by hgh

        DataTable l = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

        if (l != null)
        {
            if (l.Rows.Count > 0)
            {
                dis = l.Rows[0][Column].ToString();
            }
        }
        return dis;
    }
    public static string Getcom(int Id, string Column)
    {
        string dis = string.Empty;

        string sql = "select * from BD_Company where ISNULL(dr,0)=0 and Id=" + Id;

        DataTable l = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

        if (l != null)
        {
            if (l.Rows.Count > 0)
            {
                dis = l.Rows[0][Column].ToString();
            }
        }
        return dis;
    }
    /// <summary>
    /// 设置每页显示条数
    /// </summary>
    /// <returns></returns>
    public static string PageSize
    {
        get
        {
            string size = ConfigurationManager.AppSettings["PageSize"].ToString().Trim();

            if (!string.IsNullOrEmpty(size))
            {
                //默认每页显示条数
                size = "12";
            }
            return size;
        }
    }

    /// <summary>
    /// 代理商收货地址
    /// </summary>
    /// <param name="Id">收货地址Id</param>
    /// <returns></returns>
    public static string GetAddr(int Id)
    {
        Hi.Model.BD_DisAddr DisAddrModel = new Hi.BLL.BD_DisAddr().GetModel(Id);

        if (DisAddrModel != null)
        {
            //return DisAddrModel.Province + DisAddrModel.City + DisAddrModel.Area + DisAddrModel.Address;
            return DisAddrModel.Address;
        }
        return "";
    }
    /// <summary>
    /// 绑定商品大类
    /// </summary>
    public static void BindGoodsType(DropDownList ddl)
    {
        List<Hi.Model.SYS_GoodsType> ll = new Hi.BLL.SYS_GoodsType().GetList("", "ISNULL(dr,0)=0 and isenabled=1", " cast(SortIndex as int)");
        ddl.DataSource = ll;
        ddl.DataTextField = "GoodsTypeName";
        ddl.DataValueField = "id";
        ddl.DataBind();
    }
    /// <summary>
    /// 绑定单位
    /// </summary>
    public static void BindUnit(Repeater rpt, string AtName, int compid)
    {
        List<Hi.Model.BD_DefDoc> l = new Hi.BLL.BD_DefDoc().GetList("", "AtName='" + AtName + "' and compid=" + compid + " and isnull(dr,0)=0", "");
        if (l.Count > 0)
        {
            List<Hi.Model.BD_DefDoc_B> ll = new Hi.BLL.BD_DefDoc_B().GetList("", "DefID=" + l[0].ID + "and ISNULL(dr,0)=0 and compid=" + compid, "id asc");
            //drpList.DataSource = ll;
            //drpList.DataTextField = "AtVal";
            //drpList.DataValueField = "id";
            //drpList.DataBind();
            rpt.DataSource = ll;
            rpt.DataBind();
        }
    }
    /// <summary>
    /// 绑定规格模板
    /// </summary>
    public static void BindTemplate(DropDownList ddl, int compid)
    {
        List<Hi.Model.BD_Template> ll = new Hi.BLL.BD_Template().GetList("", "isnull(dr,0)=0 and compid=" + compid, "");
        if (ll.Count > 0)
        {
            ddl.DataSource = ll;
            ddl.DataTextField = "TemplateName";
            ddl.DataValueField = "ID";
            ddl.DataBind();
            ddl.Items.Insert(0, new ListItem("请选择模板", ""));
        }
    }


    /// <summary>
    /// 绑定销售人员
    /// </summary>
    public static void BindMan(Repeater rpt, int compid)
    {
        List<Hi.Model.BD_DisSalesMan> l = new Hi.BLL.BD_DisSalesMan().GetList("", "compid=" + compid + " and isnull(dr,0)=0", "");
        rpt.DataSource = l;
        rpt.DataBind();
    }

    /// <summary>
    /// 绑定销售人员名称
    /// </summary>
    public static string BindManName(int ID, int compid)
    {
        List<Hi.Model.BD_DisSalesMan> l = new Hi.BLL.BD_DisSalesMan().GetList("", "ID=" + ID + " and compid=" + compid + " and isnull(dr,0)=0", "");
        if (l.Count > 0)
        {
            if (l[0].ParentID == 0)
            {
                return l[0].SalesName;
            }
            else
            {
                return l[0].SalesName;
            }
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// 得到系统名称
    /// </summary>
    /// <returns></returns>
    public static string GetWebTitle()
    {
        return GetWebConfigKey("WebTitle");
    }


    public static string GetWebConfigKey(string Name)
    {
        string keyvalue = "";
        if (Name != "")
        {
            keyvalue = ConfigurationManager.AppSettings["" + Name + ""].ToString().Trim();
        }
        return keyvalue;
    }


    /// <summary>
    /// 绑定系统字典属性值
    /// </summary>
    /// <param name="drpList">下拉框</param>
    /// <param name="CustomName">自定义属性名称</param>
    public static void BindCustomDDL(DropDownList drpList, string CustomName, int Compid)
    {
        List<Hi.Model.BD_DefDoc_B> ll = new Hi.BLL.BD_DefDoc_B().GetList("Atval,ID", " AtName='" + CustomName + "' and ISNULL(dr,0)=0 and CompId=" + Compid + "", "");
        drpList.DataSource = ll;
        drpList.DataTextField = "Atval";
        drpList.DataValueField = "ID";
        drpList.DataBind();
    }

    /// <summary>
    /// 绑定企业权限
    /// </summary>
    public static void BindRoleDDL(DropDownList ddlRoleId)
    {
        List<Hi.Model.SYS_Role> ll = new Hi.BLL.SYS_Role().GetList("RoleName,ID", "IsEnabled=1 and ISNULL(dr,0)=0 and CompID=" + CompID(), "");
        ddlRoleId.DataSource = ll;
        ddlRoleId.DataTextField = "RoleName";
        ddlRoleId.DataValueField = "ID";
        ddlRoleId.DataBind();
    }

    /// <summary>
    /// 绑定企业权限
    /// </summary>
    public static void BindRoleDDL(DropDownList ddlRoleId, string Comid)
    {
        List<Hi.Model.SYS_Role> ll = new Hi.BLL.SYS_Role().GetList("RoleName,ID", "IsEnabled=1 and ISNULL(dr,0)=0 and DisID=0 and CompID=" + Comid, "");
        ddlRoleId.DataSource = ll;
        ddlRoleId.DataTextField = "RoleName";
        ddlRoleId.DataValueField = "ID";
        ddlRoleId.DataBind();
    }

    /// <summary>
    /// 绑定行业类别
    /// </summary>
    public static void BindIndDDL(DropDownList ddlRoleId)
    {
        List<Hi.Model.SYS_GType> ll = new Hi.BLL.SYS_GType().GetList("*", "IsEnabled=1 and ISNULL(dr,0)=0 and parentid=0", "");
        if (ll != null && ll.Count > 0)
        {
            ddlRoleId.DataSource = ll;
            ddlRoleId.DataTextField = "typename";
            ddlRoleId.DataValueField = "ID";
            ddlRoleId.DataBind();
            ddlRoleId.Items.Insert(0, new ListItem("请选择", "-1"));
            ddlRoleId.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// 绑定机构
    /// </summary>
    public static void BindOrg(DropDownList ddlRoleId, string type)
    {
        List<Hi.Model.BD_Org> ll = new Hi.BLL.BD_Org().GetList("OrgName,ID", "IsEnabled=1 and ISNULL(dr,0)=0", "");
        if (ll != null && ll.Count > 0)
        {
            ddlRoleId.DataSource = ll;
            ddlRoleId.DataTextField = "OrgName";
            ddlRoleId.DataValueField = "ID";
            ddlRoleId.DataBind();
            ddlRoleId.Items.Insert(0, new ListItem("请选择", "-1"));
            ddlRoleId.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// 绑定业务员
    /// </summary>
    public static void BindSaleMan(DropDownList ddlRoleId, int OrgID)
    {
        List<Hi.Model.BD_SalesMan> ll = new List<Hi.Model.BD_SalesMan>();
        if (OrgID == 0)
        {
            ll = new Hi.BLL.BD_SalesMan().GetList("SalesName,ID", "IsEnabled=1 and ISNULL(dr,0)=0", "");
        }
        else
        {
            ll = new Hi.BLL.BD_SalesMan().GetList("SalesName,ID", "IsEnabled=1 and ISNULL(dr,0)=0 and OrgID=" + OrgID, "");
        }
        if (ll != null && ll.Count > 0)
        {
            ddlRoleId.DataSource = ll;
            ddlRoleId.DataTextField = "SalesName";
            ddlRoleId.DataValueField = "ID";
            ddlRoleId.DataBind();
            ddlRoleId.Items.Insert(0, new ListItem("全部", "-1"));
            ddlRoleId.SelectedIndex = 0;
        }
    }

    /// <summary>
    /// 机构用户登录
    /// </summary>
    public static void BindOrgSale(DropDownList Org, DropDownList SaleMan, string type)
    {
        Hi.Model.SYS_AdminUser model = HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser;
        if (model != null)
        {
            if (model.UserType == 3 || model.UserType == 4)
            {
                Common.BindOrg(Org, type);
                Org.SelectedIndex=0 ;
                Org.Attributes.Add("Disabled", "true");
                if (SaleMan != null)
                {
                    Common.BindSaleMan(SaleMan, model.OrgID);
                }
            }
            else
            {
                Common.BindOrg(Org, type);
            }

        }
    }

    /// <summary>
    /// 获取业务员
    /// </summary>
    public static string getsaleman(string OrgID, bool IsSet = false)
    {
        if (IsSet)
        {
            Hi.Model.SYS_AdminUser AdminUserModel = HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser;
            if (AdminUserModel != null)
            {
                if (AdminUserModel.UserType == 3 || AdminUserModel.UserType == 4)
                {
                    OrgID = AdminUserModel.OrgID.ToString();
                }
            }
        }
        List<Hi.Model.BD_SalesMan> ll = new Hi.BLL.BD_SalesMan().GetList("", "IsEnabled=1 and ISNULL(dr,0)=0 and OrgID='" + OrgID + "'", "");
        string l = Newtonsoft.Json.JsonConvert.SerializeObject(ll);
        return l;
    }

    /// <summary>
    /// 获取业务员名称
    /// </summary>
    public static string getsalemanName(string salemanID)
    {
        List<Hi.Model.BD_SalesMan> ll = new Hi.BLL.BD_SalesMan().GetList("", "IsEnabled=1 and ISNULL(dr,0)=0 and ID='" + salemanID + "'", "");
        if (ll.Count > 0)
        {
            return ll[0].SalesName;
        }
        else
        {
            return "无此务员！";
        }
    }
    /// <summary>
    /// 根据id获取区域名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetDisAreaNameById(int id)
    {
        Hi.Model.BD_DisArea area = new Hi.BLL.BD_DisArea().GetModel(id);
        if (area != null)
        {
            return area.AreaName;
        }
        return "";
    }

    /// <summary>
    /// 根据id获取区域名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetDisAreaNameById(object disid,object compid)
    {
        List<Hi.Model.SYS_CompUser> ListComp = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and Ctype=2 and Utype in(1,5) and DisID=" + disid + " and CompID=" + compid, "");

        if (ListComp != null && ListComp.Count > 0)
        {
            Hi.Model.BD_DisArea area = new Hi.BLL.BD_DisArea().GetModel(ListComp[0].AreaID);
            if (area != null)
            {
                return area.AreaName;
            }
        }
        return "";
    }


    /// <summary>
    /// 根据id获取分类名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetDisTypeNameById(object disid, object compid)
    {
         List<Hi.Model.SYS_CompUser> ListComp = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and Ctype=2 and Utype in(1,5) and DisID=" + disid + " and CompID=" + compid, "");

         if (ListComp != null && ListComp.Count > 0)
         {
             Hi.Model.BD_DisType area = new Hi.BLL.BD_DisType().GetModel(ListComp[0].DisTypeID);
             if (area != null)
             {
                 return area.TypeName;
             }
         }
        return "";
    }

    /// <summary>
    /// 根据id获取分类名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetDisTypeNameById(int id)
    {
        Hi.Model.BD_DisType area = new Hi.BLL.BD_DisType().GetModel(id);
        if (area != null)
        {
            return area.TypeName;
        }
        return "";
    }

    /// <summary>
    /// 获取管理平台用户ID
    /// </summary>
    /// <returns></returns>
    public static int LoginID()
    {
        Hi.Model.SYS_AdminUser AdminUserModel = HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser;

        if (AdminUserModel != null)
        {
            return AdminUserModel.ID;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// 获取管理平台用户名
    /// </summary>
    /// <returns></returns>
    public static string LoginName()
    {
        Hi.Model.SYS_AdminUser AdminUserModel = HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser;

        if (AdminUserModel != null)
        {
            return AdminUserModel.LoginName;
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    ///  获取代理商ID
    /// </summary>
    /// <returns></returns>
    public static int DisID()
    {
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;

        if (model != null)
        {
            return model.DisID;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 获取代理商名称
    /// </summary>
    /// <returns></returns>
    public static string DisName()
    {
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;

        if (model != null)
        {
            return model.DisName;
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 获取用户ID
    /// </summary>
    /// <returns></returns>
    public static int UserID()
    {
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;

        if (model != null)
        {
            return model.UserID;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// 获取用户名
    /// </summary>
    /// <returns></returns>
    public static string UserName()
    {
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;

        if (model != null)
        {
            return model.UserName;
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// 获取企业ID
    /// </summary>
    /// <returns></returns>
    public static int CompID()
    {
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;

        if (model != null)
        {
            return model.CompID;
        }
        else
        {
            return 0;
        }
    }

    /// <summary>
    /// 获取代理商来源
    /// </summary>
    /// <returns></returns>
    public static int Erptype()
    {
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;

        if (model != null)
        {
            return model.Erptype;
        }
        else
        {
            return 0;
        }
    }
    /// <summary>
    /// 获取用户类别
    /// </summary>
    /// <returns></returns>
    public static int TypeID()
    {
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;

        if (model != null)
        {
            return model.TypeID;
        }
        else
        {
            return 0;
        }
    }


    /// <summary>
    /// 获取Users表用户名
    /// </summary>
    /// <param name="Id">用户Id</param>
    /// <returns></returns>
    public static string GetUserName(int Id)
    {
        Hi.Model.SYS_Users usersModel = new Hi.BLL.SYS_Users().GetModel(Id);
        if (usersModel != null)
        {
            return usersModel.TrueName != "" ? usersModel.TrueName : usersModel.UserName;
        }
        return "";
    }


    public static object GetDisValue(int id, string Name)
    {
        object value = null;
        object model = new Hi.BLL.BD_Distributor().GetModel(id);
        if (model != null)
        {
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                if (info.Name.ToLower() == Name.ToLower())
                {
                    return info.GetValue(model, null);
                }
            }
        }
        return value;
    }

    public static object GetOrgValue(int id, string Name)
    {
        object value = null;
        object model = new Hi.BLL.BD_Org().GetModel(id);
        if (model != null)
        {
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                if (info.Name.ToLower() == Name.ToLower())
                {
                    return info.GetValue(model, null);
                }
            }
        }
        return value;
    }

    public static object GetSaleManValue(int id, string Name)
    {
        object value = null;
        object model = new Hi.BLL.BD_SalesMan().GetModel(id);
        if (model != null)
        {
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                if (info.Name.ToLower() == Name.ToLower())
                {
                    return info.GetValue(model, null);
                }
            }
        }
        return value;
    }

    public static object GetDisSMValue(int id, string Name)
    {
        object value = null;
        object model = new Hi.BLL.BD_DisSalesMan().GetModel(id);
        if (model != null)
        {
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                if (info.Name.ToLower() == Name.ToLower())
                {
                    return info.GetValue(model, null);
                }
            }
        }
        return value;
    }


    public static object GetUserValue(int id, string Name)
    {
        object value = null;
        object model = new Hi.BLL.SYS_Users().GetModel(id);
        if (model != null)
        {
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                if (info.Name.ToLower() == Name.ToLower())
                {
                    return info.GetValue(model, null);
                }
            }
        }
        return value;
    }

    /// <summary>
    /// 获取订单Value
    /// </summary>
    /// <returns></returns>
    public static object GetOrderValue(int id, string Name)
    {
        object value = null;
        object model = new Hi.BLL.DIS_Order().GetModel(id);
        if (model != null)
        {
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                if (info.Name.ToLower() == Name.ToLower())
                {
                    return info.GetValue(model, null);
                }
            }
        }
        return value;
    }

    /// <summary>
    /// 获取厂商名称
    /// </summary>
    /// <returns></returns>
    public static object GetCompValue(int id, string Name)
    {
        object value = null;
        object model = new Hi.BLL.BD_Company().GetModel(id);
        if (model != null)
        {
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                if (info.Name.ToLower() == Name.ToLower())
                {
                    return info.GetValue(model, null);
                }
            }
        }
        return value;
    }

    /// <summary>
    /// 获取商品名称
    /// </summary>
    /// <returns></returns>
    public static string GetGoodsName(string id)
    {
        if (!Util.IsEmpty(id))
        {
            Hi.Model.BD_Goods mode = new Hi.BLL.BD_Goods().GetModel(Convert.ToInt32(id));
            if (mode != null)
            {
                return mode.GoodsName.ToString();
            }
            else
            {

                return "";
            }
        }
        else
        {

            return "";
        }
    }

    /// <summary>
    /// 获取商品名称
    /// </summary>
    /// <returns></returns>
    public static string GetGoodsName(string id,string col)
    {
        if (!Util.IsEmpty(id))
        {
            string sql = "select info.BarCode GoodsCode,g.GoodsName,g.CategoryID from BD_Goods g left join  BD_GoodsInfo info on info.GoodsID=g.ID where info.ID=" + id;
            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                if (col == "GoodsName")
                    return dt.Rows[0]["GoodsName"].ToString();
                else if (col == "GoodsCode")
                    return dt.Rows[0]["GoodsCode"].ToString();
                else
                    return dt.Rows[0]["CategoryID"].ToString();
            }
            else
            {
                return "";
            }
        }
        else
        {

            return "";
        }
    }

    /// <summary>
    /// 获取商品详细属性
    /// </summary>
    /// <returns></returns>
    public static string GetGoodsInfo(string id)
    {
        if (!Util.IsEmpty(id))
        {
            Hi.Model.BD_GoodsInfo mode = new Hi.BLL.BD_GoodsInfo().GetModel(Convert.ToInt32(id));
            if (mode != null)
            {
                return mode.ValueInfo.ToString().Replace(';', '；'); ;
            }
            else
            {

                return "";
            }
        }
        else
        {

            return "";
        }
    }

    public static bool GetUserExists(string Uname)
    {
        List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("", " username='" + Uname + "' and isnull(dr,0)=0", "");
        return user.Count > 0;
    }
    /// <summary>
    /// 递归得到区域Id
    /// </summary>
    public static string DisAreaId(int id, int compId, string aredId = "")
    {
        aredIdList = aredId;
        List<Hi.Model.BD_DisArea> l = new Hi.BLL.BD_DisArea().GetList("", "parentid=" + id + " and isnull(dr,0)=0 and CompanyID=" + compId, "");
        if (l.Count > 0)
        {
            foreach (Hi.Model.BD_DisArea item in l)
            {
                aredIdList += item.ID + ",";
                DisAreaId(item.ID, compId, aredIdList);
            }
        }
        return aredIdList + id;
    }
    /// <summary>
    /// 递归得到分类Id
    /// </summary>
    public static string DisTypeId(int id, int compId, string typeId = "")
    {
        typeIdList = typeId;
        List<Hi.Model.BD_DisType> l = new Hi.BLL.BD_DisType().GetList("", "parentid=" + id + " and isEnabled=0 and isnull(dr,0)=0 and compid=" + compId, "");
        if (l.Count > 0)
        {
            foreach (Hi.Model.BD_DisType item in l)
            {
                typeIdList += item.ID + ",";
                DisTypeId(item.ID, compId, typeIdList);
            }
        }
        return typeIdList + id;
    }
    /// <summary>
    /// 递归得到商品分类Id
    /// </summary>
    public static string CategoryId(int id, int compId, string typeId = "")
    {
        cateIdList = typeId;
        List<Hi.Model.SYS_GType> l = new Hi.BLL.SYS_GType().GetList("ID", "parentid=" + id + " and isEnabled=1 and isnull(dr,0)=0 ", "");
        if (l.Count > 0)
        {
            foreach (Hi.Model.SYS_GType item in l)
            {
                cateIdList += item.ID + ",";
                CategoryId(item.ID, compId, cateIdList);
            }
        }
        return cateIdList + id;
    }
    /// <summary>
    /// 递归得到商品分类Id
    /// </summary>
    public static string CategoryId1(int id, int compId, string typeId = "")
    {
        cateIdList = typeId;
        List<Hi.Model.BD_GoodsCategory> l = new Hi.BLL.BD_GoodsCategory().GetList("", "ID=" + id + " and isEnabled=1 and isnull(dr,0)=0 and compid=" + compId, "");
        if (l.Count > 0)
        {
            foreach (Hi.Model.BD_GoodsCategory item in l)
            {
                cateIdList += item.ParentId + ",";
                CategoryId1(item.ParentId, compId, cateIdList);
            }
        }
        return "," + cateIdList + id + ",";
    }
    /// <summary>
    /// 总后台递归得到商品分类Id
    /// </summary>
    public static string AdminCategoryId(int id, string typeId = "")
    {
        cateIdList = typeId;
        List<Hi.Model.BD_GoodsCategory> l = new Hi.BLL.BD_GoodsCategory().GetList("", "parentid=" + id + " and isEnabled=1 and isnull(dr,0)=0", "");
        if (l.Count > 0)
        {
            foreach (Hi.Model.BD_GoodsCategory item in l)
            {
                cateIdList += item.ID + ",";
                AdminCategoryId(item.ID, cateIdList);
            }
        }
        return cateIdList + id;
    }
    /// <summary>
    /// 得到类别名称
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public static string GetCategoryName(string id)
    {
        string str = string.Empty;
        if (!Util.IsEmpty(id))
        {

            Hi.Model.SYS_GType model = new Hi.BLL.SYS_GType().GetModel(Convert.ToInt32(id));
            if (model != null)
            {
                str = model.TypeName;
            }
        }
        return str;
    }



    /// <summary>
    /// 得到融资交易明细类型
    /// </summary>
    /// <param name="ProductLine"></param>
    /// <returns></returns>
    public static string GetFinacingType(int TypeId)
    {
        string TypeName = "";

        if (Enum.IsDefined(typeof(Enums.FinacingType), TypeId))
        {
            TypeName = ((Enums.FinacingType)TypeId).ToString();
        }
        return TypeName;
    }

    /// <summary>
    /// 得到企业钱包表的款项类型
    /// </summary>
    /// <param name="ProductLine"></param>
    /// <returns></returns>
    public static string GetPrePayStartName(int PayTypeId)
    {
        string TypeName = "";

        if (Enum.IsDefined(typeof(Enums.PrePayType), PayTypeId))
        {
            TypeName = ((Enums.PrePayType)PayTypeId).ToString();
        }
        return TypeName;
    }

    /// <summary>
    /// 得到企业钱包审核状态
    /// </summary>
    /// <param name="ProductLine"></param>
    /// <returns></returns>
    public static string GetNameBYPreStart(int auditstartId)
    {
        string StartName = "";

        if (Enum.IsDefined(typeof(Enums.PrePayState), auditstartId))
        {
            StartName = ((Enums.PrePayState)auditstartId).ToString();
        }
        return StartName;
    }

    /// <summary>
    /// 得到企业钱包表的支付状态
    /// </summary>
    /// <param name="ProductLine"></param>
    /// <returns></returns>
    public static string GetNameBYPrePayMentStart(int paystartid)
    {
        string StartName = "";

        if (Enum.IsDefined(typeof(Enums.PrePayMentState), paystartid))
        {
            StartName = ((Enums.PrePayMentState)paystartid).ToString();
        }
        return StartName;
    }

    /// <summary>
    /// 获取确认状态
    /// </summary>
    /// <param name="vdef9">自定义字段9值显示</param>
    /// <param name="paytype">支付方式</param>
    /// <returns></returns>
    public static string GetVdef9(string vdef9,string paytype)
    {
        string str = string.Empty;
        if (paytype.Equals("线下支付"))
        {
            switch (vdef9)
            {
                case "1":
                    str = "确认";
                    break;
                case "2":
                    str = "作废";
                    break;
                default:
                    str = "待确认";
                    break;

            }
        }
        else
            str = "确认";

        return str;
    }

    [Serializable]
    public class ReturnMessge
    {
        public ReturnMessge()
        {
            Result = false;
            Msg = "";
            Code = "";
            Attr1 = "";
            Error = false;
            RegisterOrder = "";
            IsRegis = false;
            Id = 0;
            CompName = "";
            Name = "";
            Type = 0;
            IsMoreUser = false;
            Compid = 0;
            Href = "";
        }
        public bool Result { get; set; }
        public string Msg { get; set; }
        public string Code { get; set; }
        public string Attr1 { get; set; }
        public bool Error { get; set; }
        public string RegisterOrder { get; set; }
        public bool IsRegis { get; set; }
        [System.Web.Script.Serialization.ScriptIgnore]
        public int Id { get; set; }
        public string CompName { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public bool IsMoreUser { get; set; }
        public int Compid { get; set; }
        public string Href { get; set; }
    }

    [Serializable]
    public class Phonecode
    {
        public Phonecode()
        {
            Phone = "";
            PhoneCode = "";
            SendTime = DateTime.MinValue;
        }
        public string Phone { get; set; }
        public string PhoneCode { get; set; }
        public DateTime SendTime { get; set; }
        public object RegisOrder { get; set; }
    }

    public static string GetRomDomCode(int len)
    {
        string validateCode = CreateRandomCode(len);
        return validateCode;
    }
    public const string charcode = "0,1,2,3,4,5,6,8,9";
    /// <summary>
    /// 生成长度为6的随机数字字符串
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public static string CreateRandomCode(int n)
    {
        string[] CharArray = charcode.Split(',');//将字符串转换为字符数组
        string randomCode = "";
        int temp = -1;

        Random rand = new Random();
        for (int i = 0; i < n; i++)
        {
            if (temp != -1)
            {
                rand = new Random(i * temp * ((int)DateTime.Now.Ticks));
            }
            int t = rand.Next(CharArray.Length - 1);
            if (temp != -1 && temp == t)
            {
                return CreateRandomCode(n);
            }
            temp = t;
            randomCode += CharArray[t];
        }
        if (int.Parse(randomCode) <= 99999)
        {
            return CreateRandomCode(6);
        }
        else
        {
            return randomCode;
        }
    }

    /// <summary>
    /// 判断加盟商某属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool DisExistsAttribute(string name, string value, string CompID, string id = "")
    {
        bool exists = false;
        if (!string.IsNullOrEmpty(id))
        {
            List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList("id", " " + name + "='" + value + "' and id<>'" + id + "' and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        else
        {

            List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList("id", " " + name + "='" + value + "' and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        return exists;
    }
    public static DataTable BindDisList(string compId)
    {
        //List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList("id,disname", "isnull(dr,0)=0 and CompID=" + compId, "");

        string sql = @"select cu.DisID id,dis.DisName disname from SYS_CompUser cu left join BD_Distributor dis on
cu.DisID=dis.ID where cu.CompID=" + compId + " and cu.CType=2 and UType=5 and cu.IsAudit=2 and cu.IsEnabled=1 and isnull(dis.dr,0)=0 and isnull(cu.dr,0)=0 order by cu.DisID desc";

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
        {
            return dt;
        }
        else
        {
            return null;
        }
    }
    /// <summary>
    /// 判断机构某属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool OrgExistsAttribute(string name, string value, string id = "")
    {
        bool exists = false;
        if (!string.IsNullOrEmpty(id))
        {
            List<Hi.Model.BD_Org> Dis = new Hi.BLL.BD_Org().GetList("", " " + name + "='" + value + "'   and id<>'" + id + "' and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        else
        {

            List<Hi.Model.BD_Org> Dis = new Hi.BLL.BD_Org().GetList("", " " + name + "='" + value + "'  and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        return exists;
    }

    /// <summary>
    /// 判断系统用户表某属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="id"></param>
    /// <returns></returns>

    public static bool SysUserExistsAttribute(string name, string value, string id = "")
    {
        bool exists = false;
        if (!string.IsNullOrEmpty(id))
        {
            List<Hi.Model.SYS_AdminUser> Dis = new Hi.BLL.SYS_AdminUser().GetList("", " " + name + "='" + value + "'   and id<>'" + id + "' and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        else
        {

            List<Hi.Model.SYS_AdminUser> Dis = new Hi.BLL.SYS_AdminUser().GetList("", " " + name + "='" + value + "'  and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        return exists;
    }

    /// <summary>
    /// 判断核心企企业某属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool CompExistsAttribute(string name, string value, string id = "")
    {
        bool exists = false;
        if (!string.IsNullOrEmpty(id))
        {
            List<Hi.Model.BD_Company> Dis = new Hi.BLL.BD_Company().GetList("id", " " + name + "='" + value + "'   and id<>'" + id + "' and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        else
        {

            List<Hi.Model.BD_Company> Dis = new Hi.BLL.BD_Company().GetList("id", " " + name + "='" + value + "'  and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        return exists;
    }

    public static bool GetUserExists(string Name, string Value, SqlTransaction Tran = null)
    {
        List<Hi.Model.SYS_Users> user = null;
        if (Tran == null)
            user = new Hi.BLL.SYS_Users().GetList("id", " " + Name + "='" + Value + "' and isnull(dr,0)=0", "");
        else
            user = new Hi.BLL.SYS_Users().GetList("id", " " + Name + "='" + Value + "' and isnull(dr,0)=0", "", Tran);
        return user.Count > 0;
    }

    public static bool GetUserExistsWhere(string Name, string Value, string Where, ref int Id, SqlTransaction Tran = null)
    {
        Id = 0;
        List<Hi.Model.SYS_Users> user = null;
        if (Tran == null)
            user = new Hi.BLL.SYS_Users().GetList("id", " " + Name + "='" + Value + "' " + Where + " and isnull(dr,0)=0", " createdate ");
        else
            user = new Hi.BLL.SYS_Users().GetList("id", " " + Name + "='" + Value + "' " + Where + "  and isnull(dr,0)=0", " createdate ", Tran);
        if (user.Count > 0)
        {
            Id = user[0].ID;
        }
        return user.Count > 0;
    }

    public static bool GetUserExists(string Name, string Value, string id, SqlTransaction Tran = null)
    {
        List<Hi.Model.SYS_Users> user = null;
        if (Tran == null)
            user = new Hi.BLL.SYS_Users().GetList("id", " " + Name + "='" + Value + "' and isnull(dr,0)=0 and id<>'" + id + "' ", "");
        else
            user = new Hi.BLL.SYS_Users().GetList("id", " " + Name + "='" + Value + "' and isnull(dr,0)=0 and id<>'" + id + "' ", "", Tran);
        return user.Count > 0;
    }


    /// <summary>
    /// 根据商品信息表Id查询图片
    /// </summary>
    /// <param name="goodInfoID"></param>
    /// <returns></returns>
    public static string picUrl(string goodInfoID)
    {
        string pic = "../../images/Goods400x400.jpg";
        Hi.Model.BD_GoodsInfo GoodsInfoModel = new Hi.BLL.BD_GoodsInfo().GetModel(goodInfoID.ToInt(0));

        if (GoodsInfoModel != null)
        {
            Hi.Model.BD_Goods goodsModel = new Hi.BLL.BD_Goods().GetModel(GoodsInfoModel.GoodsID);

            if (goodsModel != null)
            {
                if (!string.IsNullOrEmpty(goodsModel.Pic))
                {
                    pic = GetPicURL(goodsModel.Pic);
                }
            }
        }

        return pic;
    }
    

    /// <summary>  
    /// 截取字符串长度  
    /// </summary>  
    /// <param name="inputString">要截取的字符串对象</param>  
    /// <param name="len">要保留的字符个数</param>  
    /// <param name="suffix">后缀(用以替换超出长度部分)</param>  
    /// <returns></returns>  
    public static string MySubstring(string inputString, int len, string suffix)
    {
        ASCIIEncoding ascii = new ASCIIEncoding();
        int tempLen = 0;
        string tempString = "";
        byte[] s = ascii.GetBytes(inputString);
        for (int i = 0; i < s.Length; i++)
        {
            if ((int)s[i] == 63)
            {
                tempLen += 2;
            }
            else
            {
                tempLen += 1;
            }

            try
            {
                tempString += inputString.Substring(i, 1);
            }
            catch
            {
                break;
            }

            if (tempLen > len)
                break;
        }
        //如果截过则加上半个省略号 
        byte[] mybyte = System.Text.Encoding.Default.GetBytes(inputString);
        if (mybyte.Length > len)
            tempString += suffix;


        return tempString;
    }
    /// <summary>
    /// 判断是否可见价格
    /// </summary>
    /// <returns></returns>
    public static string GetPrice(int comPid, string GoodsId)
    {
        string str = "代理商可见";
        LoginModel model = HttpContext.Current.Session["UserModel"] as LoginModel;
        if (model != null)
        {
            if (comPid == model.CompID)
            {
                List<Hi.Model.BD_PromotionDetail> pd = new Hi.BLL.BD_PromotionDetail().GetList("", "isnull(dr,0)=0 and Goodsid=" + GoodsId, "ts");
                if (pd.Count > 0)
                {
                    for (int i = 0; i < pd.Count; i++)
                    {
                        List<Hi.Model.BD_Promotion> pt = new Hi.BLL.BD_Promotion().GetList("", "isnull(IsEnabled,0)=1 and isnull(dr,0)=0 and ('" + DateTime.Now.Date + "' between  ProStartTime and ProEndTime ) and id=" + pd[i].ProID, "");
                        if (pt.Count > 0)
                        {
                            if (pt[0].ProType.ToString() != "3")
                            {
                                str = pd[i].GoodsPrice.ToString("f2");
                                return str;
                            }
                        }
                    }
                }
                str = decimal.Parse(string.Format("{0:N2}", Common.GetGoodsinfoPrice(model.DisID, comPid, GoodsId))).ToString("#,##0.00");
            }
            else
            {
                str = "代理商可见";
            }
        }
        return str;
    }

    public static string GetGoodsinfoPrice(int DisID, int comPid, string GoodsId)
    {
        string Price = "0";

        string strWhere = " GoodsID=" + GoodsId + " and CompID=" + comPid + "and IsEnabled=1 and isnull(dr,0)=0";

        List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("top 1 *", strWhere, "");

        if (l != null && l.Count > 0)
        {
            foreach (Hi.Model.BD_GoodsInfo item in l)
            {
                List<Hi.Model.BD_GoodsPrice> ll = new Hi.BLL.BD_GoodsPrice().GetList("", "DisID=" + DisID + "and isnull(dr,0)=0 and isenabled=1 and compid=" + comPid + " and goodsinfoid=" + item.ID, "");
                if (ll.Count > 0)
                {
                    Price = ll[0].TinkerPrice.ToString();
                }
                else
                {
                    Price = item.TinkerPrice.ToString();
                }
            }
        }

        return Price;
    }

    /// <summary>
    /// 处理时间
    /// </summary>
    /// <returns></returns>
    public static string GetDateTime(DateTime date, string type)
    {
        if (date < DateTime.Parse("1991-01-01"))
        {
            return "";
        }
        return date.ToString(type);
    }

    /// <summary>
    /// 遍历
    /// </summary>
    /// <param name="al"></param>
    /// <returns></returns>
    public static string[] bianli(List<string[]> al)
    {
        if (al.Count == 0)
            return null;
        int size = 1;
        for (int i = 0; i < al.Count; i++)
        {
            size = size * al[i].Length;
        }
        string[] str = new string[size];
        for (int j = 0; j < size; j++)
        {
            for (int m = 0; m < al.Count; m++)
            {
                str[j] = str[j] + al[m][(j * jisuan(al, m) / size) % al[m].Length] + ",";
            }
            str[j] = str[j].Trim(',');
        }
        return str;
    }
    /// <summary>
    /// 计算
    /// </summary>
    /// <param name="al"></param>
    /// <param name="m"></param>
    /// <returns></returns>
    public static int jisuan(List<string[]> al, int m)
    {
        int result = 1;
        for (int i = 0; i < al.Count; i++)
        {
            if (i <= m)
            {
                result = result * al[i].Length;
            }
            else
            {
                break;
            }
        }
        return result;
    }
    /// <summary>
    /// 生成Guid，组织机构+年月日+32为guid
    /// </summary>
    /// <param name="guid"></param>
    /// <returns></returns>
    public static string Number_repeat(string guid)
    {
        //生成22位唯一的数字 并发可用    
        System.Threading.Thread.Sleep(1); //保证yyyyMMddHHmmssffff唯一  
        Random d = new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0));
        string strUnique = DateTime.Now.ToString("yyyyMMddHHmmssffff") + d.Next(1000, 9999);
        return WebConfigurationManager.AppSettings["OrgCode"] + strUnique;

        //字段长度太长，中金只接受32位长度
        // return WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString().Replace("-", "");
    }
    /// <summary>
    /// 字符串截取
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string GetStr(string str)
    {
        if (str.Length > 7)
        {
            str = str.Substring(0, 7) + "...";
        }
        return str;
    }

    /// <summary>
    /// 加密字符串
    /// </summary>
    /// <param name="inputString">要进行加密的字符串</param>
    /// <param name="encryptKey">需要进行解密操作的密钥</param>
    /// <param name="DesEncrypt">返回加密后的字符串</param>
    /// <param name="IsHttpRequst">是否用于HttpRequst获取</param>
    public static string DesEncrypt(string inputString, string encryptKey, bool IsHttpRequst = true)
    {
        byte[] byKey = null;
        try
        {
            byte[] IV = UTF8Encoding.UTF8.GetBytes("SYJ2015SORFTJIAMI");
            if (encryptKey.Length > 12)
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(encryptKey.Substring(0, 12));
            }
            else
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(encryptKey);
            }
            RijndaelManaged rij = new RijndaelManaged();
            rij.BlockSize = 128;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(inputString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, rij.CreateEncryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            string ToBase = Convert.ToBase64String(ms.ToArray());
            if (IsHttpRequst)
            {
                ToBase = HttpContext.Current.Server.UrlEncode(ToBase);
            }
            return ToBase;
        }
        catch
        {
            //return error.Message;
            return "";
        }
    }
    /// <summary>
    /// 解密字符串
    /// </summary>
    /// <param name="inputString">加了密的字符串</param>
    /// <param name="decryptKey">解密的密钥</param>
    /// <param name="DesDecrypt">返回解密后的字符串</param>
    ///  <param name="IsHttpRequst">是否通过HttpRequst获取</param>
    public static string DesDecrypt(string inputString, string decryptKey, bool IsHttpRequst = true)
    {
        byte[] byKey = null;
        try
        {
            if (IsHttpRequst)
            {
                if (!(inputString.IndexOf("+") >= 0) && !(inputString.IndexOf("/") >= 0))
                {
                    inputString = HttpContext.Current.Server.UrlDecode(inputString);
                }
            }
            byte[] IV = UTF8Encoding.UTF8.GetBytes("SYJ2015SORFTJIAMI");
            byte[] inputByteArray = new Byte[inputString.Length];
            if (decryptKey.Length > 12)
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey.Substring(0, 12));
            }
            else
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(decryptKey);
            }
            RijndaelManaged rij = new RijndaelManaged();
            rij.BlockSize = 128;
            inputByteArray = Convert.FromBase64String(inputString);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, rij.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = new System.Text.UTF8Encoding();
            return encoding.GetString(ms.ToArray());
        }
        catch(Exception e)
        {
            string msg=e.Message;
            return "";
        }
    }

    /// <summary>
    /// 截取过长的商品名称
    /// </summary>
    /// <returns></returns>
    public static string GetName(string name)
    {
        string str = string.Empty;
        if (!Util.IsEmpty(name))
        {
            if (name.Length > 30)
            {
                str = name.Substring(0, 30) + "...";
            }
            else
            {
                str = name;
            }
        }
        return str;
    }

    /// <summary>
    /// 发送信息
    /// </summary>
    /// <param name="phone">手机号</param>
    /// <param name="msg">发送内容</param>
    /// <returns></returns>
    public static string GetPhone(string phone, string msg)
    {
        string rstr = string.Empty;
        if (!string.IsNullOrEmpty(phone))
        {
            GetPhoneCode getphonecode = new GetPhoneCode();
            getphonecode.GetUser(ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString(), ConfigurationManager.AppSettings["PhoneCodePwd"].ToString());
            rstr = getphonecode.ReturnST(phone, msg);
        }
        else
        {
            rstr = "";
        }
        return rstr;
    }


    /// <summary>
    /// 导入Excal到DataTable
    /// </summary>
    /// <param name="file"></param>
    /// <param name="sheetName"></param>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public static DataTable ExcelToDataTable(string file, int Headinex = 0)
    {
        try
        {
            DataTable dt = new DataTable();

            XSSFWorkbook workbook = null;
            HSSFWorkbook workbook1 = null;
            if (Path.GetExtension(file) == ".xlsx")
            {
                string sheetName = string.Empty;
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    //office2003 HSSFWorkbook
                    workbook = new XSSFWorkbook(fs);
                    sheetName = workbook.GetSheetName(0);
                }
                ISheet sheet = workbook.GetSheet(sheetName);
                dt = Export2DataTable(sheet, Headinex, true);
            }
            else
            {
                string sheetName = string.Empty;
                using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read))
                {
                    //office2003 HSSFWorkbook
                    workbook1 = new HSSFWorkbook(fs);
                    sheetName = workbook1.GetSheetName(0);
                }
                ISheet sheet = workbook1.GetSheet(sheetName);
                dt = Export2DataTable2(sheet, Headinex, true);
            }
            return dt;
        }
        catch
        {
            throw new Exception("读取Excel出现错误。");
        }
    }

    /// <summary>
    /// 将指定sheet中的数据导入到datatable中
    /// </summary>
    /// <param name="sheet">指定需要导出的sheet</param>
    /// <param name="HeaderRowIndex">列头所在的行号，-1没有列头</param>
    /// <param name="needHeader"></param>
    /// <returns></returns>
    private static DataTable Export2DataTable(ISheet sheet, int HeaderRowIndex, bool needHeader)
    {
        DataTable dt = new DataTable();

        XSSFRow headerRow = null;
        int cellCount;
        try
        {
            if (HeaderRowIndex < 0 || !needHeader)
            {
                headerRow = sheet.GetRow(0) as XSSFRow;
                cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                {
                    DataColumn column = new DataColumn(Convert.ToString(i));
                    dt.Columns.Add(column);
                }
            }
            else
            {
                headerRow = sheet.GetRow(HeaderRowIndex) as XSSFRow;
                cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                {
                    ICell cell = headerRow.GetCell(i);
                    if (cell == null)
                    {
                        break;//到最后 跳出循环
                    }
                    else
                    {
                        DataColumn column = new DataColumn(headerRow.GetCell(i).ToString());
                        dt.Columns.Add(column);
                    }
                }
            }
            int rowCount = sheet.LastRowNum;
            for (int i = HeaderRowIndex + 1; i <= sheet.LastRowNum; i++)
            {
                XSSFRow row = null;
                if (sheet.GetRow(i) == null)
                {
                    row = sheet.CreateRow(i) as XSSFRow;
                }
                else
                {
                    row = sheet.GetRow(i) as XSSFRow;
                }
                DataRow dtRow = dt.NewRow();
                if (row.FirstCellNum < 0)
                    continue;
                for (int j = row.FirstCellNum; j <= cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        switch (row.GetCell(j).CellType)
                        {
                            case CellType.Boolean:
                                dtRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                break;
                            case CellType.Error:
                                dtRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                break;
                            case CellType.Formula:
                                switch (row.GetCell(j).CachedFormulaResultType)
                                {

                                    case CellType.Boolean:
                                        dtRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);

                                        break;
                                    case CellType.Error:
                                        dtRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);

                                        break;
                                    case CellType.Numeric:
                                        dtRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);

                                        break;
                                    case CellType.String:
                                        string strFORMULA = row.GetCell(j).StringCellValue;
                                        if (strFORMULA != null && strFORMULA.Length > 0)
                                        {
                                            dtRow[j] = strFORMULA.ToString();
                                        }
                                        else
                                        {
                                            dtRow[j] = null;
                                        }
                                        break;
                                    default:
                                        dtRow[j] = "";
                                        break;
                                }
                                break;
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                {
                                    dtRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                }
                                else
                                {
                                    dtRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                }
                                break;
                            case CellType.String:
                                string str = row.GetCell(j).StringCellValue;
                                if (!string.IsNullOrEmpty(str))
                                {
                                    dtRow[j] = Convert.ToString(str);
                                }
                                else
                                {
                                    dtRow[j] = null;
                                }
                                break;
                            default:
                                dtRow[j] = "";
                                break;
                        }
                    }
                }
                dt.Rows.Add(dtRow);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return dt;
    }

    private static DataTable Export2DataTable2(ISheet sheet, int HeaderRowIndex, bool needHeader)
    {
        
        DataTable dt = new DataTable();

        HSSFRow headerRow = null;
        int cellCount;
        try
        {
            if (HeaderRowIndex < 0 || !needHeader)
            {
                headerRow = sheet.GetRow(0) as HSSFRow;
                cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                {
                    DataColumn column = new DataColumn(Convert.ToString(i));
                    dt.Columns.Add(column);
                }
            }
            else
            {
                headerRow = sheet.GetRow(HeaderRowIndex) as HSSFRow;
                cellCount = headerRow.LastCellNum;
                for (int i = headerRow.FirstCellNum; i <= cellCount; i++)
                {
                    ICell cell = headerRow.GetCell(i);
                    if (cell == null)
                    {
                        break;//到最后 跳出循环
                    }
                    else
                    {
                        DataColumn column = new DataColumn(headerRow.GetCell(i).ToString());
                        dt.Columns.Add(column);
                    }
                }
            }
            int rowCount = sheet.LastRowNum;
            for (int i = HeaderRowIndex + 1; i <= sheet.LastRowNum; i++)
            {
                HSSFRow row = null;
                if (sheet.GetRow(i) == null)
                {
                    row = sheet.CreateRow(i) as HSSFRow;
                }
                else
                {
                    row = sheet.GetRow(i) as HSSFRow;
                }
                DataRow dtRow = dt.NewRow();
                if (row.FirstCellNum < 0)
                    continue;
                for (int j = row.FirstCellNum; j <= cellCount; j++)
                {
                    if (row.GetCell(j) != null)
                    {
                        switch (row.GetCell(j).CellType)
                        {
                            case CellType.Boolean:
                                dtRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);
                                break;
                            case CellType.Error:
                                dtRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);
                                break;
                            case CellType.Formula:
                                switch (row.GetCell(j).CachedFormulaResultType)
                                {

                                    case CellType.Boolean:
                                        dtRow[j] = Convert.ToString(row.GetCell(j).BooleanCellValue);

                                        break;
                                    case CellType.Error:
                                        dtRow[j] = ErrorEval.GetText(row.GetCell(j).ErrorCellValue);

                                        break;
                                    case CellType.Numeric:
                                        dtRow[j] = Convert.ToString(row.GetCell(j).NumericCellValue);

                                        break;
                                    case CellType.String:
                                        string strFORMULA = row.GetCell(j).StringCellValue;
                                        if (strFORMULA != null && strFORMULA.Length > 0)
                                        {
                                            dtRow[j] = strFORMULA.ToString();
                                        }
                                        else
                                        {
                                            dtRow[j] = null;
                                        }
                                        break;
                                    default:
                                        dtRow[j] = "";
                                        break;
                                }
                                break;
                            case CellType.Numeric:
                                if (DateUtil.IsCellDateFormatted(row.GetCell(j)))
                                {
                                    dtRow[j] = DateTime.FromOADate(row.GetCell(j).NumericCellValue);
                                }
                                else
                                {
                                    dtRow[j] = Convert.ToDouble(row.GetCell(j).NumericCellValue);
                                }
                                break;
                            case CellType.String:
                                string str = row.GetCell(j).StringCellValue;
                                if (!string.IsNullOrEmpty(str))
                                {
                                    dtRow[j] = Convert.ToString(str);
                                }
                                else
                                {
                                    dtRow[j] = null;
                                }
                                break;
                            default:
                                dtRow[j] = "";
                                break;
                        }
                    }
                }
                dt.Rows.Add(dtRow);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        return dt;
    }


    public static string CreateCode(int id)
    {
        string Code = string.Empty;
        for (int i = id.ToString().Length + 1; i <= 6; i++)
        {
            Code += "0";
        }
        return Code + id.ToString();
    }
    /// <summary>
    /// 实体类转换成DataTable
    /// 调用示例：DataTable dt= FillDataTable(Entitylist.ToList());
    /// </summary>
    /// <param name="modelList">实体类列表</param>
    /// <returns></returns>
    public static DataTable FillDataTable<T>(List<T> modelList)
    {
        if (modelList == null || modelList.Count == 0)
        {
            return null;
        }
        DataTable dt = CreateData(modelList[0]);//创建表结构

        foreach (T model in modelList)
        {
            DataRow dataRow = dt.NewRow();
            foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
            {
                dataRow[propertyInfo.Name] = propertyInfo.GetValue(model, null);
            }
            dt.Rows.Add(dataRow);
        }
        return dt;
    }
    /// <summary>
    /// 根据实体类得到表结构
    /// </summary>
    /// <param name="model">实体类</param>
    /// <returns></returns>
    public static DataTable CreateData<T>(T model)
    {
        DataTable dataTable = new DataTable(typeof(T).Name);
        foreach (PropertyInfo propertyInfo in typeof(T).GetProperties())
        {
            if (propertyInfo.Name != "CTimestamp")//些字段为oracle中的Timesstarmp类型
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, propertyInfo.PropertyType));
            }
            else
            {
                dataTable.Columns.Add(new DataColumn(propertyInfo.Name, typeof(DateTime)));
            }
        }
        return dataTable;
    }

    /// <summary>
    /// 根据商品详情ID获得商品ID
    /// </summary>
    /// <returns></returns>
    public static string GetGoodsID(string goodsinfoid)
    {
        string str = "select * from BD_GoodsInfo where dr=0 and id=" + goodsinfoid;
        DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, str);
        if (ds.Tables[0].Rows.Count > 0)
        {
            return ds.Tables[0].Rows[0]["goodsid"].ToString();
        }
        return "";
    }
    /// <summary>
    /// 判断是否可删除goods表
    /// </summary>
    /// <param name="compid"></param>
    /// <returns></returns>
    public static DataTable GoodsIsExist(string compid)
    {
        string sql = string.Format(@"select distinct GoodsinfoID  from DIS_Order as a
                                    left join  DIS_OrderDetail as b on a.ID=b.OrderID
                                    left join  BD_GoodsInfo as c on b.GoodsinfoID=c.ID
                                    where a.CompID={0} and c.CompID={0} and ISNULL(a.dr,0)=0   
                                    and  isnull(b.dr,0)=0 and  isnull(c.dr,0)=0", compid);
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        return dt;
    }
        /// <summary>
    /// 判断是否可删除goods表
    /// </summary>
    /// <param name="compid"></param>
    /// <returns></returns>
    public static DataTable GoodsIsExist2(string compid)
    {
        string sql = string.Format(@"select distinct GoodsID  from DIS_Order as a
                                     join  DIS_OrderDetail as b on a.ID=b.OrderID
                                     join  BD_GoodsInfo as c on b.GoodsinfoID=c.ID
                                    where a.CompID={0} and c.CompID={0} and ISNULL(a.dr,0)=0   
                                    and  isnull(b.dr,0)=0 and  isnull(c.dr,0)=0 ", compid);
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        return dt;
    }
    

    /// <summary>
    /// 判断是否有权限  Add by hgh
    /// </summary>
    /// <param name="uId">用户ID</param>
    /// <param name="funCode">权限Code</param>
    /// <returns></returns>
    public static bool HasRight(int compid, int uId, string funCode, int disID = 0)
    {
        bool retB = false;

        //厂商管理员、代理商管理员
        if (TypeID() == 4 || TypeID() == 5)
            return true;

        //string sql = "select * from SYS_RoleSysFun rf join Sys_CompUser u on u.RoleID=rf.RoleID where u.UserID=" + uId + " and rf.FunCode='" + funCode + "'";
        string sql = string.Empty;
        if (disID == 0)
            sql = @"SELECT FunCode FROM dbo.SYS_RoleSysFun WHERE CompID=" + compid + " and FunCode='" + funCode + "'and RoleID IN (SELECT RoleID FROM SYS_RoleUser WHERE UserID=" + uId + " AND IsEnabled=1 AND dr=0) GROUP BY FunCode";
        else
            sql = @"SELECT FunCode FROM dbo.SYS_RoleSysFun WHERE CompID=" + compid + " and  DisID=" + disID + " and FunCode='" + funCode + "'and RoleID IN (SELECT RoleID FROM SYS_RoleUser WHERE UserID=" + uId + " AND IsEnabled=1 AND dr=0) GROUP BY FunCode";

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
            retB = true;

        return retB;
    }

    /// <summary>
    /// 判断是否有权限  Add by hgh (不排除管理员类型)
    /// </summary>
    /// <param name="uId">用户ID</param>
    /// <param name="funCode">权限Code</param>
    /// <returns></returns>
    public static bool HasRightAll(int compid, int uId, string funCode, int disID = 0)
    {
        bool retB = false;

        string sql = string.Empty;
        if (disID == 0)
            sql = @"SELECT FunCode FROM dbo.SYS_RoleSysFun WHERE CompID=" + compid + " and FunCode='" + funCode + "'and RoleID IN (SELECT RoleID FROM SYS_RoleUser WHERE UserID=" + uId + " AND IsEnabled=1 AND dr=0) GROUP BY FunCode";
        else
            sql = @"SELECT FunCode FROM dbo.SYS_RoleSysFun WHERE CompID=" + compid + " and  DisID=" + disID + " and FunCode='" + funCode + "'and RoleID IN (SELECT RoleID FROM SYS_RoleUser WHERE UserID=" + uId + " AND IsEnabled=1 AND dr=0) GROUP BY FunCode";

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
            retB = true;

        return retB;
    }

    /// <summary>
    /// 判断该用户是否有管理员岗位 add by hgh
    /// </summary>
    /// <param name="uId"></param>
    /// <returns></returns>
    public static bool HasAdminRole(int uId)
    {
        bool retB = false;
        string sql = "select cu.Userid from  SYS_CompUser cu  join SYS_Role r on cu.RoleID=r.ID where r.RoleName='企业管理员' and cu.utype=4 and cu.Userid=" + uId;
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
            retB = true;
        return retB;
    }

    /// <summary>
    /// 判断该该条数据代理商是否有操作权限 add by hgh
    /// </summary>
    /// <param name="mName">模块名称</param>
    /// <param name="kId">主键KeyID</param>
    /// <param name="disId">代理商ID</param>
    /// <returns></returns>
    public static bool PageDisOperable(string mName, int kId, int disId)
    {
        bool retB = false;
        string sql = "";
        if (mName == "Order")
        {
            sql = "select id from DIS_Order where DisID='" + disId + "' and ID=" + kId;
        }
        else if (mName == "ReturnOrder")
        {
            sql = "select id from DIS_OrderReturn where DisID='" + disId + "' and ID=" + kId;
        }
        else if (mName == "PayPre")
        {
            sql = "select id from PAY_PrePayment where DisID='" + disId + "' and ID=" + kId;
        }
        else if (mName == "Message")
        {
            sql = "select id from DIS_Suggest where DisID='" + disId + "' and ID=" + kId;
        }
        else if (mName == "")
        {

        }
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
            retB = true;
        return retB;
    }
    /// <summary>
    /// 判断改该条数据企业是否有操作权限 add by hgh
    /// </summary>
    /// <param name="mName">模块名称</param>
    /// <param name="kId">主键KeyID</param>
    /// <param name="compId">企业ID</param>
    /// <returns></returns>
    public static bool PageCompOperable(string mName, int kId, int compId)
    {
        bool retB = false;
        string sql = "";
        if (mName == "Order")
        {
            sql = "select id from DIS_Order where CompID='" + compId + "' and ID=" + kId;
        }
        else if (mName == "ReturnOrder")
        {
            sql = "select id from DIS_OrderReturn where CompID='" + compId + "' and ID=" + kId;
        }
        else if (mName == "OrderOut")
        {
            sql = "select id from DIS_OrderOut where CompID='" + compId + "' and ID=" + kId;
        }
        else if (mName == "")
        {

        }
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
            retB = true;
        return retB;
    }

    /// <summary>
    /// 获取商品备注
    /// </summary>
    /// <returns></returns>
    public static string GetGoodsMemo(int goodsId)
    {

        Hi.Model.BD_Goods model = new Hi.BLL.BD_Goods().GetModel(goodsId);
        if (model != null)
        {
            return model.memo;
        }
        else
        {
            return "";
        }
    }
    /// <summary>
    /// datatable转实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <returns></returns>
    public static T GetEntity<T>(DataTable table) where T : new()
    {
        T entity = new T();
        foreach (DataRow row in table.Rows)
        {
            foreach (var item in entity.GetType().GetProperties())
            {
                if (row.Table.Columns.Contains(item.Name))
                {
                    if (DBNull.Value != row[item.Name])
                    {
                        item.SetValue(entity, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                    }

                }
            }
        }
        return entity;
    }
    /// <summary>
    /// DataRow转实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <returns></returns>
    public static T GetEntity<T>(DataRow[] Rows) where T : new()
    {
        T entity = new T();
        foreach (DataRow row in Rows)
        {
            foreach (var item in entity.GetType().GetProperties())
            {
                if (row.Table.Columns.Contains(item.Name))
                {
                    if (DBNull.Value != row[item.Name])
                    {
                        item.SetValue(entity, Convert.ChangeType(row[item.Name], item.PropertyType), null);
                    }

                }
            }
        }
        return entity;
    }
    #region 商品可销售区域黑名单

    /// <summary>
    /// 根据代理商ID获取可采购的商品ID集合，返回list
    /// </summary>
    /// <param name="disID"></param>
    /// <returns></returns>
    public static List<GoodsID> DisEnAreaGoodsID(string disID, string CompID)
    {
        e = "";
        List<GoodsID> list = new List<GoodsID>();

        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(int.Parse(disID));
        if (dis == null || dis.IsEnabled == 0 || dis.dr == 1)
            return null;

        string areaStr = string.Empty;

        //if (dis.AreaID.ToString() == "0" || dis.AreaID.ToString() == "")
        //    areaStr = " ID=0";
        //else
        //{
        //    //查询企业经交商区域
        //    List<Hi.Model.BD_DisArea> AreaList = new Hi.BLL.BD_DisArea().GetList("", " CompanyID=" + CompID, "");
        //    string AreaID = Common.Area(dis.AreaID, AreaList);
        //    areaStr += " isnull(areaID,0) in (" + AreaID.Substring(0, AreaID.Length - 1) + ")"; //代理商区域
        //}

        areaStr += " compID=" + dis.CompID + " and DisID=" + disID + " and isnull(dr,0)=0";
        List<Hi.Model.BD_GoodsAreas> areas = new Hi.BLL.BD_GoodsAreas().GetList("", areaStr, "");

        //string sql = "0";
        if (areas != null && areas.Count > 0)
        {
            foreach (Hi.Model.BD_GoodsAreas area in areas)
            {
                //sql += "," + area.GoodsID;
                GoodsID GoodsID = new GoodsID();
                GoodsID.goodsID = area.GoodsID.ToString();
                list.Add(GoodsID);
            }
        }

        //List<Hi.Model.BD_Goods> goodsList = new Hi.BLL.BD_Goods().GetList("", " compID='" + dis.CompID + "' and ISNULL(dr,0)=0 and ID  in (" + sql + ")", "");
        //if (goodsList.Count == 0)
        //    return null;
        //foreach (Hi.Model.BD_Goods good in goodsList)
        //{
        //    GoodsID GoodsID = new GoodsID();
        //    GoodsID.goodsID = good.ID.ToString();
        //    list.Add(GoodsID);
        //}

        return list;
    }

    /// <summary>
    /// 递归获取代理商所在区域 市--省--大区
    /// </summary>
    public static string Area(int AreaID, List<Hi.Model.BD_DisArea> AreaList)
    {
        //Hi.Model.BD_DisArea area = new Hi.BLL.BD_DisArea().GetModel(AreaID);
        //if (area == null || area.dr == 1)
        //    return "";
        //e += area.ID + ",";
        //if (area.ParentID != 0)
        //{
        //    Area(area.ParentID);
        //}
        if (AreaList != null && AreaList.Count > 0)
        {

            List<Hi.Model.BD_DisArea> al = AreaList.FindAll(p => p.ID == AreaID);
            e += al[0].ID + ",";
            if (al[0].ParentID != 0)
            {
                Area(al[0].ParentID, AreaList);
            }
        }

        return e;
    }

    /// <summary>
    /// 递归获取代理商区域 :从父集到子级
    /// </summary>
    //public static string AllArea(int disTypeID)
    //{
    //    string str = " ParentID=" + disTypeID;
    //    List<Hi.Model.BD_DisArea> areas = new Hi.BLL.BD_DisArea().GetList("", str, "");
    //    if (areas.Count != 0)
    //    {
    //        foreach (Hi.Model.BD_DisArea area in areas)
    //        {
    //            Areas += area.ID + ",";
    //            Area(area.ID);
    //        }
    //    }
    //    Areas += disTypeID.ToString();
    //    return disType;
    //}

    /// <summary>
    /// 递归获取代理商分类 :从父集到子级(有问题)
    /// </summary>
    //public static string DisType(int disTypeID)
    //{
    //    string str = " ParentID=" + disTypeID;
    //    List<Hi.Model.BD_DisType> areas = new Hi.BLL.BD_DisType().GetList("", str, "");
    //    if (areas.Count != 0)
    //    {
    //        foreach (Hi.Model.BD_DisType area in areas)
    //        {
    //            disType += area.ID + ",";
    //            Area(area.ID);
    //        }
    //    }
    //    disType += disTypeID.ToString();
    //    return disType;
    //}

    public class GoodsID
    {
        public string goodsID { get; set; }
    }

    #endregion

    #region 消息推送：安卓、微信

    public delegate void Jpushdega(string sendType, string orderID, string userType, decimal money = 0);


    /// <summary>
    /// 消息推送:同时推送微信和安卓，WebReference
    /// </summary>
    /// <param name="sendType">发送类型 "1":下单通知 "2":订单支付通知 "3":签收提醒 "4":退货申请 "5":修改订单 "41":代人下单通知 "42":订单审批 "43":订单发货 "44":退货审核 "45":退款 "46":厂商修改订单</param>
    /// <param name="orderID">操作订单ID</param>
    /// <param name="userType">用户  "0" 推送给代理商 "1"推送给厂商</param>
    public void GetWxService(string sendType, string orderID, string userType, decimal paymoney = 0)
    {
        Common.Jpushdega jpushdega = new Common.Jpushdega(new Common().GetWxServiceSend);
        jpushdega.BeginInvoke(sendType, orderID, userType, paymoney, null, null);
    }

    /// <summary>
    /// 消息推送:同时推送微信和安卓，WebReference
    /// </summary>
    /// <param name="sendType">发送类型 "1":下单通知 "2":订单支付通知 "3":签收提醒 "4":退货申请 "5":修改订单 "41":代人下单通知 "42":订单审批 "43":订单发货 "44":退货审核 "45":退款 "46":厂商修改订单</param>
    /// <param name="orderID">操作订单ID</param>
    /// <param name="userType">用户  "0" 推送给代理商 "1"推送给厂商</param>
    public void GetWxServiceSend(string sendType, string orderID, string userType, decimal paymoney = 0)
    {
        try
        {
            if (ConfigurationManager.AppSettings["PushSwitch"].ToString() == "1")
            {
                if (sendType != "1" && sendType != "2" && sendType != "3" && sendType != "4" &&
                    sendType != "41" && sendType != "42" && sendType != "43" && sendType != "44" && sendType != "45")
                    return;
                Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(int.Parse(orderID));
                if (order == null)
                    return;
                if (userType != "0" && userType != "1")
                    return;

                AppService appService = new AppService();
                appService.AnPush(userType.Trim(), sendType.Trim(), orderID.Trim(), money: paymoney);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {

        }
    }

    /// <summary>
    /// 促销推送
    /// </summary>
    /// <param name="msgID"></param>
    /// <param name="type"></param>
    public void GetMsgService(int msgID, int type)
    {
        AppService appService = new AppService();
        appService.MsgPush(msgID.ToString(), type.ToString());
    }

    #endregion

    public static string GetUTypeName(string Utype)
    {
        string Name = "";

        switch (Utype)
        {
            case "1": Name = "系统管理员"; break;
            case "2": Name = "系统用户"; break;
            case "3": Name = "机构管理员"; break;
            case "4": Name = "机构用户"; break;
            case "5": Name = "业务员"; break;
        }

        return Name;
    }

    /// <summary>
    /// 获取企业发布的信息类型名称
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetCPNewStateName(object obj)
    {
        string str = "";
        string newstype = obj.ToString();
        if (!string.IsNullOrEmpty(newstype))
        {
            if (newstype == "1")
            {
                str = "新闻";
            }
            else if (newstype == "2")
            {
                str = "通知";
            }
            else if (newstype == "3")
            {
                str = "公告";
            }
            else if (newstype == "4")
            {
                str = "促销";
            }
            else if (newstype == "5")
            {
                str = "企业动态";
            }

        }
        return str;
    }

    public static void BindSMType(DropDownList ddl, string DefaultName = "")
    {
        foreach (int T in Enum.GetValues(typeof(Enums.DisSMType)))
        {
            ddl.Items.Add(new ListItem(Enum.GetName(typeof(Enums.DisSMType), T), T.ToString()));
        }
        if (!string.IsNullOrEmpty(DefaultName))
        {
            ddl.Items.Insert(0, new ListItem(DefaultName, "-1"));
        }
    }

    public static void BindSMParent(DropDownList ddl, int Compid, string DefaultName = "")
    {
        List<Hi.Model.BD_DisSalesMan> models = new Hi.BLL.BD_DisSalesMan().GetList("", " isnull(dr,0)=0 and IsEnabled=1 and CompID=" + Compid + " and SalesType=" + ((int)Enums.DisSMType.业务经理) + "", " createdate asc");
        foreach (Hi.Model.BD_DisSalesMan model in models)
        {
            ddl.Items.Add(new ListItem(model.SalesName, model.ID.ToString()));
        }
        if (!string.IsNullOrEmpty(DefaultName))
        {
            ddl.Items.Insert(0, new ListItem(DefaultName, "-1"));
        }
    }

    public static void BindDoCmentType(DropDownList ddl, string DefaultName = "")
    {
        foreach (int T in Enum.GetValues(typeof(Enums.CertificatesNature)))
        {
            ddl.Items.Add(new ListItem(Enum.GetName(typeof(Enums.CertificatesNature), T), T.ToString()));
        }
        if (!string.IsNullOrEmpty(DefaultName))
        {
            ddl.Items.Insert(0, new ListItem(DefaultName, "-1"));
        }
    }

    public static void BindErptype(DropDownList ddl, int erptype, string DefaultName = "")
    {
        ddl.Items.Clear();
        List<Hi.Model.BD_Company> Comp = new Hi.BLL.BD_Company().GetList("", " isnull(dr,0)=0 and isnull(Erptype,0)>0", "");
        foreach (int T in Enum.GetValues(typeof(Enums.Erptype)))
        {
            //if ((Comp.Find(comp => comp.Erptype == T) == null || T == erptype))
            //{
            //    ddl.Items.Add(new ListItem(Enum.GetName(typeof(Enums.Erptype), T), T.ToString()));
            //}
            if (T == (int)Enums.Erptype.平台企业 || (Comp.Find(comp => comp.Erptype == erptype) != null) || Comp.Count == 0)
            {
                ddl.Items.Add(new ListItem(Enum.GetName(typeof(Enums.Erptype), T), T.ToString()));
            }
        }
        if (!string.IsNullOrEmpty(DefaultName))
        {
            ddl.Items.Insert(0, new ListItem(DefaultName, "-1"));
        }
    }

    public static object GetOpenAccountValue(int id, string Name)
    {
        object value = null;
        object model = new Hi.BLL.PAY_OpenAccount().GetModel(id);
        if (model != null)
        {
            foreach (PropertyInfo info in model.GetType().GetProperties())
            {
                if (info.Name.ToLower() == Name.ToLower())
                {
                    return info.GetValue(model, null);
                }
            }
        }
        return value;
    }

    /// <summary>
    /// 判断开销户某属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool OenAExistsAttribute(string name, string value, string id = "")
    {
        if (!string.IsNullOrEmpty(id))
        {
            List<Hi.Model.PAY_OpenAccount> Dis = new Hi.BLL.PAY_OpenAccount().GetList("", " " + name + "='" + value + "'   and id<>'" + id + "' and isnull(dr,0)=0 ", "");
            return Dis.Count > 0;
        }
        else
        {
            List<Hi.Model.PAY_OpenAccount> Dis = new Hi.BLL.PAY_OpenAccount().GetList("", " " + name + "='" + value + "'  and isnull(dr,0)=0 ", "");
            return Dis.Count > 0;
        }
    }

    /// <summary>
    /// 判断代理商地址某属性值是否存在
    /// </summary>
    /// <param name="name"></param>
    /// <param name="value"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    public static bool DisAddrExistsAttribute(string name, string value, string Disid, string id = "")
    {
        bool exists = false;
        if (!string.IsNullOrEmpty(id))
        {
            List<Hi.Model.BD_DisAddr> Dis = new Hi.BLL.BD_DisAddr().GetList("", " " + name + "='" + value + "' and DisID=" + Disid + "  and id<>'" + id + "' and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        else
        {

            List<Hi.Model.BD_DisAddr> Dis = new Hi.BLL.BD_DisAddr().GetList("", " " + name + "='" + value + "' and DisID=" + Disid + " and isnull(dr,0)=0 ", "");
            if (Dis.Count > 0)
            {
                exists = true;
            }
        }
        return exists;
    }

    public static void BindPro(System.Web.UI.HtmlControls.HtmlSelect select, string DefaultName = "")
    {
        select.Items.Clear();
        List<Hi.Model.BF_ZD_PROVINCE> BPro = new Hi.BLL.BF_ZD_PROVINCE().GetList("", "", "");
        foreach (Hi.Model.BF_ZD_PROVINCE model in BPro)
        {
            select.Items.Add(new ListItem(model.PROVNAME, model.PROVCODE));
        }
        if (!string.IsNullOrEmpty(DefaultName))
        {
            select.Items.Insert(0, new ListItem(DefaultName, "-1"));
        }
    }

    public class ResultMessage
    {
        public bool result { get; set; }
        public string code { get; set; }
    }

    #region 促销活动 add by szj 2015-09-23

    /// <summary>
    /// 促销是否启用
    /// </summary>
    /// <param name="IsE"></param>
    /// <returns></returns>
    public static string GetIsEnabled(object IsE)
    {
        if (IsE.ToString() == "0")
        {
            return "停用";
        }
        else if (IsE.ToString() == "1")
        {
            return "启用";
        }
        else
        {
            return "";
        }
    }

    /// <summary>
    /// 促销方式
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static string GetProType(object obj)
    {
        string type = obj.ToString();
        if (type == "1")
        {
            return "赠品";
        }
        else if (type == "2")
        {
            return "优惠";
        }
        else if (type == "3")
        {
            return "满送";
        }
        else if (type == "4")
        {
            return "打折";
        }
        else if (type == "5")
        {
            return "满减";
        }
        else
        {
            return "满折";
        }
    }

    /// <summary>
    /// 判断商品是不是在促销中
    /// </summary>
    /// <param name="GoodsID">商品Id</param>
    /// <returns></returns>
    public static int GetPro(object GoodsID, string GoodsInfoId, string CompId)
    {
        if (GoodsID.ToString() == "" || CompId == "")
        {
            return 0;
        }

        string sql = string.Empty;

        if (GoodsInfoId != "")
        {
            sql = string.Format(@"select pro.ID,pro.CompID,pro.ProType,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and prod.GoodInfoID={2} and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 order by pro.CreateDate desc", GoodsID.ToString(), CompId, GoodsInfoId);
        }
        else
        {
            sql = string.Format(@"select pro.ID,pro.CompID,pro.ProType,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 order by pro.CreateDate desc", GoodsID.ToString(), CompId);
        }

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        DateTime now = DateTime.Now;

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                if (Convert.ToDateTime(item["ProStartTime"]) <= now && Convert.ToDateTime(item["ProEndTime"]).AddDays(1) > now)
                {
                    return item["ID"].ToString().ToInt(0);
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// 返回最新促销价格
    /// </summary>
    /// <returns></returns>
    public static decimal GetProPrice(object GoodsID, string GoodsInfoId, string CompId)
    {
        if (GoodsID.ToString() == "" || CompId == "")
        {
            return 0;
        }

        string sql = string.Empty;

        if (GoodsInfoId != "")
        {
            sql = string.Format(@"select top 1 pro.ID,pro.CompID,pro.ProType,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and prod.GoodInfoID={2} and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 and (pro.ProStartTime<=GETDATE() and DATEADD(D,1,pro.ProEndTime)>GETDATE()) order by pro.CreateDate desc", GoodsID.ToString(), CompId, GoodsInfoId);
        }
        else
        {
            sql = string.Format(@"select  top 1 pro.ID,pro.CompID,pro.ProType,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and isnull(pro.dr,0)=0 and  ISNULL(pro.IsEnabled,0)=1 and (pro.ProStartTime<=GETDATE() and DATEADD(D,1,pro.ProEndTime)>GETDATE()) order by pro.CreateDate desc", GoodsID.ToString(), CompId);
        }

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        DateTime now = DateTime.Now;

        if (dt != null && dt.Rows.Count > 0)
        {
            Hi.Model.BD_GoodsInfo info = new Hi.BLL.BD_GoodsInfo().GetModel(dt.Rows[0]["GoodInfoID"].ToString().ToInt(0));
            if (info.IsOffline == 0)
                return 0;
            if (dt.Rows[0]["ProType"].ToString() == "3")
            {
                return string.Format("{0:N2}", info.TinkerPrice.ToString()).ToDecimal(0).ToString("#,##0.00").ToDecimal(0);
            }
            return string.Format("{0:N2}", dt.Rows[0]["GoodsPrice"].ToString()).ToDecimal(0).ToString("#,##0.00").ToDecimal(0);
        }
        return 0;
    }

    /// <summary>
    /// 查询企业参加促销的商品信息
    /// </summary>
    /// <param name="CompId">企业Id</param>
    /// <returns>返回促销的商品ID</returns>
    public static List<GoodsID> GetProList(string CompId)
    {
        List<GoodsID> l = new List<GoodsID>();

        string sql = string.Format(@"select pro.ID,pro.CompID,pro.ProTitle,pro.ProType,pro.IsEnabled,pro.CreateDate,pro.CreateUserID,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice,prod.GoodsName from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where pro.CompID={0} and ISNULL(pro.IsEnabled,0)=1 and isnull(pro.dr,0)=0 order by pro.CreateDate desc", CompId);

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        DateTime now = DateTime.Now;
        GoodsID g = null;
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                if (Convert.ToDateTime(item["ProStartTime"]) <= now && Convert.ToDateTime(item["ProEndTime"]).AddDays(1) > now)
                {
                    g = new GoodsID();
                    g.goodsID = item["GoodsId"].ToString();
                    l.Add(g);
                }
            }
        }
        return l;
    }

    /// <summary>
    /// 判断订单明细中的促销商品的促销活动是否可用
    /// </summary>
    /// <param name="CompID">企业ID</param>
    /// <param name="GoodsID">商品ID</param>
    /// <param name="GoodInfoID">商品信息ID</param>
    /// <param name="GoodsPrice">订单商品价格</param>
    /// <returns>0，可用，1、不可用</returns>
    public static int GetOrderPro(int CompID, int GoodInfoID, decimal GoodsPrice)
    {
        Hi.Model.BD_GoodsInfo InfoModel = new Hi.BLL.BD_GoodsInfo().GetModel(GoodInfoID);

        if (InfoModel != null)
        {
            if (InfoModel.IsOffline == 1)
                return 0;

            string sql = string.Format(@"select * from BD_Promotion as pro left join 
BD_PromotionDetail as prod on pro.ID=prod.ProID 
where pro.CompID={0} and  prod.GoodsID={1} and prod.GoodInfoID={2} and GoodsPrice={3}
and ISNULL(pro.IsEnabled,0)=1 and isnull(pro.dr,0)=0 and  (pro.ProStartTime<=GETDATE() and DATEADD(D,1,pro.ProEndTime)>GETDATE())", CompID, InfoModel.GoodsID, GoodInfoID, GoodsPrice);

            DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
            DateTime now = DateTime.Now;

            if (dt != null && dt.Rows.Count > 0)
            {
                return 1;
            }
        }
        return 0;
    }

    /// <summary>
    ///促销  满送数量
    /// </summary>
    /// <param name="GoodsID">商品Id</param>
    /// <param name="GoodsInfoId">商品信息ID</param>
    /// <param name="CompId">企业ID</param>
    /// <param name="Num">购买商品数量</param>
    /// <returns>订购数量没有买满返回 0、其他返回满送的商品数量</returns>
    public static int GetProNum(string GoodsID, string GoodsInfoId, int CompId, string Num, out string pty, out string ppty)
    {
        pty = "0";
        ppty = "";
        if (GoodsID.ToString() == "" || CompId.ToString() == "")
        {
            return 0;
        }

        string sql = string.Empty;

        if (GoodsInfoId != "")
        {
            sql = string.Format(@"select pro.ID,pro.CompID,pro.Type,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and prod.GoodInfoID={2} and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 order by pro.CreateDate desc", GoodsID.ToString(), CompId, GoodsInfoId);
        }
        else
        {
            sql = string.Format(@"select pro.ID,pro.CompID,pro.Type,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 order by pro.CreateDate desc", GoodsID.ToString(), CompId);
        }

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        DateTime now = DateTime.Now;

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                if (Convert.ToDateTime(item["ProStartTime"]) <= now && Convert.ToDateTime(item["ProEndTime"]).AddDays(1) > now)
                {
                    pty = item["ProType"].ToString();

                    //促销类型，促销方式，促销价格（1、2、4是商品价格，3是送的商品数量），
                    //打折率（1、2 是0，3是满件数  4是打折0-100）
                    //赔送商品数量
                    if (Num.ToInt(0) >= Convert.ToInt32(item["DisCount"]))
                    {
                        ppty = item["Type"] + "," + pty + "," + item["GoodsPrice"] + "," + item["DisCount"];
                        if (item["ProType"].ToString() == "3")
                        {
                            int sendNum = Num.ToInt(0) / Convert.ToInt32(item["DisCount"]);
                            return Convert.ToInt32(item["GoodsPrice"]) * sendNum;
                        }
                        else
                            return 0;
                    }
                    else
                        return 0;
                }
            }
        }
        return 0;
    }

    /// <summary>
    /// 判断订单促销
    /// </summary>
    /// <param name="TotalPrice">下单总金额</param>
    /// <returns>返回促销的金额</returns>
    public static decimal GetProPrice(decimal TotalPrice, out string proID, out string ProIDD, out string ProType, int CompId)
    {
        proID = "0";
        ProIDD = "0";
        ProType = "";
        string sql = string.Format(@"select p.ID,p.Type,p.ProType,p.ProStartTime,p.ProEndTime,pd2.OrderPrice, pd2.Discount,pd2.ID IDD from BD_Promotion as p left join BD_PromotionDetail2 as pd2 on p.ID=pd2.ProID where p.[type]=2 and IsEnabled=1 and isnull(p.dr,0)=0 and p.CompID={0}  and (p.ProStartTime<=GETDATE() and DATEADD(D,1,p.ProEndTime)>GETDATE()) order by p.CreateDate desc,pd2.OrderPrice desc", CompId);

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

        decimal Price = 0;
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                Price = 0;
                if (item["OrderPrice"].ToString().ToDecimal(0) <= TotalPrice)
                {
                    proID = item["ID"].ToString();
                    ProIDD = item["IDD"].ToString();
                    if (item["ProType"].ToString() == "5")
                    {
                        //满减
                        Price = string.Format("{0:N2}", item["Discount"].ToString()).ToDecimal(0).ToString("#,##0.00").ToDecimal(0);
                        ProType = "5," + item["OrderPrice"].ToString().ToDecimal(0) + "," + item["Discount"].ToString();
                    }
                    else if (item["ProType"].ToString() == "6")
                    {
                        //满折
                        decimal Discount = item["Discount"].ToString().ToDecimal(0);
                        TotalPrice = TotalPrice * (1 - (Discount / 100));

                        Price = string.Format("{0:N2}", TotalPrice).ToDecimal(0).ToString("#,##0.00").ToDecimal(0);
                        ProType = "6," + item["OrderPrice"].ToString().ToDecimal(0) + "," + item["Discount"].ToString();
                    }
                    return Price;
                }
            }
            return Price;
        }
        return 0;
    }
    #endregion


    #region 判断企业来源 add by szj 2015-11-12
    /// <summary>
    /// 判断企业来源
    /// </summary>
    /// <param name="CompID">企业ID</param>
    /// <returns>0、本平台用户  1、 U8  2、 U9 </returns>
    public static int CompErpType(int CompID)
    {
        Hi.Model.BD_Company compModel = new Hi.BLL.BD_Company().GetModel(CompID);

        if (compModel.Erptype == 0)
        {
            return 0;
        }
        return compModel.Erptype;
    }

    #endregion

    #region 商品分类选择是否折叠 add by 2016-03-14

    /// <summary>
    /// 判断企业商品分类是否折叠
    /// </summary>
    /// <param name="CompID">企业ID</param>
    /// <returns>0、不折叠 1、折叠</returns>
    public static int RsertFolding(object type, int CompID)
    {
        if (type.ToString() == "1")
        {
            List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("Value", " Name='商品分类选择是否折叠' and CompID=" + CompID + " and isnull(dr,0)=0", "");

            if (sl != null && sl.Count > 0)
                return sl[0].Value.ToInt(0);
        }
        return 0;
    }

    /// <summary>
    /// 判断企业商品分类选择是否折叠
    /// </summary>
    /// <param name="CompID">企业ID</param>
    /// <returns>0、不折叠 1、折叠</returns>
    public static int RsertFolding(object type, object parentid, int CompID)
    {
        if (type.ToString() == "1")
        {
            List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("", " Name='商品分类选择是否折叠' and CompID=" + CompID + " and isnull(dr,0)=0", "");

            if (sl != null && sl.Count > 0)
            {
                if (parentid.ToString().ToInt(0) != 0)
                {
                    //判断是否有下一级分类
                    List<Hi.Model.BD_GoodsCategory> l = new Hi.BLL.BD_GoodsCategory().GetList("", "compid=" + CompID + " and isnull(dr,0)=0 and isenabled=1 and parentid=" + parentid.ToString().Trim(), "sortIndex");
                    if (l != null && l.Count > 0)
                        return sl[0].Value.ToInt(0); //有下一级分类返回系统商品分类设置
                    return 0; //没有下一级分类返回不折叠
                }
                return sl[0].Value.ToInt(0);
            }
        }
        return 0;
    }
    #endregion

    #region 商品初始化,用于导入商品之后

    /// <summary>
    /// 初始化商品ViewInfo信息
    /// </summary>
    /// <param name="goodsList"></param>
    public void InitialGoods(List<Hi.Model.BD_Goods> goodsList)
    {
        GetInitialGoodsList(goodsList);
    }

    /// <summary>
    /// 获取需要更新的goodsList
    /// </summary>
    /// <param name="list"></param>
    public void GetInitialGoodsList(List<Hi.Model.BD_Goods> list)
    {
        List<Hi.Model.BD_Goods> goodsList = new List<Hi.Model.BD_Goods>();
        if (list != null && list.Count > 0)
        {
            foreach (Hi.Model.BD_Goods item in list)
            {
                Common com = new Common();
                com.sAttr = "";
                com.sAttr1 = "";
                item.ViewInfos = com.GoodsType(item.ID.ToString(), item.CompID.ToString());

                string strWhere = " GoodsID=" + item.ID + "and isnull(ValueInfo,'')='" + com.sAttr + "' and IsOffline=1 and CompID=" + item.CompID + "and IsEnabled=1 and isnull(dr,0)=0";
                string strWhere1 = " GoodsID=" + item.ID + "and isnull(ValueInfo,'')='" + com.sAttr1 + "' and IsOffline=1 and CompID=" + item.CompID + "and IsEnabled=1 and isnull(dr,0)=0";

                List<Hi.Model.BD_GoodsInfo> l = new Hi.BLL.BD_GoodsInfo().GetList("", strWhere, "");
                if (l.Count == 0)
                    l = new Hi.BLL.BD_GoodsInfo().GetList("", strWhere1, "");

                if (l != null && l.Count > 0)
                    item.ViewInfoID = l[0].ID;

                goodsList.Add(item);
            }
            if (goodsList.Count > 0)
                new Hi.BLL.BD_Goods().Update(goodsList);
        }
    }

    public string sAttr = "";
    public string sAttr1 = "";

    public string GoodsType(string GoodsID, string CompID)
    {
        System.Text.StringBuilder goodsAttr = new System.Text.StringBuilder();
        string attr = string.Empty;
        DataTable dt = new Hi.BLL.BD_GoodsAttrs().GetAttrToAttrInfoDt("gattr.ID,gattr.CompID,gattr.GoodsID,gattr.AttrsName,ginfo.ID,ginfo.AttrsInfoName", " gattr.dr=0 and gattr.GoodsId=" + GoodsID + "  and gattr.CompID=" + CompID + "and IsNull(ginfo.ID,0)!=0", "");
        if (dt != null && dt.Rows.Count > 0)
        {
            //绑定商品属性
            int j = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //属性值为空是直接跳出
                if (dt.Rows[i]["AttrsInfoName"].ToString().Equals(""))
                    continue;
                if (attr != dt.Rows[i]["AttrsName"].ToString())
                {
                    //绑定第一次属性及属性值
                    attr = dt.Rows[i]["AttrsName"].ToString();
                    sAttr += attr + ":" + dt.Rows[i]["AttrsInfoName"].ToString() + "；";
                    sAttr1 = attr + ":" + dt.Rows[i]["AttrsInfoName"].ToString() + "；" + sAttr1;
                    j = 1;
                    goodsAttr.AppendFormat("</div><div class='attr' id='" + dt.Rows[i]["GoodsID"].ToString() + "_" + attr + "'>");
                    goodsAttr.AppendFormat("<span>" + attr + "：</span>");
                    goodsAttr.AppendFormat("<a class='a' id='" + dt.Rows[i]["GoodsID"].ToString() + "_" + i + "' onclick='onAttrGoods(\"" + dt.Rows[i]["GoodsID"].ToString() + "\",\"" + attr + "\",\"" + dt.Rows[i]["AttrsInfoName"].ToString() + "\",\"" + i + "\")'>" + dt.Rows[i]["AttrsInfoName"].ToString() + "</a>");
                }
                else
                {

                    goodsAttr.AppendFormat("<a id='" + dt.Rows[i]["GoodsID"].ToString() + "_" + i + "' onclick='onAttrGoods(\"" + dt.Rows[i]["GoodsID"].ToString() + "\",\"" + attr + "\",\"" + dt.Rows[i]["AttrsInfoName"].ToString() + "\",\"" + i + "\")'>" + dt.Rows[i]["AttrsInfoName"].ToString() + "</a>");
                    j++;
                }
            }
        }
        return goodsAttr.ToString();
    }

    #endregion

    /// <summary>
    /// 提供查询数据库的泛型方法,
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="StrHead"></param>
    /// <param name="StrWhere">以and开头</param>
    /// <param name="StrOrder">直接填写字段</param>
    /// <param name="Tran"></param>
    /// <returns></returns>
    public static List<T> GetDataSource<T>(string StrHead, string StrWhere, string StrOrder = "", SqlTransaction Tran = null)
    {
        #region 拼装Sql

        StringBuilder strSql = new StringBuilder();
        strSql.Append("select ");

        //*可以优化成反射
        if (string.IsNullOrWhiteSpace(StrHead) || StrHead.Trim() == "*")
        {
            strSql.Append(string.Join(",", typeof(T).GetProperties().Select(p => p.Name)));
        }
        else
        {
            strSql.Append(StrHead);
        }

        strSql.Append(" from ");
        strSql.Append(typeof(T).Name);

        if (!string.IsNullOrWhiteSpace(StrWhere))
        {
            strSql.Append(" where 1=1 ");
            strSql.Append(StrWhere);
        }
        if (!string.IsNullOrWhiteSpace(StrOrder))
        {
            strSql.Append(" order by ");
            strSql.Append(StrOrder);
        }

        #endregion

        DataSet ds = new DataSet();
        if (Tran != null)
            ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran);
        else
            ds = SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0] != null)
        {
            DataTable dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                List<T> resultList = new List<T>();
                foreach (DataRow dr in dt.Rows)
                {
                    T tModel = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in typeof(T).GetProperties())
                    {
                        if (dt.Columns.Contains(property.Name))
                        {
                            if (dr[property.Name] != DBNull.Value)
                            {
                                property.SetValue(tModel,
                                    property.Name.Contains("ID")
                                        ? Convert.ToInt32(dr[property.Name].ToString())
                                        : dr[property.Name], null);
                            }
                        }
                    }
                    resultList.Add(tModel);
                }
                return resultList;
            }
        }
        return default(List<T>);
    }


    #region   修改免手续费次数
    /// <summary>
    /// 修改支付配置表中的免手续费支付次数。
    /// </summary>
    /// <param name="compId">企业Id</param>
    /// <returns></returns>
    public static bool UpmzfcsByCompid(int compId)
    {
        bool fal = false;
        //根据企业Id查询是否有免支付次数
        //查询该企业的设置
        List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + compId, "");
        if (Sysl.Count > 0)
        {
            //免手续费支付次数
            int mfcs = Convert.ToInt32(Sysl[0].Pay_mfcs);

            if (mfcs > 0)
            {
                mfcs = mfcs - 1;
                Sysl[0].Pay_mfcs = mfcs;
                fal = new Hi.BLL.Pay_PaymentSettings().Update(Sysl[0]);
            }
        }


        return fal;

    }

    #endregion

    /// <summary>
    /// 查看支付设置，是否是担保支付
    /// </summary>
    /// <param name="CompID">厂商ID</param>
    /// <returns></returns>
    public static string PaySetingsValue(int CompID)
    {
        string str = "0";
        //查询该企业的设置
        List<Hi.Model.SYS_SysName> Sysl = new Hi.BLL.SYS_SysName().GetList("", " name='支付方式' and  CompID=" + CompID, "");
        if (Sysl != null && Sysl.Count > 0)
            str = Sysl[0].Value;

        return DesEncrypt(str, Common.EncryptKey);

    }

    /// <summary>
    /// 判断是否维护了银行卡收款帐号
    /// </summary>
    /// <returns></returns>
    public static int GetPayMentBank()
    {
        LoginModel logUser = HttpContext.Current.Session["UserModel"] as LoginModel;
        int CompID = logUser.CompID;
        int str = -1;
        //查询该企业的设置
        List<Hi.Model.PAY_PaymentBank> Sysl = new Hi.BLL.PAY_PaymentBank().GetList("", " Isno=1 and  CompID=" + CompID, "");
        if (Sysl != null && Sysl.Count > 0)
            str = Sysl[0].Isno;

        return str;

    }

    /// <summary>
    /// 查看是否启用微信支付或者支付宝支付
    /// </summary>
    /// <param name="CompID">厂商ID</param>
    /// <returns></returns>
    public static Hi.Model.Pay_PayWxandAli GetPayWxandAli(int CompID)
    {
        Hi.Model.Pay_PayWxandAli model = new Hi.Model.Pay_PayWxandAli();
        LoginModel logUser = HttpContext.Current.Session["UserModel"] as LoginModel;
        //if (logUser.Ctype == 1)
        //{
        //    model.wx_appid = "1qaz2wsx3edc4rfv5tgb6yhn7ujm8ik9";
        //    model.wx_mchid = "1385860502";
           
        //    return model;
        //}
        if (logUser.CompID != null && logUser.CompID > 0)
            CompID = logUser.CompID;
    
        //查询该企业的设置
        List<Hi.Model.Pay_PayWxandAli> Sysl = new Hi.BLL.Pay_PayWxandAli().GetList("", " CompID=" + CompID, "");
        if (Sysl != null && Sysl.Count > 0)
            model = Sysl[0];

        return model;

    }

    /// <summary>
    /// 网银支付----计算获取手续费
    /// </summary>
    /// <param name="CompID">厂商Id</param>  
    /// <param name="AccountType">支付账户类型 11个人，12 企业，13信用卡</param>
    /// <param name="bankid">银行ID</param>
    /// <param name="payPrice">支付金额</param>
    /// <returns></returns>
    public static string[] GetSxf(int CompID, string AccountType, string bankid, decimal payPrice)
    {
        string str = string.Empty;
        string sxfsq = "-1";
        decimal sxf = 0;
        decimal comp_sxf = 0;//收费方是厂商是，为了不改变支付金额，故声明此变量来记录，厂商手续费。

        //查询该企业的设置
        List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + CompID, "");
        if (Sysl.Count > 0)
        {
            //手续费收取
            sxfsq = Convert.ToString(Sysl[0].pay_sxfsq);

            //支付方式--线上or线下
            string zffs = Convert.ToString(Sysl[0].pay_zffs);

            //免手续费支付次数
            int mfcs = Convert.ToInt32(Sysl[0].Pay_mfcs);

            //银联支付比例
            decimal ylzfbl = Convert.ToDecimal(Sysl[0].pay_ylzfbl) / 1000;
            decimal ylzfstart = Convert.ToDecimal(Sysl[0].pay_ylzfstart);
            decimal ylzfend = Convert.ToDecimal(Sysl[0].pay_ylzfend);
            //网银支付
            decimal b2cwyzfbl = Convert.ToDecimal(Sysl[0].pay_b2cwyzfbl) / 1000;
            decimal bc2wyzfstart = Convert.ToDecimal(Sysl[0].vdef1);//封底

            //b2b网银支付
            decimal b2bwyzfbl = Convert.ToDecimal(Sysl[0].pay_b2bwyzf);


            //收取代理商手续费 (没有免支付次数时，才计算手续费)
            if (sxfsq == "1" && mfcs <= 0)
            {
                if (AccountType == "11"  || AccountType == "13")
                {
                    //if (bankid == "888")
                    //{
                    //    sxf = payPrice * ylzfbl;
                    //    if (sxf <= ylzfstart)
                    //        sxf = ylzfstart;
                    //    else if (sxf >= ylzfend)
                    //        sxf = ylzfend;
                    //}
                    //else

                    sxf = payPrice * b2cwyzfbl;
                    //if (sxf < bc2wyzfstart)
                     //   sxf = bc2wyzfstart;

                }
                else
                    sxf = b2bwyzfbl;
            }
            else if (sxfsq == "2" && mfcs <= 0)//厂商收费时判断，支付金额必须大于手续费
            {
                if (AccountType == "11" || AccountType == "13")
                {
                    //if (bankid == "888")
                    //{
                    //    sxf = payPrice * ylzfbl;
                    //    if (sxf <= ylzfstart)
                    //        sxf = ylzfstart;
                    //    else if (sxf >= ylzfend)
                    //        sxf = ylzfend;
                    //}
                    //else

                    sxf = payPrice * b2cwyzfbl;
                   // if (sxf < bc2wyzfstart)
                     //   sxf = bc2wyzfstart;

                }
                else
                    sxf = b2bwyzfbl;

                comp_sxf = sxf;

                if (sxf > payPrice)//手续费大于支付金额时，不允许支付
                    str = "支付金额小于手续费，不允许支付！";
                sxf = 0;
            }
        }
        string[] Josn = new string[] { sxf.ToString(), sxfsq, str,comp_sxf.ToString()}; //"{\"sxf\":\"" + sxf + "\",\"sxfsq\":\"" + sxfsq + "\",\"strMsg\":\"" + str + "\"}";

        return Josn;

    }


    /// <summary>
    ///快捷支付--获取手续费
    /// </summary>
    /// <param name="CompID">厂商ID</param>
    /// <param name="payPrice">支付金额</param>
    /// <returns></returns>
    public static string[] GetFastPay_sxf(int CompID,decimal payPrice) 
    {
        string str = string.Empty;

        string sxfsq = "-1";
        decimal sxf = 0;
        //查询该企业的设置
        List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + CompID, "");
        if (Sysl.Count > 0)
        {
            //手续费收取
            sxfsq = Convert.ToString(Sysl[0].pay_sxfsq);

            //支付方式--线上or线下
            string zffs = Convert.ToString(Sysl[0].pay_zffs);

            //免手续费支付次数
            int mfcs = Convert.ToInt32(Sysl[0].Pay_mfcs);

            //快捷支付比例
            decimal kjzfbl = Convert.ToDecimal(Sysl[0].pay_kjzfbl) / 1000;
            decimal kjzfstart = Convert.ToDecimal(Sysl[0].pay_kjzfstart);
            decimal kjzfend = Convert.ToDecimal(Sysl[0].pay_kjzfend);

            //收取代理商手续费 (没有免支付次数时，才计算手续费)
            if (sxfsq == "1" && mfcs <= 0)
            {

                sxf = payPrice * kjzfbl;
                //if (sxf <= kjzfstart)
                //    sxf = kjzfstart;
                //else if (sxf >= kjzfend)
                //    sxf = kjzfend;
            }
            //收取代理商手续费 (没有免支付次数时，才计算手续费)
            else if (sxfsq == "2" && mfcs <= 0)
            {

                sxf = payPrice * kjzfbl;
                //if (sxf <= kjzfstart)
               //     sxf = kjzfstart;
               // else if (sxf >= kjzfend)
                //    sxf = kjzfend;

                if (sxf > payPrice)//支付金额小于手续费时，提示不允许支付。
                    str="支付金额小于手续费，不允许支付";
                  

                sxf = 0;
            }
        }
        string[] Josn = new string[] { sxf.ToString(), sxfsq, str }; 
        return Josn;
    }

    /// <summary>
    /// 支付宝，微信支付----计算获取手续费
    /// </summary>
    /// <param name="CompID">厂商Id</param>  
    /// <param name="payPrice">支付金额</param>
    /// <returns></returns>
    public static string[] GetAli_Sxf(int CompID, decimal payPrice)
    {
        string str = string.Empty;
        string sxfsq = "-1";
        decimal sxf = 0;
        decimal comp_sxf = 0;//收费方是厂商是，为了不改变支付金额，故声明此变量来记录，厂商手续费。

        //查询该企业的设置
        List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + CompID, "");
        if (Sysl.Count > 0)
        {
            //手续费收取
            sxfsq = Convert.ToString(Sysl[0].pay_sxfsq);

            //支付方式--线上or线下
            string zffs = Convert.ToString(Sysl[0].pay_zffs);

            //免手续费支付次数
            int mfcs = Convert.ToInt32(Sysl[0].Pay_mfcs);

            //银联支付比例
            decimal ylzfbl = Convert.ToDecimal(Sysl[0].pay_ylzfbl) / 1000;
            decimal ylzfstart = Convert.ToDecimal(Sysl[0].pay_ylzfstart);
            decimal ylzfend = Convert.ToDecimal(Sysl[0].pay_ylzfend);
            //网银支付
            decimal b2cwyzfbl = Convert.ToDecimal(7)/1000;// Convert.ToDecimal(Sysl[0].pay_b2cwyzfbl) / 1000;
            decimal bc2wyzfstart = Convert.ToDecimal(Sysl[0].vdef1);//封底

            //b2b网银支付
            decimal b2bwyzfbl = Convert.ToDecimal(Sysl[0].pay_b2bwyzf);


            //收取代理商手续费 (没有免支付次数时，才计算手续费)
            if (sxfsq == "1" && mfcs <= 0)
            { 
                    sxf = payPrice * b2cwyzfbl;
                    //if (sxf < bc2wyzfstart)
                    //    sxf = bc2wyzfstart;

            }
            else if (sxfsq == "2" && mfcs <= 0)//厂商收费时判断，支付金额必须大于手续费
            {
                    sxf = payPrice * b2cwyzfbl;
                    //if (sxf < bc2wyzfstart)
                    //    sxf = bc2wyzfstart;
                comp_sxf = sxf;
                if (sxf > payPrice)//手续费大于支付金额时，不允许支付
                    str = "支付金额小于手续费，不允许支付！";
                sxf = 0;
            }
        }
        string[] Josn = new string[] { sxf.ToString(), sxfsq, str, comp_sxf.ToString() }; //"{\"sxf\":\"" + sxf + "\",\"sxfsq\":\"" + sxfsq + "\",\"strMsg\":\"" + str + "\"}";

        return Josn;

    }


    /// <summary>
    /// 格式化手续费
    /// </summary>
    /// <param name="d"></param>
    /// <param name="unit"></param>
    /// <returns></returns>
    public static decimal Round(decimal d, decimal unit)
    {

        decimal rm = d % unit;

        decimal result = d - rm;

        if (rm >= unit / 2)
        {

            result += unit;

        }

        return result;

    }

    /// <summary>
    /// 生产服务的连接接口
    /// </summary>
    /// <param name="Timeout"></param>
    /// <returns></returns>
    public static MyObjectServiceClient CallService(int Timeout = 0)
    {
        try
        {
            string url = ConfigurationManager.AppSettings["clientaddress"]; //WebConfigurationManager.AppSettings["clientaddress"];
            var servieClient = MyObjectServiceClient.GetInstance(url);
            servieClient.Format = "json";
            if (Timeout > 0)
            {
                servieClient.Timeout = new TimeSpan(Timeout);
            }
            return servieClient;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public static string RequestTokenCode
    {
        get
        {
            return "667372EC18C28D8BA490A6C3B78D4D58";
        }
    }

    /// <summary>
    /// 删除脚本的非法js
    /// </summary>
    /// <param name="Htmlstring">需要过滤的字符</param>
    /// <returns></returns>
    public static string NoHTML(string Htmlstring)
    {
        if (string.IsNullOrWhiteSpace(Htmlstring))
        {
            return Htmlstring;
        }
            
        //删除HTML   
        //Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "",
        //  RegexOptions.IgnoreCase);
        //Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
        //Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);
        //Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"",
        //  RegexOptions.IgnoreCase);
        //Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&",
        //  RegexOptions.IgnoreCase);
        //Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ",
        //  RegexOptions.IgnoreCase);
        //Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1",
        //  RegexOptions.IgnoreCase);
        //Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2",
        //  RegexOptions.IgnoreCase);
        //Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3",
        //  RegexOptions.IgnoreCase);
        //Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9",
        //  RegexOptions.IgnoreCase);
        //Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "",
        //  RegexOptions.IgnoreCase);

        //add by kb  防止sql注入
        Htmlstring = Regex.Replace(Htmlstring, "(?i)(insert|delete|truncate|update|declare|frame|style|expression|select|create|script|object|alert|href|1=1|<(.[^>]*)>.*?<(.[^>]*)>|<(.[^>]*)>|&(lt|#60)|&(gt|#62)) ", "");
        Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
        return Htmlstring;
    }

    /// <summary>
    /// 获取厂商是否购买服务
    /// </summary>
    /// <param name="CompID">厂商ID</param>
    /// <returns></returns>
    public static string GetCompService(string CompID,out string CreateDate)
    {
        Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(CompID));
        if (comp.EnabledEndDate.ToString() == "0001/1/1 0:00:00")
        {
            CreateDate = DateTime.Now.ToString();
            return DateTime.Now.ToString();
        }
       else  if (comp.EnabledEndDate < DateTime.Now.AddDays(1))
        {
            CreateDate = "1";
            return "1";
        }
        CreateDate = comp.EnabledStartDate.ToString();
        return comp.EnabledEndDate.ToString();
    }

    /// <summary>
    /// 获取厂商是否购买服务
    /// </summary>
    /// <param name="CompID">厂商ID</param>
    /// <returns></returns>
    public static string GetCompServiceNew(string CompID, out string CreateDate)
    {
        //add by hgh  知道某些企业不需购买服务
        //if (CompID == "4062")//
        //{
        //    CreateDate = "2020-12-31";
        //    return "2020-12-31";
        //}
        //if (CompID == "1046")//协强
        //{
        //    CreateDate = "2020-12-31";
        //    return "2020-12-31";
        //}
        //Hi.Model.BD_Company comp = new Hi.BLL.BD_Company().GetModel(Convert.ToInt32(CompID));
        //if (comp.EnabledEndDate.ToString() == "0001/1/1 0:00:00")
        //{
        //    CreateDate = "0";
        //    return "0";
        //}
        //else if (comp.EnabledEndDate < DateTime.Now.AddDays(1))
        //{
        //    CreateDate = "0";
        //    return "0";
        //}
        //CreateDate = comp.EnabledStartDate.ToString();
        //return comp.EnabledEndDate.ToString();

        //add by hgh
        CreateDate = "2099-12-31";
        return "2099-12-31";
    }

    /// <summary>
    /// 判断是否是销售经理
    /// </summary>
    /// <param name="DisSalesManID">ID</param>
    /// <param name="IDlist">下属员工ID</param>
    /// <returns></returns>
    public static bool GetDisSalesManType(string DisSalesManID, out string IDlist)
    {
        string listid = "0";
        Hi.Model.BD_DisSalesMan moder = new Hi.BLL.BD_DisSalesMan().GetModel(Convert.ToInt32(DisSalesManID));
        if (moder.SalesType == 2)
        {
          List< Hi.Model.BD_DisSalesMan> moderList = new Hi.BLL.BD_DisSalesMan().GetList("", " ParentID="+ DisSalesManID + " and IsEnabled=1 and dr=0", "");
            if (moderList.Count > 0)
            {
                foreach (var item in moderList)
                {
                    listid += item.ID + ",";
                }
                IDlist = listid.Substring(0, listid.Length - 1) + "," + DisSalesManID + "";//获取下属员工的ID集合
            }
            else
            {
                IDlist = DisSalesManID;
            }
            
            return true;
        }
        IDlist = "0";
        return false;
    
    }

    /// <summary>
    /// 获取用户下的厂商
    /// </summary>
    /// <param name="UserID">用户ID</param>
    /// <returns></returns>
    public static void ListComps(System.Web.UI.HtmlControls.HtmlSelect ddrComp, String UserID, String CompID)
    {
        List<Hi.Model.BD_Company> ListComp = new List<Hi.Model.BD_Company>();

        List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("*", " dr=0 and  Userid=" + UserID + " and IsAudit=2  ", " createdate ");

        string Compid = string.Join(",", ListCompUser.Where(T => T.CType == 2).ToList().Select(T => T.CompID));
        string[] CompDis = Compid.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        if (Compid != "")
        {
            ListComp = new Hi.BLL.BD_Company().GetList("ID,CompName", " isnull(dr,0)=0 and AuditState=2 and IsEnabled=1  and ID in(" + Compid + ")", "createdate desc");
        }

        ddrComp.DataSource = ListComp;
        ddrComp.DataTextField = "CompName";
        ddrComp.DataValueField = "id";
        if (CompID != "")
            ddrComp.Value = CompID;
        ddrComp.DataBind();
    }


    /// <summary>
    /// 绑定招商信息厂商
    /// </summary>
    /// <param name="ddrComp"></param>
    /// <param name="UserID"></param>
    /// <param name="disID"></param>
    public static void ListCMComps(System.Web.UI.HtmlControls.HtmlSelect ddrComp, String UserID, String disID)
    {
        string sql = @"select distinct com.ID,com.CompName from 
(select fca.CMID,fca.CompID from YZT_FCArea fca left join BD_Distributor dis 
on (fca.Province+fca.City+fca.Area=dis.Province+dis.City+dis.Area or 
fca.Province+fca.City=dis.Province+dis.City or fca.Province=dis.Province) 
and dis.IsEnabled=1 where 1=1 and  dis.ID=" + disID + " union select fcd.CMID,fcd.CompID from YZT_FCDis fcd where fcd.DisID=" + disID + " union select ID,CompID from YZT_CMerchants where type=1 and dr=0) a left join BD_Company com on a.CompID=com.ID";
        DataTable CMDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (CMDt != null && CMDt.Rows.Count > 0)
        {
            ddrComp.DataSource = CMDt;
            ddrComp.DataTextField = "CompName";
            ddrComp.DataValueField = "ID";
            ddrComp.DataBind();
        }
    }


    /// <summary>
    /// 绑定已申请招商信息厂商
    /// </summary>
    /// <param name="ddrComp"></param>
    /// <param name="UserID"></param>
    /// <param name="disID"></param>
    public static void ListFMComps(System.Web.UI.HtmlControls.HtmlSelect ddrComp, String UserID, String disID)
    {
        string sql = @"
select distinct com.ID,com.CompName from dbo.YZT_FirstCamp fc 
left join BD_Company com on fc.CompID=com.ID where DisID=" + disID + " and fc.dr=0";

        DataTable CMDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (CMDt != null && CMDt.Rows.Count > 0)
        {
            ddrComp.DataSource = CMDt;
            ddrComp.DataTextField = "CompName";
            ddrComp.DataValueField = "ID";
            ddrComp.DataBind();
        }
    }

    /// <summary>
    /// 判断是否已存在通过的代理商
    /// </summary>
    /// <param name="cmID">招商信息ID</param>
    /// <param name="compID">厂商ID</param>
    /// <returns>true 已存在 false 不存在 </returns>
    public static bool isFC(string cmID, string compID)
    {
        string sql = "select * from YZT_FirstCamp fc where isnull(fc.dr,0)=0 and fc.State=2 and fc.CMID=" + cmID + " and fc.CompID=" + compID;

        DataTable htDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (htDt != null && htDt.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// 判断代理商营业执照过期时间
    /// </summary>
    /// <param name="disid">代理商ID</param>
    /// <param name="type">1、代理商登录  2、厂商登录</param>
    /// <returns></returns>
    public static bool FCan(string disid, string type, out string msg)
    {
        bool falg = false;
        msg = type == "1" ? "您的营业执照" : "代理商的营业执照";

        string sql = @"select fc.ID,fc.CompID,fc.DisID,fc.type,fc.Content,an.ID anID,an.type antype,an.fileAlias,an.fileName,an.validDate from YZT_FCmaterials fc left join YZT_Annex an on fc.ID=an.fcID and an.fileAlias='4' where fc.DisID=" + disid + " and an.type=5";


        DateTime now = DateTime.Now;

        DataTable fcDt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (fcDt != null && fcDt.Rows.Count > 0)
        {
            DateTime startdate = DateTime.Parse(now.ToString("yyyy-MM-dd"));
            DateTime enddate = fcDt.Rows[0]["validDate"].ToString() == "" ? DateTime.MinValue : DateTime.Parse(Convert.ToDateTime(fcDt.Rows[0]["validDate"]).ToString("yyyy-MM-dd"));

            int d = enddate.Subtract(startdate).Days;

            if (d <= 0)
            {
                falg = true;
                msg += "已过期，请尽快上传新的营业执照";
            }
            else if (d < 7)
            {
                falg = true;
                msg += "还有" + d + "天过期";
            }
            else
            {
                msg = "";
            }
        }
        return falg;
    }

    /// <summary>
    /// 获取图片路径
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="resizeFormat"></param>
    /// <param name="compId"></param>
    /// <returns></returns>
    public static string GetPicURL(string fileName, string resizeFormat = null, string compId = null)
    {
        string companyId = compId;
        if (string.IsNullOrEmpty(compId))
        {
            LoginModel logUser = HttpContext.Current.Session["UserModel"] as LoginModel;
            companyId = (logUser != null) ? logUser.CompID.ToString() : "";
        }

        string basePath = Common.GetWebConfigKey("OssImgPath") + "company/" + companyId + "/";

        if (!string.IsNullOrEmpty(fileName) && fileName.Trim() != "X")
        {
            return basePath + fileName + (!string.IsNullOrEmpty(resizeFormat) ? "?x-oss-process=style/" + resizeFormat : "");
        }
        return Common.GetWebConfigKey("OssImgPath") + "havenopicmax.gif";
    }

    public static string GetPicBaseUrl(string compId = null)
    {
        string companyId = compId;
        if (string.IsNullOrEmpty(compId))
        {
            LoginModel logUser = HttpContext.Current.Session["UserModel"] as LoginModel;
            companyId = (logUser != null) ? logUser.CompID.ToString() : "";
        }

        return Common.GetWebConfigKey("OssImgPath") + "company/" + companyId + "/";
    }
}