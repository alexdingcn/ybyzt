using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Xml;
using Com.Alipay;
using LitJson;

/// <summary>
///WebServiceUpdate 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class WebServiceUpdate : System.Web.Services.WebService {

    public WebServiceUpdate () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod]
    public string HelloWorld() {
        return "Hello World";
    }
    #region 登录

    #region 新版

    //[WebMethod(Description = "普通方式登录")]
    //public string Login(string JSon)
    //{
    //    SYS_Users bll = new SYS_Users();
    //    SYS_Users.ResultLogin result = bll.Login(AESHelper.Decrypt_android(JSon));
    //    JavaScriptSerializer js = new JavaScriptSerializer();
    //    return AESHelper.Encrypt_android(js.Serialize(result));
    //}

    [WebMethod(Description = "手机登录")]
    public string LoginByPhone(string JSon)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultLogin result = bll.LoginByPhone(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 旧版

    [WebMethod(Description = "经销商登录")]
    public string ResellerLogin(string JSon)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultDisLog result = bll.ResellerLogin(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业登录")]
    public string CompanyLogin(string JSon)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultCompLog result = bll.CompanyLogin(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #endregion

    #region  企业信息

    [WebMethod(Description = "企业信息列表")]
    public string GetResellerCompany(string JSon)
    {
        BD_Company bll = new BD_Company();
        BD_Company.ResultList result = bll.GetResellerCompany(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 商品

    [WebMethod(Description = "企业商品分类列表")]
    public string GetResellerProductClassifyList(string JSon)
    {
        BD_GoodsCategory_Update bll = new BD_GoodsCategory_Update();
        BD_GoodsCategory_Update.ResultGoodsCategory result = bll.GetResellerProductClassifyList(AESHelper.Decrypt_android(JSon));
        //BD_GoodsCategory_Update.ResultGoodsCategory result = bll.GetResellerProductClassifyList(JSon);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
        //return js.Serialize(result);
    }

    [WebMethod(Description = "获取某一分类产品信息")]
    public string GetProductList(string JSon,string from)
    {
        BD_Goods bll = new BD_Goods();
        BD_GoodsCategory.ResultProductList result = bll.GetProductsList(AESHelper.Decrypt_android(JSon),from);
       // BD_GoodsCategory.ResultProductList result = bll.GetProductsList(JSon);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
        //return js.Serialize(result);
    }

    [WebMethod(Description = "搜索商品列表")]
    public string SearchProductList(string JSon,string from)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultProductList result = bll.SearchProductList(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "搜索商品")]
    public string SearchGoodst(string JSon)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultGoodsList result = bll.SearchGoods(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "搜索商品")]
    public string SearchGoodsList(string JSon,string from)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultProductList result = bll.SearchGoodsList(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "商品收藏与取消")]
    public string SetCollect(string JSon)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultCollect result = bll.SetCollect(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "收藏商品列表")]
    public string GetCollectList(string JSon)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultProductList result = bll.GetCollectList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 订单

    [WebMethod(Description = "经销商订单列表")]
    public string GetResellerOrderList(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultOrderList result = bll.GetResellerOrderList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "经销商订单详情")]
    public string GetResellerOrderDetail(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultOrderInfo result = bll.GetResellerOrderDetail(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业订单列表")]
    public string GetCompanyOrderList(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultOrderList result = bll.GetCompanyOrderList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业订单详情")]
    public string GetCompanyOrderDetail(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultOrderInfo result = bll.GetCompanyOrderDetail(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业订单审核")]
    public string SubCompanyApprove(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultAudit result = bll.SubCompanyApprove(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业订单发货")]
    public string SubProduct(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultAudit result = bll.SubProduct(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业订单签收")]
    public string ConfirmReceipt(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultOutConfirm result = bll.ConfirmReceipt(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "订单提交:支持批量操，支持非同一操作")]
    public string SubResellerOrder(string JSon,string from)
    {
        DIS_OrderSub bll = new DIS_OrderSub();
        DIS_OrderSub.ResultSubOrder result = bll.SubResellerOrder(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "退货申请")]
    public string SubOrderReturn(string JSon)
    {
        DIS_OrderSub bll = new DIS_OrderSub();
        DIS_OrderSub.ResultAudit result = bll.SubOrderReturn(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "退货审核")]
    public string AuditOrderReturn(string JSon)
    {
        DIS_OrderSub bll = new DIS_OrderSub();
        DIS_OrderSub.ResultAudit result = bll.AuditOrderReturn(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "商品检测")]
    public string GetOrderCheckList(string JSon)
    {
        DIS_OrderSub bll = new DIS_OrderSub();
        DIS_OrderSub.ResultCheck result = bll.GetOrderCheckList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 促销

    [WebMethod(Description = "订单促销")]
    public string GetOrderProList(string JSon)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultOrderProList result = bll.GetOrderPro(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 统计数据

    [WebMethod(Description = "经销商统计数据")]
    public string GetDisNum(string JSon)
    {
        userIndex bll = new userIndex();
        userIndex.ResultJSC result = bll.GetDisNum(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业统计数据")]
    public string GetCompNum(string JSon)
    {
        jsc bll = new jsc();
        jsc.ResultJSC result = bll.GetCompNum(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 新闻公告

    [WebMethod(Description = "未读新闻公告个数")]
    public string GetAnnouncement(string JSon)
    {
        BD_CompNews bll = new BD_CompNews();
        BD_CompNews.ResultNoRead result = bll.GetAnnouncement(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "新闻公告列表")]
    public string CompNewsList(string JSon)
    {
        BD_CompNews bll = new BD_CompNews();
        BD_CompNews.ResultNewsList result = bll.CompNewsList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "新闻公告详情")]
    public string GetNewsInfo(string JSon)
    {
        BD_CompNews bll = new BD_CompNews();
        BD_CompNews.ResultNewsInfo result = bll.GetNewsInfo(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "新闻添加")]
    public string CompNewsAdd(string JSon)
    {
        BD_CompNews bll = new BD_CompNews();
        BD_CompNews.ResultNewsAdd result = bll.CompNewsAdd(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "过期列表")]
    public string GetMessageDeleteId(string JSon)
    {
        BD_CompNews bll = new BD_CompNews();
        BD_CompNews.ResultNoTime result = bll.GetMessageDeleteId(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 用户

    [WebMethod(Description = "经销商获取个人信息")]
    public string GetResellerInfo(string JSon)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultDisInfo result = bll.GetResellerInfo(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 收货地址

    [WebMethod(Description = "获取经销商收货地址列表")]
    public string GetResellerShippingAddressList(string JSon,string from)
    {
        BD_DisAddr bll = new BD_DisAddr();
        BD_DisAddr.ResultAddr result = bll.GetResellerShippingAddressList(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "经销商收货地址新增")]
    public string ResellerAddrAdd(string JSon)
    {
        BD_DisAddr bll = new BD_DisAddr();
        BD_DisAddr.ResultAddrAdd result = bll.ResellerAddrAdd(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 物流公司

    [WebMethod(Description = "获取核心企业物流绑定列表")]
    public string GetLogisticsList(string JSon)
    {
        BD_ComLogistics bll = new BD_ComLogistics();
        BD_ComLogistics.ResultLogistics result = bll.GetLogisticsList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 短信验证

    [WebMethod(Description = "获取短信验证码")]
    public string GetPhoneCode(string JSon)
    {
        SYS_PhoneCode bll = new SYS_PhoneCode();
        SYS_PhoneCode.PhoneCode result = bll.GetPhoneCode(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "通过短信验证码修改密码")]
    public string SendNewPrePayPwd(string JSon)
    {
        SYS_PhoneCode bll = new SYS_PhoneCode();
        SYS_PhoneCode.PhoneCode result = bll.ChangePwdByCode(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 推送

    [WebMethod(Description = "订单推送")]
    public void AnPush(string userType, string sendType, string orderID)
    {
        MsgSend bll = new MsgSend();
        bll.GetWxService(sendType, orderID, userType);
    }

    [WebMethod(Description = "新闻推送")]
    public void MsgPush(string msgID, string type)
    {
        string otype = type == "" || type == "-1" ? "-1" : type;
        new MsgSend().GetMsgService(msgID, otype);
    }

    #endregion

    #region 加密、解密

    [WebMethod(Description = "解密")]
    public string Transfer(string JSon)
    {
        return AESHelper.Decrypt_android(JSon);
    }

    [WebMethod(Description = "加密")]
    public string Maker(string JSon)
    {
        return AESHelper.Encrypt_android(JSon);
    }

    #endregion

    #region 获取最新版本

    [WebMethod(Description = "获取版本号: 0 企业app版本号，1 经销商app版本号")]
    public string Version()
    {
        BD_Company bll = new BD_Company();
        BD_Company.ResultVersion result = bll.GetVersion();
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 记录日志

    [WebMethod(Description = "登录日志")]
    public void LoginLog(string userName, string loginUrl, string UType, string IsLogin, string IP, string remark = "")
    {
        try
        {
            if (IsLogin == "1")
            {
                Common.EditLog("安全日志", userName, "用户" + userName + "登录管理系统成功。",
                    "系统安全模块", loginUrl, 0, 1, Convert.ToInt32(UType), IP);
            }
            else
            {
                Common.EditLog("安全日志", userName, "用户" + userName + "登录管理系统失败" + (remark == "" ? "。" : ("," + remark + "。")),
                    "系统安全模块", loginUrl, 0, 1, Convert.ToInt32(UType), IP);
            }
        }
        catch
        {
        }
    }

    [WebMethod(Description = "记录手机异常")]
    public string WriteLog(string JSon)
    {
        try
        {
            JsonData JInfo = JsonMapper.ToObject(AESHelper.Decrypt_android(JSon));

            string strPath = HttpRuntime.AppDomainAppPath.ToString() + "APP\\CompLog.txt";
            LogHelper log = new LogHelper(strPath, JInfo["Content"].ToString().Trim());
            log.Write();
            if (log.Result)
                return AESHelper.Encrypt_android("{\"Result\":\"T\",\"Description\":\"\"}");
            else
                return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"\"}");
        }
        catch (Exception ex)
        {
            return AESHelper.Encrypt_android("{\"Result\":\"F\",\"Description\":\"\"}");
        }
    }

    #endregion
}
