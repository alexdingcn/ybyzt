using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
using System.Data.SqlClient;
using System.Web.Security;
using LitJson;
using Newtonsoft.Json;
using DIS_Logistics = Hi.Model.DIS_Logistics;
using System.Text.RegularExpressions;
using NPOI.SS.Formula.Functions;

/// <summary>
///Common 的摘要说明
/// </summary>
public partial class Common
{
    public static string Connection = ConfigurationManager.AppSettings["ConnectionString_Log"];
    static string e = string.Empty;
    static string Areas = string.Empty;
    static string disType = string.Empty;
    static string category = string.Empty;
    public static string EncryptKey = "HaiYuSE9SFOT";

    public Common()
    {
        e = "";
        Areas = "";
        disType = "";
    }

    #region 获取用户

    /// <summary>
    /// 根据OpenID得到用户实体
    /// </summary>
    /// <param name="openID"></param>
    /// <returns></returns>
    public Hi.Model.SYS_Users GetUserByOpenID(string openID)
    {
        List<Hi.Model.SYS_Users> userList = new Hi.BLL.SYS_Users().GetList("", " openID='" + openID + "' and dr=0 and IsEnabled=1", "");
        if (userList == null || userList.Count == 0 || userList.Count > 1)
            return null;
        Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
        foreach (Hi.Model.SYS_Users auser in userList)
        {
            user = auser;
        }
        return user;
    }

    /// <summary>
    /// 检测用户是否合法
    /// </summary>
    /// <param name="userID"></param>
    /// <param name="compID"></param>
    /// <param name="disID"></param>
    /// <returns></returns>
    public bool IsLegitUser(int userID, out Hi.Model.SYS_Users one, int compID = 0, int disID = 0)
    {
        one = null;
        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(userID);
        if (user == null)
        {
            return false;
        }

        string str = "userID=" + userID ;
        str += disID == 0 ? " and CType=1" : " and disID=" + disID ;
        str += compID == 0 ? " and CType=2" : " and compID=" + compID;
        if (compID == 0 && disID == 0) return false;

        List<Hi.Model.SYS_CompUser> list = new Hi.BLL.SYS_CompUser().GetList("",str + " and IsNull(dr,0)=0","");
        if (list == null||list.Count==0)
        {
            return false;
        }
        else
        {
            one = user;
            return true;
        }
    }

    #endregion

    #region 登录日志

    /// <summary>
    /// 登录操作日志
    /// </summary>
    /// <param name="EditLog">日志文本</param>
    /// <param name="LName">模块页面</param>
    /// <param name="Module">系统模块</param>
    /// <param name="LoginPage">模块页面</param>
    /// <param name="login">登录是否成功： 0： 不成功  1： 成功</param>
    /// <param name="LoginType">类型0：登录跟踪 1：表示操作跟踪</param>
    /// <param name="Remark"></param>
    /// <param name="Type">用户登录类型 0：平台总后台登录 1：经销商用户  2：公共用户  3：企业用户 4：企业管理员  5：经销商管理员 </param>
    public static void EditLog(string EditLog, string LName, string Remark, string Module, 
        string LoginPage, int LoginType, int login, int type,string IP = "")
    {

        try
        {
            int LoginId = 0;   //登录人Id
            string LoginName = LName; //登录人名称
            string LoginIp = string.Empty;  //登录Ip
            DateTime LoginStartDate = DateTime.Now; //登录时间
            int LoginUserType = type;  //用户登录类型

            LoginIp = IP;

            Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(LName);
            if (user != null)
            {
                LoginName = LName;
                LoginId = user.ID;
            }
            else
            {
                LoginName = LName;
                LoginId = 0;
            }

            string LoginNote = string.Empty;
            if (LoginType == 1)
            {
                LoginNote = LoginName + ":" + EditLog;
            }
            else
            {
                LoginNote = EditLog;
            }

            string sql = "insert into [A_LoginLog] (LoginStartDate,LoginNote,Module,LoginPage,LoginType,Remark,LoginId,LoginName,LoginIp,[LoginUserType],[Login]) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')";
            SqlHelper.ExecuteSql(Connection, string.Format(sql, LoginStartDate, LoginNote, Module, LoginPage, LoginType, Remark, LoginId, LoginName, LoginIp, LoginUserType, login));
        }
        catch { }

        #region
        //LSY.Model.A_AdminLog LogModel = new LSY.Model.A_AdminLog();
        //LogModel.LoginStartDate = DateTime.Now;
        //if (LoginType == 1)//操作日志
        //{
        //    LogModel.LoginNote = UModel.Name + ":" + EditLog;
        //}
        //else
        //{
        //    LogModel.LoginNote = EditLog;
        //}
        //LogModel.Module = Module;
        //LogModel.LoginPage = LoginPage;
        //LogModel.LoginType = LoginType;
        //LogModel.Remark = Remark;
        //if (UModel != null)
        //{
        //    LogModel.LoginId = UModel.id;
        //    LogModel.LoginName = UModel.LoginId;
        //    LogModel.LoginIp = UModel.LastLoginIP;
        //}
        //else
        //{
        //    LogModel.LoginIp = HttpContext.Current.Request.ServerVariables.Get("Remote_Addr").ToString();
        //}
        //new LSY.BLL.A_AdminLog().Add(LogModel);
        #endregion
    }

    /// <summary>
    /// 获取web经销商端ip
    /// </summary>
    /// <returns></returns>
    public static string GetWebClientIp()
    {

        string userIP = "::1";

        try
        {
            if (System.Web.HttpContext.Current == null || System.Web.HttpContext.Current.Request == null || System.Web.HttpContext.Current.Request.ServerVariables == null)
                return "";

            string CustomerIP = "";

            //CDN加速后取到的IP simone 090805
            CustomerIP = System.Web.HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            if (!string.IsNullOrEmpty(CustomerIP))
            {
                return CustomerIP;
            }

            CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];


            if (!String.IsNullOrEmpty(CustomerIP))
                return CustomerIP;

            if (System.Web.HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null)
            {
                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (CustomerIP == null)
                    CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            else
            {
                CustomerIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

            }

            if (string.Compare(CustomerIP, "unknown", true) == 0)
                return System.Web.HttpContext.Current.Request.UserHostAddress;
            return CustomerIP;
        }
        catch { }

        return userIP;

    }

    #endregion

    #region 商品分类

    /// <summary>
    /// 商品分类，包含其子分类
    /// </summary>
    /// <param name="disTypeID"></param>
    /// <returns></returns>
    public static string AllCategory(int categoryID)
    {
        string res = string.Empty;
        category = string.Empty;
        res = Category(categoryID) + categoryID.ToString();
        return res;
    }

    public static string Category(int categoryID)
    {
        
        string str = " ParentID=" + categoryID;
        str += "and isnull(isenabled ,0) = 1 and isnull(dr,0) = 0 ";
        List<Hi.Model.BD_GoodsCategory> areas = new Hi.BLL.BD_GoodsCategory().GetList("", str, "");
        if (areas.Count != 0)
        {
            foreach (Hi.Model.BD_GoodsCategory area in areas)
            {
                category += area.ID + ",";
                Category(area.ID);
            }
        }
        return category;
    }

    #endregion

    #region 商品可销售性

    /// <summary>
    /// 商品是否可用
    /// </summary>
    /// <param name="goodsInfoID"></param>
    /// <returns></returns>
    public static bool IsOffline(int goodsInfoID)
    {
        Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(goodsInfoID);
        if (goodsInfo == null || goodsInfo.IsEnabled == false || goodsInfo.dr == 1 || goodsInfo.IsOffline!=1)
            return false;
        Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
        if (goods == null || goods.IsEnabled == 0 || goods.dr == 1)
            return false;
        else
        {
            if (goods.OfflineStateDate != Convert.ToDateTime("0001-1-1 0:00:00") && goods.OfflineStateDate > DateTime.Now)
                return false;
            if (goods.OfflineEndDate!= Convert.ToDateTime("0001-1-1 0:00:00") && goods.OfflineEndDate < DateTime.Now)
                return false;
            return true;
        }
    }

    //获取核心企业下的经销商是否可以购买此goodsinfoid的商品
    public static bool getGoodInfoID(string compid, string disid,string goodsinfoid)
    {

        string sql = "select cd.GoodsID from YZT_ContractDetail cd left join YZT_Contract con on cd.ContID=con.ID where compid ="+compid+" ";
        sql += "and disid ="+disid+" and cd.GoodsID="+goodsinfoid+" union select cm.GoodsID from YZT_CMerchants cm left join YZT_FirstCamp ";
        sql += "fc on cm.ID =fc.CMID left join  YZT_ContractDetail cd on cd.FCID=fc.ID where cm.dr=0 and fc.State=2 and fc.compid ="+compid+" ";
        sql += "and fc.disid ="+disid+" and cm.GoodsID = "+goodsinfoid+" ";

        DataTable dsList = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

        if (dsList == null || dsList.Rows.Count <= 0)
            return false;
        else
            return true;


    }

    /// <summary>
    /// 根据经销商ID获取可采购的商品ID集合，返回list
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
        //    areaStr += " isnull(areaID,0) in (" + AreaID.Substring(0, AreaID.Length - 1) + ")"; //经销商区域
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
    /// 递归获取经销商所在区域 市--省--大区
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
    /// 递归获取经销商所在区域 市--省--大区
    /// </summary>
    //public static string Area(int AreaID)
    //{
    //    Hi.Model.BD_DisArea area = new Hi.BLL.BD_DisArea().GetModel(AreaID);
    //    if (area == null || area.dr == 1)
    //        return "";
    //    e += area.ID + ",";
    //    if (area.ParentID != 0)
    //    {
    //        Area(area.ParentID);
    //    }
    //    return e;
    //}

    /// <summary>
    /// 递归获取经销商区域 :从父集到子级
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
    /// 递归获取经销商分类 :从父集到子级(有问题)
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

    #region 促销和收藏的商品

    /// <summary>
    /// szj 收藏和促销
    /// </summary>
    /// <param name="compID"></param>
    /// <param name="disID"></param>
    /// <param name="strWhere"></param>
    /// <returns></returns>
    public static string GetGoodsListStr(string compID, string disID, string strWhere)
    {
        string sql = string.Format(@"select * from 
BD_Goods as g 
left join
(
	select GoodsID from BD_DisCollect 
	where CompID={0} and DisID={1} and ISNULL(IsEnabled,0)=1 and ISNULL(dr,0)=0 group by GoodsID
) as d 
on g.ID=d.GoodsID 
left join 
(
	select prod.GoodsID as proGoodsID from BD_Promotion as pro 
	left join BD_PromotionDetail as prod 
	on pro.ID=prod.ProID 
	where  pro.CompID={0} and (pro.ProStartTime<=GETDATE() and DATEADD(D,1,pro.ProEndTime)>GETDATE()) and ISNULL(pro.IsEnabled,0)=1 
	group by prod.GoodsID
) as b 
on b.proGoodsID=g.ID
where 1=1 {2}
order by b.proGoodsID desc,d.GoodsID desc,g.ID asc", compID, disID, strWhere);
        return sql;
    }

    #endregion

    #region 获取订单 和 订单状态

    /// <summary>
    /// 根据商品名称获取订单ID
    /// </summary>
    /// <returns></returns>
    public static string GetOrderByGoodsName(string goodsName, string compID, string disID = "")
    {
        string strWhere = " GoodsName like '%" + goodsName + "%' and compID='" + compID + "'";
        //if (disID.Trim() != "")
        //    strWhere += " and disID='" + disID + "'";
        List<Hi.Model.BD_Goods> goodsList = new Hi.BLL.BD_Goods().GetList("", strWhere, "");
        if (goodsList != null && goodsList.Count != 0)
        {
            string goodsID = goodsList.Aggregate("-1", (current, good) => current +","+ good.ID);
            List<Hi.Model.BD_GoodsInfo> goodsInfoList = new Hi.BLL.BD_GoodsInfo().GetList("",
                " GoodsID in (" + goodsID + ") and compID= '" + compID + "' and isoffline=1", "");
            if (goodsInfoList != null && goodsInfoList.Count != 0)
            {
                string goodsInfo = goodsInfoList.Aggregate("-1", (current, goodInfo) => current +","+ goodInfo.ID);
                string orderstr = " GoodsInfoID in(" + goodsInfo + ")";
                if (disID.Trim() != "")
                    orderstr += " and disID='" + disID + "'";
                List<Hi.Model.DIS_OrderDetail> detailList = new Hi.BLL.DIS_OrderDetail().GetList("", orderstr, "");
                if (detailList != null && detailList.Count != 0)
                {
                    return detailList.Aggregate("-1", (current, item) => current + ("," + item.OrderID));
                }
            }
        }
        return "-1";
    }

    /// <summary>
    /// 根据订单编号获取订单实体
    /// </summary>
    /// <param name="receiptNo"></param>
    /// <returns></returns>
    public Hi.Model.DIS_Order GetOrderByReceiptNo(string receiptNo)
    {
        List<Hi.Model.DIS_Order> orderList = new Hi.BLL.DIS_Order().GetList("",
            " ReceiptNo ='" + receiptNo.Trim() + "' and ISNULL(dr,0)=0 and Otype!=9", "");
        if (orderList == null || orderList.Count == 0)
            return null;
        Hi.Model.DIS_Order order = new Hi.Model.DIS_Order();
        foreach (var disOrder in orderList)
        {
            order = disOrder;
        }
        return order;
    }

    /// <summary>
    /// 返回订单各种信息状态
    /// </summary>
    /// <param name="Otype"></param>
    /// <param name="PayState"></param>
    /// <param name="OState"></param>
    /// <param name="ReturnState"></param>
    /// <param name="IsEnSend"></param>
    /// <param name="IsEnPay"></param>
    /// <param name="IsEnAudit"></param>
    /// <param name="IsEnReceive"></param>
    /// <param name="IsEnReturn"></param>
    public static void GetEspecialType(string Otype, string PayState, string OState, string ReturnState,
        out string IsEnSend, out string IsEnPay, out string IsEnAudit, out string IsEnReceive, out string IsEnReturn)
    {
        #region 发货

        if (OState == "2" && ((Otype == "0" || Otype == "2") && (PayState == "1" || PayState == "2") || Otype == "1"))
        {
            IsEnSend = "1";
        }
        else
        {
            IsEnSend = "0";
        }

        #endregion

        #region 支付

        if (!Otype.Equals("9") &&
            (ReturnState.Equals("0") || ReturnState.Equals("1")) &&
            !OState.Equals("6") &&
            (
                (Otype.Equals("1") && (!OState.Equals("-1") && !OState.Equals("0") && !OState.Equals("1")) &&
                 (PayState.Equals("0") || PayState.Equals("1"))) ||
                (!Otype.Equals("1") && (OState.Equals("2") || OState.Equals("4") || OState.Equals("5")) &&
                 (PayState.Equals("0") || PayState.Equals("1")))
                )
            )
        {
            IsEnPay = "1";
        }
        else
        {
            IsEnPay = "0";
        }

        #endregion

        #region 审核

        if (OState == "1" && ReturnState == "0")
        {
            IsEnAudit = "1";
        }
        else
        {
            IsEnAudit = "0";
        }

        #endregion

        #region 收货

        IsEnReceive = OState == "4" ? "1" : "0";

        #endregion

        #region 退货审核
        if (OState == "5" && ReturnState == "2")
        {
            IsEnReturn = "1";
        }
        else
        {
            IsEnReturn = "0";
        }
        #endregion
    }

    /// <summary>
    /// 企业订单状态 1：待付款 2：待发货 3：待收货 4：已收货 5：退款/售后 
    /// 6：已审核 7：未审核 8：已拒绝 9：已发货 10：已付款 11：部分付款 12：已作废
    /// </summary>
    /// <param name="ostate"></param>
    /// <param name="paystate"></param>
    /// <param name="otype"></param>
    /// <param name="returnState"></param>
    /// <returns></returns>
    public static string GetCompOrderType(int ostate, int paystate, int otype, int returnState)
    {
        string state = string.Empty;

        if ((ostate == (int)Enums.OrderState.已审 && paystate == (int)Enums.PayState.已支付) ||
            (otype == (int)Enums.OType.赊销订单 && ostate == +(int)Enums.OrderState.已审))
            state = "2";
        else if (ostate == (int)Enums.OrderState.退货处理 ||
            ostate == (int)Enums.OrderState.已退货 ||
            paystate == (int)Enums.PayState.申请退款 ||
            paystate == (int)Enums.PayState.已退款 ||
            returnState == (int)Enums.ReturnState.申请退货 ||
            returnState == (int)Enums.ReturnState.退货退款)
            state = "5";
        else switch (paystate)
            {
                case (int)Enums.PayState.未支付:
                    state = "1";
                    break;
                case (int)Enums.PayState.部分支付:
                    state = "11";
                    break;
                case (int)Enums.PayState.已支付:
                    state = "10";
                    break;
                default:
                    switch (ostate)
                    {
                        case (int)Enums.OrderState.已发货:
                            state = "3";
                            break;
                        case (int)Enums.OrderState.已到货:
                            state = "4";
                            break;
                        case (int)Enums.OrderState.已审:
                            state = "6";
                            break;
                        case (int)Enums.OrderState.待审核:
                            state = "7";
                            break;
                    }
                    break;
            }

        return state;
    }

    /// <summary>
    /// 返回订单状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string GetOState(string state)
    {
        if (state == "-2")
        {
            return "全部";
        }
        else if (state == "-1")
        {
            return "退回";
        }
        else if (state == "0")
        {
            return "已生成";
        }
        else if (state == "1")
        {
            return "已提交";
        }
        else if (state == "2")
        {
            return "已审核";
        }
        else if (state == "3")
        {
            return "退货处理";
        }
        else if (state == "4")
        {
            return "已发货";
        }
        else if (state == "5")
        {
            return "已到货";
        }
        else if (state == "6")
        {
            return "已作废";
        }
        else if (state == "7")
        {
            return "已退货";
        }
        else if (state == "8")
        {
            return "企业钱包申请";
        }

        return "";
    }

    /// <summary>
    /// 经销商订单状态
    /// </summary>
    /// <param name="ostate"></param>
    /// <param name="paystate"></param>
    /// <param name="otype"></param>
    /// <param name="returnState"></param>
    /// <returns></returns>
    public static string GetDisOrderType(int ostate, int paystate, int otype, int returnState)
    {
        string state = string.Empty;

        if ((ostate == (int)Enums.OrderState.已审 && paystate == (int)Enums.PayState.已支付) ||
                 otype == (int)Enums.OType.赊销订单 && ostate == +(int)Enums.OrderState.已审)
            state = "2";
        else if (ostate == (int)Enums.OrderState.退货处理 ||
        ostate == (int)Enums.OrderState.已退货 ||
        paystate == (int)Enums.PayState.申请退款 ||
        paystate == (int)Enums.PayState.已退款 ||
        returnState == (int)Enums.ReturnState.申请退货 ||
        returnState == (int)Enums.ReturnState.退货退款)
            state = "5";
        else switch (ostate)
            {
                case (int)Enums.OrderState.已到货:
                    state = "4";
                    break;
                default:
                    if (ostate == (int)Enums.OrderState.已审 && paystate != (int)Enums.PayState.已支付)
                        state = "6";
                    else if (ostate == (int)Enums.OrderState.待审核)
                        state = "7";
                    else if (ostate == (int)Enums.OrderState.退回)
                        state = "8";
                    else if (ostate == (int)Enums.OrderState.已发货)
                        state = "9";
                    else if (ostate == (int)Enums.OrderState.已作废)
                        state = "12";
                    else if (paystate == (int)Enums.PayState.未支付)
                        state = "1";
                    else if (paystate == (int)Enums.PayState.已支付)
                        state = "10";
                    else if (paystate == (int)Enums.PayState.部分支付)
                        state = "11";
                    break;
            }

        return state;
    }

    /// <summary>
    /// 中文显示
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public static string GetOrderType(string state)
    {
        if (state == "1")
        {
            return "待付款";
        }
        else if (state == "2")
        {
            return "待发货";
        }
        else if (state == "3")
        {
            return "待收货";
        }
        else if (state == "4")
        {
            return "已收货";
        }
        else if (state == "5")
        {
            return "退款/售后";
        }
        else if (state == "6")
        {
            return "已审核";
        }
        else if (state == "7")
        {
            return "未审核";
        }
        else if (state == "8")
        {
            return "已拒绝";
        }
        else if (state == "9")
        {
            return "已发货";
        }
        else if (state == "10")
        {
            return "已付款";
        }
        else if (state == "11")
        {
            return "部分付款";
        }
        else if (state == "12")
        {
            return "已作废";
        }
        return "";
    }

    #endregion

    #region 物流

    /// <summary>
    /// 获取物流信息
    /// </summary>
    /// <param name="outID"></param>
    /// <returns></returns>
    public static List<Hi.Model.DIS_Logistics> GetExpress(string outID)
    {
        List<Hi.Model.DIS_Logistics> reList = new List<DIS_Logistics>();

        Hi.Model.DIS_OrderOut OrderOutModel = new Hi.BLL.DIS_OrderOut().GetModel(Convert.ToInt32(outID));
        if (OrderOutModel != null)
        {
            List<Hi.Model.DIS_Logistics> l = new Hi.BLL.DIS_Logistics().GetList("",
                "isnull(dr,0)=0 and orderId=" + OrderOutModel.OrderID + " and orderOutId=" + OrderOutModel.ID, "");
            if (l.Count > 0)
            {
                foreach (Hi.Model.DIS_Logistics item in l)
                {
                    Hi.Model.DIS_Logistics model = new Hi.BLL.DIS_Logistics().GetModel(item.Id);
                    //item.ComPName;
                    //item.LogisticsNo;

                    if (item.Type == 1)//爱快递
                    {
                        //现有物流
                        string ApiKey = ConfigurationManager.AppSettings["ExpressApiKey"];
                        string typeCom = model.ComPName;
                        typeCom = TypeCom(typeCom);
                        string nu = model.LogisticsNo;
                        //string apiurl = "http://api.kuaidi100.com/api?id=" + ApiKey + "&com=" + typeCom + "&nu=" + nu + "&show=0&muti=1&order=asc";
                        string apiurl = "http://www.aikuaidi.cn/rest/?key=" + ApiKey + "&order=" + nu + "&id=" + typeCom + "&ord=asc&show=json";
                        WebRequest request = WebRequest.Create(@apiurl);
                        WebResponse response = request.GetResponse();
                        Stream stream = response.GetResponseStream();
                        Encoding encode = Encoding.UTF8;
                        StreamReader reader = new StreamReader(stream, encode);
                        string detail = reader.ReadToEnd();
                        Logistics logistics = JsonConvert.DeserializeObject<Logistics>(detail);
                        //if (logistics.message == "ok")
                        //{
                        if (logistics.errCode == "0")
                        {
                            
                            List<Information> information = logistics.data;
                            model.Context = JsonConvert.SerializeObject(information);
                            new Hi.BLL.DIS_Logistics().Update(model);
                        }
                    }
                    
                    reList.Add(model);
                }
            }
            else
            {
                return null;
            }

        }
        return reList;
    }

    /// <summary>
    /// 物流公司匹配
    /// </summary>
    /// <param name="typeCom"></param>
    /// <returns></returns>
    public static string TypeCom(string typeCom)
    {
        if (typeCom == "AAE全球专递")
        {
            typeCom = "aae";
        }
        if (typeCom == "安捷快递")
        {
            typeCom = "anjiekuaidi";
        }
        if (typeCom == "安信达快递")
        {
            typeCom = "anxindakuaixi";
        }
        if (typeCom == "百福东方")
        {
            typeCom = "baifudongfang";
        }
        if (typeCom == "彪记快递")
        {
            typeCom = "biaojikuaidi";
        }
        if (typeCom == "BHT")
        {
            typeCom = "bht";
        }
        if (typeCom == "BHT")
        {
            typeCom = "bht";
        }
        if (typeCom == "希伊艾斯快递")
        {
            typeCom = "cces";
        }
        if (typeCom == "中国东方")
        {
            typeCom = "Coe";
        }
        if (typeCom == "长宇物流")
        {
            typeCom = "changyuwuliu";
        }
        if (typeCom == "大田物流")
        {
            typeCom = "datianwuliu";
        }
        if (typeCom == "德邦物流")
        {
            typeCom = "debangwuliu";
        }
        if (typeCom == "DPEX")
        {
            typeCom = "dpex";
        }
        if (typeCom == "DHL")
        {
            typeCom = "dhl";
        }
        if (typeCom == "D速快递")
        {
            typeCom = "dsukuaidi";
        }
        if (typeCom == "fedex")
        {
            typeCom = "fedex";
        }
        if (typeCom == "飞康达物流")
        {
            typeCom = "feikangda";
        }
        if (typeCom == "凤凰快递")
        {
            typeCom = "fenghuangkuaidi";
        }
        if (typeCom == "港中能达物流")
        {
            typeCom = "ganzhongnengda";
        }
        if (typeCom == "广东邮政物流")
        {
            typeCom = "guangdongyouzhengwuliu";
        }
        if (typeCom == "汇通快运")
        {
            typeCom = "huitongkuaidi";
        }
        if (typeCom == "恒路物流")
        {
            typeCom = "hengluwuliu";
        }
        if (typeCom == "华夏龙物流")
        {
            typeCom = "huaxialongwuliu";
        }
        if (typeCom == "佳怡物流")
        {
            typeCom = "jiayiwuliu";
        }
        if (typeCom == "京广速递")
        {
            typeCom = "jinguangsudikuaijian";
        }
        if (typeCom == "急先达")
        {
            typeCom = "jixianda";
        }
        if (typeCom == "佳吉物流")
        {
            typeCom = "jiajiwuliu";
        }
        if (typeCom == "加运美")
        {
            typeCom = "jiayunmeiwuliu";
        }
        if (typeCom == "快捷速递")
        {
            typeCom = "kuaijiesudi";
        }
        if (typeCom == "联昊通物流")
        {
            typeCom = "lianhaowuliu";
        }
        if (typeCom == "龙邦物流")
        {
            typeCom = "longbanwuliu";
        }
        if (typeCom == "民航快递")
        {
            typeCom = "minghangkuaidi";
        }
        if (typeCom == "配思货运")
        {
            typeCom = "peisihuoyunkuaidi";
        }
        if (typeCom == "全晨快递")
        {
            typeCom = "quanchenkuaidi";
        }
        if (typeCom == "全际通物流")
        {
            typeCom = "quanjitong";
        }
        if (typeCom == "全日通快递")
        {
            typeCom = "quanritongkuaidi";
        }
        if (typeCom == "全一快递")
        {
            typeCom = "quanyikuaidi";
        }
        if (typeCom == "盛辉物流")
        {
            typeCom = "shenghuiwuliu";
        }
        if (typeCom == "速尔物流")
        {
            typeCom = "suer";
        }
        if (typeCom == "盛丰物流")
        {
            typeCom = "shengfengwuliu";
        }
        if (typeCom == "天地华宇")
        {
            typeCom = "tiandihuayu";
        }
        if (typeCom == "天天快递")
        {
            typeCom = "tiantian";
        }
        if (typeCom == "TNT")
        {
            typeCom = "tnt";
        }
        if (typeCom == "UPS")
        {
            typeCom = "ups";
        }
        if (typeCom == "万家物流")
        {
            typeCom = "wanjiawuliu";
        }
        if (typeCom == "文捷航空速递")
        {
            typeCom = "wenjiesudi";
        }
        if (typeCom == "伍圆速递")
        {
            typeCom = "wuyuansudi";
        }
        if (typeCom == "万象物流")
        {
            typeCom = "wanxiangwuliu";
        }
        if (typeCom == "新邦物流")
        {
            typeCom = "xinbangwuliu";
        }
        if (typeCom == "信丰物流")
        {
            typeCom = "xinfengwuliu";
        }
        if (typeCom == "星晨急便")
        {
            typeCom = "xingchengjibian";
        }
        if (typeCom == "鑫飞鸿物流")
        {
            typeCom = "xinhongyukuaidi";
        }
        if (typeCom == "亚风速递")
        {
            typeCom = "yafengsudi";
        }
        if (typeCom == "一邦速递")
        {
            typeCom = "yibangwuliu";
        }
        if (typeCom == "优速物流")
        {
            typeCom = "youshuwuliu";
        }
        if (typeCom == "远成物流")
        {
            typeCom = "yuanchengwuliu";
        }
        if (typeCom == "圆通速递")
        {
            typeCom = "yuantong";
        }
        if (typeCom == "源伟丰快递")
        {
            typeCom = "yuanweifeng";
        }
        if (typeCom == "元智捷诚快递")
        {
            typeCom = "yuanzhijiecheng";
        }
        if (typeCom == "越丰物流")
        {
            typeCom = "yuefengwuliu";
        }
        if (typeCom == "韵达快递")
        {
            typeCom = "yunda";
        }
        if (typeCom == "源安达")
        {
            typeCom = "yuananda";
        }
        if (typeCom == "运通快递")
        {
            typeCom = "yuntongkuaidi";
        }
        if (typeCom == "宅急送")
        {
            typeCom = "zhaijisong";
        }
        if (typeCom == "中铁快运")
        {
            typeCom = "zhongtiewuliu";
        }
        if (typeCom == "中通速递")
        {
            typeCom = "zhongtong";
        }
        if (typeCom == "中通快递")
        {
            typeCom = "zhongtong";
        }
        if (typeCom == "中邮物流")
        {
            typeCom = "zhongyouwuliu";
        }
        if (typeCom == "顺丰速递")
        {
            typeCom = "shunfeng";
        }
        if (typeCom == "申通快递")
        {
            typeCom = "shentong";
        }
        if (typeCom == "EMS快递")
        {
            typeCom = "ems";
        }
        return typeCom;
    }

    #region 物流返回

    public class Logistics
    {
        public string nu { get; set; }//物流单号
        public string comcontact { get; set; }//快递电话
        public string companytype { get; set; }
        public string com { get; set; }//物流公司编号
        public string status { get; set; }//查询结果状态： 0：物流单暂无结果， 1：查询成功， 2：接口出现异常，
        public string codenumber { get; set; }
        public string state { get; set; }//快递单当前的状态 ：　 
        //0：在途，即货物处于运输过程中；
        //1：揽件，货物已由快递公司揽收并且产生了第一条跟踪信息；
        //2：疑难，货物寄送过程出了问题；
        //3：签收，收件人已签收；
        //4：退签，即货物由于用户拒签、超区等原因退回，而且发件人已经签收；
        //5：派件，即快递正在进行同城派件；
        //6：退回，货物正处于退回发件人的途中；
        public List<Information> data { get; set; }
        public string message { get; set; }
        public string comurl { get; set; }
        public string condition { get; set; }
        public string ischeck { get; set; }
        public string errCode { get; set; }
    }

    public class Information
    {
        public string time { get; set; }
        public string location { get; set; }
        public string context { get; set; }
        public string content { get; set; }
    }

    #endregion

    #endregion

    #region 生成编码

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
    public static string GetCode(string strName, string OrderId)
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
        { }
        return returnstr;
    }

    #endregion

    #region 订单日志

    /// <summary>
    /// 新增业务日志,带事务（可为空）
    /// </summary>
    /// <param name="LogClass">单据名称</param>
    /// <param name="ApplicationId">业务ID</param>
    /// <param name="LogType">业务名称 新增、审核、退回、提交...</param>
    /// <param name="LogRemark">备注</param>
    /// <param name="order">新增时，改实体ID暂无</param>
    public static string AddSysBusinessLog(Hi.Model.DIS_Order order, Hi.Model.SYS_Users user, string LogClass, string ApplicationId, string LogType, string LogRemark, SqlTransaction TranSaction = null)
    {
        string res = string.Empty;
        try
        {
            //Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(order.modifyuser);
            if (user == null)
                return "false";

            Hi.Model.SYS_SysBusinessLog SysBusinessLogModel = new Hi.Model.SYS_SysBusinessLog();
            SysBusinessLogModel.CompID = order.CompID;
            SysBusinessLogModel.LogClass = LogClass;
            SysBusinessLogModel.ApplicationId = Convert.ToInt32(ApplicationId);
            SysBusinessLogModel.LogType = LogType;
            SysBusinessLogModel.OperatePersonId = order.modifyuser;
            SysBusinessLogModel.OperatePerson = string.IsNullOrEmpty(user.TrueName) ? user.UserName : user.TrueName;
            SysBusinessLogModel.LogRemark =  LogRemark + "（手机）";
            SysBusinessLogModel.LogTime = DateTime.Now;
            SysBusinessLogModel.ts = DateTime.Now;

            res = new Hi.BLL.SYS_SysBusinessLog().Add(SysBusinessLogModel, TranSaction).ToString();
            if (res == "0")
                return "0";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return res;
    }



    #endregion

    #region 促销
    
    /// <summary>
    /// 返回最新促销价格
    /// </summary>
    /// <returns></returns>
    public static decimal GetProPrice(object GoodsID, string GoodsInfoId, string CompId, out int proID)
    {
        proID = 0;
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
            sql = string.Format(@"select  top 1 pro.ID,pro.CompID,pro.ProType,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and isnull(pro.dr,0)=0 and  ISNULL(pro.IsEnabled,0)=1 and (pro.ProStartTime<=GETDATE() and DATEADD(D,1,pro.ProEndTime)>GETDATE()) order by pro.CreateDate desc", GoodsID.ToString(), CompId);
        }

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

        if (dt != null && dt.Rows.Count > 0)
        {
            proID = Convert.ToInt32(dt.Rows[0]["ID"].ToString());
            if (dt.Rows[0]["ProType"].ToString() == "3")//满送
            {
                Hi.Model.BD_GoodsInfo info = new Hi.BLL.BD_GoodsInfo().GetModel(GoodsInfoId.ToInt(0));
                return string.Format(info.TinkerPrice.ToString()).ToDecimal(0).ToString().ToDecimal(0);
            }
            return string.Format(dt.Rows[0]["GoodsPrice"].ToString()).ToDecimal(0).ToString().ToDecimal(0);
        }
        return 0;
    }


    /// <summary>
    /// 返回满赠数量
    /// </summary>
    /// <returns></returns>
    public static decimal GetProNum(int ProID, decimal Num)
    {
        string sql = string.Empty;
        sql = "select pro.Discount,prod.GoodsPrice from BD_Promotion  as pro left join BD_PromotionDetail as prod on pro.ID = prod.ProID";
        sql += "where pro.ID = " + ProID + " and ISNULL(pro.IsEnabled,0)=1 and isnull(pro.dr,0)=0  and pro.protype =3";
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                //int sendNum = Num.ToInt(0) / Convert.ToInt32(item["DisCount"]);
                int sendNum = (int)(Num / decimal.Parse(item["DisCount"].ToString()));
                return decimal.Parse(item["GoodsPrice"].ToString()) * sendNum;
                //return Convert.ToInt32(item["GoodsPrice"]) * sendNum;
            }
        }
        return 0;
    }

    /// <summary>
    /// 获取商品最新的促销
    /// </summary>
    /// <param name="GoodsID">商品ID</param>
    /// <param name="GoodsInfoId">商品信息ID</param>
    /// <param name="CompId">核心企业ID</param>
    /// <param name="Num">商品购买数量</param>
    /// <param name="pty">促销类型（输入参数）</param>
    /// <param name="ppty">促销提示（输入参数）</param>
    /// <param name="proID">促销ID（输入参数）</param>
    /// <returns>返回</returns>
    public static decimal GetProNum(string GoodsID, string GoodsInfoId, int CompId, decimal Num, out string pty, out string ppty, out string proID)
    {
        pty = "0";
        ppty = "";
        proID = "";
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
                    proID = item["ID"].ToString();
                    pty = item["ProType"].ToString();
                    ppty = item["Type"] + "," + pty + "," + item["GoodsPrice"] + "," + item["DisCount"];

                    //促销类型，促销方式，促销价格（1、2、4是商品价格，3是送的商品数量），
                    //打折率（1、2 是0，3是满件数  4是打折0-100）
                    //赔送商品数量
                    if (Num >= Convert.ToDecimal(item["DisCount"]))
                    {
                        if (item["ProType"].ToString() == "3")
                        {
                            decimal sendNum = Convert.ToInt32(Math.Floor(Num / Convert.ToInt32(item["DisCount"])));
                            return Convert.ToDecimal(item["GoodsPrice"]) * sendNum;
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
    /// 判断订单明细中的促销商品的促销活动是否可用(3个参数)
    /// 判断该商品有无促销(2个参数)
    /// </summary>
    /// <param name="CompID">企业ID</param>
    /// <param name="GoodsID">商品ID</param>
    /// <param name="GoodInfoID">商品信息ID</param>
    /// <param name="GoodsPrice">订单商品价格</param>
    /// <returns>0，可用，1、不可用</returns>
    public static int GetOrderPro(int CompID, int GoodInfoID, decimal GoodsPrice = 0)
    {
        Hi.Model.BD_GoodsInfo InfoModel = new Hi.BLL.BD_GoodsInfo().GetModel(GoodInfoID);

        if (InfoModel != null)
        {
            string sql = string.Empty;
            if (GoodsPrice!=0)
                sql = string.Format(@"select * from BD_Promotion as pro left join 
BD_PromotionDetail as prod on pro.ID=prod.ProID 
where pro.CompID={0} and  prod.GoodsID={1} and prod.GoodInfoID={2} and GoodsPrice={3}
and ISNULL(pro.IsEnabled,0)=1 and isnull(pro.dr,0)=0 and  (pro.ProStartTime<=GETDATE() and DATEADD(D,1,pro.ProEndTime)>GETDATE())", CompID, InfoModel.GoodsID, GoodInfoID, GoodsPrice);
            else
            {
                sql = string.Format(@"select * from BD_Promotion as pro left join 
BD_PromotionDetail as prod on pro.ID=prod.ProID 
where pro.CompID={0} and  prod.GoodsID={1} and prod.GoodInfoID={2} 
and ISNULL(pro.IsEnabled,0)=1 and isnull(pro.dr,0)=0 and  (pro.ProStartTime<=GETDATE() and DATEADD(D,1,pro.ProEndTime)>GETDATE())", CompID, InfoModel.GoodsID, GoodInfoID);

            }
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
    public static int GetProNum(string GoodsID, string GoodsInfoId, int CompId, string Num, out string pty)
    {
        pty = "0";
        if (GoodsID.ToString() == "" || CompId.ToString() == "")
        {
            return 0;
        }

        string sql = string.Empty;

        if (GoodsInfoId != "")
        {
            sql = string.Format(@"select pro.ID,pro.CompID,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and prod.GoodInfoID={2} and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 order by pro.CreateDate desc", GoodsID.ToString(), CompId, GoodsInfoId);
        }
        else
        {
            sql = string.Format(@"select pro.ID,pro.CompID,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID where  prod.GoodsID={0} and pro.CompID={1} and isnull(pro.dr,0)=0 and ISNULL(pro.IsEnabled,0)=1 order by pro.CreateDate desc", GoodsID.ToString(), CompId);
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
                    //赔送商品数量
                    if (Num.ToInt(0) >= Convert.ToInt32(item["DisCount"]))
                    {
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
    public static decimal GetProPrice(decimal TotalPrice, out string proID, int CompId)
    {
        proID = "0";
        //ProIDD = "0";
        //ProType = "";
        string sql = string.Format(@"select p.ID,p.Type,p.ProType,p.ProStartTime,p.ProEndTime,pd2.OrderPrice, pd2.Discount,pd2.ID IDD from BD_Promotion as p left join BD_PromotionDetail2 as pd2 on p.ID=pd2.ProID where p.[type]=2 and IsEnabled=1 and p.CompID={0}  and (p.ProStartTime<=GETDATE() and DATEADD(D,1,p.ProEndTime)>GETDATE()) order by p.CreateDate desc,pd2.OrderPrice desc", CompId);

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                decimal Price = 0;
                if (item["OrderPrice"].ToString().ToDecimal(0) <= TotalPrice)
                {
                    proID = item["ID"].ToString();
                    //ProIDD = item["IDD"].ToString();
                    if (item["ProType"].ToString() == "5")
                    {
                        //满减
                        Price = item["Discount"].ToString().ToDecimal(0);
                        //ProType = "5," + item["OrderPrice"].ToString().ToDecimal(0) + "," + item["Discount"].ToString();
                    }
                    else if (item["ProType"].ToString() == "6")
                    {
                        //满折
                        decimal Discount = item["Discount"].ToString().ToDecimal(0);
                        TotalPrice = TotalPrice * (1 - (Discount / 100));

                        Price = TotalPrice;
                        //ProType = "6," + item["OrderPrice"].ToString().ToDecimal(0) + "," + item["Discount"].ToString();
                    }
                }
                return Price;
            }
        }
        return 0;
    }

       /// <summary>
    /// 判断订单促销
    /// </summary>
    /// <param name="TotalPrice">下单总金额</param>
    /// <returns>返回促销的金额</returns>
    public static decimal GetProPrice(decimal TotalPrice, out string proID,out string ProIDD,out string ProType,int CompId)
    {
        proID = "0";
        ProIDD = "0";
        ProType = "";
        string sql = string.Format(@"select p.ID,p.Type,p.ProType,p.ProStartTime,p.ProEndTime,pd2.OrderPrice, pd2.Discount,pd2.ID IDD from BD_Promotion as p left join BD_PromotionDetail2 as pd2 on p.ID=pd2.ProID where p.[type]=2 and IsEnabled=1 and p.CompID={0}  and (p.ProStartTime<=GETDATE() and DATEADD(D,1,p.ProEndTime)>GETDATE()) order by p.CreateDate desc,pd2.OrderPrice desc", CompId);

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

        if (dt != null && dt.Rows.Count > 0)
        {
            foreach (DataRow item in dt.Rows)
            {
                decimal Price = 0;
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
                }
                return Price;
            }
        }
        return 0;
    }
    #endregion
    /// <summary>
    /// 获取此时订单促销的列表
    /// </summary>
    /// <param name="compID"></param>
    /// <returns></returns>
    public static List<BD_GoodsCategory.ResultOrderPro> ReturnOrderProList(string compID)
    {
        string sql = string.Format(@"select p.ID,p.Type,p.ProType,p.ProStartTime,p.ProEndTime,pd2.OrderPrice, pd2.Discount,pd2.ID IDD from BD_Promotion as p left join BD_PromotionDetail2 as pd2 on p.ID=pd2.ProID where p.[type]=2 and IsEnabled=1 and p.CompID={0}  and (p.ProStartTime<=GETDATE() and DATEADD(D,1,p.ProEndTime)>GETDATE()) order by p.CreateDate desc,pd2.OrderPrice desc", compID);

        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        List<BD_GoodsCategory.ResultOrderPro> list = new List<BD_GoodsCategory.ResultOrderPro>();
        if (dt != null && dt.Rows.Count > 0)
        {
            list.AddRange(
                from DataRow item in dt.Rows
                select new BD_GoodsCategory.ResultOrderPro()
                {
                    ProID = item["ID"].ToString(),
                    ProType = item["ProType"].ToString(),
                    ProStartTime = Convert.ToDateTime(item["ProStartTime"].ToString()).ToString("yy-MM-dd"),
                    ProEndTime = Convert.ToDateTime(item["ProEndTime"].ToString()).ToString("yy-MM-dd"),
                    OrderPrice = Convert.ToDecimal(item["OrderPrice"].ToString()).ToString("0.00"),
                    Discount = Convert.ToDecimal(item["Discount"].ToString()).ToString("0.00")
                });
        }

        return list;
    }

    /// <summary>
    /// 订单促销返回提示
    /// </summary>
    /// <param name="ProIDD">订单促销明细ID</param>
    /// <param name="ProPrice"></param>
    /// <returns></returns>
    public static string proOrderType(string ProIDD, string ProPrice, string ProType)
    {
        string str = "";

        if (ProType != "")
        {
            string[] type = ProType.Split(new char[] { ',' });
            if (type.Length == 3)
            {
                if (type[0].ToString() == "5")
                {
                    //满减
                    str = "满减活动(满" + type[1].ToDecimal(0).ToString("N") + "减" + type[2].ToDecimal(0).ToString("N") + ")";
                }
                else if (type[0].ToString() == "6")
                {
                    str = "满折活动(满" + type[1].ToDecimal(0).ToString("N") + "打折" + type[2].ToDecimal(0).ToString("N") + "%)";
                }
            }

        }
        else if (ProIDD != "" && ProType == "")
        {
            string sql = string.Format(@"select pro.ID,pro.CompID,pro.Type,pro.ProType,pro.DisCount,pro.ProStartTime,pro.ProEndTime,prod.GoodsID,prod.GoodInfoID,prod.GoodsPrice,prod2.OrderPrice,prod2.Discount as ProDiscount,prod2.ID as IDD from BD_Promotion as pro left join BD_PromotionDetail as prod on pro.ID=prod.ProID left join BD_PromotionDetail2 as prod2 on pro.ID=prod2.ProID  where prod2.ID={0} order by pro.CreateDate desc", ProIDD);

            DataTable dt = DBUtility.SqlHelper.Query(DBUtility.SqlHelper.LocalSqlServer, sql).Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow item in dt.Rows)
                {
                    decimal Price = 0;

                    Price = item["OrderPrice"].ToString().ToDecimal(0);

                    if (item["ProType"].ToString() == "5")
                    {
                        //满减
                        str = "满减活动(满" + Price.ToString("N") + "减" + item["ProDiscount"].ToString().ToDecimal(0).ToString("N") + ")";
                    }
                    else
                    {
                        str = "满折活动(满" + Price.ToString("N") + "打折" + item["ProDiscount"].ToString().ToDecimal(0).ToString("N") + "%)";
                    }
                }
            }
        }

        return str;
    }

 

    #region 查询经销商是否可赊销 和 订单是否需要审核

    /// <summary>
    /// 查询经销商是否可赊销
    /// </summary>
    /// <param name="DisId">经销商Id</param>
    /// <returns></returns>
    public static bool getCreditType(int DisId)
    {
        bool falg = false;

        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(DisId);

        if (disModel != null)
        {
            if (disModel.CreditType.ToString().Equals("1"))
            {
                falg = true;
            }
        }
        return falg;
    }

    /// <summary>
    /// 判断订单是否需要审核
    /// </summary>
    /// <param name="DisId">经销商Id</param>
    /// <param name="Otype">订单类型</param>
    /// <returns></returns>
    public static int OrderEnAudit(int DisId, int Otype)
    {
        /*
         * 销售订单、赊销订单 经销商审核标志为无需审时 
         * 特价订单  都需要审核
         **/
        int isAudit = 0;

        Hi.Model.BD_Distributor DisModel = new Hi.BLL.BD_Distributor().GetModel(DisId);
        if (Otype == 0 || Otype == 1)
        {
            if (DisModel != null)
            {
                if (DisModel.IsCheck == 0)//0不需要 1需要
                {
                    isAudit = 1;
                }
            }
        }
        return isAudit;//0需审核 1不需要
    }

    #endregion


    #region 分页模拟

    /// <summary>
    /// 加载数据，* 可以优化，不然大数据会卡呀
    /// </summary>
    /// <param name="preID">当前ID</param>
    /// <param name="preField">ID字段</param>
    /// <param name="tabName">表名</param>
    /// <param name="orderBy">排序字段，唯一；传空默认ID排序</param>
    /// <param name="sort">0:降序 默认 1：升序</param>
    /// <param name="strWhere">其他排序条件，以and开始</param>
    /// <param name="getType">0:加载更多 1:加载老数据</param>
    /// <param name="rows">加载条数</param>
    /// <returns></returns>
    public string PageSqlString(string preID, string preField, string tabName, string orderBy, string sort,
                                string strWhere, string getType, string rows)
    {
        int sortIndex = 0; //当前数据是第几行
        string strsql = string.Empty; //搜索sql

        #region 模拟分页
        sort = sort == "1" ? " asc" : " desc";
        if (preID == "-1")//第一次加载
        {
            strsql = "SELECT * FROM (select row_number() over (order by " + orderBy.Trim() + " " + sort +
                     ") as rowNum,* from " + tabName.Trim() + " where 1=1 " + strWhere.Trim() + ")x " +
                     "where rowNum between 1 and " + int.Parse(rows);
        }
        else
        {
            //找到当前criticalProductID在DataTabel中的第几行
            string sqlstr = "SELECT * FROM (select row_number() over (order by " + orderBy.Trim() + " " + sort +
                     ") as rowNum,* from " + tabName.Trim() + " where 1=1 " + strWhere.Trim() + ")x ";

            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr);

            if (ds != null)
            {
                foreach (
                    DataRow row in
                        ds.Tables[0].Rows.Cast<DataRow>().Where(row => row[preField.Trim()].ToString() == preID))
                {
                    sortIndex = Convert.ToInt32(row["rowNum"]);
                    break;
                }
                if (sortIndex == 0) return "";
                if (getType == "1") //加载更多:新数据
                {
                    strsql = "SELECT * FROM (select row_number() over (order by " + orderBy.Trim() + " " + sort +
                             ") as rowNum,* from " + tabName.Trim() + " where 1=1 " + strWhere.Trim() + ")x " +
                             "where rowNum between " + (sortIndex + 1) + " and " + (sortIndex + int.Parse(rows));
                }
                else //加载老数据
                {
                    strsql = "SELECT * FROM (select row_number() over (order by " + orderBy.Trim() + " " + sort +
                             ") as rowNum,* from " + tabName.Trim() + " where 1=1 " + strWhere.Trim() + ")x " +
                             "where rowNum between " + (sortIndex - int.Parse(rows)) + " and " + (sortIndex - 1);
                }
                return strsql;
            }
            if (sortIndex == 0)
            {
                return "";
            }
        }

        #endregion

        return strsql;
    }


    /// <summary>
    /// 加载数据，* 可以优化，不然大数据会卡呀
    /// </summary>
    /// <param name="preID">当前ID</param>
    /// <param name="preField">ID字段</param>
    /// <param name="tabName">表名</param>
    /// <param name="orderBy">排序字段，唯一；传空默认ID排序</param>
    /// <param name="sort">0:降序 默认 1：升序</param>
    /// <param name="strWhere">其他排序条件，以and开始</param>
    /// <param name="getType">0:加载更多 1:加载老数据</param>
    /// <param name="rows">加载条数</param>
    /// <returns></returns>
    public string PageSqlString_reseller(string preID, string preField, string tabName, string orderBy, string sort,
                                string strWhere, string getType, string rows)
    {
        int sortIndex = 0; //当前数据是第几行
        string strsql = string.Empty; //搜索sql

        #region 模拟分页
        sort = sort == "1" ? " asc" : " desc";
        if (preID == "-1")//第一次加载
        {
            strsql = "SELECT * FROM (select row_number() over (order by " + orderBy.Trim() + " " + sort +
                     ") as rowNum,dis.ID,dis.DisName,dis.Principal,dis.Phone,dis.City,dis.Province,dis.Area from " + tabName.Trim() + " where 1=1 " + strWhere.Trim() + ")x " +
                     "where rowNum between 1 and " + int.Parse(rows);
        }
        else
        {
            //找到当前criticalProductID在DataTabel中的第几行
            string sqlstr = "SELECT * FROM (select row_number() over (order by " + orderBy.Trim() + " " + sort +
                     ") as rowNum,dis.ID,dis.DisName,dis.Principal,dis.Phone,dis.City,dis.Province,dis.Area from " + tabName.Trim() + " where 1=1 " + strWhere.Trim() + ")x ";

            DataSet ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sqlstr);

            if (ds != null)
            {
                foreach (
                    DataRow row in
                        ds.Tables[0].Rows.Cast<DataRow>().Where(row => row[preField.Trim()].ToString() == preID))
                {
                    sortIndex = Convert.ToInt32(row["rowNum"]);
                    break;
                }
                if (sortIndex == 0) return "";
                if (getType == "1") //加载更多:新数据
                {
                    strsql = "SELECT * FROM (select row_number() over (order by " + orderBy.Trim() + " " + sort +
                             ") as rowNum,* from " + tabName.Trim() + " where 1=1 " + strWhere.Trim() + ")x " +
                             "where rowNum between " + (sortIndex + 1) + " and " + (sortIndex + int.Parse(rows));
                }
                else //加载老数据
                {
                    strsql = "SELECT * FROM (select row_number() over (order by " + orderBy.Trim() + " " + sort +
                             ") as rowNum,* from " + tabName.Trim() + " where 1=1 " + strWhere.Trim() + ")x " +
                             "where rowNum between " + (sortIndex - int.Parse(rows)) + " and " + (sortIndex - 1);
                }
                return strsql;
            }
            if (sortIndex == 0)
            {
                return "";
            }
        }

        #endregion

        return strsql;
    }

    #endregion

    #region 生成6位短信验证码

    /// <summary>
    /// 推送许可证
    /// </summary>
    /// <param name="Phone"></param>
    /// <param name="Msg"></param>
    public static void SendMsg(string Phone, string Msg)
    {
        GetPhoneCode getphonecode = new GetPhoneCode();
        getphonecode.GetUser(
            System.Configuration.ConfigurationManager.AppSettings["PhoneCodeAccount"].ToString(),
            System.Configuration.ConfigurationManager.AppSettings["PhoneCodePwd"].ToString());
        string rstr = getphonecode.ReturnST(Phone, Msg);
    }

    /// <summary>
    /// 生成验证码
    /// </summary>
    /// <param name="n"></param>
    /// <returns></returns>
    public string CreateRandomCode(int n)
    {
        string code = "";
        Random rand = new Random();
        for (int i = 0; i < n; i++)
        {
            if (i == 0)
            {
                code += rand.Next(1, 9).ToString();
            }
            else
            {
                code += rand.Next(0, 9).ToString();
            }
        }
        return code;
    }

    #endregion

    #region 异常日志

    /// <summary>
    /// 异常日志
    /// </summary>
    /// <param name="str"></param>
    /// <param name="name"></param>
    /// <param name="type"></param>
    public static void CatchInfo(string str, string name, string type = "0")
    {
        string strPath = HttpRuntime.AppDomainAppPath.ToString() + "APP\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
        LogHelper log = new LogHelper(strPath, "描述： " + str + "\r\n" + "接口： " + name + "\r\n" + "时间： " + DateTime.Now + "\r\n");
        log.Write();
    }

    #endregion

    #region 泛型获取实体集

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

        //属性取代*
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
        ds = Tran != null ? SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(), Tran) : SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString());

        if (ds == null || ds.Tables.Count <= 0 || ds.Tables[0] == null) return default(List<T>);
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
                        try
                        {
                            if (dr[property.Name] != DBNull.Value)
                                property.SetValue(tModel, dr[property.Name], null);
                        }
                        catch (Exception)
                        {
                            if (property.Name.Contains("ID"))
                                property.SetValue(tModel, Convert.ToInt32(dr[property.Name].ToString()), null);
                        }
                    }
                        
                }
                resultList.Add(tModel);
            }
            return resultList;
        }
        return default(List<T>);
    }

    #endregion

    #region  获取订单Value
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
    #endregion 


    #region  根据 app传递 UserID 获取 sys_compuser实体
    public static  Hi.Model.SYS_CompUser Get_CompUser(int UserId) 
    {
        Hi.Model.SYS_CompUser CompUser_Model = null;
        string str = "userID=" + UserId;
        List<Hi.Model.SYS_CompUser> list = new Hi.BLL.SYS_CompUser().GetList("", str , "");
        if (list.Count>0)
        {
            CompUser_Model=list[0];
        }

        return CompUser_Model;
    }
    /// <summary>
    /// 获取订单设置
    /// </summary>
    /// <param name="Name">设置名称</param>
    /// <param name="CompID">核心企业ID</param>
    /// <returns>0、不审核,1、审核</returns>
    public static string rdoOrderAudit(string Name, int CompID)
    {
        List<Hi.Model.SYS_SysName> sl = new Hi.BLL.SYS_SysName().GetList("", " CompID=" + CompID + " and Name='" + Name + "' and dr=0", "");
        if (sl != null && sl.Count > 0)
        {
            return sl[0].Value;
        }
        return "0";
    }

    //写参数后面删除
    public  static void WriteLog(string fileName, string msg)
    {
        FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write);
        StreamWriter wtr = new StreamWriter(fs);
        wtr.WriteLine("-----------------------------------------");
        wtr.WriteLine(Convert.ToString(DateTime.Now));
        wtr.WriteLine(msg);
        wtr.Flush();
        wtr.Close();
        fs.Close();
    }
    #endregion

    /// <summary>
    /// 根据订单ID获取订单扩展表实体
    /// </summary>
    /// <param name="receiptNo"></param>
    /// <returns></returns>
    public Hi.Model.DIS_OrderExt GetOrderExtByOrderID(string orderid)
    {
        List<Hi.Model.DIS_OrderExt> orderextlist = new Hi.BLL.DIS_OrderExt().GetList("",
            " OrderID ='" + orderid.Trim() + "'", "");
        if (orderextlist == null || orderextlist.Count == 0)
            return null;
        Hi.Model.DIS_OrderExt orderext = new Hi.Model.DIS_OrderExt();
        foreach (var disOrderext in orderextlist)
        {
            orderext = disOrderext;
        }
        return orderext;
    }
    //传入事务的查询发货单明细
    public List<Hi.Model.DIS_OrderOutDetail> GetOrderOutDetailList(string strWhat, string strWhere, string strOrderby,SqlTransaction mytrans)
    {
         
        return GetList(strWhat, strWhere, strOrderby,mytrans) as List<Hi.Model.DIS_OrderOutDetail>;
    }

    /// <summary>
    /// 获取泛型数据列表,在单表查询时使用
    /// </summary>
    public IList<Hi.Model.DIS_OrderOutDetail> GetList(string strWhat, string strWhere, string strOrderby,SqlTransaction mytrans)
    {
        return GetList(GetDataSet(strWhat, strWhere, strOrderby, mytrans));
        //return new Hi.SQLServerDAL.DIS_OrderOutDetail().GetList(GetDataSet(strWhat, strWhere, strOrderby, mytrans));
    }

    /// <summary>
    /// 获取数据集,在单表查询时使用
    /// </summary>
    public DataSet GetDataSet(string strWhat, string strWhere, string strOrderby,SqlTransaction mytrans)
    {
        if (string.IsNullOrEmpty(strWhat))
            strWhat = "*";
        StringBuilder strSql = new StringBuilder("select " + strWhat + " from [DIS_OrderOutDetail]");
        if (!string.IsNullOrEmpty(strWhere))
            strSql.Append(" where " + strWhere);
        if (!string.IsNullOrEmpty(strOrderby))
            strSql.Append(" order by " + strOrderby);
        return SqlHelper.Query(SqlHelper.LocalSqlServer, strSql.ToString(),mytrans);
    }

    /// <summary>
    /// 由数据集得到泛型数据列表
    /// </summary>
    private IList<Hi.Model.DIS_OrderOutDetail> GetList(DataSet ds)
    {
        return GetListEntity<Hi.Model.DIS_OrderOutDetail>(ds.Tables[0]);
    }

    /// <summary>
    /// datatable转实体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="table"></param>
    /// <returns></returns>
    public static List<T> GetListEntity<T>(DataTable table)
    {
        List<T> resultList = new List<T>();
        int value = 0;
        if (table != null && table.Rows.Count > 0)
        {
            foreach (DataRow dr in table.Rows)
            {
                T tModel = Activator.CreateInstance<T>();
                foreach (PropertyInfo property in typeof(T).GetProperties())
                {
                    if (table.Columns.Contains(property.Name))
                    {
                        if (DBNull.Value != dr[property.Name])
                        {
                            switch (dr[property.Name].GetType().Name)
                            {
                                case "Int64":
                                    int.TryParse(dr[property.Name].ToString(), out value);
                                    property.SetValue(tModel, value, null);
                                    ; break;
                                default:
                                    property.SetValue(tModel, dr[property.Name], null);
                                    ; break;
                            }
                        }
                    }
                }
                resultList.Add(tModel);
            }
            return resultList;
        }
        return resultList;
    }

    /// <summary>
    /// datatable转实体
    /// </summary>
    //public bool IsableUpdate(string TableName, string ts,int ID)
    //{
    //    string strsql = "select ts from " + TableName + " whe";
    //}
    /// <summary>
    ///算出附件的大小
    /// </summary>
 public static string getsize(string url)
{
    float size;
    string return_size= string.Empty;
    if (File.Exists(url))
    {
        FileInfo fileinfo = new FileInfo(url);
        size = fileinfo.Length;
        return_size = size.ToString("F2");
        //if (size < 1024)
        //{
        //    return_size = size.ToString("F2") + "B";
        //}
        //if (size < 1048576)
        //{
        //    return_size = (size / 1024).ToString("F2") + "KB";
        //}
        //else if (size < 1073741824)
        //{
        //    return_size = (size / 1024 / 1024).ToString("F2") + "M";

        //}
        //else
        //{
        //    return_size = (size / 1024 / 1024 / 1024).ToString("F2") + "G";
        //}
    }
    return return_size;

}

 public static List<class_ver3.Att> Getattalist(string attname_all,int orderID)
 {
     string[] attname = null;
     class_ver3.Att atta = null;
     string attname_b = string.Empty;
     int last_doub = 0;
     int last = 0;
     string size = string.Empty;
     List<class_ver3.Att> attlist = new List<class_ver3.Att>();
     if (ClsSystem.gnvl(attname_all, "") == "")
         return null;
     attname= Regex.Split(attname_all, "@@", RegexOptions.IgnoreCase);
     foreach (string attname_c in attname)
     {
         if (attname_c == "")
             continue;
         atta = new class_ver3.Att();
         //取出此文件名最后一次出现^^的索引
         last = attname_c.LastIndexOf("^^");
         //截除原本的文件名
         attname_b = attname_c.Substring(0, last);
         //取出文件名最后一次出现.的索引
         last_doub = attname_c.LastIndexOf(".");
         //原文件的全名就是原文件名加后缀名
         attname_b += attname_c.Substring(last_doub, attname_c.Length - last_doub);
         atta.AttName = attname_b;
         atta.AttUrl = ConfigurationManager.AppSettings["ImgViewPath"].ToString().Trim() + "OrderFJ/" + attname_c;
         atta.OrderId = orderID.ToString();
         //取出文件大小
         size = Common.getsize(ConfigurationManager.AppSettings["ImgPath"].ToString().Trim() + "OrderFJ/" + attname_c);
         atta.AttSize = size;
         attlist.Add(atta);
   
     }
     return attlist;
 }
 public delegate List<class_ver3.Att> Getattdel(string attname_all, int orderID);

 /// <summary>
 /// 查看是否启用微信支付或者支付宝支付
 /// </summary>
 /// <param name="CompID">核心企业ID</param>
 /// <returns></returns>
 public static Hi.Model.Pay_PayWxandAli GetPayWxandAli(int CompID)
 {
     //LoginModel logUser = HttpContext.Current.Session["UserModel"] as LoginModel;
     //CompID = logUser.CompID;
     Hi.Model.Pay_PayWxandAli model = new Hi.Model.Pay_PayWxandAli();
     //查询该企业的设置
     List<Hi.Model.Pay_PayWxandAli> Sysl = new Hi.BLL.Pay_PayWxandAli().GetList("", " CompID=" + CompID, "");
     if (Sysl != null && Sysl.Count > 0)
         model = Sysl[0];

     return model;

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
     catch
     {
         //return error.Message;
         return "";
     }
 }

 /// <summary>
 /// 创建订单信息
 /// </summary>
 public String getOrderInfo(String subject, String body, String price, String OrderNumber, String PARTNER, String SELLER)
 {
     // 签约合作者身份ID
     String orderInfo = "partner=" + "\"" + PARTNER + "\"";

     // 签约卖家支付宝账号
     orderInfo += "&seller_id=" + "\"" + SELLER + "\"";

     // 商户网站唯一订单号
     orderInfo += "&out_trade_no=" + "\"" + OrderNumber + "\"";
     //orderInfo += "&out_trade_no=" + "\"" + getOutTradeNo() + "\"";
     // orderInfo += "&out_trade_no=" + "\"" +
     // userOrderDetail.Result.PayTradeNo + "\"";

     // 商品名称
     orderInfo += "&subject=" + "\"" + subject + "\"";

     // 商品详情
     if (body != "")
     {
         orderInfo += "&body=" + "\"" + body + "\"";
     }

     // 商品金额
     orderInfo += "&total_fee=" + "\"" + price + "\"";

     // 服务器异步通知页面路径
     //        orderInfo += "&notify_url=" + "\""
     //                + "http://cus.ganxike.com/pay.notify/gxkmobile/"
     //                + userOrderDetail.Result.OrderId + "/alipay/"
     //                + userOrderDetail.Result.OrderFee + "/" + getOutTradeNo()
     //                + "\"";
     orderInfo += "&notify_url=" + "\"" +
             ConfigurationManager.AppSettings["ALIPAY_NOTIFY_URL"].ToString()
             + "\"";

     // 服务接口名称， 固定值
     orderInfo += "&service=\"mobile.securitypay.pay\"";

     // 支付类型， 固定值
     orderInfo += "&payment_type=\"1\"";

     // 参数编码， 固定值
     orderInfo += "&_input_charset=\"utf-8\"";

     // 设置未付款交易的超时时间
     // 默认30分钟，一旦超时，该笔交易就会自动被关闭。
     // 取值范围：1m～15d。
     // m-分钟，h-小时，d-天，1c-当天（无论交易何时创建，都在0点关闭）。
     // 该参数数值不接受小数点，如1.5h，可转换为90m。
     orderInfo += "&it_b_pay=\"30m\"";

     // extern_token为经过快登授权获取到的alipay_open_id,带上此参数用户将使用授权的账户进行支付
     // orderInfo += "&extern_token=" + "\"" + extern_token + "\"";

     // 支付宝处理完请求后，当前页面跳转到商户指定页面的路径，可空
     orderInfo += "&return_url=\"m.alipay.com\"";

     // 调用银行卡支付，需配置此参数，参与签名， 固定值 （需要签约《无线银行卡快捷支付》才能使用）
     // orderInfo += "&paymethod=\"expressGateway\"";

     return orderInfo;
 }

 /// <summary>
 /// 获取签名方式
 /// </summary>
 public String getSignType()
 {
     return "sign_type=\"RSA\"";
 }


 /// <summary>
 /// 获取签名方式
 /// </summary>
 //public String GetNotifyUrl(String subject, String body, String price, String OrderNumber, String PARTNER, String SELLER)
 //{
 //    StringBuilder notifyurl = new StringBuilder();
 //    notifyurl.Append(ConfigurationManager.AppSettings["ALIPAY_NOTIFY_URL"].ToString());
 //    notifyurl.Append("?");
 //    notifyurl.Append("total_amount="+price);
 //    notifyurl.Append("&buyer_id="+PARTNER);
 //    notifyurl.Append("&body=" + body);
 //    notifyurl.Append("&trade_no="+OrderNumber);
 //    notifyurl.Append("&refund_fee=0.00");
 //    notifyurl.Append("&notify_time="+DateTime.Now);
 //    notifyurl.Append("&subject="+subject);
 //    notifyurl.Append("&sign_type=RSA");
 //    notifyurl.Append("&charset=utf-8");
 //    notifyurl.Append("&notify_type=trade_status_sync");
 //    notifyurl.Append("&out_trade_no"+OrderNumber);
 //}

 /// <summary>
 /// 新增积分记录
 /// </summary>
 /// <param name="CompId">核心企业ID</param>
 /// <param name="DisId">经销商ID</param>
 /// <param name="IntegralType">积分类型</param>
 /// <param name="type">加减积分  1、加积分  2、减积分 </param>
 /// <param name="OrderId">订单ID</param>
 /// <param name="Integral">积分数量</param>
 /// <param name="Source">积分来源</param>
 /// <param name="Remarks">备注</param>
 /// <param name="modifyuser">更新人ID</param>
 /// <returns>返回 0、新增失败   </returns>
 public static int AddIntegral(int CompId, int DisId, string IntegralType, int type, int OrderId, decimal Integral, string Source, string Remarks, int modifyuser)
 {
     Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(DisId);

     //判断经销商是否存在
     if (disModel != null)
     {
         decimal DisIntegral = disModel.Integral;  //经销商已有积分
         decimal OldIntegral = DisIntegral;  //旧积分
         decimal NewIntegral = 0;    //新积分
         if (type == 1)
         {
             //加积分
             NewIntegral = DisIntegral + Integral;
         }
         else
         {
             //减积分
             if (DisIntegral < Integral)
             {
                 //已有积分小于新增积分数  积分为0
                 NewIntegral = 0;
             }
             else
             {
                 NewIntegral = DisIntegral - Integral;
             }
         }

         string sql = string.Format("update BD_Distributor set Integral=" + NewIntegral + " where ID=" + DisId + ";");
         sql += string.Format("INSERT INTO DIS_Integral([CompID],[DisID],[OrderID],[IntegralType],[OldIntegral],[Integral],[NewIntegral],[Source],[Remarks],[CreateDate],[type],[IsView],[ts],[dr],[modifyuser]) VALUES({0},{1},{2},'{3}',{4},{5},{6} ,'{7}','{8}','{9}',{10},0,'{11}',0,{12})", CompId, DisId, OrderId, IntegralType, OldIntegral, Integral, NewIntegral, Source, Remarks, DateTime.Now, type, DateTime.Now, modifyuser);


         SqlTransaction TranSaction = null;
         SqlConnection Connection = new SqlConnection(SqlHelper.LocalSqlServer);
         Connection.Open();
         TranSaction = Connection.BeginTransaction();
         try
         {
             SqlCommand cmd = new SqlCommand(sql, Connection, TranSaction);
             cmd.CommandType = CommandType.Text;

             int rowsAffected = SqlHelper.GetInt(cmd.ExecuteNonQuery().ToString());
             if (rowsAffected > 0)
             {
                 TranSaction.Commit();
                 return rowsAffected;
             }
             else
             {
                 TranSaction.Rollback();
                 return 0;
             }
         }
         catch (Exception ex)
         {
             TranSaction.Rollback();
             return 0;
         }
         finally
         {
             Connection.Dispose();
         }
     }

     return 0;
 }


 /// <summary>
 /// 新增业务日志
 /// </summary>
 /// <param name="LogClass">单据名称</param>
 /// <param name="ApplicationId">业务ID</param>
 /// <param name="LogType">业务名称 新增、审核、退回、提交...</param>
 /// <param name="LogRemark">备注</param>
 /// <param name="UserID">操作人Id</param>
 public static void AddSysBusinessLog(int CompId, string LogClass, string ApplicationId, string LogType, string LogRemark, string UserID)
 {
     try
     {

         Hi.Model.SYS_Users userModel = new Hi.BLL.SYS_Users().GetModel(Convert.ToInt32(UserID));
         if (userModel != null)
         {

             Hi.Model.SYS_SysBusinessLog SysBusinessLogModel = new Hi.Model.SYS_SysBusinessLog();
             SysBusinessLogModel.CompID = CompId;
             SysBusinessLogModel.LogClass = LogClass;
             SysBusinessLogModel.ApplicationId = Convert.ToInt32(ApplicationId);
             SysBusinessLogModel.LogType = LogType;
             SysBusinessLogModel.OperatePersonId = Convert.ToInt32(UserID);
             SysBusinessLogModel.OperatePerson = userModel.TrueName == "" ? userModel.UserName : userModel.TrueName;
             SysBusinessLogModel.LogRemark = LogRemark;
             SysBusinessLogModel.LogTime = DateTime.Now;
             SysBusinessLogModel.ts = DateTime.Now;

             new Hi.BLL.SYS_SysBusinessLog().Add(SysBusinessLogModel);
         }
     }
     catch
     {
     }
 }


    /// <summary>
    /// 两个结构相同的dt，链接成一个dt
    /// </summary>
 public DataTable getNewDatatable(DataTable dt1, DataTable dt2)
 {
     DataTable newdt = dt1.Clone();
     try
     {
         Object[] obj = new Object[newdt.Columns.Count];
         for (int i = 0; i < dt1.Rows.Count; i++)
         {
             dt1.Rows[i].ItemArray.CopyTo(obj, 0);
             newdt.Rows.Add(obj);
         }
         for (int i = 0; i < dt2.Rows.Count; i++)
         {
             dt2.Rows[i].ItemArray.CopyTo(obj,0);
             newdt.Rows.Add(obj);
         }
     }
     catch
     {
     }
     return newdt;
 }


 /// <summary>
 /// 修改订单
 /// </summary>
 /// <param name="OrderInfoModel"></param>
 /// <param name="OrderDetailList"></param>
 /// <returns></returns>
 public static int UpdateOrder(DateTime dts, Hi.Model.DIS_Order OrderInfoModel, Hi.Model.DIS_OrderExt OrderExt, List<Hi.Model.DIS_OrderDetail> OrderDetailList, string delOrderD)
 {

     //判断订单时间
     Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
     Hi.BLL.DIS_OrderExt OrderExtBll = new Hi.BLL.DIS_OrderExt();
     Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();
     Hi.BLL.BD_Rebate bate = new Hi.BLL.BD_Rebate();
     if (new Hi.BLL.DIS_Order().Getts("Dis_Order", OrderInfoModel.ID, dts) == 0)
         return -1;

     int OrderId = 0;
     //返回修改库存的sql
     System.Text.StringBuilder sqlInven = new System.Text.StringBuilder();
     int IsInve = Common.rdoOrderAudit("商品是否启用库存", OrderInfoModel.CompID).ToInt(0);

     if (IsInve == 0)
     {
         //修改商品库存，先返还订单明细删除的商品库存
         sqlInven.AppendFormat(new Hi.BLL.DIS_Order().GetSqlInventory(delOrderD, OrderDetailList));
     }

     //SqlConnection con = new SqlConnection(LocalSqlServer);
     //con.Open();
     //System.Data.IsolationLevel.RepeatableRead
     SqlTransaction sqlTrans = DBUtility.SqlHelper.CreateStoreTranSaction();
     //可以做循环   

     try
     {
         //private object thislock = OrderInfoModel.ID as object;
         //lock (thislock)
         //{
         OrderId = OrderBll.UpdateOrder(sqlTrans.Connection, OrderInfoModel, sqlTrans);
         if (OrderDetailList.Count <= 0)
         {
             OrderId = 0;
             sqlTrans.Rollback();
         }
         else
         {
             if (OrderExt != null)
             {
                 //修改订单扩展表
                 if (!OrderExtBll.Update(sqlTrans.Connection, OrderExt, sqlTrans))
                 {
                     OrderId = 0;
                     sqlTrans.Rollback();
                 }
             }

             if (!delOrderD.Equals(""))
             {
                 //修改时 删除商品后 清除在数据库中存在的该商品
                 if (OrderDetailBll.GetDel(delOrderD, sqlTrans.Connection, sqlTrans) < 0)
                 {
                     OrderId = 0;
                     sqlTrans.Rollback();
                 }
             }

             foreach (Hi.Model.DIS_OrderDetail item in OrderDetailList)
             {
                 Hi.Model.DIS_OrderDetail OrderDeModel = OrderDetailBll.GetModel(item.ID);
                 int count = 0;
                 if (OrderDeModel != null)
                 {
                     if (IsInve == 0)
                         //修改订单明细时，先返还商品库存
                         sqlInven.AppendFormat("update BD_GoodsInfo set Inventory+={0} where ID={1};", OrderDeModel.GoodsNum + Convert.ToDecimal(OrderDeModel.ProNum), OrderDeModel.GoodsinfoID);

                     item.OrderID = OrderInfoModel.ID;
                     //修改订单时，订单明细里存在该商品 修改商品信息
                     count = OrderDetailBll.UpdateOrderDetail(sqlTrans.Connection, item, sqlTrans);
                     if (count == 0)
                     {
                         OrderId = 0;
                         sqlTrans.Rollback();
                     }
                 }
                 else
                 {
                     //修改订单时，订单明细里不存在该商品新增商品信息
                     item.OrderID = OrderInfoModel.ID;
                     count = OrderDetailBll.AddOrderDetail(sqlTrans.Connection, item, sqlTrans);
                     if (count == 0)
                     {
                         OrderId = 0;
                         sqlTrans.Rollback();
                     }
                 }
             }
             if (IsInve == 0)
             {
                 //修改商品库存，先返还订单明细删除的商品库存
                 //sqlInven.AppendFormat(new Hi.BLL.DIS_Order().GetSqlInventory(delOrderD,  OrderDetailList));
                 if (new Hi.BLL.DIS_OrderDetail().GetUpdateInventory(sqlInven.ToString(), sqlTrans.Connection, sqlTrans) <= 0)
                 {
                     OrderId = 0;
                     sqlTrans.Rollback();
                 }
             }

             if (Common.rdoOrderAudit("订单支付返利是否启用", OrderInfoModel.CompID) == "1")
             {
                 //订单支付返利启用
                 if (bate.TransEditRebate(OrderInfoModel.DisID, OrderInfoModel.bateAmount, OrderInfoModel.ID, OrderInfoModel.CreateUserID, sqlTrans))
                 {
                     sqlTrans.Commit();
                     return OrderId;
                 }
             }
             sqlTrans.Commit();
         }
         //}
     }
     catch
     {
         OrderId = 0;
         sqlTrans.Rollback();
     }
     finally
     {
     }

     return OrderId;
 }

 /// <summary>
 /// 修改发货单
 /// </summary>
 /// <param name="omodel">订单</param>
 /// <param name="ll">订单明细</param>
 /// <param name="llo">发货单明细</param>
 /// <returns></returns>
 public int GetOutUpOrder(Hi.Model.DIS_Order omodel,Hi.Model.DIS_OrderOut orderoutModel, List<Hi.Model.DIS_OrderDetail> ll, List<Hi.Model.DIS_OrderOutDetail> llo)
 {
     SqlTransaction sqlTrans = DBUtility.SqlHelper.CreateStoreTranSaction();

     int outid = 0;
     try
     {
         //修改订单主表状态
         if (new Hi.BLL.DIS_Order().UpdateOrder(sqlTrans.Connection, omodel, sqlTrans) < 0)
         {
             outid = 0;
             return outid;
         }
         if (!new Hi.BLL.DIS_OrderOut().Update(orderoutModel, sqlTrans))
         {
             outid = 0;
             return outid;
         }
         //修改订单明细状态
         foreach (Hi.Model.DIS_OrderDetail item in ll)
         {
             if (new Hi.BLL.DIS_OrderDetail().UpdateOrderDetail(sqlTrans.Connection, item, sqlTrans) < 0)
             {
                 outid = 0;
                 return outid;
             }
         }
         //修改发货单明细
         foreach (Hi.Model.DIS_OrderOutDetail item in llo)
         {
             if (new Hi.BLL.DIS_OrderOutDetail().Update(item, sqlTrans) < 0)
             {
                 outid = 0;
                 return outid;
             }
         }
         sqlTrans.Commit();
         outid = 1;
     }
     catch (Exception)
     {
         if (sqlTrans.Connection != null)
             sqlTrans.Rollback();
         throw;
     }

     return outid;
 }



    /// <summary>
    /// 根据商品skuid取销量
    /// </summary>
    /// <param name="SKUID">商品skuid</param>
 public static  string GetSaleNum(int skuid)
 {
     try
     {
         string strsql = "select isnull( sum(isnull(b.GoodsNum ,0)), 0 ) from BD_GoodsInfo a left join DIS_OrderDetail b ";
         strsql += "on a.ID = b.GoodsinfoID where a.id = "+skuid+" group by a.id";
         string salenum = ClsSystem.gnvl(SqlAccess.ExecuteScalar(strsql, SqlHelper.LocalSqlServer), "");
         return salenum;
 
     }
     catch
     {
         return "";
     }
 }

 /// <summary>
 /// 获取返利总金额
 /// </summary>
 /// <param name="OrderID">订单ID</param>
 /// <param name="DisID">经销商ID</param>
 /// <returns></returns>
 public static string GetRebate(int OrderID, int DisID)
 {
     if (OrderID != 0)
     {
         return new Hi.BLL.BD_Rebate().GetEditEnableAmount(OrderID, DisID).ToString();
     }
     else
     {
         return (new Hi.BLL.BD_Rebate().GetRebateEnableAmount(DisID)).ToString("N");
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
         List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList("id", " " + name + "='" + value + "' and CompID='" + CompID + "'  and id<>'" + id + "' and isnull(dr,0)=0 ", "");
         if (Dis.Count > 0)
         {
             exists = true;
         }
     }
     else
     {

         List<Hi.Model.BD_Distributor> Dis = new Hi.BLL.BD_Distributor().GetList("id", " " + name + "='" + value + "' and CompID='" + CompID + "' and isnull(dr,0)=0 ", "");
         if (Dis.Count > 0)
         {
             exists = true;
         }
     }
     return exists;
 }

 public static bool GetUserExists(string Uname)
 {
     List<Hi.Model.SYS_Users> user = new Hi.BLL.SYS_Users().GetList("", " username='" + Uname + "' and isnull(dr,0)=0", "");
     return user.Count > 0;
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

 /// <summary>
 /// 修改附件
 /// </summary>
 /// <param name="orderid"></param>
 /// <param name="filepath"></param>
 /// <returns></returns>
 public bool Update(string orderid, string filepath, string type, Hi.Model.SYS_Users logUser)
 {
     bool bol = false;
     try
     {
         Hi.Model.DIS_Order model = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(orderid));
         if (model != null)
         {
             string fj = model.Atta;//附件
             string[] fjlist = fj.Split(new string[] { "@@" }, StringSplitOptions.RemoveEmptyEntries);
             string strlist = string.Empty;
             if (type == "del")
             {
                 for (int i = 0; i < fjlist.Length; i++)
                 {
                     if (filepath != fjlist[i])
                     {
                         strlist += fjlist[i] + "@@";
                     }
                 }
             }
             else
             {
                 strlist = fj + filepath + "@@";
             }
             model.modifyuser = logUser.ID;
             model.ts = DateTime.Now;
             model.Atta = strlist;
             bol = new Hi.BLL.DIS_Order().Update(model);
         }
     }
     catch (Exception)
     {
         bol = false;
     }
     return bol;
 }

    /// <summary>
    /// 递归获得经销商的末级分类
    /// </summary>
    /// <param name="parentid">父分类id</param>
    /// <param name="list_distype"></param>
    /// <returns></returns>
 public static string GetClassifyID(string parentid, List<Hi.Model.BD_DisType> list_distype)
 {
     StringBuilder a = new StringBuilder();
     List<Hi.Model.BD_DisType> list = list_distype.FindAll(p => p.ParentId == Int32.Parse(parentid));

     for (int i = 0; i < list.Count; i++)
     {
         //a.Append(",");
         a.Append(GetClassifyID(list[i].ID.ToString(), list_distype));
     }
     a.Append(",");
     a.Append(parentid);
     return a.ToString();
 }
 public  string sAttr = "";
 public string sAttr1 = "";

 public string GoodsType(string GoodsID, string CompID)
 {
     //string sAttr = "";
     //string sAttr1 = "";
     System.Text.StringBuilder goodsAttr = new System.Text.StringBuilder();
     string attr = string.Empty;
     DataTable dt = new Hi.BLL.BD_GoodsAttrs().GetAttrToAttrInfoDt("gattr.ID,gattr.CompID,gattr.GoodsID,gattr.AttrsName,ginfo.ID,ginfo.AttrsInfoName", " gattr.GoodsId=" + GoodsID + "  and gattr.CompID=" + CompID + "and IsNull(ginfo.ID,0)!=0", "");
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
 /// <summary>
 /// 获取两个时间相差秒数的绝对值
 /// </summary>
 /// <param name="ts_begin">开始时间</param>
 /// <param name="ts_end">结束时间</param>
 public static double ExecDateDiff(string ts_begin, string ts_end)
 {
     //取出两个时间的差值的绝对值
     TimeSpan begin = new TimeSpan(DateTime.Parse(ts_begin).Ticks);
     TimeSpan end = new TimeSpan(DateTime.Parse(ts_end).Ticks);
     TimeSpan ts_diff = end.Subtract(begin).Duration();
     return ts_diff.TotalSeconds;

 }



 /// <summary>
 /// 删除脚本的非法js
 /// </summary>
 /// <param name="Htmlstring">需要过滤的字符</param>
 /// <returns></returns>
 public static string NoHTML(string Htmlstring)
 {
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
     Htmlstring = Regex.Replace(Htmlstring, "(?i)(exec|insert|delete|drop|truncate|update|declare|frame|or|style|expression|and|select|create|script|img|body|meta|object|alert|href|1=1|<(.[^>]*)>.*?<(.[^>]*)>|<(.[^>]*)>|&(lt|#60)|&(gt|#62))", "");
     Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();
     return Htmlstring;

 }
    /// <summary>
    /// 获取图片路径
    /// </summary>
    /// <param name="fileName"></param>
    /// <param name="resizeFormat"></param>
    /// <param name="compId"></param>
    /// <returns></returns>
    public static string GetPicURL(string fileName, string resizeFormat = null, int compId = 0)
    {
        string basePath = ConfigurationManager.AppSettings["OssImgPath"] + "company/" + Convert.ToString(compId) + "/";

        if (!string.IsNullOrEmpty(fileName) && fileName.Trim() != "X")
        {
            return basePath + fileName + (!string.IsNullOrEmpty(resizeFormat) ? "?x-oss-process=style/" + resizeFormat : "");
        }
        return ConfigurationManager.AppSettings["OssImgPath"] + "havenopicmax.gif";
    }

    public static string GetAnnexDescription(int annexType)
    {
        switch(annexType)
        {
            case 5:
                return "营业执照";
            case 7:
                return "医疗器械经营许可证";
            case 8:
                return "医疗器械备案号";
            case 9:
                return "开户许可证";
        }
        return string.Empty;
    }
}