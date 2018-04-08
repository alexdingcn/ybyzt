using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;
using System.Web.Script.Serialization;
using LitJson;

/// <summary>
///WechatOrderPay 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class WechatOrderPay : System.Web.Services.WebService
{

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public WechatOrderPay()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }
    OrderPay orderP = new OrderPay();
    Transfer transfer = new Transfer();

    [WebMethod(Description = "快捷支付绑定（发送短信验证）")]
    public string Tx2531(string from, string JSon)
    {
        if (from == "android" || from == "ios"  || Convert.ToInt32(from)>=0)
        {
            string result = orderP.Tx2531(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
    [WebMethod(Description = "快捷支付绑定（验证并绑定）")]
    public string Tx2532(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.Tx2532(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
    [WebMethod(Description = "订单快捷支付（发送短信验证）")]
    public string Tx1375(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.Tx1375(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
    [WebMethod(Description = "订单快捷支付（验证并支付）")]
    public string Tx1376(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.Tx1376(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
    [WebMethod(Description = "查询绑定银行卡")]
    public string GetFastpayBank(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.GetFastpayBank(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
    [WebMethod(Description = "核心企业获取转账汇款列表")]
    public string GetTransfer(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = transfer.GetTransfer(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"参数错误！\"}");
        }
    }
    [WebMethod(Description = "核心企业获取预收款列表")]
    public string GetReceivables(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = transfer.GetReceivables(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"参数错误！\"}");
        }
    }
    [WebMethod(Description = "核心企业获取预收款详情")]
    public string GetReDetailed(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = transfer.GetReDetailed(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("{\"result\":\"F\",\"Description\":\"参数错误！\"}");
        }
    }
    [WebMethod(Description = "经销商获取企业钱包列表")]
    public string GetPrepay(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.GetPrepay(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
    [WebMethod(Description = "经销商获取转账汇款列表")]
    public string GetTransfers(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.GetTransfers(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
    [WebMethod(Description = "企业钱包充值/转账汇款(发送验证码)")]
    public string ReTx1375(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.ReTx1375(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
    [WebMethod(Description = "企业钱包充值/转账汇款(检验验证码)")]
    public string ReTx1376(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.ReTx1376(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
    [WebMethod(Description = "获取微信支付信息")]
    public string GetPayID(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.GetPayID(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
    [WebMethod(Description = "获取订单支付状态")]
    public string GetPayState(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.GetPayState(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }


    [WebMethod(Description = "经销商订单支付明细查询")]
    public string GetOrdersPaymentList(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.GetOrdersPaymentList(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }

    [WebMethod(Description = "经销商获取待支付账单列表")]
    public string GetBillPaymentList(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.GetBillPaymentList(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }

    #region 应收款 By zxz

    [WebMethod(Description = "应收款列表")]
    public string GetDisAccountList(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            VIEW_Arrearage bll = new VIEW_Arrearage();
            VIEW_Arrearage.ResultDisAccountList result = bll.GetDisAccountList(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }

    [WebMethod(Description = "应收款详情")]
    public string GetDisAccountInfo(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            VIEW_Arrearage bll = new VIEW_Arrearage();
            DIS_Order.ResultOrderList result = bll.GetDisAccountInfo(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();

            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }

    #endregion

    [WebMethod(Description = "获取支付手续费")]
    public string Getzfsxf(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.Getzfsxf(AESHelper.Decrypt_android(JSon));
            //string result = orderP.Getzfsxf();
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }


    [WebMethod(Description = "查询银行卡信息")]
    public string GetVisiableBankList(string from, string JSon)
    {
        if (from == "android" || from == "ios" || Convert.ToInt32(from) >= 0)
        {
            string result = orderP.GetVisiableBankList(AESHelper.Decrypt_android(JSon));
            return AESHelper.Encrypt_android(result);
        }
        else
        {
            return AESHelper.Encrypt_android("");
        }
    }
}
