using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_Order_OrderCreateInfo : CompPageBase
{
    Hi.BLL.DIS_Order OrderInfoBll = new Hi.BLL.DIS_Order();
    Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();

    public int DisId = 0;
    public string page = "1";//默认初始页

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    public void Bind()
    {
        //取消按钮显示
        //this.btnRemove.Attributes.Add("style", "display:none;");
        //this.btnRemove.Style["display"] = "none";

        Hi.Model.DIS_Order OrderInfoModel = OrderInfoBll.GetModel(KeyID);

        if (OrderInfoModel != null)
        {
            this.lblDisName.InnerText = Common.GetDis(OrderInfoModel.DisID, "DisName");
            this.hidDisId.Value = OrderInfoModel.DisID.ToString();
            DisId = OrderInfoModel.DisID;

            this.lblOtype.InnerText = OrderInfoType.OType(OrderInfoModel.Otype);
            this.lblReceiptNo.InnerText = OrderInfoModel.ReceiptNo;
            this.lblRemark.InnerText = OrderInfoModel.Remark;

            this.hidAddrId.Value = OrderInfoModel.AddrID.ToString();
            this.lblAddr.InnerText = OrderInfoModel.Address;

            this.lblCreateDate.InnerText = OrderInfoModel.CreateDate.ToString("yyyy-MM-dd");
            this.lblOState.InnerText = OrderInfoType.OState(OrderInfoModel.ID);
            this.lblPayState.InnerText = OrderInfoType.PayState(OrderInfoModel.PayState);

            this.lblTotalPrice.InnerText = OrderInfoModel.TotalAmount.ToString("0.00");
            //this.lblOtherAmount.InnerText = OrderInfoModel.OtherAmount.ToString("0.00");
            this.lblPayedPrice.InnerText = OrderInfoModel.PayedAmount.ToString("0.00");

            this.lblDisUser.InnerText = Common.GetUserName(OrderInfoModel.DisUserID);
            this.lblArriveDate.InnerText = OrderInfoModel.ArriveDate == DateTime.MinValue ? "" : OrderInfoModel.ArriveDate.ToString("yyyy-MM-dd");

            BindOrderDetail(OrderInfoModel.DisID);

            if (OrderInfoModel.OState == (int)Enums.OrderState.退回)
            {
                //this.Remove.Attributes.Add("style", "display:none;"); //取消订单
                //this.Return.Attributes.Add("style", "display:none;"); //退回
                //this.Edit.Attributes.Add("style", "display:inline-block;");   //编辑
                //this.Del.Attributes.Add("style", "display:none;");  //删除
                this.Shipping.Attributes.Add("style", "display:none;"); //发货
                //this.Clearing.Attributes.Add("style", "display:none;");  //去结算
                //this.PrePayMonery.Attributes.Add("style", "display:none;"); //预收款申请
                //this.Submit.Attributes.Add("style", "display:inline-block;");  //提交
                this.Audit.Attributes.Add("style", "display:none;");  //审核   
            }
            if (OrderInfoModel.OState == (int)Enums.OrderState.未提交)
            {
                //this.Remove.Attributes.Add("style", "display:none;");
                //this.Return.Attributes.Add("style", "display:none;");
            }
            if (OrderInfoModel.OState == (int)Enums.OrderState.待审核)
            {
                //this.Edit.Attributes.Add("style", "display:none;");
                //this.Del.Attributes.Add("style", "display:none;");
                this.Shipping.Attributes.Add("style", "display:none;");
                //this.Clearing.Attributes.Add("style", "display:none;");
                //this.PrePayMonery.Attributes.Add("style", "display:none;");
                //this.Submit.Attributes.Add("style", "display:none;");

                //this.Return.Attributes.Add("style", "display:inline-block;");
                this.Audit.Attributes.Add("style", "display:inline-block;");
                //this.Remove.Attributes.Add("style", "display:inline-block;");
            }
            if (OrderInfoModel.OState < (int)Enums.OrderState.待审核 || OrderInfoModel.OState > (int)Enums.OrderState.已审)
            {
                //this.Return.Attributes.Add("style", "display:none;");
                this.Audit.Attributes.Add("style", "display:none;");
                this.Shipping.Attributes.Add("style", "display:none;");
                //this.Clearing.Attributes.Add("style", "display:none;");
                //this.PrePayMonery.Attributes.Add("style", "display:none;");
            }
            if (OrderInfoModel.OState == (int)Enums.OrderState.已审)
            {
                //this.Return.Attributes.Add("style", "display:none;");
                this.Audit.Attributes.Add("style", "display:none;");
                //this.Edit.Attributes.Add("style", "display:none;");
                //this.Del.Attributes.Add("style", "display:none;");
                //this.Submit.Attributes.Add("style", "display:none;");

                //this.Remove.Attributes.Add("style", "display:inline-block;");

                if (OrderInfoModel.Otype == (int)Enums.OType.赊销订单)
                {
                    //this.Clearing.Attributes.Add("style", "display:none;");
                    //this.PrePayMonery.Attributes.Add("style", "display:none;");
                    this.Shipping.Attributes.Add("style", "display:inline-block;");
                }
                else
                {
                    if (OrderInfoModel.PayState > (int)Enums.PayState.未支付)
                    {
                        //this.Clearing.Attributes.Add("style", "display:none;");
                        //this.PrePayMonery.Attributes.Add("style", "display:none;");

                        this.Shipping.Attributes.Add("style", "display:inline-block;");
                    }
                    else
                    {
                        //this.Clearing.Attributes.Add("style", "display:none;");
                        //this.PrePayMonery.Attributes.Add("style", "display:none;");

                        this.Shipping.Attributes.Add("style", "display:none;");
                    }
                }
            }
            if (OrderInfoModel.OState == (int)Enums.OrderState.已作废)
            {
                //this.Return.Attributes.Add("style", "display:none;");
                //this.Remove.Attributes.Add("style", "display:none;");
                this.Shipping.Attributes.Add("style", "display:none;");
                //this.Submit.Attributes.Add("style", "display:none;");

                //if (OrderInfoModel.PayState > (int)Enums.PayState.未支付)
                //{
                //    this.Clearing.Attributes.Add("style", "display:none;");
                //    this.PrePayMonery.Attributes.Add("style", "display:none;");
                //}
            }
            
        }
    }

    /// <summary>
    /// 绑定订单明细
    /// </summary>
    public void BindOrderDetail(int DisID)
    {
        SelectGoods.OrderDetail(KeyID, DisID, this.CompID);
        DataTable dt = Session["GoodsInfo"] as DataTable;
        this.rpDtl.DataSource = dt;
        this.rpDtl.DataBind();
        SelectGoods.Clear(DisID, this.CompID);
    }

    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAudit_Click(object sender, EventArgs e)
    {

        Hi.Model.DIS_Order OrderInfoModel = OrderInfoBll.GetModel(KeyID);

        if (OrderInfoModel != null)
        {
            if (OrderInfoModel.OState == (int)Enums.OrderState.待审核)
            {
                string sql = " update [DIS_Order] set [OState]=" + (int)Enums.OrderState.已审 + " where ID=" + KeyID;
                if (OrderInfoBll.UpdateOrderState(sql))
                {
                    Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "审核通过", "");
                    Bind();
                }
            }
            else
            {
                JScript.AlertMsgOne(this, "数据状态不正确,不能进行审核!", JScript.IconOption.错误, 2500);
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "数据不存在", JScript.IconOption.错误, 2500);
        }

    }
    
    /// <summary>
    /// 提交
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{

    //    Hi.Model.DIS_Order OrderInfoModel = OrderInfoBll.GetModel(KeyID);

    //    if (OrderInfoModel != null)
    //    {
    //        if (OrderInfoModel.OState == (int)Enums.OrderState.未提交 || OrderInfoModel.OState == (int)Enums.OrderState.退回)
    //        {
    //            int OState = 1;
    //            if (OrderInfoModel.IsAudit == 1)
    //            {
    //                //无需审核
    //                OState = (int)Enums.OrderState.已审;
    //            }
    //            string sql = " update [DIS_Order] set [OState]=" + OState + " where ID=" + KeyID;

    //            if (OrderInfoBll.UpdateOrderState(sql))
    //            {
    //                Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "提交", "");
    //                Bind();
    //            }
    //        }
    //        else
    //        {
    //            JScript.ShowAlert(this, "数据状态不正确,不能进行审核!");
    //        }
    //    }
    //    else
    //    {
    //        JScript.ShowAlert(this, "数据不存在!");
    //    }
    //}

    ///// <summary>
    ///// 退回
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btnReturn_Click(object sender, EventArgs e)
    //{

    //    Hi.Model.DIS_Order OrderInfoModel = OrderInfoBll.GetModel(KeyID);

    //    if (OrderInfoModel != null)
    //    {
    //        if (OrderInfoModel.OState > (int)Enums.OrderState.未提交)
    //        {
    //            string sql = " update [DIS_Order] set [OState]=" + (int)Enums.OrderState.退回 + " where ID=" + KeyID;

    //            if (OrderInfoBll.UpdateOrderState(sql))
    //            {
    //                Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "退回", "");
    //                Bind();
    //            }
    //        }
    //    }
    //    else
    //    {
    //        JScript.ShowAlert(this, "数据不存在!");
    //    }
    //}

    ///// <summary>
    ///// 生成订单
    ///// </summary>
    ///// <param name="sender"></param>
    ///// <param name="e"></param>
    //protected void btnCopyOrder_Click(object sender, EventArgs e)
    //{
    //    decimal TotalAmount = 0; //订单总价

    //    Hi.Model.DIS_Order OrderInfoModel = null;

    //    OrderInfoModel = OrderInfoBll.GetModel(KeyID);

    //     if (OrderInfoModel != null)
    //     {
    //         OrderInfoModel.ReceiptNo = SysCode.GetNewCode("销售单");

    //         //TotalAmount	总价
    //         //AuditAmount	审核后总价
    //         //OtherAmount	其它费用

    //         OrderInfoModel.PayState = (int)Enums.PayState.未支付;
    //         OrderInfoModel.OState = (int)Enums.OrderState.待审核;
    //         OrderInfoModel.PayedAmount = 0;
    //         OrderInfoModel.ReturnState = 0;
    //         OrderInfoModel.DisUserID = this.UserID;
    //         OrderInfoModel.CreateUserID = this.UserID;
    //         OrderInfoModel.CreateDate = DateTime.Now;

    //         OrderInfoModel.AuditUserID = 0;
    //         OrderInfoModel.AuditDate = DateTime.MinValue;
    //         OrderInfoModel.AuditRemark = "";
    //         OrderInfoModel.ReturnMoneyDate = DateTime.MinValue;
    //         OrderInfoModel.ReturnMoneyUserId = 0;
    //         OrderInfoModel.ReturnMoneyUser = "";
    //         OrderInfoModel.ts = DateTime.Now;
    //         OrderInfoModel.dr = 0;
    //         OrderInfoModel.modifyuser = 0;

    //         List<Hi.Model.DIS_OrderDetail> l = OrderDetailBll.GetList("", " OrderId=" + KeyID, "");

    //         List<Hi.Model.DIS_OrderDetail> dl=new List<Hi.Model.DIS_OrderDetail>();
             
    //         foreach (Hi.Model.DIS_OrderDetail item in l)
    //         {
    //             item.Price = 0;
    //             item.AuditAmount = 0;
    //             item.sumAmount = item.Price * item.GoodsNum;

    //             TotalAmount += item.sumAmount;

    //             item.ts = DateTime.Now;
    //             item.dr = 0;
    //             item.modifyuser = 0;
                 
    //             dl.Add(item);
    //         }
    //         OrderInfoModel.TotalAmount = TotalAmount;
    //         OrderInfoModel.AuditAmount = TotalAmount;
    //         //OrderInfoModel.OtherAmount = 0;

    //         int OrderId = OrderInfoType.TansOrder(OrderInfoModel, dl);

    //         if (OrderId > 0)
    //         {
    //             Utils.AddSysBusinessLog(this.CompID, "Order", OrderId.ToString(), "订单新增", "");
    //             JScript.AlertMsg(this, "新增成功！", "OrderCreateInfo.aspx?Id=" + OrderId);
    //         }
    //     }
    //}
}