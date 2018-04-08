using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Distributor_newOrder_printorder : System.Web.UI.Page
{
    //订单ID
    public int KeyID = 0;
    //代理商ID
    public int DisID = 0;
    //厂商ID
    public int CompID = 0;
    //订单下单数量保留小数位数
    public string Digits = "0";
    public string codeno = "";
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

        string where = "and isnull(o.dr,0)=0 and o.otype<>9 and o.DisID=" + DisID + " and o.ID= " + KeyID;

        DataTable dt = new Hi.BLL.DIS_Order().GetList("", where);
    
        if (dt != null && dt.Rows.Count > 0)
        {
            //this.Literal1.Text= barcode.get39(dt.Rows[0]["ReceiptNo"].ToString(), 1, 40);
            
           
            Digits = OrderInfoType.rdoOrderAudit("订单下单数量是否取整", dt.Rows[0]["CompID"].ToString().ToInt(0));

            // 订单编号
            lblReceiptNo.InnerText = dt.Rows[0]["ReceiptNo"].ToString();
            codeno = dt.Rows[0]["ReceiptNo"].ToString();
            this.Image1.ImageUrl = "~/Distributor/newOrder/Code39.aspx?KeyID=" + codeno;
            lblCreateDate.InnerText = dt.Rows[0]["CreateDate"].ToString().ToDateTime().ToString("yyyy-MM-dd");
            lblArrDate.InnerText = dt.Rows[0]["ArriveDate"].ToString() == "" ? "" : dt.Rows[0]["ArriveDate"].ToString().ToDateTime().ToString("yyyy-MM-dd");

            //订单流程
            lblTotalAmount.InnerText = dt.Rows[0]["TotalAmount"].ToString() == "" ? "0.00" : dt.Rows[0]["TotalAmount"].ToString().ToDecimal().ToString("0.00");
            lblProAmount.InnerText = dt.Rows[0]["ProAmount"].ToString() == "" ? "0.00" : dt.Rows[0]["ProAmount"].ToString().ToDecimal().ToString("0.00");

            lblbateAmount.InnerText = dt.Rows[0]["bateAmount"].ToString() == "" ? "0.00" : dt.Rows[0]["bateAmount"].ToString().ToDecimal().ToString("0.00");


            lblPostFee.InnerText = dt.Rows[0]["PostFee"].ToString() == "" ? "0.00" : dt.Rows[0]["PostFee"].ToString().ToDecimal().ToString("0.00");
            lblAuditAmount.InnerText = dt.Rows[0]["AuditAmount"].ToString() == "" ? "0.00" : dt.Rows[0]["AuditAmount"].ToString().ToDecimal().ToString("0.00");
            //代理商
            if (Common.TypeID()==1|| Common.TypeID() == 5)
            {
                name.InnerText = "厂商：";
                lblDisName.InnerText = Common.Getcom(dt.Rows[0]["CompID"].ToString().ToInt(), "CompName");
            }
            else {
                name.InnerText = "代理商：";
                lblDisName.InnerText = Common.GetDis(dt.Rows[0]["DisID"].ToString().ToInt(0), "DisName");
            }
            //下单信息
            //lblArriveDate.InnerText = dt.Rows[0]["ArriveDate"].ToString().ToDateTime() == DateTime.MinValue ? "" : dt.Rows[0]["ArriveDate"].ToString().ToDateTime().ToString("yyyy-MM-dd");
            lblGiveMode.InnerText = dt.Rows[0]["GiveMode"].ToString();

            iRemark.InnerText = dt.Rows[0]["Remark"].ToString();

            if (dt.Rows[0]["AddrID"].ToString() != "")
            {
                //收货地址
                lblPrincipal.InnerText = dt.Rows[0]["Principal"].ToString();
                lblPhone.InnerText = dt.Rows[0]["Phone"].ToString();
                lblAddress.InnerText = dt.Rows[0]["Address"].ToString();
            }
            else
                iaddr.InnerText = "无";

            //开票信息
            //if (dt.Rows[0]["IsOBill"].ToString().ToInt(0) != 1)
            //{
            //    lblRise.InnerText = dt.Rows[0]["Rise"].ToString();
            //    lblContent.InnerText = dt.Rows[0]["Content"].ToString();
            //    lblOBank.InnerText = dt.Rows[0]["OBank"].ToString();
            //    lblOAccount.InnerText = dt.Rows[0]["OAccount"].ToString();
            //    lblTRNumber.InnerText = dt.Rows[0]["TRNumber"].ToString();
            //}
            //else
            //    this.iInvoice.InnerHtml = "不开发票";


            //发票信息
            //lblBillNo.InnerText = dt.Rows[0]["BillNo"].ToString();
            //lblIsBill.InnerText = dt.Rows[0]["IsBill"].ToString() == "1" ? "是" : "否";

            //订单所有商品明细
            DataTable l = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", " IsNUll(o.dr,0)=0 and o.OrderId=" + KeyID);
            if (l != null && l.Rows.Count > 0)
            {
                rptOrderD.DataSource = l;
                rptOrderD.DataBind();
            }
        }
    }

    public string protitle(string ProID, string Protype, string unit)
    {
        string str = "";
        if (ProID != "")
        {
            if (!string.IsNullOrEmpty(Protype))
            {
                string[] type = Protype.Split(new char[] { ',' });
                str = ConvertJson.IsCx(type[0], type[1], type[2], type[3], unit);

                if (!Util.IsEmpty(type[0]))
                {
                    if (type[0] == "0")//特价促销
                    {
                        str = "特价商品";
                    }
                    else if (type[0] == "1")//商品促销
                    {
                        if (type[1] == "3")
                        { //商品促销满送
                            str = "满" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(type[3]).ToString("#,##" + Digits))) + unit + " ，获赠商品（" + decimal.Parse(string.Format("{0:N2}", Convert.ToDecimal(type[2]).ToString("#,##" + Digits))) + "）" + unit;
                        }
                        else if (type[1] == "4")//商品促销打折
                        {
                            str = "在原订货价基础上打" +(Convert.ToDecimal(type[3]) / 10).ToString("N") + "折";
                        }
                    }
                }
            }
        }

        if (!string.IsNullOrEmpty(str))
            str = "<i class=\"\">促销：</i>" + str + "";
        return str;
    }
}