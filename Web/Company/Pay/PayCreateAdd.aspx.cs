using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.IO;

public partial class Company_Pay_PayCreateAdd : CompPageBase
{

    

    Hi.BLL.PAY_PaymentAccount PAbll = new Hi.BLL.PAY_PaymentAccount();

   public  string Uid = "0";
    protected void Page_Load(object sender, EventArgs e)
    {

        
        if (!IsPostBack)
        {
            //this.DisListID.CompID = this.CompID.ToString();

            if (Request.QueryString["UID"] == "")
            {
                Uid ="0";
            }
            else
            {
                Uid = Common.DesDecrypt(Request.QueryString["UID"], Common.EncryptKey);
            }
            //代理商赋值
            this.txtDisUser.InnerText = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(Uid)).DisName;
            this.hidDisUserID.Value = Uid;

            this.hidCompId.Value = this.CompID.ToString();
            Bind();
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
        //定义冲正的变量
        string txtPayCorrectDis = string.Empty;
        string txtPayCorrectPrice = string.Empty;
        //string txtPayCorrectDate = string.Empty;
        string txtPayCorrectType = string.Empty;
        string txtPayCorrectRemark = string.Empty;
        //获取补录金额，输入的数据       
        txtPayCorrectPrice =Common.NoHTML( this.txtPayCorrectPrice.Value.Trim());
        if (Convert.ToDecimal(txtPayCorrectPrice) <= 0)
        {
            JScript.AlertMethod(this, "操作失败,补录金额不能小于或等于零！", JScript.IconOption.错误, "function (){ location.replace(); }");//'" + ("PayCreateAdd.aspx?UID="+Uid) + "'
            return;
        }
        txtPayCorrectRemark =Common.NoHTML( this.txtRemark.Value.Trim());
        txtPayCorrectDis =Common.NoHTML( this.hidDisUserID.Value);// this.DisListID.Disid;//获取代理商ID
        //调用model,对属性进行赋值
        Hi.Model.PAY_PrePayment prepaymentmodel = new Hi.Model.PAY_PrePayment();
        prepaymentmodel.CompID =this.CompID;
        prepaymentmodel.PreType = 2;
        prepaymentmodel.vdef3 =this.ddltype.Value;//来源
        prepaymentmodel.DisID = Convert.ToInt32(txtPayCorrectDis);
        prepaymentmodel.Start = 1;
        prepaymentmodel.price = Convert.ToDecimal(txtPayCorrectPrice);
        prepaymentmodel.Paytime = DateTime.Now;
        prepaymentmodel.CreatDate = DateTime.Now;
        prepaymentmodel.CrateUser =this.UserID;
        prepaymentmodel.AuditState = 2;//2已审，0 未审
        prepaymentmodel.IsEnabled = 1;
        prepaymentmodel.ts = DateTime.Now;
        prepaymentmodel.dr =0;
        prepaymentmodel.modifyuser =this.UserID;
        prepaymentmodel.vdef1 = txtPayCorrectRemark;
        prepaymentmodel.vdef5 = hrOrderFj.Value;//保存附件的名称
        prepaymentmodel.vdef6 = ddlPaytype.Value;//支付方式
        //调用保存方法
        Hi.BLL.PAY_PrePayment prepaymentsave = new Hi.BLL.PAY_PrePayment();
        int reslut = prepaymentsave.Add(prepaymentmodel);
        //判断返回值int
        if (reslut > 0)
        {
            Utils.AddSysBusinessLog(this.CompID, "PrePayment", reslut.ToString(), "预收款补录新增", prepaymentmodel.vdef1);
           // JScript.AlertMsg(this, "操作成功", "PayCreateInfo.aspx?KeyID=" + reslut);
            Response.Redirect("PayCreateInfo.aspx?KeyID=" + Common.DesEncrypt(reslut.ToString(), Common.EncryptKey));
        }
        else
        {
            JScript.AlertMethod(this, "操作失败！", JScript.IconOption.错误, "function (){ location.replace(); }");//'" + ("PayCreateInfo.aspx") + "'
        }


    }
}