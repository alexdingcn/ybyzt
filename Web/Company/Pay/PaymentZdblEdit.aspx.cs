using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;

public partial class Company_Pay_PaymentZdblEdit : CompPageBase
{

    Hi.BLL.PAY_PaymentAccount PAbll = new Hi.BLL.PAY_PaymentAccount();


    protected void Page_Load(object sender, EventArgs e)
    {


        if (!IsPostBack)
        {
            hid_start.Value = Request.QueryString["start"] + "";
            int state = Convert.ToInt32(Request.QueryString["start"]);
            if (state == 2 || state == 0)
            {
                btnSave.Text = "发货";
            }
            else if (state == 1)
            {

                btnSave.Text = "销账";
            }

            Hi.Model.DIS_Order OrdModel = new Hi.Model.DIS_Order();
            OrdModel = new Hi.BLL.DIS_Order().GetModel(KeyID);

            this.txtdisname.InnerText = new Hi.BLL.BD_Distributor().GetModel(OrdModel.DisID).DisName;
            this.hidDisId.Value = OrdModel.DisID.ToString();
            this.txtordercode.InnerText = OrdModel.ReceiptNo;
            this.hidordid.Value = OrdModel.ID.ToString();
            this.txtPayCorrectPrice.InnerText = (OrdModel.AuditAmount - OrdModel.PayedAmount).ToString("0.00");
            this.hidCompId.Value = this.CompID.ToString();

            //Bind();
        }
    }

    protected void Bind()
    {
        //if(KeyID>0)
        //{
        //    Hi.BLL.PAY_PaymentAccount PAbll = new Hi.BLL.PAY_PaymentAccount();
        //    Hi.Model.PAY_PaymentAccount PAmodel = PAbll.GetModel(KeyID);

        //    this.txtcompID.Value = "张三";
        //    this.hidcompID.Value = "1";
        //    this.txtqy.ID = PAmodel.Region;
        //    this.txtpaycode.Value = PAmodel.PayCode;
        //    this.txtpayname.Value = PAmodel.payName;
        //    this.ddltype.Value = PAmodel.type.ToString();
        //    this.txtRemark.Value = PAmodel.Remark;
        // }
    }


    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {

        Hi.Model.DIS_Order orderone = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(this.hidordid.Value));
        if (orderone.vdef1 == "1")
        {
            JScript.AlertMethod(this, "此账单已补录！", JScript.IconOption.错误, "function (){ location.replace('" + ("PayOrderList.aspx") + "'); }");
        }
        else
        {

            //定义变量
            string txtPayCorrectDis = string.Empty;
            string txtPayCorrectPrice = string.Empty;
            //string txtPayCorrectDate = string.Empty;
            string txtPayCorrectType = string.Empty;
            string txtPayCorrectRemark = string.Empty;
            //获取收款补录输入的数据       
            txtPayCorrectPrice = this.txtPayCorrectPrice.InnerText;
            if (Convert.ToDecimal(txtPayCorrectPrice) <=0)
            {
                JScript.AlertMethod(this, "操作失败,补录金额不能小于或等于零！", JScript.IconOption.错误, "function (){ location.replace(); }");//'" + ("PayCreateAdd.aspx?UID="+Uid) + "'
                return;
            }


            txtPayCorrectRemark = this.txtRemark.Value.Trim();
            txtPayCorrectDis = this.hidDisId.Value;//获取代理商ID
            //调用model,对属性进行赋值
            Hi.Model.PAY_PrePayment prepaymentmodel = new Hi.Model.PAY_PrePayment();
            prepaymentmodel.CompID = this.CompID;
            prepaymentmodel.OrderID = Convert.ToInt32(this.hidordid.Value);
            prepaymentmodel.PreType = 8;//账单收款补录
            prepaymentmodel.vdef3 = this.ddltype.Value;
            prepaymentmodel.DisID = Convert.ToInt32(txtPayCorrectDis);
            prepaymentmodel.Start = 1;
            prepaymentmodel.price = Convert.ToDecimal(txtPayCorrectPrice);
            prepaymentmodel.Paytime = DateTime.Now;
            prepaymentmodel.CreatDate = DateTime.Now;
            prepaymentmodel.CrateUser = this.UserID;
            prepaymentmodel.AuditState = 2;//2已审，0 未审
            prepaymentmodel.IsEnabled = 1;
            prepaymentmodel.ts = DateTime.Now;
            prepaymentmodel.dr = 0;
            prepaymentmodel.modifyuser = this.UserID;
            prepaymentmodel.vdef1 = txtPayCorrectRemark;
            //调用保存方法
            Hi.BLL.PAY_PrePayment prepaymentsave = new Hi.BLL.PAY_PrePayment();
            int reslut = prepaymentsave.Add(prepaymentmodel);
            //判断返回值int
            if (reslut > 0)
            {
                Utils.AddSysBusinessLog(this.CompID, "Order", this.hidordid.Value, "账单收款补录新增", prepaymentmodel.vdef1);

                //修改订单为已支付
                Hi.Model.DIS_Order modelorder = new Hi.Model.DIS_Order();
                modelorder = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(this.hidordid.Value));
                modelorder.PayedAmount = Convert.ToDecimal(txtPayCorrectPrice);
                modelorder.PayState = 2;
                bool fal = new Hi.BLL.DIS_Order().Update(modelorder);
                if (fal)
                {
                    string ordid = this.hidordid.Value;
                    Utils.AddSysBusinessLog(this.CompID, "Order", ordid, "账单核销", "");
                    //int state = Convert.ToInt32(hid_start.Value);
                    //if (state == 2 || state == 0)
                    //{
                    //    //补充发货记录
                    //    Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(ordid));
                    //    OrderInfoType.ShopOrder(OrderModel);
                    //    //发货日志+推送
                    //    Utils.AddSysBusinessLog(this.CompID, "Order", ordid, "订单发货", "");
                    //    new Common().GetWxService("0", "4", ordid);//消息推送

                    //}

                    //修改补录标示
                    Hi.Model.DIS_Order OrderModel1 = new Hi.BLL.DIS_Order().GetModel(Convert.ToInt32(ordid));
                    OrderModel1.vdef1 = "1";
                    new Hi.BLL.DIS_Order().Update(OrderModel1);

                    //跳转
                    Response.Redirect("../Order/OrderZdtsInfo.aspx?KeyID=" + ordid + "&go=1");

                }



            }
            else
            {
                JScript.AlertMethod(this, "操作失败！", JScript.IconOption.错误, "function (){ location.replace('" + ("PaymentInfo.aspx&type=0") + "'); }");
            }

        }
    }
}