using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.Script.Serialization;
using System.Web.Configuration;

public partial class Company_Pay_payAccountAdd : CompPageBase
{
    Hi.BLL.PAY_PaymentAccount PAbll = new Hi.BLL.PAY_PaymentAccount();
 
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            txtqy.CompID = CompID.ToString();
            Bind();
        }
    }

    protected void Bind()
    {
        //this.txtComp.Value = new Hi.BLL.BD_Company().GetModel(CompID).CompName;
        //this.txtComp.Disabled = true;
        if (KeyID > 0)
        {

            Hi.Model.PAY_PaymentAccount PAmodel = PAbll.GetModel(KeyID);
            //string str
            //this.txtcompID.Id ="";
            //this.txtcompID.Compid = Convert.ToString(PAmodel.CompID);
            //this.hidtxtcompid.Value = PAmodel.CompID.ToString();
            
            //this.txtorgcode.Value = PAmodel.OrgCode;
            this.txtqy.Id = PAmodel.Region.ToString();
            //this.txtpaycode.Value = PAmodel.PayCode;
            //this.txtpayname.Value = PAmodel.payName;
            this.ddltype.Value = PAmodel.type.ToString();
            this.txtRemark.Value = PAmodel.Remark;
        }
    }


    /// <summary>
    /// 保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hi.Model.PAY_PaymentAccount PAModel = new Hi.Model.PAY_PaymentAccount();
        int ddltype=Convert.ToInt32(this.ddltype.Value);
        if (KeyID > 0)
        {
            PAModel.CompID = CompID;//Convert.ToInt32(this.txtcompID.Compid == "" ? this.hidtxtcompid.Value : this.txtcompID.Compid);

            PAModel.OrgCode = WebConfigurationManager.AppSettings["PayOrgCode"];
            //PAModel.payName = this.txtpayname.Value.Trim();
            //PAModel.PayCode = this.txtpaycode.Value.Trim();
            PAModel.Region = Convert.ToString(this.txtqy.Id);
            PAModel.PayType = 1;
            PAModel.type = ddltype;
            PAModel.Remark = this.txtRemark.Value.Trim();
            PAModel.modifyuser = this.UserID;
            PAModel.vdef1 = string.Empty;
            PAModel.vdef2 = string.Empty;
            PAModel.vdef3 = string.Empty;
            PAModel.vdef4 = string.Empty;
            PAModel.vdef5 = string.Empty;
            PAModel.ID = KeyID;
            bool reslet = PAbll.Update(PAModel);
            if (reslet)
            {
                if (ddltype == 12 || ddltype == 11)
                {
                    JScript.AlertMethod(this, "操作成功,接下来请添加绑定银行卡！", JScript.IconOption.笑脸, "function (){ location.replace('" + ("PayAccountInfo.aspx?KeyID=" + KeyID) + "'); }");
                }
                else
                    Response.Redirect("PayAccountInfo.aspx?KeyID=" + KeyID);
                    //JScript.AlertMsg(this, "操作成功！", "PayAccountInfo.aspx?KeyID=" + KeyID);
            }
        }
        else
        {


            PAModel.CompID = CompID; //Convert.ToInt32(this.txtcompID.Compid == "" ? this.hidtxtcompid.Value : this.txtcompID.Compid);
            PAModel.Isno = 1;
            PAModel.OrgCode = WebConfigurationManager.AppSettings["PayOrgCode"];
            PAModel.payName = "";//this.txtpayname.Value.Trim();
            PAModel.PayCode = "";// this.txtpaycode.Value.Trim();
            PAModel.Region = Convert.ToString(this.txtqy.Id);
            PAModel.PayType = 1;
            PAModel.type = Convert.ToInt32(this.ddltype.Value);
            PAModel.Remark = this.txtRemark.Value.Trim();
            PAModel.Start = 1;
            PAModel.CreateUser = this.UserID;
            PAModel.CreateDate = DateTime.Now;
            PAModel.ts = DateTime.Now;
            PAModel.dr = 0;
            PAModel.modifyuser = this.UserID;
            PAModel.vdef1 = string.Empty;
            PAModel.vdef2 = string.Empty;
            PAModel.vdef3 = string.Empty;
            PAModel.vdef4 = string.Empty;
            PAModel.vdef5 = string.Empty;
            int succes = PAbll.Add(PAModel);
            if (succes > 0)
            {
                //Utils.AddSysBusinessLog(this.CompID, "Order", OrderId.ToString(), "订单新增", "");
                //JScript.AlertMsg(this, "操作成功！", "PayAccountInfo.aspx?KeyID=" + succes);
                Response.Redirect("PayAccountInfo.aspx?KeyID=" + succes);
            }

        }

    }
}