using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Company_Report_CompOrderInfo_ZD : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    private void Bind()
    {
        if (KeyID != 0)
        {


            string sql = "SELECT * FROM [dbo].[CompCollection_view] where  CompID=" + this.CompID + " and ID=" + KeyID;

            DataTable ds = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];

            if (ds != null && ds.Rows.Count > 0 )
            {
                int orderid = Convert.ToInt32(ds.Rows[0]["orderID"]);
                Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(orderid);

                this.lblReceiptNo.InnerText = orderModel.ReceiptNo.ToString();
                this.lblOState.InnerText = OrderInfoType.OState(orderid);
                this.lblPayState.InnerText = OrderInfoType.PayState(orderModel.PayState);
                this.lblTotalPrice.InnerText = orderModel.AuditAmount.ToString("N");
                this.lblPayedPrice.InnerText = ds.Rows[0]["Price"].ToString().ToDecimal(0).ToString("N");
                this.lblPayAuomet.InnerText = orderModel.PayedAmount.ToString("N");
                if (ds.Rows[0]["Date"] != null && ds.Rows[0]["Date"].ToString() != "")
                    this.lblArriveDate.InnerText = Convert.ToDateTime(ds.Rows[0]["Date"]).ToString("yyyy-MM-dd");
                this.lblDisUser.InnerText = Common.GetUserName(orderModel.DisUserID);
                this.lblCreateDate.InnerText = Convert.ToDateTime(orderModel.CreateDate).ToString("yyyy-MM-dd");

                this.lblPaySource.InnerText = ds.Rows[0]["Source"].ToString();

            }
        }
    }
}