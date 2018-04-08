using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;

public partial class Distributor_newOrder_printout : System.Web.UI.Page
{
    //发货单ID
    public int KeyID = 0;
    //代理商ID
    public int DisID = 0;
    //厂商ID
    public int CompID = 0;
    //订单下单数量保留小数位数
    public string Digits = "0";

    public string codeno = "";
    public string orderno = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            databind();
        }
    }

    public void databind()
    {

        if (!string.IsNullOrEmpty(Request["KeyID"]))
            KeyID = Common.DesDecrypt((Request["KeyID"] + ""), Common.EncryptKey).ToInt(0);
        if (!string.IsNullOrEmpty(Request["DisID"]))
            DisID = (Request["DisID"] + "").ToInt(0);
        
        //发货单
        DataTable lo = new Hi.BLL.DIS_OrderOut().GetList("", " isnull(o.dr,0)=0 and o.IsAudit<>3 and o.ID=" + KeyID);

        //已发货商品
        DataTable loud = new Hi.BLL.DIS_OrderOutDetail().GetOrderOutDe("", " isnull(od.dr,0)=0 and od.OrderOutID=" + KeyID);
       
        if (lo != null && lo.Rows.Count > 0)
        {

            int OrderID = lo.Rows[0]["OrderID"].ToString().ToInt(0);

            Hi.Model.DIS_Order omodel = new Hi.BLL.DIS_Order().GetModel(OrderID);
            if (omodel != null)
                orderno = omodel.ReceiptNo;

            if (Common.TypeID() == 1 || Common.TypeID() == 5)
            {
                //代理商 厂商
                name.InnerText = "供应商：";
                lblName.InnerText = Common.Getcom(lo.Rows[0]["CompID"].ToString().ToInt(), "CompName");
            }
            else
            {
                name.InnerText = "代理商：";
                lblName.InnerText = Common.GetDis(lo.Rows[0]["DisID"].ToString().ToInt(0), "DisName");
                
            }
            //司机姓名：张三 ， 司机手机：1391391386 ， 车牌号：苏A1234
            //this.Literal1.Text = barcode.get39(lo.Rows[0]["ReceiptNo"].ToString(), 1, 40);
            lblReceiptNo.InnerText = lo.Rows[0]["ReceiptNo"].ToString();
            codeno = lo.Rows[0]["ReceiptNo"].ToString();
           
            this.Image1.ImageUrl = "~/Distributor/newOrder/Code39.aspx?KeyID=" + codeno;

            lblArrDate.InnerText = lo.Rows[0]["SendDate"].ToString() != "" ? lo.Rows[0]["SendDate"].ToString().ToDateTime().ToString("yyyy-MM-dd") : "";
            if (lo.Rows[0]["CarUser"].ToString() != "")
                lblLogistics.InnerText = "司机姓名：" + lo.Rows[0]["CarUser"] + " ， 司机手机：" + lo.Rows[0]["CarNo"] + " ， 车牌号：" + lo.Rows[0]["Car"];

            if (loud != null && loud.Rows.Count > 0)
            {
                rptOrderD.DataSource = loud;
                rptOrderD.DataBind();
            }
        }

    }
   
}
