using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_Pay_TransferAdd :DisPageBase
{
    public Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
    public Hi.BLL.PAY_PrePayment prepayBll = new Hi.BLL.PAY_PrePayment();

    protected void Page_Load(object sender, EventArgs e)
    {
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
            string guid = Common.NoHTML(this.txtGuid.Value);
            string price = Common.NoHTML(this.txtPrice.Value);
            string remark = Common.NoHTML(this.txtRemark.Value);
            prepayM.CompID = this.CompID;
            prepayM.DisID = this.DisID;
            prepayM.OrderID = 0;
            prepayM.Start = 2;
            prepayM.PreType = 6;
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
            int id = prepayBll.Add(prepayM);
            Utils.AddSysBusinessLog(this.CompID, "PrePayment", id.ToString(), "转账汇款新增", "");
            //Response.Redirect("Recharge.aspx?KeyID=" + Common.DesEncrypt(id.ToString(),Common.EncryptKey));
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>window.open('Recharge.aspx?KeyID=" + Common.DesEncrypt(id.ToString(), Common.EncryptKey) + "','transfer');</script>");
        }
        catch (Exception ex)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "", "<script>window.open('Error.aspx?msg=" + ex.Message + "','transfer');</script>");
        }
    }
}