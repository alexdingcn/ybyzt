using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_Pay_remittanceAdd : DisPageBase
{
    public Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
    public Hi.BLL.PAY_PrePayment prepayBll = new Hi.BLL.PAY_PrePayment();
    public string PrePrice = "0";


    protected void Page_Load(object sender, EventArgs e)
    {
        PrePrice = new Hi.BLL.PAY_PrePayment().sums(this.DisID, this.CompID).ToString("0.00");
        if (!IsPostBack)
        {
            Bind();
        }
    }

    public void Bind()
    {
        string compName = new Hi.BLL.BD_Company().GetModel(this.CompID).CompName;
        this.compName.InnerHtml = compName;
    }

    protected void Btn_Recharge(object sender, EventArgs e)
    {
        try
        {
            string guid = this.txtGuid.Value;
            string price = this.txtPrice.Value;
            string remark = Common.NoHTML( this.txtRemark.Value);
            prepayM.CompID = this.CompID;
            prepayM.DisID = this.DisID;
            prepayM.OrderID = 0;
            prepayM.Start = 2;
            prepayM.PreType = 1;
            prepayM.price = Convert.ToDecimal(price);
            prepayM.Paytime = DateTime.Now;
            prepayM.CreatDate = DateTime.Now;
            prepayM.OldId = 0;
            prepayM.CrateUser = this.UserID;
            prepayM.AuditState = 2;
            prepayM.AuditUser = 0;
            prepayM.IsEnabled = 1;
            prepayM.ts = DateTime.Now;
            prepayM.modifyuser = this.UserID;
            prepayM.vdef1 = remark;
            prepayM.guid = Common.Number_repeat(Guid.NewGuid().ToString().Replace("-", ""));
            int id = prepayBll.Add(prepayM);
            Utils.AddSysBusinessLog(this.CompID, "PrePayment", id.ToString(), "企业钱包充值", "");
            // Response.Redirect("Recharge.aspx?KeyID=" + Common.DesEncrypt(id.ToString(), Common.EncryptKey));

            //string str = Request.Url.Scheme + "://" + Request.Url.Host + ":" + Request.Url.Port;
          

            //ATrance.HRef = "Recharge.aspx?KeyID=" + Common.DesEncrypt(id.ToString(), Common.EncryptKey) + "";
           // ClientScript.RegisterStartupScript(this.GetType(), "", "<script>window.open('Recharge.aspx?KeyID=" + Common.DesEncrypt(id.ToString(), Common.EncryptKey) + "','transfer');</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script> var win=window.open('Recharge.aspx?KeyID=" + Common.DesEncrypt(id.ToString(), Common.EncryptKey) + "','transfer'); if(!win){window.location.href ='Recharge.aspx?KeyID=" + Common.DesEncrypt(id.ToString(), Common.EncryptKey)+"';}</script>");
           // ClientScript.RegisterStartupScript(this.GetType(), "", "<script>document.getElementById('" + ATrance.ClientID + "').click();$('#" + ATrance.ClientID + "').attr('href','');</script>");
        
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>window.open('Error.aspx?msg=" + ex.Message + "','transfer');</script>");
        }
    }
}