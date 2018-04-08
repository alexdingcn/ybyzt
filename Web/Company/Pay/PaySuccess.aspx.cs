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

            //////Type = Common.DesDecrypt(Convert.ToString(Request.QueryString["type"]), Common.EncryptKey);
            ////if (Type != "3")
            ////{
            //    Pid = Convert.ToInt32(Common.DesDecrypt(Convert.ToString(Request.QueryString["Pid"]), Common.EncryptKey));
            //    PPid = Convert.ToInt32(Common.DesDecrypt(Convert.ToString(Request.QueryString["PPid"]), Common.EncryptKey));
            //    if (Request.QueryString["Fid"] != null)
            //    {
            //        Fid = Convert.ToInt32(Common.DesDecrypt(Convert.ToString(Request.QueryString["Fid"]), Common.EncryptKey));
            //    }
            //    if (Request.QueryString["IsRef"] != null)
            //    {
            //        IsRef = Convert.ToString(Request.QueryString["IsRef"]);
            //    }
            //}

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
                Bind();
                GetMessage(0);
    
           
        }
    }
    public void Bind()
    {
        decimal price = 0;
        Hi.Model.Pay_Service orderM = new Hi.BLL.Pay_Service().GetModel(KeyID);
        price = orderM.PayedPrice;
        this.PayedAmount.InnerHtml = price.ToString("0.00");
    }


    /// <summary>
    /// 返回成功信息
    /// </summary>
    /// <param name="type">1,订单支付  2 ,账单支付  3，充值</param>
    /// <returns></returns>
    public string GetMessage(int type)
    {

           str = "付款成功，已支付";
           url = " 您可以<a href='../jsc.aspx'>返回后台</a>";
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