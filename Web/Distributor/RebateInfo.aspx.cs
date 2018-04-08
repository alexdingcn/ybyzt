using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CFCA.Payment.Api;
using DBUtility;

public partial class Distributor_RebateInfo : DisPageBase
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    Hi.BLL.BD_Rebate OrderBll = new Hi.BLL.BD_Rebate();
    Hi.BLL.BD_RebateDetail OrderDetailBll = new Hi.BLL.BD_RebateDetail();

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        if (KeyID != 0)
        {
            Hi.Model.BD_Rebate OrderModel = OrderBll.GetModel(KeyID);

            if (OrderModel != null)
            {
                this.lblOtype.InnerText = OrderModel.RebateType == 1 ? "整单返利" : "分摊返利";
                this.lblReceiptNo.InnerText = OrderModel.ReceiptNo;
                this.lblRemark.InnerText = OrderModel.Remark;

                this.lblCreateDate.InnerText = OrderModel.UserdAmount.ToString("N");
                this.lblAddType.InnerText = OrderModel.RebateState == 1 || OrderModel.EndDate < DateTime.Now ? "有效" : "失效";
                this.lblTotalPrice.InnerText = OrderModel.RebateAmount.ToString("N");
                this.lblPayedPrice.InnerText = OrderModel.EnableAmount.ToString("N");
                this.lblArriveDate.InnerText = OrderModel.StartDate.ToString("yyyy-MM-dd") + "至" + OrderModel.EndDate.ToString("yyyy-MM-dd");

                BindOrderDetail(OrderModel.DisID);
            }
            else
            {
                this.rpDtl.DataSource = "";
                this.rpDtl.DataBind();
            }
        }
        else
        {
            Response.Redirect("../NoOperable.aspx");
            return;
        }
    }

    /// <summary>
    /// 绑定订单明细
    /// </summary>
    public void BindOrderDetail(int DisID)
    {
        List<Hi.Model.BD_RebateDetail> list = OrderDetailBll.GetList("", " RebateID = " + KeyID + " and IsNull(dr,0) = 0 ", " createdate desc");

        if (list != null)
        {
            this.rpDtl.DataSource = list;
            this.rpDtl.DataBind();
        }
        else
        {
            this.rpDtl.DataSource = "";
            this.rpDtl.DataBind();
        }
        SelectGoods.Clear(DisID, this.CompID);
    }

}