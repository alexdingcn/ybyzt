using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using CFCA.Payment.Api;
using System.Text;
using DBUtility;
using System.Data.SqlClient;

public partial class Distributor_Pay_PrePayInfo :DisPageBase
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    Hi.BLL.PAY_PrePayment PAbll = new Hi.BLL.PAY_PrePayment();
    public static bool Auditstatr = false;
    public int KeyID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString["KeyID"] == "")
        {
            KeyID = 0;
        }
        else
        {
            KeyID = Convert.ToInt32(Common.DesDecrypt(Request.QueryString["KeyID"].ToString(), Common.EncryptKey));
        }
        if (!Common.PageDisOperable("PayPre", KeyID, this.DisID))
        {
            Response.Redirect("../../NoOperable.aspx", true);
            return;
        }
        if (!IsPostBack)
        {
            Bind();
        }
    }

    public void Bind()
    {
        if (KeyID > 0)
        {

            Hi.Model.PAY_PrePayment Ppmodel = PAbll.GetModel(KeyID);
            this.lbldis.InnerText = Common.GetDis(Ppmodel.DisID, "DisName");
            this.lblcreatetime.InnerText = Convert.ToDateTime(Ppmodel.CreatDate).ToString("yyyy-MM-dd");
            //this.lblauditstate.InnerText = Common.GetNameBYPreStart(Ppmodel.AuditState);
            this.lblcreateuser.InnerText = Common.GetUserName(Ppmodel.CrateUser);
            this.lblprice.InnerText = Convert.ToDecimal(Ppmodel.price).ToString("0.00");
            this.lblpaytype.InnerText = Common.GetPrePayStartName(Ppmodel.PreType);
            this.lblRemark.InnerText = Ppmodel.vdef1;
            //this.lbljs.InnerText = Ppmodel.vdef2 == "1" ? "已结算" : "未结算";
            //this.Audit.Visible = Ppmodel.AuditState == 2 ? false : true;

        }

        
    }
}