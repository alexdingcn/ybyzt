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
        if (Request.QueryString["KeyID"] == "" || Request.QueryString["KeyID"]==null)
        {
            KeyID = 0;
        }
        else
        {
            KeyID = Convert.ToInt32(Common.DesDecrypt(Convert.ToString(Request.QueryString["KeyID"]), Common.EncryptKey));
        }
        
        if (!IsPostBack)
        {
            this.lblMsg.InnerText =Common.DesDecrypt(Request["msg"],Common.EncryptKey);

            if (Request.QueryString["type"] == "")
            {

                GetMessage(0);
            }
            else
            {
                GetMessage(Convert.ToInt32(Common.DesDecrypt(Request.QueryString["type"].ToString(), Common.EncryptKey)));
            
            }
        }
    }

    /// <summary>
    /// 返回错误信息
    /// </summary>
    /// <param name="type">1,订单/账单支付  2 ,充值  3，网银支付</param>
    /// <returns></returns>
    public string GetMessage(int type) 
    {
        
        switch (type) 
        {
            case 1:
                str= "无法完成付款！";
                url = "您可以<a href='../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "'>查看订单详情</a>";
                break;
            case 2:
                str= "无法完成付款！";
                url = "可以点击回到<a href='../UserIndex.aspx'>我的桌面</a>";
                break;
            case 3:
                str= "付款失败！";
                url = "您可以<a href='../neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "'>查看订单详情</a>";
                break;
            default:
                str = "付款失败！";
                url = "";
                    break;
        
        }
        return str;

    }
    
}