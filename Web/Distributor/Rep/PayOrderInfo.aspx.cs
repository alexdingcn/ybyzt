

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Distributor_Rep_PayOrderInfo :DisPageBase
{
    //Hi.Model.SYS_Users user = null;
    //int KeyID = 0;
    int PreType = -1;
    int orderid = -1;

    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);

        if (!IsPostBack)
        {
            //if (Request["KeyID"] != null)
            //{
            //    string Id = Common.DesDecrypt(Request["KeyID"].ToString(), Common.EncryptKey);
            //    KeyID = Id.ToInt(0);
            //}
            //支付类型
            if (Request["PreType"] != null)
                PreType =Convert.ToInt32(Common.DesDecrypt(Request["PreType"].ToString(), Common.EncryptKey));
            //订单Id
            if (Request["orderid"] != null)
                 orderid = Convert.ToInt32(Common.DesDecrypt(Request["orderid"].ToString(), Common.EncryptKey));

            Bind();
        }
    }

    private void Bind()
    {
        if (KeyID != 0)
        {
            Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(orderid);

            string sql = "SELECT * FROM [dbo].[CompCollection_view] where PreType=" + PreType + " and ID=" + KeyID;

            DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

            if (ds != null && ds.Rows.Count > 0 && orderModel != null)
            {
                this.lblReceiptNo.InnerText = orderModel.ReceiptNo.ToString();

                this.lblOState.InnerText = OrderInfoType.OState(orderModel.ID);
                this.lblPayState.InnerText = OrderInfoType.PayState(orderModel.PayState);

                this.lblTotalPrice.InnerText = orderModel.AuditAmount.ToString("N");
                this.lblPayedPrice.InnerText = ds.Rows[0]["Price"].ToString().ToDecimal(0).ToString("N");
                this.lblPayAuomet.InnerText = orderModel.PayedAmount.ToString("N");
                this.lblArriveDate.InnerText = Convert.ToDateTime(ds.Rows[0]["Date"]).ToString("yyyy-MM-dd");
                this.lblDisUser.InnerText = Common.GetUserName(orderModel.DisUserID);
                this.lblCreateDate.InnerText = Convert.ToDateTime(orderModel.CreateDate).ToString("yyyy-MM-dd");

                this.lblPaySource.InnerText = ds.Rows[0]["Source"].ToString();
                
            }


        }
    }
}