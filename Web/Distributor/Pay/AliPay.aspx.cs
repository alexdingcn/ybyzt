using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WxPayAPI;
using System.Data;
using System.Web.Configuration;
using System.Data.SqlClient;
using DBUtility;
using System.Web.Script.Serialization;
using LitJson;
public partial class Distributor_Pay_AliPay : DisPageBase
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public decimal txtPayOrder = 0;
    public string orderid =string.Empty;
    public string GoodsName = string.Empty;

    public int payid = 0;
    public int prepayid = 0;

    public int rechengID = 0;//企业钱包充值preID
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

            //初次加载，生成二维码连接
            orderid = Request.Form["hidOid"];
            txtPayOrder = Convert.ToDecimal(Request.Form["hidPayprice"]);//本次支付总金额              
            GetUrl(orderid, txtPayOrder);


        }

    }


    public void GetUrl(string oid, decimal payprice)
    {


        Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(oid));
        this.lblOrderNO.InnerText = OrderModel.ReceiptNo.Trim().ToString();
        this.fee.InnerText = this.CompName;//收款方

        string[] json = Tx1401.tx1401("20", oid, payprice);
        //生成成功
        //if (Convert.ToBoolean(json[0]))
        Image.ImageUrl = "WxPay.aspx?data=" + HttpUtility.UrlEncode(Common.DesDecrypt(json[1], Common.EncryptKey));
        this.hidguid.Value = json[2];//支付交易流水号
        // else
        //    ErrMessage(json[1], oid);

    }





    /// <summary>
    /// 错误处理页面
    /// </summary>
    /// <param name="msg">提示消息</param>
    public void ErrMessage(string msg, string orderid)
    {
        Response.Redirect("Error.aspx?type=" + Common.DesEncrypt(rechengID > 0 ? "2" : "3", Common.EncryptKey) + "&KeyID=" + Common.DesEncrypt(orderid, Common.EncryptKey) + "&msg=" + Common.DesEncrypt(msg, Common.EncryptKey), false);
    }
}

   