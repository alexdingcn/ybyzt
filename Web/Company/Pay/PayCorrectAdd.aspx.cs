using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;

public partial class Company_Pay_PayCorrectAdd : CompPageBase
{



    Hi.BLL.PAY_PaymentAccount PAbll = new Hi.BLL.PAY_PaymentAccount();

   public  string Uid = "0";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //this.DisListID.CompID = CompID.ToString();

            if (Request.QueryString["UID"] == "")
            {
                Uid = "0";
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
        string disid = Convert.ToString(Request.QueryString["disid"]);
        //if (!string.IsNullOrEmpty(disid))
        //    this.DisListID.Hid_Id = disid;
        decimal price = Convert.ToDecimal(Request.QueryString["price"]);
        if (price > 0)
            this.txtPayCorrectPrice.Value = price.ToString();
        string remark = Convert.ToString(Request.QueryString["remark"]);
        if (!string.IsNullOrEmpty(remark))
            this.txtRemark.Value = remark;
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
        //获取冲正输入的数据       
        txtPayCorrectPrice =Common.NoHTML( this.txtPayCorrectPrice.Value.Trim());
        if (Convert.ToDecimal(txtPayCorrectPrice) <= 0)
        {
            JScript.AlertMethod(this, "操作失败,冲正金额不能小于或等于零！", JScript.IconOption.错误, "function (){ location.replace(); }");//'" + ("PayCreateAdd.aspx?UID="+Uid) + "'
            return;
        }
        txtPayCorrectDis =Common.NoHTML( this.hidDisUserID.Value);// this.DisListID.Disid;//获取代理商ID
        txtPayCorrectRemark =Common.NoHTML( this.txtRemark.Value.Trim());//获取备注
        int compid = CompID;
        int disID = 0;
        if (this.CompUser.DisID != 0)
            disID = this.CompUser.DisID;
        //判断冲正金额是否大于企业钱包金额
        decimal nowprePrice = new Hi.BLL.PAY_PrePayment().sums(Convert.ToInt32(txtPayCorrectDis), compid);
        decimal czprice = Convert.ToDecimal(txtPayCorrectPrice);
        if (czprice > nowprePrice)
        {
            JScript.AlertMethod(this, "操作失败,冲正金额" + czprice + " 大于企业钱包余额" + nowprePrice.ToString("0.00"), JScript.IconOption.错误, "function (){ location.replace('" + ("PayCorrectAdd.aspx?UID=" + Common.DesEncrypt(txtPayCorrectDis.ToString(), Common.EncryptKey) ) + "'); }");
            return;
        }


        //调用model,对属性进行赋值
        Hi.Model.PAY_PrePayment prepaymentmodel = new Hi.Model.PAY_PrePayment();
        prepaymentmodel.CompID = compid;
        prepaymentmodel.PreType = 3;
        prepaymentmodel.DisID = Convert.ToInt32(txtPayCorrectDis);
        prepaymentmodel.Start = 1;
        prepaymentmodel.price = -Convert.ToDecimal(txtPayCorrectPrice);
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

            Utils.AddSysBusinessLog(this.CompID, "PrePayment", reslut.ToString(), "企业钱包冲正新增", "");
            // JScript.AlertMsg(this, "操作成功", "PayCorrectInfo.aspx?KeyID=" + reslut);
            Response.Redirect("PayCorrectInfo.aspx?KeyID=" + Common.DesEncrypt(reslut.ToString(), Common.EncryptKey) );
        }
        else
        {
            JScript.AlertMethod(this, "操作失败！", JScript.IconOption.哭脸, "function (){ location.replace('" + ("PayCorrectList.aspx") + "'); }");
        }


    }
}