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
using System.Text;

/// <summary>
///AppService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class AppService : System.Web.Services.WebService {

    public AppService () {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    #region 登录

    #region 新版

    [WebMethod(Description = "普通方式登录")]
    public string Login(string JSon,string from)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultLogin result = bll.Login(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
     //[WebMethod(Description = "普通方式登录")]
    //public string Login(string from,string JSon )
    //{
    //    SYS_Users bll = new SYS_Users();
    //    if (from != null && from.Length == 0)
    //    {
    //        from = "kong";
    //    }
    //    return from;
    //    //SYS_Users.ResultLogin result = bll.Login(AESHelper.Decrypt_android(JSon));
    //    //JavaScriptSerializer js = new JavaScriptSerializer();
    //    //return AESHelper.Encrypt_android(js.Serialize(result));
    //}

    [WebMethod(Description = "手机登录")]
    public string LoginByPhone(string JSon,string from)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultLogin result = bll.LoginByPhone(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "微信获取用户信息")]
    public string WXGetUserinfo(string JSon, string from)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultLogin result = bll.WXGetUserinfo(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "绑定，修改，注销OpenID")]
    public void EditOpenID(string JSon, string from)
    {
        SYS_Users bll = new SYS_Users();
        bll.EditOpenID(AESHelper.Decrypt_android(JSon), from);
    }
    #endregion

    #region 旧版

    [WebMethod(Description = "经销商登录")]
    public string ResellerLogin(string JSon,string from)
    {
        SYS_Users bll = new SYS_Users();
        SYS_Users.ResultDisLog result = bll.ResellerLogin(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业登录")]
    public string CompanyLogin(string JSon,string from)
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
    public string GetResellerCompany(string JSon,string from)
    {
         BD_Company bll = new  BD_Company();
         BD_Company.ResultList result = bll.GetResellerCompany(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "获取登录用户厂商信息")]
    public string GetUserCompany(string JSon, string from)
    {
        BD_Company bll = new BD_Company();
        BD_Company.ResultList result = bll.GetUserCompany(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 商品

    [WebMethod(Description = "企业商品分类列表")]
    public string GetResellerProductClassifyList(string JSon,string from)
    {
         BD_GoodsCategory bll = new  BD_GoodsCategory();
         BD_GoodsCategory.ResultGoodsCategory result = bll.GetResellerProductClassifyList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "获取某一分类产品信息")]
    public string GetProductList(string JSon,string from)
    {
        //Common.WriteLog(@"D:\u8\json_start.txt", JSon);
        BD_Goods bll = new BD_Goods();
        BD_GoodsCategory.ResultProductList result = bll.GetProductsList(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "核心企业根据经销商名称/编码，商品名称/编码获取获取商品列表")]
    public string CompanyProductSearch(string JSon, string from)
    {
        //Common.WriteLog(@"D:\u8\json_start.txt", JSon);
        BD_Goods_ver3 bll = new BD_Goods_ver3();
        BD_Goods_ver3.ResultCompanyProductSearch result = bll.CompanyProductSearch(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "经销商获取商品")]
    public string ResellerProductSearch(string JSon, string from)
    {
        //Common.WriteLog(@"D:\u8\json_start.txt", JSon);
        BD_Goods_ver3 bll = new BD_Goods_ver3();
        BD_Goods_ver3.ResultCompanyProductSearch result = bll.ResellerProductSearch(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业搜索商品列表")]
    public string SearchProductList(string JSon,string from)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultProductList result = bll.SearchProductList(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "经销商搜索商品列表")]
    public string ResellerSearchGoodsList(string JSon, string from)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultProductList result = bll.ResellerSearchGoodsList(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "搜索商品")]
    public string SearchGoodst(string JSon,string from)
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
    public string SetCollect(string JSon,string from)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultCollect result = bll.SetCollect(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    
    [WebMethod(Description = "收藏商品列表")]
    public string GetCollectList(string JSon,string from)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultProductList result = bll.GetCollectList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "商品上下架跟修改库存")]
    public string ComPanyProductsEdit(string JSon, string from)
    {
        BD_Goods_ver3 bll = new BD_Goods_ver3();
        BD_Goods_ver3.ResultProductsEdit result = bll.CompanyProductsEdit(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "取出经销商对应的核心企业的配置信息")]
    public string GetConfiguration(string JSon, string from)
    {
        BD_Goods_ver3 bll = new BD_Goods_ver3();
        BD_Goods_ver3.ResultConfig result = bll.GetConfiguration(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业获取商品计量单位")]
    public string GetUnit(string JSon, string from)
    {
        BD_Goods_ver3 bll = new BD_Goods_ver3();
        BD_Goods_ver3.ResultUnit result = bll.GetUnit(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业根据规格属性模板获取模板属性")]
    public string GetTemplateValue(string JSon, string from)
    {
        BD_Goods_ver3 bll = new BD_Goods_ver3();
        BD_Goods_ver3.TemplateValueResult result = bll.GetTemplateValue(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业修改商品信息")]
    public string EditGoods(string JSon, string from)
    {
        BD_Goods_ver3 bll = new BD_Goods_ver3();
        BD_Goods_ver3.ResultProductsEdit result = bll.EditGoods(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

     [WebMethod(Description = "核心企业新增商品信息")]
    public string AddGoods(string JSon, string from)
    {
        BD_Goods_ver3 bll = new BD_Goods_ver3();
        BD_Goods_ver3.ResultProductsEdit result = bll.AddGoods(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

     [WebMethod(Description = "核心企业判断商品的属性与属性值是否可以删除")]
     public string IsAttrDel(string JSon, string from)
     {
         BD_Goods_ver3 bll = new BD_Goods_ver3();
         BD_Goods_ver3.ResultProductsEdit result = bll.IsAttrDel(AESHelper.Decrypt_android(JSon));
         JavaScriptSerializer js = new JavaScriptSerializer();
         return AESHelper.Encrypt_android(js.Serialize(result));
     }
    #endregion

    #region 订单

    [WebMethod(Description = "经销商订单列表")]
    public string GetResellerOrderList(string JSon,string from)
    {
         DIS_Order bll = new  DIS_Order();
         DIS_Order.ResultOrderList result = bll.GetResellerOrderList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "经销商订单详情")]
    public string GetResellerOrderDetail(string JSon,string from)
    {
        if (from.ToLower() == "android" || from.ToLower() == "ios" || float.Parse(from) < 3)
        {
            DIS_Order bll = new DIS_Order();
            DIS_Order.ResultOrderInfo result = bll.GetResellerOrderDetail(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        else if (float.Parse(from) >= 3)
        {
            Dis_Order_Version3 bll = new Dis_Order_Version3();
            Dis_Order_Version3.ResultOrderInfo result = bll.GetResellerOrderDetail(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        return "版本号错误！";
    }

    [WebMethod(Description = "企业订单列表")]
    public string GetCompanyOrderList(string JSon,string from)
    {
        if (from.ToLower() == "android" || from.ToLower() == "ios" || float.Parse(from) < 6)
        {
            DIS_Order bll = new DIS_Order();
            DIS_Order.ResultOrderList result = bll.GetCompanyOrderList(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        else
        {
            Dis_Order_Version3 bll = new Dis_Order_Version3();
            Dis_Order_Version3.ResultOrderList result = bll.GetCompanyOrderList(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
    }

    [WebMethod(Description = "企业根据订单号或经销商名称/编码获取订单列表")]
    public string CompanyOrderSearch(string JSon,string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultOrderList result = bll.CompanyOrderSearch(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业订单详情")]
    public string GetCompanyOrderDetail(string JSon,string from)
    {
        if (from.ToLower() == "android" || from.ToLower() == "ios" || float.Parse(from) < 3)
        {
            DIS_Order bll = new DIS_Order();
            DIS_Order.ResultOrderInfo result = bll.GetCompanyOrderDetail(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        else if (float.Parse(from) >= 3)
        {
            Dis_Order_Version3 bll = new Dis_Order_Version3();
            Dis_Order_Version3.ResultOrderInfo result = bll.GetCompanyOrderDetail(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        return "版本号错误！";
    }

    [WebMethod(Description = "企业订单审核")]
    public string SubCompanyApprove(string JSon,string from)
    {
         DIS_Order bll = new  DIS_Order();
         DIS_Order.ResultAudit result = bll.SubCompanyApprove(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业订单发货")]
    public string SubProduct(string JSon,string from)
    {
        if (from.ToLower() == "android" || from.ToLower() == "ios" || float.Parse(from) < 3)
        {
            DIS_Order bll = new DIS_Order();
            DIS_Order.ResultAudit result = bll.SubProduct(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        else if (float.Parse(from) >= 3)
        {
            Dis_Order_Version3 bll = new Dis_Order_Version3();
            Dis_Order_Version3.ResultAudit result = bll.SubProduct(AESHelper.Decrypt_android(JSon),from);
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        return "版本号错误！";
    }

    [WebMethod(Description = "企业订单签收")]
    public string ConfirmReceipt(string JSon,string from)
    {
        if (from.ToLower() == "android" || from.ToLower() == "ios" || float.Parse(from) < 3)
        {
            DIS_Order bll = new DIS_Order();
            DIS_Order.ResultOutConfirm result = bll.ConfirmReceipt(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        else if (float.Parse(from) >= 3)
        {
            Dis_Order_Version3 bll = new Dis_Order_Version3();
            Dis_Order_Version3.ResultOutConfirm result = bll.ConfirmReceipt(AESHelper.Decrypt_android(JSon));
            JavaScriptSerializer js = new JavaScriptSerializer();
            return AESHelper.Encrypt_android(js.Serialize(result));
        }
        return "版本号错误！";
    }

    [WebMethod(Description = "订单提交:支持批量操，支持非同一操作")]
    public string SubResellerOrder(string JSon,string from)
    {
         DIS_OrderSub bll = new  DIS_OrderSub();
         DIS_OrderSub.ResultSubOrder result = bll.SubResellerOrder(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "退货申请")]
    public string SubOrderReturn(string JSon,string from)
    {
        DIS_OrderSub bll = new DIS_OrderSub();
        DIS_OrderSub.ResultAudit result = bll.SubOrderReturn(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "退货审核")]
    public string AuditOrderReturn(string JSon,string from)
    {
        DIS_OrderSub bll = new DIS_OrderSub();
        DIS_OrderSub.ResultAudit result = bll.AuditOrderReturn(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "编辑订单")]
    public string EditComPanyOrder(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultEdit result = bll.EditComPanyOrder(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "编辑发货单")]
    public string EditOrderOut(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultEdit result = bll.EditOrderOut(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "修改订单获取商品价格")]
    public string GetProductPrice(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultProductPrice result = bll.GetProductPrice(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "商品检测")]
    public string GetOrderCheckList(string JSon,string from)
    {
        DIS_OrderSub bll = new DIS_OrderSub();
        DIS_OrderSub.ResultCheck result = bll.GetOrderCheckList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "订单作废")]
    public string CancelOrder(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultEdit result = bll.CancelOrder(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "发货单作废")]
    public string CancelOrderOut(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultEdit result = bll.CancelOrderOut(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "附件上传")]
    public string Unload(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultEdit result = bll.Unload(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "修改物流信息")]
    public string EditLogistics(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultEditLogistics result = bll.EditLogistics(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "订单线下支付")]
    public string OrderPay(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.OrderPay result = bll.EditOrderPay(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "订单线下支付上传附件")]
    public string OrderPayAttch(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.OrderPay result = bll.OrderPayAttch(JSon);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return js.Serialize(result);
    }

    #endregion

    #region 促销

    [WebMethod(Description = "订单促销")]
    public string GetOrderProList(string JSon,string from)
    {
        BD_GoodsCategory bll = new BD_GoodsCategory();
        BD_GoodsCategory.ResultOrderProList result = bll.GetOrderPro(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 统计数据
    [WebMethod(Description = "查询核心企业统计")]
    public string GetComOrderSta(string JSon,string from)
    {
        GetComOrderSta bll = new GetComOrderSta();
        GetComOrderSta.ReturnComOrderSta result = bll.GetComOrder(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "查询经销商订单统计列表")]
    public string GetResellerOrderStaList(string JSon,string from)
    {
        GetResellerOrderStaList bll = new GetResellerOrderStaList();
        GetResellerOrderStaList.ReturnOrderStaList result = bll.GetOrderStaList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "查询经销商订单统计")]
    public string GetResellerOrderSta(string JSon,string from)
    {
        GetResellerOrderSta bll = new GetResellerOrderSta();
        GetResellerOrderSta.ReturnOrderSta result = bll.GetOrderSta(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "经销商统计数据")]
    public string GetDisNum(string JSon,string from)
    {
        userIndex bll = new userIndex();
        userIndex.ResultJSC result = bll.GetDisNum(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "企业统计数据")]
    public string GetCompNum(string JSon,string from)
    {
        jsc bll = new jsc();
        jsc.ResultJSC result = bll.GetCompNum(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "获取核心企业简报")]
    public string GetCompyBriefing(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultCompyBriefing result = bll.GetCompyBriefing(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "查询核心企业简报的订单列表")]
    public string GetCompyBriefingList(string JSon, string from)
    {
        GetCompyBriefingList bll = new GetCompyBriefingList();
        GetCompyBriefingList.ResultOrderList result = bll.GetBriefingList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "获取核心企业统计")]
    public string GetCompanySta(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultCompanySta result = bll.GetCompanySta(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));

    }
    #endregion

    #region 新闻公告

    [WebMethod(Description = "未读新闻公告个数")]
    public string GetAnnouncement(string JSon,string from)
    {
        BD_CompNews bll = new BD_CompNews();
        BD_CompNews.ResultNoRead result = bll.GetAnnouncement(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "新闻公告列表")]
    public string CompNewsList(string JSon,string from)
    {
        BD_CompNews bll = new BD_CompNews();
        BD_CompNews.ResultNewsList result = bll.CompNewsList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "新闻公告详情")]
    public string GetNewsInfo(string JSon,string from)
    {
        BD_CompNews bll = new BD_CompNews();
        BD_CompNews.ResultNewsInfo result = bll.GetNewsInfo(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "新闻添加")]
    public string CompNewsAdd(string JSon,string from)
    {
        BD_CompNews bll = new BD_CompNews();
        BD_CompNews.ResultNewsAdd result = bll.CompNewsAdd(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "过期列表")]
    public string GetMessageDeleteId(string JSon,string from)
    {
        BD_CompNews bll = new BD_CompNews();
        BD_CompNews.ResultNoTime result = bll.GetMessageDeleteId(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 用户

    [WebMethod(Description = "经销商获取个人信息")]
    public string GetResellerInfo(string JSon,string from)
    {
        SYS_Users bll = new  SYS_Users();
        SYS_Users.ResultDisInfo result = bll.GetResellerInfo(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 收货地址

    [WebMethod(Description = "获取经销商收货地址列表")]
    public string GetResellerShippingAddressList(string JSon,string from)
    {
         BD_DisAddr bll = new  BD_DisAddr();
         BD_DisAddr.ResultAddr result = bll.GetResellerShippingAddressList(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "经销商收货地址新增")]
    public string ResellerAddrAdd(string JSon,string from)
    {
        BD_DisAddr bll = new BD_DisAddr();
        BD_DisAddr.ResultAddrAdd result = bll.ResellerAddrAdd(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #endregion

    #region 物流公司

    [WebMethod(Description = "获取核心企业物流绑定列表")]
    public string GetLogisticsList(string JSon,string from)
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
     [WebMethod(Description = "获取验证图片")]
    public string GetCaptchaPhoto()
    {
        CheckCode bll = new CheckCode();
        CheckCode.ResultCaptchaPhoto result = bll.GetCaptchaPhoto();
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

     [WebMethod(Description = "发送验证码 ")]
     public string GetCaptcha(string JSon, string from)
     {
         SYS_PhoneCode bll = new SYS_PhoneCode();
         SYS_PhoneCode.ResultCaptcha result = bll.GetCaptcha(AESHelper.Decrypt_android(JSon),from);
         JavaScriptSerializer js = new JavaScriptSerializer();
         return AESHelper.Encrypt_android(js.Serialize(result));
     }
     [WebMethod(Description = "找回密码-发送新密码 ")]
     public string GetLoginCaptcha(string JSon, string from)
     {
         SYS_PhoneCode bll = new SYS_PhoneCode();
         SYS_PhoneCode.ResultLoginCaptcha result = bll.GetLoginCaptcha(AESHelper.Decrypt_android(JSon), from);
         JavaScriptSerializer js = new JavaScriptSerializer();
         return AESHelper.Encrypt_android(js.Serialize(result));
     }
     [WebMethod(Description = "提交核心企业入驻申请")]
     public string SendEnterRequest(string JSon, string from)
     {
         CheckCode bll = new CheckCode();
         CheckCode.ResultCompEnter result = bll.SendEnterRequest(AESHelper.Decrypt_android(JSon), from);
         JavaScriptSerializer js = new JavaScriptSerializer();
         return AESHelper.Encrypt_android(js.Serialize(result));
         
     }
    #endregion
    
    #region 推送

    [WebMethod(Description = "订单推送")]
    public void AnPush(string userType, string sendType, string orderID,decimal money = 0)
    {
        MsgSend bll = new MsgSend();
        bll.GetWxService(sendType, orderID, userType,money:money);
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
        if (JSon == null || JSon == "")
        {
            return "false";
        }
        return AESHelper.Decrypt_android(JSon);
    }

    [WebMethod(Description = "加密")]
    public string Maker(string JSon)
    {
        if (JSon == null || JSon == "")
        {
            return "false";
        }
        return AESHelper.Encrypt_android(JSon);
    }
    [WebMethod(Description = "支付key加密")]
    public string Maker_string(string key)
    {
        return AESHelper.Encrpt_string(key);
    }
    [WebMethod(Description = "MD5加密")]
    public string Maker_MD5(string JSon)
    {
        return AESHelper.Encrypt_MD5(JSon);
    }
    //[WebMethod(Description = "老版本加密")]
    //public string Maker_old(string JSon)
    //{
    //    return AESHelper.Encrypt_android_old(JSon);
    //}
    //[WebMethod(Description = "老版本解密")]
    //public string Transfer_old(string JSon)
    //{
    //    return AESHelper.Decrypt_android_old(JSon);
    //}
    #endregion

    #region 获取最新版本

    [WebMethod(Description = "获取版本号: 0 企业app版本号，1 经销商app版本号,2.0以后版本用")]
    public string Version_New()
    {
        Context.Response.AddHeader("Access-Control-Allow-Origin","*");
        Context.Response.AddHeader("Access-Control-Allow-Methods", "POST");
        BD_Company bll = new BD_Company();
        BD_Company.ResultVersion result = bll.GetVersion();
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "获取版本号: 0 企业app版本号，1 经销商app版本号，2.0及以前版本用")]
    public string Version()
    {
        BD_Company bll = new BD_Company();
        BD_Company.ResultVersion result = bll.GetVersion();
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android_old(js.Serialize(result));
    }

    #endregion

    #region 记录日志

    [WebMethod(Description = "登录日志")]
    public void LoginLog(string userName, string loginUrl, string UType, string IsLogin, string IP,string remark = "")
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
    [WebMethod(Description = "获取可用返利")]
    public string GetRebate(string JSon,string from)
    {
        GetRebate bll = new GetRebate();
        GetRebate.GetRebateResult result = bll.GetRebateList(AESHelper.Decrypt_android(JSon),from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    #endregion

    [WebMethod(Description = "获取经销商开票信息")]
    public string GetResellerBillList(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultBillList result = bll.GetResellerBillList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "获取支付宝支付密钥")]
    public string GetAliPayID(string JSon, string from)
    {
        Pay_Update pay = new Pay_Update();
        Pay_Update.ResultAli result = pay.Result_Ali(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "获取支付宝支付信息")]
    public string GetPayInfo(string JSon, string from)
    {
        Pay_Update pay = new Pay_Update();
        Pay_Update.ResultPayInfo result = pay.GetPayInfo(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "获取经销商/核心企业新增订单提示")]
    public string GetOrderPrompt(string JSon, string from)
    {
        Dis_Order_Version3 pay = new Dis_Order_Version3();
        Dis_Order_Version3.ResultOrderPrompt  result = pay.GetOrderPrompt(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "获取核心企业对应的经销商列表")]
    public string GetResellerList(string JSon, string from)
    {
        Dis_Order_Version3 bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultResellerList result = bll.GetResellerList(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "订单线下支付")]
    public string OffLinePay(string JSon, string from)
    {
        OrderPay bll = new OrderPay();
        OrderPay.ResultOffLinePay result = bll.OffLinePay(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "核心企业订单获取发货信息列表")]
    public string GetOrderSendGoodsInfo(string JSon, string from)
    {
        Dis_Order_Version3  bll = new Dis_Order_Version3();
        Dis_Order_Version3.ResultGetOrderOut result = bll.GetOrderSendGoodsInfo(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    #region//经销商
     [WebMethod(Description = "获取经销商分类列表")]
    public string GetResellerClassifyList(string JSon, string from)
    {
        Reseller bll = new Reseller();
        Reseller.ResultResellerClassify result = bll.GetResellerClassifyList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "获取经销商详细信息")]
     public string GetResellerDetail(string JSon, string from)
     {
         Reseller bll = new Reseller();
         Reseller.ResultResellerDetail result = bll.GetResellerDetail(AESHelper.Decrypt_android(JSon));
         JavaScriptSerializer js = new JavaScriptSerializer();
         return AESHelper.Encrypt_android(js.Serialize(result));
     }

    [WebMethod(Description = "获取经销商区域分类")]
    public string GetResellerAreaList(string JSon, string from)
    {
        Reseller bll = new Reseller();
        Reseller.ResultResellerArea result = bll.GetResellerAreaList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业修改经销商登录信息")]
    public string EditResellerAccount(string JSon, string from)
    {
        Reseller bll = new Reseller();
        Reseller.ResultEditResellerAccount result = bll.EditResellerAccount(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业修改经销商信息")]
    public string EditReseller(string JSon, string from)
    {
        Reseller bll = new Reseller();
        Reseller.ReseltResellerEdit result = bll.EditReseller(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业新增经销商")]
    public string AddReseller(string JSon, string from)
    {
        Reseller bll = new Reseller();
        Reseller.ReseltResellerEdit result = bll.AddReseller(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    #endregion

    #region 核心企业
    [WebMethod(Description = "核心企业获取个人信息")]
    public string GetCompanyInfo(string JSon, string from)
    {
        Company bll = new Company();
        class_ver3.CompanyInfo result = bll.GetCompanyInfo(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业修改登录账号")]
    public string EditCompanyAccount(string JSon, string from)
    {
        Company bll = new Company();
        Company.EditResult result = bll.EditCompanyAccount(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业修改系统设置")]
    public string EditCompanySysSettings(string JSon, string from)
    {
        Company bll = new Company();
        Company.EditResult result = bll.EditCompanySysSettings(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业修改个人信息")]
    public string EditCompanyInfo(string JSon, string from)
    {
        Company bll = new Company();
        Company.EditResult result = bll.EditCompanyInfo(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业修改登录密码")]
    public string EditCompanyLoginPassword(string JSon, string from)
    {
        Company bll = new Company();
        Company.EditResult result = bll.EditCompanyLoginPassword(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }


    [WebMethod(Description = "核心企业启用停用收款账户")]
    public string EditCompanyPayAccount(string JSon, string from)
    {
        Company bll = new Company();
        Company.EditResult result = bll.EditCompanyPayAccount(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "商品图片上传")]
    public string UpGoodsPic(string JSon, string from)
    {
        UpLoadPic bll = new UpLoadPic();
        Dis_Order_Version3.ResultEdit result = bll.UpGoodsPic(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "核心企业注册附件上传")]
    public string CompFileUp(string JSon, string from)
    {
        UpLoadPic bll = new UpLoadPic();
        Dis_Order_Version3.ResultEdit result = bll.CompFileUp(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "经销商修改登录密码")]
    public string EditResellerLoginPassword(string JSon, string from)
    {
        GetCompyBriefingList bll = new GetCompyBriefingList();
        GetCompyBriefingList.EditResult result = bll.EditResellerLoginPassword(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "经销商新增收货地址")]
    public string AddResellerShippingAddressList(string JSon, string from)
    {
        GetCompyBriefingList bll = new GetCompyBriefingList();
        GetCompyBriefingList.EditResult result = bll.AddResellerShippingAddressList(AESHelper.Decrypt_android(JSon));
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    #endregion

    #region 微信

    [WebMethod(Description = "微信获取权限验证config")]
    public string GetWXconfig(string JSon, string from)
    {
        WX bll = new WX();
        WX.jsapi_ticket result = bll.getWXconfig(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "微信首页获取商品详情")]
    public string WXGetProductInfo(string JSon, string from)
    {
        Company bll = new Company();
        Company.ResultWXProduct result = bll.WXGetProductInfo(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "微信首页获取商品列表")]
    public string WXGetProductList(string JSon, string from)
    {
        Company bll = new Company();
        Company.ResultWXProductList result = bll.WXGetProductList(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }
    [WebMethod(Description = "微信首页获取店铺信息")]
    public string WXGetCompInfo(string JSon, string from)
    {
        Company bll = new Company();
        Company.ResultWXCompInfo result = bll.WXGetCompInfo(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

    [WebMethod(Description = "微信首页获取店铺图片")]
    public string WXGetCompanyImg(string JSon, string from)
    {
        Company bll = new Company();
        Company.ResultCompanyImg result = bll.WXGetCompanyImg(AESHelper.Decrypt_android(JSon), from);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(result));
    }

     [WebMethod(Description = "微信首页获取店铺图片")]
    public string wxJSImage(string JSon, string from)
    {
        WX wx = new WX();
        string aa=wx.getJSImage(JSon);
        JavaScriptSerializer js = new JavaScriptSerializer();
        return AESHelper.Encrypt_android(js.Serialize(aa));
    }
    #endregion
}
