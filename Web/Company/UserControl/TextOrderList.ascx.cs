using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Disany_UserControl_TextOrderList : System.Web.UI.UserControl
{
    private string orderid = string.Empty;
    public string Name
    {
        get { return txt_OrderCoder.Value.Trim(); }
    }
    public string Class
    {
        get { return txt_OrderCoder.Attributes["class"]; }
        set { txt_OrderCoder.Attributes.Add("class", value); }
    }
    private string compid;
    public string CompID
    {
        get { return compid; }
        set { compid = value; }
    }
    public string Orderid
    {
        get { return hid_OrderID.Value; }
        set { orderid = value; }
    }
    public string Hid_Id
    {
        get { return hid_OrderID.ClientID; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(orderid))
            {
                object value = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(orderid)).ReceiptNo;
                if (value != null)
                {
                    this.txt_OrderCoder.Value = value.ToString();
                    this.hid_OrderID.Value = Orderid;
                }
            }
        }
    }
}