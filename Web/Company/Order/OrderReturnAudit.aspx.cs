

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DBUtility;

/***
 *  退货审核通过：
 *      1、判断退货单状态为待审核（DIS_OrderReturn表 ReturnState=1）；
 *      2、判断订单状态为已到货（DIS_Order表 Ostate=5）订单退货状态为申请退货（DIS_Order表 ReturnState=2）；
 *      3、订单已支付、部分支付修改 
 *               退货单状态为已完结（DIS_OrderReturn表 ReturnState=4）、
 *               订单状态为已退货（DIS_Order表 Ostate=7）、
 *               订单支付状态为已退款（DIS_Order表 PayState=5）、
 *               订单退货状态为已退货款（DIS_Order表 ReturnState=3）、
 *               并生成企业钱包金额 退款给代理商；
 *      4、订单未支付 退货单状态为已审 （DIS_OrderReturn表 ReturnState=2）、订单状态为已退货（DIS_Order表 Ostate=7）、不生成企业钱包金额；
 * 退货审核退回：
 *      1、判断退货单状态为待审核（DIS_OrderReturn表 ReturnState=1）；
 *      2、修改退货单状态为退回（DIS_OrderReturn表 ReturnState=-1），退回后不能从新提交；
 ***/
public partial class Company_Order_OrderReturnAudit : CompPageBase
{
    Hi.BLL.DIS_OrderReturn OrderReturnBll = new Hi.BLL.DIS_OrderReturn();
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind()
    {
        if (KeyID != 0)
        {
            if (!Common.PageCompOperable("ReturnOrder", KeyID, CompID))
            {
                Response.Redirect("../../NoOperable.aspx");
                return;
            }
        }

        this.lblAuditUser.InnerText = Common.GetUserName(this.UserID);
        this.hidAuditUserID.Value = this.UserID.ToString();
        //this.txtArriveDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
        this.lblArriveDate.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 审核通过
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        Hi.Model.DIS_OrderReturn OrderReturnModel = OrderReturnBll.GetModel(KeyID);

        string AuditUserID = string.Empty;
        string AuditDate = string.Empty;
        string AuditRemark = string.Empty;
        int pay = 0;

        if (!string.IsNullOrEmpty(this.hidAuditUserID.Value)) {
            AuditUserID = this.hidAuditUserID.Value.Trim().ToString();
        }
        //if (!string.IsNullOrEmpty(this.txtArriveDate.Value.Trim().ToString())) 
        //{
        //    AuditDate = this.txtArriveDate.Value.Trim().ToString();
        //}
        if (!string.IsNullOrEmpty(this.lblArriveDate.InnerText.Trim().ToString()))
        {
            AuditDate = this.lblArriveDate.InnerText.Trim().ToString();
        }
        if (!string.IsNullOrEmpty(this.txtRemark.Value.Trim().ToString())) {
            AuditRemark = this.txtRemark.Value.Trim().ToString();
        }

        if (OrderReturnModel != null)
        {
            //判断退货单状态是否正确
            if (OrderReturnModel.ReturnState == (int)Enums.AuditState.提交)
            {
                Hi.Model.DIS_Order OrderModel=new Hi.BLL.DIS_Order().GetModel(OrderReturnModel.OrderID);

                if (OrderModel == null) {
                    JScript.AlertMsgOne(this, "订单信息有误！不能通过审核", JScript.IconOption.错误, 2500);
                    return;
                }
                int State = 0;
                string StateRemark = string.Empty;
                string LogRemark = string.Empty;

                StateRemark = "退货审核通过"  ;

                //订单支付状态
                pay = OrderModel.PayState;
                LogRemark = "退货退款：" + OrderModel.PayedAmount.ToString("N");

                //判断订单状态是否正确
                if ((OrderModel.OState == (int)Enums.OrderState.已发货 || OrderModel.OState == (int)Enums.OrderState.已到货) && OrderModel.ReturnState == (int)Enums.ReturnState.申请退货)
                {
                    if (OrderModel.PayState == (int)Enums.PayState.已支付 || pay == (int)Enums.PayState.部分支付)
                    {
                        //订单已支付退货退款  
                        //State = (int)Enums.AuditState.已审;
                        State = (int)Enums.AuditState.已完结;
                        OrderModel.OState = (int)Enums.OrderState.已退货;
                        OrderModel.PayState = (int)Enums.PayState.已退款;
                        OrderModel.ReturnState = (int)Enums.ReturnState.退货退款;
                    }
                    else
                    {
                        //订单未支付退货
                        OrderModel.OState = (int)Enums.OrderState.已退货;
                        State = (int)Enums.AuditState.已完结;
                    }

                    #region
                    //if (OrderModel.Otype == (int)Enums.OType.赊销订单)
                    //{
                        //if (OrderModel.PayState == (int)Enums.PayState.已支付 || pay == (int)Enums.PayState.部分支付 || OrderModel.PayState == (int)Enums.PayState.已结算)
                        //{
                        //    //订单已支付退货退款  
                        //    //State = (int)Enums.AuditState.已审;
                        //    State = (int)Enums.AuditState.已完结;
                        //    OrderModel.OState = (int)Enums.OrderState.已退货;
                        //    OrderModel.PayState = (int)Enums.PayState.已退款;
                        //    OrderModel.ReturnState = (int)Enums.ReturnState.退货退款;
                        //}
                        //else
                        //{
                        //    //订单未支付退货
                        //    OrderModel.OState = (int)Enums.OrderState.已退货;
                        //    State = (int)Enums.AuditState.已审;
                        //}
                    //}
                    //else
                    //{
                    //    //OrderModel.OState = (int)Enums.OrderState.退货处理;
                    //    //State = (int)Enums.AuditState.已审;
                    //    if (OrderModel.PayState == (int)Enums.PayState.已支付 || pay == (int)Enums.PayState.部分支付 || OrderModel.PayState == (int)Enums.PayState.已结算)
                    //    {
                    //        OrderModel.OState = (int)Enums.OrderState.已退货;
                    //        OrderModel.PayState = (int)Enums.PayState.已退款;
                    //        OrderModel.ReturnState = (int)Enums.ReturnState.退货退款;

                    //        State = (int)Enums.AuditState.已完结;
                    //    }
                    //    else
                    //    {
                    //        OrderModel.OState = (int)Enums.OrderState.已退货;
                    //        State = (int)Enums.AuditState.已审;
                    //    }
                    //}
                    #endregion
                    OrderModel.ts = DateTime.Now;
                    OrderModel.modifyuser = this.UserID;

                    OrderReturnModel.AuditUserID = AuditUserID.ToInt(0);
                    OrderReturnModel.AuditDate = AuditDate.ToDateTime();
                    OrderReturnModel.AuditRemark = AuditRemark;
                    OrderReturnModel.ReturnState = State;
                    OrderReturnModel.ts = DateTime.Now;
                    OrderReturnModel.modifyuser = this.UserID;


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
                    PrepayModel.guid= Common.Number_repeat("");

                    try
                    {
                        //order = new Hi.BLL.DIS_Order().UpdateOrderByggh(con, OrderModel, sqlTrans, KeyID, (int)Enums.AuditState.已完结);
                        order = OrderInfoType.ReturnOrderUpdate(con, sqlTrans, OrderReturnModel, OrderModel);

                        //if (OrderModel.Otype == (int)Enums.OType.赊销订单)
                        //{
                        //    //支付的订单 生成企业钱包
                        //    if (pay == (int)Enums.PayState.已支付||pay == (int)Enums.PayState.部分支付|| pay == (int)Enums.PayState.已结算)
                        //    {
                        //        prepay = new Hi.BLL.PAY_PrePayment().InsertPrepay(con, PrepayModel, sqlTrans);
                        //    }
                        //    else
                        //    {
                        //        //未支付的订单 不生成企业钱包
                        //        prepay = 1;
                        //    }
                        //}
                        //else
                        //{
                        if (pay == (int)Enums.PayState.已支付 || pay == (int)Enums.PayState.部分支付)
                        {
                            //支付的订单 生成企业钱包
                            prepay = new Hi.BLL.PAY_PrePayment().InsertPrepay(con, PrepayModel, sqlTrans);

                            //退款减积分
                            OrderInfoType.AddIntegral(CompID, OrderModel.DisID, "1", 2, OrderModel.ID, OrderModel.PayedAmount, "订单退款", "", this.UserID);
                        }
                        else
                        {
                            //未支付的订单 不生成企业钱包
                            prepay = 1;
                        }
                        //}

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
                        if (OrderInfoType.rdoOrderAudit("订单下单数量是否取整", this.CompID) == "0")
                        {
                            //退货审核通过，返还商品库存
                            string sql = new Hi.BLL.DIS_Order().GetSqlAddInve(OrderModel.ID, null, 0);
                            new Hi.BLL.DIS_Order().UpdateOrderState(sql);
                        }

                        if (pay == (int)Enums.PayState.未支付)
                        {
                            // type : "4":订单发货通知;"3":订单状态变更通知(待发货、审批通过);"2":订单支付通知;"1":下单通知
                            new Common().GetWxService("44", OrderReturnModel.OrderID.ToString(), "0");

                            //代理商手机号
                            string Phone = Common.GetDis(OrderModel.DisID, "Phone");
                            string msg = "您的订单：" + OrderModel.ReceiptNo + "退货申请已通过！";
                            //退款向代理商推送信息提示
                            Common.GetPhone(Phone, msg);
                        }
                        else
                        {
                             //type : "4":订单发货通知;"3":订单状态变更通知(待发货、审批通过);"2":订单支付通知;"1":下单通知
                            new Common().GetWxService("44", OrderReturnModel.OrderID.ToString(), "0");
                            new Common().GetWxService("45", OrderReturnModel.OrderID.ToString(), "0");

                            //代理商手机号
                            string Phone = Common.GetDis(OrderModel.DisID, "Phone");
                            string msg = "您的订单：" + OrderModel.ReceiptNo + "退货金额已退回您的企业钱包账户,请查收！";
                            //退款向代理商推送信息提示
                            Common.GetPhone(Phone, msg);

                        }

                        Utils.AddSysBusinessLog(this.CompID, "Order", OrderReturnModel.OrderID.ToString(), StateRemark, LogRemark);

                        Response.Write("<script language=\"javascript\">window.parent.AuditReturn('" + Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) + "');</script>");
                    }
                }
                else
                {
                    JScript.AlertMsgOne(this, "订单信息有误！不能通过审核!", JScript.IconOption.错误, 2500);
                    return;
                }
            }
            else
            {
                JScript.AlertMsgOne(this, "数据状态不正确,不能进行审核!", JScript.IconOption.错误, 2500);
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "数据不存在!", JScript.IconOption.错误, 2500);
        }
    }

    /// <summary>
    /// 退回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        Hi.Model.DIS_OrderReturn OrderReturnModel = OrderReturnBll.GetModel(KeyID);

        string AuditUserID = string.Empty;
        string AuditDate = string.Empty;
        string AuditRemark = string.Empty;

        if (!string.IsNullOrEmpty(this.hidAuditUserID.Value))
        {
            AuditUserID = this.hidAuditUserID.Value.Trim().ToString();
        }
        if (!string.IsNullOrEmpty(this.lblArriveDate.InnerText.Trim().ToString()))
        {
            AuditDate = this.lblArriveDate.InnerText.Trim().ToString();
        }
        if (!string.IsNullOrEmpty(this.txtRemark.Value.Trim().ToString()))
        {
            AuditRemark = this.txtRemark.Value.Trim().ToString();
        }

        if (OrderReturnModel != null)
        {
            if (OrderReturnModel.ReturnState == (int)Enums.AuditState.提交)
            {

                Hi.Model.DIS_Order OrderModel = new Hi.BLL.DIS_Order().GetModel(OrderReturnModel.OrderID);

                if (OrderModel == null)
                {
                    JScript.AlertMsgOne(this, "订单信息有误!", JScript.IconOption.错误, 2500);
                    return;
                }
                OrderModel.ReturnState = (int)Enums.ReturnState.拒绝退货;
                OrderModel.modifyuser = this.UserID;

                int State = 0;
                string StateRemark = string.Empty;

                State = (int)Enums.AuditState.退回;
                StateRemark = "退货审核退回";

                OrderReturnModel.AuditUserID = AuditUserID.ToInt(0);
                OrderReturnModel.AuditDate = AuditDate.ToDateTime();
                OrderReturnModel.AuditRemark = AuditRemark;
                OrderReturnModel.ReturnState = State;
                OrderReturnModel.ts = DateTime.Now;
                OrderReturnModel.modifyuser = this.UserID;

                //退货审核通过时  修改订单退货状态：0
                if (OrderInfoType.ReturnOrderUpdate(OrderReturnModel, OrderModel) > 0)
                {
                    Utils.AddSysBusinessLog(this.CompID, "Order", OrderReturnModel.OrderID.ToString(), StateRemark, AuditRemark);

                    //type : "4":订单发货通知;"3":订单状态变更通知(待发货、审批通过);"2":订单支付通知;"1":下单通知
                    //string JSon="{"type":"1","userID":"1027","orderID":"1030"}";
                    new Common().GetWxService("44", OrderReturnModel.OrderID.ToString(), "0");

                    Response.Write("<script language=\"javascript\">window.parent.AuditReturn('" + Common.DesEncrypt(this.KeyID.ToString(), Common.EncryptKey) + "');</script>");
                }
            }
            else
            {
                JScript.AlertMsgOne(this, "数据状态不正确,不能进行审核!", JScript.IconOption.错误, 2500);
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "数据不存在!", JScript.IconOption.错误, 2500);
        }
    }
}