using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_Pay_Error : System.Web.UI.Page
{
    public string msg = "";
    public int KeyID = 0;
    public string str = string.Empty;
    public string url = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
       
        if (!IsPostBack)
        {
            this.lblMsg.InnerText = "购买失败";
            GetMessage(0);
        }
    }

    /// <summary>
    /// 返回错误信息
    /// </summary>
    /// <param name="type">1,订单/账单支付  2 ,充值  3，网银支付</param>
    /// <returns></returns>
    public string GetMessage(int type) 
    {

      str= "付款失败！";
       url = "您可以<a href='../SysManager/Service.aspx'>再次购买</a>";
        return str;

    }
    
}