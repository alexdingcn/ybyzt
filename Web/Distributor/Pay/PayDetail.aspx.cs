using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Distributor_Pay_PayDetail : System.Web.UI.Page
{
    public int KeyID = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserModel"] is LoginModel)
        {
            if (Request.QueryString["KeyID"] == "")
            {
                KeyID = 0;
            }
            else
            {
                KeyID = Convert.ToInt32(Common.DesDecrypt(Request.QueryString["KeyID"].ToString(), Common.EncryptKey));
            }

            if (!IsPostBack)
            {
                Bind();
            }
        }
    }
    public void Bind()
    {
        DataTable PayDetail = new Hi.BLL.PAY_PrePayment().GetPayedItem(KeyID);
        this.rptPayDetail.DataSource = PayDetail;
        this.rptPayDetail.DataBind();
    }
}