



using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_Order_OrderOutInfo : DisPageBase
{
    Hi.BLL.DIS_OrderOut OrderOutBll = new Hi.BLL.DIS_OrderOut();
    //Hi.Model.SYS_Users user = null;

    public int OrderId = 0;
    //public int DisId = 0;
    //public int KeyID = 0;
    //public int CompID = 0;
    public string ProID = "0";
    public string ProPrice = "";
    public string ProIDD = "0";
    public string ProType = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);

        //if (User != null)
        //{
        //    DisId = user.DisID;
        //    CompID = user.CompID;

            string Id = Common.DesDecrypt(Request["KeyID"].ToString(), Common.EncryptKey);
            KeyID = Id.ToInt(0);

            if (!IsPostBack)
            {
                Bind();
            }
        //}
    }

    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind()
    {
        if (KeyID != 0)
        {
            Hi.Model.DIS_OrderOut OrderOutModel = OrderOutBll.GetModel(KeyID);

            if (OrderOutModel != null)
            {
                OrderId = OrderOutModel.OrderID;
                
                this.lblReceiptNo.InnerText = OrderOutModel.ReceiptNo;
                this.lblOrderNo.InnerText = OrderInfoType.getOrder(OrderOutModel.OrderID, "ReceiptNo");
                DisID = OrderOutModel.DisID;

                ProID = OrderInfoType.getOrderExt(OrderOutModel.OrderID, "ProID");
                ProPrice = OrderInfoType.getOrderExt(OrderOutModel.OrderID, "ProAmount");
                ProIDD = OrderInfoType.getOrderExt(OrderOutModel.OrderID, "ProDID");
                ProType = OrderInfoType.getOrderExt(OrderOutModel.OrderID, "Protype");

                this.lblDisName.InnerText = Common.GetDis(OrderOutModel.DisID, "DisName");
                this.hidDisId.Value = OrderOutModel.DisID.ToString();

                this.lblSendDate.InnerText = OrderOutModel.SendDate == DateTime.MinValue ? "" : OrderOutModel.SendDate.ToString("yyyy-MM-dd");
                //this.lblExpress.InnerText = OrderOutModel.Express;
                //this.lblExpressNo.InnerText = OrderOutModel.ExpressNo;
                //this.lblExpressPerson.InnerText = OrderOutModel.ExpressPerson;
                //this.lblExpressTel.InnerText = OrderOutModel.ExpressTel;
                //this.lblExpressBao.InnerText = OrderOutModel.ExpressBao.ToString();
                //this.lblPostFee.InnerText = OrderOutModel.PostFee.ToString("N");
                this.lblActionUser.InnerText = OrderOutModel.ActionUser;
                this.lblRemark.InnerText = OrderOutModel.Remark;

                //签收信息
                this.lblIsSign.InnerText = OrderOutModel.IsSign == 0 ? "未签收" : "已签收";
                this.lblSignUser.InnerText = OrderOutModel.SignUser;
                this.hidSignUserId.Value = OrderOutModel.SignUserId.ToString();
                this.lblSignDate.InnerText = OrderOutModel.SignDate == DateTime.MinValue ? "" : OrderOutModel.SignDate.ToString("yyyy-MM-dd");
                this.lblSignRemark.InnerText = OrderOutModel.SignRemark;

                BindOrderDetail(OrderOutModel.OrderID, OrderOutModel.DisID);
            }
        }
    }


    /// <summary>
    /// 绑定订单明细
    /// </summary>
    public void BindOrderDetail(int OrderId, int DisID)
    {
        SelectGoods.Clear();
        SelectGoods.OrderDetail(OrderId, DisID, this.CompID);
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