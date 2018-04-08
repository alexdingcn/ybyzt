using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_Pay_PaySuccess : System.Web.UI.Page
{
    public int KeyID = 0;
    public int Pid = 0;
    public int PPid = 0;
    public int Fid = 0;
    public string IsRef = "";


    public string str = string.Empty;

    public string url = string.Empty;

    public string lastmesage = string.Empty;

    public string Type = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["KeyID"] == "")
        {
            KeyID = 0;
        }
        else
        {
            KeyID = Convert.ToInt32(Common.DesDecrypt(Convert.ToString(Request.QueryString["KeyID"]), Common.EncryptKey));

            Type = Common.DesDecrypt(Convert.ToString(Request.QueryString["type"]), Common.EncryptKey);
            if (Type != "3")
            {
                Pid = Convert.ToInt32(Common.DesDecrypt(Convert.ToString(Request.QueryString["Pid"]), Common.EncryptKey));
                PPid = Convert.ToInt32(Common.DesDecrypt(Convert.ToString(Request.QueryString["PPid"]), Common.EncryptKey));
                if (Request.QueryString["Fid"] != null)
                {
                    Fid = Convert.ToInt32(Common.DesDecrypt(Convert.ToString(Request.QueryString["Fid"]), Common.EncryptKey));
                }
                if (Request.QueryString["IsRef"] != null)
                {
                    IsRef = Convert.ToString(Request.QueryString["IsRef"]);
                }
            }

        }

        //if (Type != "3")
        //{
        //    if (!Common.PageDisOperable("Order", KeyID, this.DisID))
        //    {
        //        Response.Redirect("../../NoOperable.aspx", true);
        //        return;
        //    }
        //}


        if (!IsPostBack)
        {
            if (Type != "3")
                Bind();

            if (Request.QueryString["type"] == "")
            {

                GetMessage(0);
            }
            else
            {
                GetMessage(Convert.ToInt32(Type));

            }
        }
    }
    public void Bind()
    {
        decimal price = 0;
        Hi.Model.DIS_Order orderM = new Hi.BLL.DIS_Order().GetModel(KeyID);
        Hi.Model.PAY_Payment payM = new Hi.BLL.PAY_Payment().GetModel(Pid);
        Hi.Model.PAY_Financing FinM = new Hi.BLL.PAY_Financing().GetModel(Fid);
        if (payM != null)
            price += payM.PayPrice;
        Hi.Model.PAY_PrePayment prepayM = new Hi.BLL.PAY_PrePayment().GetModel(PPid);
        if (prepayM != null)
            price += prepayM.price * -1;
        if (FinM != null)
            price += FinM.AclAmt;
        this.PayedAmount.InnerHtml = price.ToString("0.00");
    }


    /// <summary>
    /// 返回成功信息
    /// </summary>
    /// <param name="type">1,订单支付  2 ,账单支付  3，充值</param>
    /// <returns></returns>
    public string GetMessage(int type)
    {

        switch (type)
        {
            case 1:
                str = "付款成功，已支付";
                url = " 您可以<a href='../newOrder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "'>查看订单详情</a><a href='../OrderList.aspx'>返回订单管理</a>";
                lastmesage = "<a href='../newOrder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "' id='lblmessage'>5</a>";

                break;
            case 2:
                str = "付款成功，已支付";
                url = " 您可以<a href='../OrderZDInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "'>查看账单详情</a><a href='orderZDList.aspx'>返回我的账单</a>";
                lastmesage = " <a href='../OrderZDInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "' id='lblmessage'>5</a>";
                break;
            case 3:
                str = "交易成功，已支付";
                Hi.Model.PAY_Payment payM = new Hi.Model.PAY_Payment();
                List<Hi.Model.PAY_Payment> payMList = new Hi.BLL.PAY_Payment().GetList("", "orderid=" + KeyID, "");
                if (payMList.Count > 0)
                    payM = payMList[0];
                this.PayedAmount.InnerHtml = payM.PayPrice.ToString("0.00");


                url = "可以点击回到<a href='PrePayList.aspx'>企业钱包查询</a>";

                lastmesage = "<a href='PrePayList.aspx' id='lblmessage'>5</a>";


                break;
            default:
                str = "付款失败！";
                url = "";
                break;

        }
        return str;

    }

    public string PreType(int Pre)
    {
        if (Pre == 1)
        {
            return "<a href='PrePayList.aspx'>企业钱包查询</a>";
        }
        else if (Pre == 6)
        {
            return "<a href='TransferList.aspx'>转账汇款查询</a>";
        }
        return "<a href='PrePayList.aspx'>企业钱包查询</a>";
    }
    public string GetUrl(int Pre)
    {
        if (Pre == 1)
        {
            return "PrePayList.aspx";
        }
        else if (Pre == 6)
        {
            return "TransferList.aspx";
        }
        return "PrePayList.aspx";
    }
}