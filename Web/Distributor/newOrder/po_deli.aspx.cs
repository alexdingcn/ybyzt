using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_newOrder_po_deli : System.Web.UI.Page
{

    //订单ID
    public int KeyID = 0;
    //代理商ID
    public int DisID = 0;
    //修改的类型  0、交货日期  1、配送方式
    public int type = 0;
    public string title = "交货日期";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["type"]))
                type = Request["type"].ToString().ToInt(0);

            GiveMode.Visible = false;
            senddate.Visible = false;

            if (type == 0)
            {
                senddate.Visible = true;
                title = "交货日期";
            }
            else
            {
                GiveMode.Visible = true;
                title = "配送方式";
            }
            this.hidType.Value = type.ToString();

            databind();
        }
    }

    public void databind()
    {
        if (!string.IsNullOrEmpty(Request["KeyID"]))
            KeyID =Common.DesDecrypt(Request["KeyID"].ToString(),Common.EncryptKey).ToInt(0);

        var tip = "";
        if (!string.IsNullOrEmpty(Request["tipval"]))
            tip = Request["tipval"].ToString();
        if (type == 0)
            this.txtDate.Value = tip.Trim();
        else
        {
            if (tip == "送货")
                checkbox_5_1.Checked = true;
            else
                checkbox_5_2.Checked = true;
        }
    }
     [WebMethod]
    public static string Edit(string KeyID, string type, string tip) 
    {
        
        Common.ResultMessage Msg = new Common.ResultMessage();
        Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
        Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(Common.DesDecrypt(KeyID, Common.EncryptKey).ToInt(0));
        if (OrderModel != null)
        {
            if (OrderModel.OState == -1 || OrderModel.OState == 0 || OrderModel.OState == 1 || OrderModel.OState == 2)
            {
                if (type == "0")
                {
                    if (tip != "")
                        OrderModel.ArriveDate = tip.ToDateTime();
                    else
                        OrderModel.ArriveDate = DateTime.MinValue;
                    OrderModel.ts = DateTime.Now;
                    if (OrderBll.Update(OrderModel))
                    {
                        Msg.result = true;
                    }
                }

                else
                {
                    OrderModel.GiveMode = tip;
                    OrderModel.ts = DateTime.Now;
                    if (OrderBll.Update(OrderModel))
                    {
                        Msg.result = true;
                    }
                }
            }
        }
        else
        {
            Msg.code = "未查找到数据";
        }
        return new JavaScriptSerializer().Serialize(Msg);
    }
}