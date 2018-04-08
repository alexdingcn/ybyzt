using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Company_Order_OrderZdtsInfo : CompPageBase
{
    Hi.BLL.DIS_Order OrderInfoBll = new Hi.BLL.DIS_Order();
    Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();

    public int DisId = 0;
    public string page = "1";//默认初始页

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Common.PageCompOperable("Order", KeyID, CompID))
        {
            Response.Redirect("~/NoOperable.aspx");
            return;
        }

        if (!IsPostBack)
        {
            Bind();
        }
    }

    public void Bind()
    {
        //取消按钮显示

        if (KeyID > 0)
        {

            Hi.Model.DIS_Order OrderInfoModel = OrderInfoBll.GetModel(KeyID);
            //if (Request["showtype"] == "PayZdblList")
            //{
            //    zd.InnerText = "账单查询详细";
            //}
            //if (Request["showtype"] == "PaymentZdblcxList")
            //{
            //    //zd.InnerText = "账单补录详细";
            //}
            string PostType = string.Empty;
            if (Request["PostType"] != null)
            {
                PostType = Request["PostType"].ToString();
            }

            if (OrderInfoModel != null)
            {
                this.lblDisName.InnerText = Common.GetDis(OrderInfoModel.DisID, "DisName");
                this.hidDisId.Value = OrderInfoModel.DisID.ToString();
                DisId = OrderInfoModel.DisID;

                this.lblReceiptNo.InnerText = OrderInfoModel.ReceiptNo;
                this.lblRemark.InnerText = OrderInfoModel.Remark;
                this.lblAddr.InnerText = OrderInfoModel.vdef2;

                this.lblCreateDate.InnerText = OrderInfoModel.CreateDate == DateTime.MinValue ? "" : OrderInfoModel.CreateDate.ToString("yyyy-MM-dd");
                this.lblOState.InnerText = OrderInfoType.OState(OrderInfoModel.ID);
                this.lblPayState.InnerText = OrderInfoType.PayState(OrderInfoModel.PayState);
                if (OrderInfoModel.PayState == (int)Enums.PayState.未支付)
                {
                    this.lblPayState.Attributes.Add("style", "Color:Red");
                }
                else
                {
                    this.lblPayState.Attributes.Add("style", "Color:green");
                }


                this.lblTotalPrice.InnerText = OrderInfoModel.AuditAmount.ToString("N");
                this.lblPayedPrice.InnerText = OrderInfoModel.PayedAmount.ToString("N");

                this.lblDisUser.InnerText = Common.GetUserName(OrderInfoModel.DisUserID);
                this.lblArriveDate.InnerText = OrderInfoModel.ArriveDate == DateTime.MinValue ? "" : OrderInfoModel.ArriveDate.ToString("yyyy-MM-dd");

                if (!PostType.Equals("1"))
                {
                    if (OrderInfoModel.OState == (int)Enums.OrderState.退回)
                    {
                        this.Remove.Visible = false;
                        this.Edit.Visible = true;
                    }
                    if (OrderInfoModel.OState == (int)Enums.OrderState.未提交)
                    {
                        this.Remove.Visible = false;
                    }
                    if (OrderInfoModel.OState == (int)Enums.OrderState.待审核)
                    {
                        if (OrderInfoModel.AddType != (int)Enums.AddType.企业补单)
                        {
                            this.Edit.Visible = false;
                        }
                        this.Remove.Visible = true;
                    }
                    if (OrderInfoModel.OState == (int)Enums.OrderState.已审)
                    {
                        this.Edit.Visible = false;
                        if (OrderInfoModel.PayState == (int)Enums.PayState.未支付)
                        {
                            this.Remove.Visible = true;
                        }
                        else
                        {
                            this.Remove.Visible = false;
                        }
                    }
                    if (OrderInfoModel.OState == (int)Enums.OrderState.已作废)
                    {
                        this.Remove.Visible = false;
                        this.CopyOrder.Visible = false;
                        this.Edit.Visible = false;
                    }
                    if (OrderInfoModel.OState > (int)Enums.OrderState.退货处理 && OrderInfoModel.OState < (int)Enums.OrderState.已到货)
                    {
                        this.Remove.Visible = false;
                        this.CopyOrder.Visible = false;
                        this.Edit.Visible = false;
                    }
                    if ((OrderInfoModel.OState >= (int)Enums.OrderState.退货处理 && OrderInfoModel.ReturnState != (int)Enums.ReturnState.未退货) && OrderInfoModel.OState <= (int)Enums.OrderState.已退货)
                    {
                        this.Remove.Visible = false;
                        this.CopyOrder.Visible = false;
                        this.Edit.Visible = false;
                    }
                }
                else
                {
                    this.Remove.Visible = false;
                    this.CopyOrder.Visible = false;
                    this.Edit.Visible = false;
                }
                if (PostType.Equals("3"))
                {
                    btn.Visible = false;
                    this.btnright.Attributes.Add("style", "margin-top:0px; margin-left:0px; width:auto;");
                    //btntitle.Visible = false;
                    this.lblpaytime.InnerText = Request["Date"] == null ? "" : Request["Date"].ToString();
                    this.paytime1.Visible = true;
                    this.paytime2.Visible = true;
                }
                else
                {
                    this.state.Attributes.Add("colspan", "5");
                }
            }
        }
        else
        {
            Response.Write("数据错误!");
            Response.End();
        }
    }
}