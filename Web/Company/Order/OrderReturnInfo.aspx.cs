
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using DBUtility;

/**
 * 退货单审核：代理商提交退货单后，企业审核退货单信息。
 * **/
public partial class Company_Order_OrderReturnInfo : CompPageBase
{
    Hi.BLL.DIS_OrderReturn OrderReturnBll = new Hi.BLL.DIS_OrderReturn();
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    public int DisId = 0;
    public int OrderId = 0;
    public string ProID = "0";
    public string ProPrice = "";
    public string ProIDD = "0";
    public string ProType = "";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string type = Request.QueryString["type"] + "";
            if (type == "")
            {
                this.title.InnerText = "退货审核";
            }
            else
            {
                this.title.InnerText = "退款审核";
            }
            Bind();

            if (!Common.HasRight(this.CompID, this.UserID, "1015"))
                this.Audit.Visible = false;
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        string type = Request.QueryString["type"] + "";
        if (KeyID != 0)
        {
            if (!Common.PageCompOperable("ReturnOrder", KeyID, CompID))
            {
                Response.Redirect("../../NoOperable.aspx");
                return;
            }

            Hi.Model.DIS_OrderReturn OrderReturnModel = OrderReturnBll.GetModel(KeyID);

            if (OrderReturnModel != null)
            {
                this.lblDisName.InnerText = Common.GetDis(OrderReturnModel.DisID, "DisName");
                this.hidDisId.Value = OrderReturnModel.DisID.ToString();
                DisId = OrderReturnModel.DisID;
                OrderId = OrderReturnModel.OrderID;

                Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(OrderId);

                this.lblReceiptNo.InnerText = OrderReturnModel.ReceiptNo;

                this.lblReturnDate.InnerText = OrderReturnModel.ReturnDate == DateTime.MinValue ? "" : OrderReturnModel.ReturnDate.ToString("yyyy-MM-dd");

                this.lblReturnUserID.InnerText = Common.GetUserName(OrderReturnModel.ReturnUserID.ToString().ToInt(0));
                this.lblReturnState.InnerText = OrderInfoType.ReturnState(OrderReturnModel.ReturnState);
                //this.lblExpress.InnerText = OrderReturnModel.Express;
                //this.lblExpressNo.InnerText = OrderReturnModel.ExpressNo;
                this.lblReturnContent.InnerText = OrderReturnModel.ReturnContent;

                ProID = OrderInfoType.getOrderExt(OrderReturnModel.OrderID, "ProID");
                ProPrice = OrderInfoType.getOrderExt(OrderReturnModel.OrderID, "ProAmount");
                ProIDD = OrderInfoType.getOrderExt(OrderReturnModel.OrderID, "ProDID");
                ProType = OrderInfoType.getOrderExt(OrderReturnModel.OrderID, "Protype");

                //订单信息
                this.lblOState.InnerText = OrderInfoType.OState(OrderModel.ID);
                this.lblPayState.InnerHtml = OrderInfoType.PayState(OrderModel.PayState);
                if (OrderModel.PayState == (int)Enums.PayState.未支付)
                    this.lblPayState.Attributes.Add("style", "Color:Red");
                else
                    this.lblPayState.Attributes.Add("style", "Color:green");

                this.lblAddType.InnerText = OrderInfoType.AddType(OrderModel.AddType);
                this.lblOtype.InnerText = OrderInfoType.OType(OrderModel.Otype);
                //订单总价
                this.lblTotalPrice.InnerText = OrderInfoType.getOrder(OrderReturnModel.OrderID, "AuditAmount");


                if (OrderReturnModel.AuditUserID == 0)
                {
                    this.trAuditUser.Attributes.Add("style", "display:none;");
                    this.trAuditRemark.Attributes.Add("style", "display:none;");
                }
                else
                {
                    this.trAuditUser.Attributes.Add("style", "");
                    this.trAuditRemark.Attributes.Add("style", "");
                }
                this.lblAuditUser.InnerText = Common.GetUserName(OrderReturnModel.AuditUserID);
                this.lblAuditDate.InnerText = OrderReturnModel.AuditDate == DateTime.MinValue ? "" : OrderReturnModel.AuditDate.ToString("yyyy-MM-dd");
                this.lblAuditRemark.InnerText = OrderReturnModel.AuditRemark;

                if (this.Erptype != 0)
                {
                    //U8、U9等用户  不能对订单进行操作 
                    this.Audit.Visible = false;
                    this.ReturnMoney.Visible = false;
                }
                else
                {
                    //非U8、U9等用户  可以对订单进行操作
                    #region 
                    if (OrderReturnModel.ReturnState < (int)Enums.AuditState.提交)
                    {
                        //this.Audit.Attributes.Add("style", "display:none;");
                        //this.ReturnMoney.Attributes.Add("style", "display:none;");

                        this.Audit.Visible = false;
                        this.ReturnMoney.Visible = false;
                    }
                    else if (OrderReturnModel.ReturnState == (int)Enums.AuditState.提交)
                    {
                        this.Audit.Visible = true;
                        this.ReturnMoney.Visible = false;

                        //this.Audit.Attributes.Add("style", "display:inline-block;");
                        //this.ReturnMoney.Attributes.Add("style", "display:none;");
                    }
                    else if (OrderReturnModel.ReturnState == (int)Enums.AuditState.已审)
                    {
                        this.Audit.Visible = false;
                        //this.Audit.Attributes.Add("style", "display:none;");
                        if (OrderModel != null)
                        {
                            if (OrderModel.Otype != (int)Enums.OType.赊销订单)
                            {
                                if (type == "")
                                {
                                    this.ReturnMoney.Visible = false;
                                    //this.ReturnMoney.Attributes.Add("style", "display:none;");
                                }
                                else
                                {
                                    this.ReturnMoney.Visible = true;
                                    //this.ReturnMoney.Attributes.Add("style", "display:inline-block;");
                                }
                            }
                            else
                            {
                                if (OrderModel.PayState == (int)Enums.PayState.已支付)
                                {
                                    if (type == "")
                                    {
                                        this.ReturnMoney.Visible = false;
                                        //this.ReturnMoney.Attributes.Add("style", "display:none;");
                                    }
                                    else
                                    {
                                        this.ReturnMoney.Visible = true;
                                        //this.ReturnMoney.Attributes.Add("style", "display:inline-block;");
                                    }
                                }
                                else
                                {
                                    this.ReturnMoney.Visible = false;
                                    //this.ReturnMoney.Attributes.Add("style", "display:none;");
                                }
                            }
                        }
                    }
                    else if (OrderReturnModel.ReturnState == (int)Enums.AuditState.已完结)
                    {
                        this.Audit.Visible = false;
                        this.ReturnMoney.Visible = false;
                        //this.Audit.Attributes.Add("style", "display:none;");
                        //this.ReturnMoney.Attributes.Add("style", "display:none;");
                    }
                    #endregion
                }
                BindOrderDetail(OrderReturnModel.OrderID, OrderReturnModel.DisID);
            }

        }
        else
        {
            Response.Write("数据错误!");
            Response.End();
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

    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAudit_Click(object sender, EventArgs e)
    {
        //Bind();
        Response.Redirect("OrderReturnInfo.aspx?KeyID=" + Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) + "&go=2&type=" + Request.QueryString["type"] + "");
    }

    /// <summary>
    /// 确认退款
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReturnMoney_Click(object sender, EventArgs e)
    {
        Hi.Model.DIS_OrderReturn OrderReturnModel = OrderReturnBll.GetModel(KeyID);

        if (OrderReturnModel != null)
        {
            if (OrderReturnModel.ReturnState == (int)Enums.AuditState.已审)
            {
                Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(OrderReturnModel.OrderID);

                if (OrderModel == null)
                {
                    JScript.AlertMsgOne(this, "订单信息有误!", JScript.IconOption.错误, 2500);
                    return;
                }
                if (OrderModel.OState == (int)Enums.OrderState.退货处理 && OrderModel.ReturnState == (int)Enums.ReturnState.申请退货)
                {
                    OrderModel.OState = (int)Enums.OrderState.已退货;
                    OrderModel.PayState = (int)Enums.PayState.已退款;
                    OrderModel.ReturnState = (int)Enums.ReturnState.退货退款;
                    OrderModel.ts = DateTime.Now;
                    OrderModel.modifyuser = this.UserID;

                    //OrderReturnModel.ReturnState = (int)Enums.AuditState.已完结;
                    //OrderReturnModel.ts = DateTime.Now;
                    //OrderReturnModel.modifyuser = this.UserID;
                    //退货审确认退款
                    //if (OrderInfoType.ReturnOrderUpdate(OrderReturnModel, OrderModel) > 0)
                    //{

                    #region   把退款金额变为代理商的企业钱包金额
                    int order = 0;
                    int prepay = 0;
                    SqlConnection con = new SqlConnection(LocalSqlServer);
                    con.Open();
                    SqlTransaction sqlTrans = con.BeginTransaction();

                    Hi.Model.PAY_PrePayment PrepayModel = new Hi.Model.PAY_PrePayment();
                    Hi.BLL.PAY_PrePayment PrepayBLL = new Hi.BLL.PAY_PrePayment();
                    PrepayModel.CompID = OrderModel.CompID;
                    PrepayModel.DisID = OrderModel.DisID;
                    PrepayModel.OrderID = OrderModel.ID;
                    PrepayModel.Start = 1;
                    PrepayModel.PreType = 4;
                    PrepayModel.price = OrderModel.PayedAmount;  //已支付金额
                    PrepayModel.Paytime = DateTime.Now;
                    PrepayModel.CrateUser = this.UserID;
                    PrepayModel.CreatDate = DateTime.Now;
                    PrepayModel.OldId = 0;

                    PrepayModel.AuditState = 2;
                    PrepayModel.IsEnabled = 1;
                    PrepayModel.AuditUser = this.UserID;
                    PrepayModel.dr = 0;
                    PrepayModel.ts = DateTime.Now;

                    try
                    {
                        order = new Hi.BLL.DIS_Order().UpdateOrderByggh(con, OrderModel, sqlTrans, KeyID, (int)Enums.AuditState.已完结);
                        prepay = new Hi.BLL.PAY_PrePayment().InsertPrepay(con, PrepayModel, sqlTrans);
                        sqlTrans.Commit();
                    }
                    catch
                    {
                        order = 0;
                        prepay = 0;
                        sqlTrans.Rollback();
                    }
                    finally
                    {
                        con.Close();
                    }
                    #endregion

                    if (prepay > 0 && order > 0)
                    {
                        //代理商手机号
                        string Phone = Common.GetDis(OrderModel.DisID, "Phone");
                        string msg = "您的订单：" + OrderModel.ReceiptNo + "退货金额已退回您的企业钱包账户,请查收！[ " + Common.GetCompValue(CompID, "CompName") + "]";
                        //退款向代理商推送信息提示
                        Common.GetPhone(Phone, msg);

                        Utils.AddSysBusinessLog(this.CompID, "Order", OrderReturnModel.OrderID.ToString(), "退货退款确认", "");
                    }
                    string type = Request.QueryString["type"] + "";
                    Response.Redirect("OrderReturnInfo.aspx?KeyID=" + Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) + "&go=2&type=" + type);

                    //Bind();
                    //Response.Write("<script language=\"javascript\">window.parent.AuditReturn(" + KeyID + ");</script>");
                    //}
                }
                else
                {
                    JScript.AlertMsgOne(this, "订单信息有误!", JScript.IconOption.错误, 2500);
                    return;
                }
            }
            else
            {
                JScript.AlertMsgOne(this, "订单状态有误!", JScript.IconOption.错误, 2500);
                return;
            }
        }
    }
}