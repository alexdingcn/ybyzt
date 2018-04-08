using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

/// <summary>
/// 打印订单
/// </summary>
public partial class Company_Order_OrderPrint : CompPageBase
{
    Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
    private int disID;

    public int DisID
    {
        get { return disID; }
        set { disID = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
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
            if (!Common.PageCompOperable("Order", KeyID, CompID))
            {
                Response.Redirect("../../NoOperable.aspx");
                return;
            }

            Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(KeyID);

            if (OrderModel != null)
            {
                DisID = OrderModel.DisID;

                this.PrintDate.InnerText = DateTime.Now.ToString("yyyy年MM月dd日");
                this.lblReceiptNo.InnerText = OrderModel.ReceiptNo;
                this.lblDisName.InnerText = Common.GetDis(OrderModel.DisID, "DisName");

                this.lblCreateDate.InnerText = OrderModel.CreateDate == DateTime.MinValue ? "" : OrderModel.CreateDate.ToString("yyyy-MM-dd");
                this.lblDisUser.InnerText = Common.GetUserName(OrderModel.DisUserID);

                this.lblOtype.InnerText = OrderInfoType.OType(OrderModel.Otype);
                this.lblTotalPrice.InnerText = OrderModel.AuditAmount.ToString("N");

                this.lblPayState.InnerText = OrderInfoType.PayState(OrderModel.PayState);
                this.lblPayPrice.InnerText = OrderModel.PayedAmount.ToString("N");
                if (OrderModel.PayState == (int)Enums.PayState.未支付)
                    this.lblPayState.Attributes.Add("style", "Color:Red");
                else
                    this.lblPayState.Attributes.Add("style", "Color:green");


                this.lblAddr.InnerText = Common.GetAddr(OrderModel.AddrID);
                this.lblRemark.InnerText = OrderModel.Remark;

                BindOrderDetail(DisID);
            }
        }
    }

    /// <summary>
    /// 绑定订单明细
    /// </summary>
    public void BindOrderDetail(int DisID)
    {
        SelectGoods.Clear(this.CompID);
        SelectGoods.OrderDetail(KeyID, DisID, this.CompID);
        DataTable dt = Session["GoodsInfo"] as DataTable;
        if (dt != null)
        {
            this.rpDtl.DataSource = dt;
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