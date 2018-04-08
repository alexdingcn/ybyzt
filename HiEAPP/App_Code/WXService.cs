using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using LitJson;

/// <summary>
///WXService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。
// [System.Web.Script.Services.ScriptService]
public class WXService : System.Web.Services.WebService {

    public WXService () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    #region 登录

    [WebMethod(Description = "微信用户第一次登录")]
    public string WXLogin(string JSon)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultWXLogin result = bll.WXLogin(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    [WebMethod(Description = "微信用户解除绑定")]
    public string WXRelieveUser(string JSon)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultWXLogin result = bll.WXRelieveUser(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    #endregion

    #region 用户

    [WebMethod(Description = "通过openid获取用户id")]
    public string GetUserIDByWXOpenid(string JSon)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultUserID result = bll.GetUserIDByWXOpenid(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    [WebMethod(Description = "通过openid获取经销商个人信息")]
    public string GetUserInfo(string JSon)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultDisInfo result = bll.GetUserInfo(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    [WebMethod(Description = "经销商获取个人信息")]
    public string GetResellerInfo(string JSon)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultDisInfo result = bll.GetResellerInfo(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    #endregion

    #region 订单

    [WebMethod(Description = "test1:GetDisOrderList")]
    public string GetDisOrderList(string JSon)
    {
        DIS_Order.WXOrderList result = new DIS_Order().GetDisOrderList(AESHelper.Decrypt_php(JSon));
        return AESHelper.Encrypt_php(new JavaScriptSerializer().Serialize(result));
    }

    [WebMethod(Description = "经销商订单列表")]
    public string WXGetResellerOrderList(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultOrderList result = bll.WXGetResellerOrderList(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    [WebMethod(Description = "经销商订单详情")]
    public string WXGetResellerOrderDetail(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultOrderInfo result = bll.WXGetResellerOrderDetail(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业订单列表")]
    public string WXGetCompanyOrderList(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultOrderList result = bll.WXGetCompanyOrderList(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业订单详情")]
    public string WXGetCompanyOrderDetail(string JSon)
    {
        DIS_Order bll = new DIS_Order();
        DIS_Order.ResultOrderInfo result = bll.WXGetCompanyOrderDetail(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    #endregion

    #region 企业钱包

    [WebMethod(Description = "经销商企业钱包")]
    public string WXGetPrePaymentList(string JSon)
    {
        PAY_PrePayment bll = new PAY_PrePayment();
        PAY_PrePayment.ResultPre result = bll.WXGetPrePaymentList(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    #endregion

    #region 消息通知

    [WebMethod(Description = "消息通知")]
    public string GetNoticeList(string JSon)
    {
        SYS_Information bll = new SYS_Information();
        SYS_Information.ResuletInfo result = bll.GetNoticeList(AESHelper.Decrypt_php(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_php(js.Serialize(result));
    }

    #endregion
}
