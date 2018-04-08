
	using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;

/***
 * 订单提交：订单状态为未提交、退回（Ostate=0、Ostate=-1）时，可以提交订单，按订单是否需要审核（IsAudit）。
 *           修改订单状态：
 *           1、 订单需要审核（IsAudit=0） 修改订单状态为待审核（OState=1）；
 *           2、订单不需要审核（IsAudit=1） 修改订单状态为已审核（OState=2）；
 * 订单支付：订单状态为已审核（Ostate=2）、支付状态为未支付及部分支付（PayState=0、PayState=1）时可以进行支付。
 *            修改支付状态为已支付（PayState=2）；
 * 取消订单：订单状态为待审核（Ostate=1）、已审核未支付（Ostate=2，PayState=0）时，可以取消订单,修改订单状态为已取消（Ostate=6）；
 * 删除订单：只有订单状态为未提交、退回、已取消（Ostate=-1、Ostate=0 、Ostate=6）的可以删除；
 *  
 * 复制订单: 订单状态为已审核（Ostate=2）、已发货(Ostate=4)、已到货未退货（Ostate=5，ReturnState=0）的订单可以复制为一个新的订单，复制               订单时，订单状态为未提交（Ostate=0）、支付状态为未支付（PayState=0）、订单是否需要审核（IsAduit）根据代理商当前订单是否审               核标志判断。
 * 订单签收：订单状态为已发货（Ostate=4），代理商可以签收；
 ***/
public partial class Distributor_OrderZDInfo : DisPageBase
{
    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;
    Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
    Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();

    //Hi.Model.SYS_Users user = null;

    //public int DisId;
    //public int CompID;
    //public int KeyID = 0;
    //public int UserID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        //if (user != null)
        //{
        //    DisId = user.DisID;
        //    CompID = user.CompID;
        //    UserID = user.ID;

        //    if (Request["KeyID"] != null)
        //    {
        //        string Id = Common.DesDecrypt(Request["KeyID"].ToString(), Common.EncryptKey);
        //        KeyID = Id.ToInt(0);
        //    }

        if (!IsPostBack)
        {
            Bind();
        }
        if (!Common.HasRight(this.CompID, this.UserID, "2213", this.DisID))
            this.payIcon.Visible = false;
            //}
        }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        if (KeyID != 0)
        {
            //判断改该条数据代理商是否有操作权限
            if (!Common.PageDisOperable("Order", KeyID, this.DisID))
            {
                Response.Redirect("../NoOperable.aspx");
                return;
            }

            Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(KeyID);

            if (OrderModel != null)
            {

               // this.lblOtype.InnerText = OrderInfoType.OType(OrderModel.Otype);
                this.lblReceiptNo.InnerText = OrderModel.ReceiptNo;
                this.lblRemark.InnerText = OrderModel.Remark;

                //this.hidAddrId.Value = OrderModel.AddrID.ToString();
                //this.lblAddr.InnerText = Common.GetAddr(OrderModel.AddrID);
                //this.lblAddr.InnerText = OrderModel.Address;

                this.lblCreateDate.InnerText = OrderModel.CreateDate == DateTime.MinValue ? "" : OrderModel.CreateDate.ToString("yyyy-MM-dd");
                this.lblOState.InnerText = OrderInfoType.OState(OrderModel.ID);
                if (OrderInfoType.PayState(OrderModel.PayState) == "已支付")
                {
                    this.lblPayState.InnerHtml = "<span style='color:green'>" + OrderInfoType.PayState(OrderModel.PayState) + "</span>";
                }
                else if (OrderInfoType.PayState(OrderModel.PayState) == "未支付")
                {
                    this.lblPayState.InnerHtml = "<span style='color:red'>" + OrderInfoType.PayState(OrderModel.PayState) + "</span>";
                }
                else
                {
                    this.lblPayState.InnerHtml = "<span style='color:blue'>" + OrderInfoType.PayState(OrderModel.PayState) + "</span>";
                }
               // this.lblAddType.InnerText = OrderInfoType.AddType(OrderModel.AddType);

                //this.lblTotalPrice.InnerText = OrderModel.TotalAmount.ToString("N");
                this.lblTotalPrice.InnerText = OrderModel.AuditAmount.ToString("N");
                //this.lblOtherAmount.InnerText = OrderModel.OtherAmount.ToString("N");
                //this.lblAuditAmount.InnerText = OrderModel.AuditAmount.ToString("N");
                this.lblPayedPrice.InnerText = OrderModel.PayedAmount.ToString("N");

                this.lblDisUser.InnerText = Common.GetUserName(OrderModel.DisUserID);
                this.lblArriveDate.InnerText = OrderModel.ArriveDate == DateTime.MinValue ? "" : OrderModel.ArriveDate.ToString("yyyy-MM-dd");
                this.lblfymc.InnerText = OrderModel.vdef2;
                //this.lblAuditUser.InnerText = Common.GetUserName(OrderModel.AuditUserID);
                //this.lblAuditDate.InnerText = OrderModel.AuditDate == DateTime.MinValue ? "" : OrderModel.AuditDate.ToString("yyyy-MM-dd");
                //this.lblAuditRemark.InnerText = OrderModel.AuditRemark;

               

                

                //订单操作
                OrderState(OrderModel);

                #region 页面操作
                if (Request["type1"] == "RepOrderList")
                {
                    payIcon.Visible = false;
                    prnIcon.Visible = false;
                    offIcon.Visible = false;
                    copyIcon.Visible = false;
                    SingIcon.Visible = false;
                    removeIcon.Visible = false;
                    DelIcon.Visible = false;
                    editIcon.Visible = false;
                    Exp.Visible = false;
                    Log.Visible = false;
                    returnIcon.Visible = true;
                }
                if (Request["type1"] == "RepDetailsList")
                {
                    payIcon.Visible = false;
                    prnIcon.Visible = false;
                    offIcon.Visible = false;
                    copyIcon.Visible = false;
                    SingIcon.Visible = false;
                    removeIcon.Visible = false;
                    DelIcon.Visible = false;
                    editIcon.Visible = false;
                    Exp.Visible = false;
                    Log.Visible = false;
                    returnIcon.Visible = true;
                }
                if (Request["type1"] == "RepZdzfDetailsList")
                {
                    payIcon.Visible = false;
                    prnIcon.Visible = false;
                    offIcon.Visible = false;
                    copyIcon.Visible = false;
                    SingIcon.Visible = false;
                    removeIcon.Visible = false;
                    DelIcon.Visible = false;
                    editIcon.Visible = false;
                    Exp.Visible = false;
                    Log.Visible = false;
                    returnIcon.Visible = true;
                }
                //支付
                if (Request["type1"] == "orderpaylist")
                {
                    payIcon.Visible = true;
                    prnIcon.Visible = false;
                    offIcon.Visible = false;
                    copyIcon.Visible = false;
                    SingIcon.Visible = false;
                    removeIcon.Visible = false;
                    DelIcon.Visible = false;
                    editIcon.Visible = false;
                    Exp.Visible = false;
                    Log.Visible = false;
                    returnIcon.Visible = true;
                }
                if (Request["type1"] == "UserList")
                {
                    payIcon.Visible = false;
                    prnIcon.Visible = false;
                    offIcon.Visible = false;
                    copyIcon.Visible = false;
                    SingIcon.Visible = false;
                    removeIcon.Visible = false;
                    DelIcon.Visible = false;
                    editIcon.Visible = false;
                    Exp.Visible = false;
                    Log.Visible = true;
                    returnIcon.Visible = true;
                }
                if (Request["type1"] == "ReceivingList")
                {
                    payIcon.Visible = false;
                    prnIcon.Visible = false;
                    offIcon.Visible = false;
                    copyIcon.Visible = false;
                    SingIcon.Visible = true;
                    removeIcon.Visible = false;
                    DelIcon.Visible = false;
                    editIcon.Visible = false;
                    Exp.Visible = true;
                    //this.Exp.Attributes.Add("style", "display:inline-block;");
                    Log.Visible = true;
                    returnIcon.Visible = true;
                }
                if (Request["type1"] == "ReceivingList1")
                {
                    payIcon.Visible = false;
                    prnIcon.Visible = false;
                    offIcon.Visible = false;
                    copyIcon.Visible = false;
                    SingIcon.Visible = false;
                    removeIcon.Visible = false;
                    DelIcon.Visible = false;
                    editIcon.Visible = false;
                    Exp.Visible = true;
                    //this.Exp.Attributes.Add("style", "display:inline-block;");
                    Log.Visible = true;
                    returnIcon.Visible = true;
                }
                if (Request["type1"] == "ReturnOrderList1")
                {
                    payIcon.Visible = false;
                    prnIcon.Visible = false;
                    offIcon.Visible = false;
                    copyIcon.Visible = false;
                    SingIcon.Visible = false;
                    removeIcon.Visible = true;
                    DelIcon.Visible = false;
                    editIcon.Visible = false;
                    Exp.Visible = false;
                    Log.Visible = true;
                    returnIcon.Visible = true;
                }
                #endregion

              
            }
           
        }
        else
        {
            Response.Redirect("../NoOperable.aspx");
            return;
        }
    }

 

    /// <summary>
    /// 订单操作状态
    /// </summary>
    /// <param name="OState">状态</param>
    public void OrderState(Hi.Model.DIS_Order OrderModel)
    {
        if (OrderModel.OState == (int)Enums.OrderState.退回 || OrderModel.OState == (int)Enums.OrderState.未提交)
        {
            //签收
            //this.SingIcon.Attributes.Add("style", "display:none;");
            ////取消订单
            //this.offIcon.Attributes.Add("style", "display:none;");
            ////退货
            //this.removeIcon.Attributes.Add("style", "display:none;");
            ////支付
            //this.payIcon.Attributes.Add("style", "display:none;");
            ////支付查询
            ////this.payInfo.Attributes.Add("style", "display:none;");
            ////提交
            //this.prnIcon.Attributes.Add("style", "display:inline-block;");
            ////编辑
            //this.editIcon.Attributes.Add("style", "display:inline-block;");
            ////删除
            //this.DelIcon.Attributes.Add("style", "display:inline-block;");

            //签收
            this.SingIcon.Visible = false;
            //取消订单
            this.offIcon.Visible = false;
            //退货
            this.removeIcon.Visible = false;
            //支付
            this.payIcon.Visible = false;
            //提交
            this.prnIcon.Visible = true;
            //编辑
            this.editIcon.Visible = true;
            //删除
            this.DelIcon.Visible = true;

        }
        if (OrderModel.OState == (int)Enums.OrderState.待审核)
        {
            //签收
            //this.SingIcon.Attributes.Add("style", "display:none;");
            ////支付查询
            ////this.payInfo.Attributes.Add("style", "display:none;");
            ////提交
            //this.prnIcon.Attributes.Add("style", "display:none;");
            ////编辑
            //this.editIcon.Attributes.Add("style", "display:none;");
            ////删除
            //this.DelIcon.Attributes.Add("style", "display:none;");
            ////退货
            //this.removeIcon.Attributes.Add("style", "display:none;");
            ////支付
            //this.payIcon.Attributes.Add("style", "display:none;");
            ////取消订单
            //this.offIcon.Attributes.Add("style", "display:none;");

            //签收
            this.SingIcon.Visible = false;
            //取消订单
            this.offIcon.Visible = false;
            //退货
            this.removeIcon.Visible = false;
            //支付
            this.payIcon.Visible = false;
            //提交
            this.prnIcon.Visible = false;
            //编辑
            this.editIcon.Visible = false;
            //删除
            this.DelIcon.Visible = false;

        }
        if (OrderModel.OState == (int)Enums.OrderState.已审)
        {
            //签收
            this.SingIcon.Visible=false;  //.Attributes.Add("style", "display:none;");
            //提交
            this.prnIcon.Visible = false;   //.Attributes.Add("style", "display:none;");
            //编辑
            this.editIcon.Visible = false;     //.Attributes.Add("style", "display:none;");
            //删除
            this.DelIcon.Visible = false;      //.Attributes.Add("style", "display:none;");
            //退货
            this.removeIcon.Visible = false;  //.Attributes.Add("style", "display:none;");

            //支付查询
            //this.payInfo.Attributes.Add("style", "display:inline-block;");

            if (OrderModel.PayState == (int)Enums.PayState.未支付 || OrderModel.PayState == (int)Enums.PayState.部分支付)
            {
                this.payIcon.Visible = true;   //.Attributes.Add("style", "display:inline-block;"); //未支付
                //取消订单
                this.offIcon.Visible = false;  //.Attributes.Add("style", "display:none;");
            }
            else if (OrderModel.PayState == (int)Enums.PayState.已支付)
            {


                this.payIcon.Visible = false;   //.Attributes.Add("style", "display:none;"); //已支付
                //取消订单
                this.offIcon.Visible = false;   //.Attributes.Add("style", "display:none;");
            }
            else
            {

                this.payIcon.Visible = false;   //.Attributes.Add("style", "display:none;"); //已支付
                //取消订单
                this.offIcon.Visible = false;   //.Attributes.Add("style", "display:none;");
            }
        }
        if (OrderModel.OState == (int)Enums.OrderState.已发货 || OrderModel.OState == (int)Enums.OrderState.已到货)
        {
            //提交
            this.prnIcon.Visible = false; //.Attributes.Add("style", "display:none;");
            //编辑
            this.editIcon.Visible = false; //.Attributes.Add("style", "display:none;");
            //删除
            this.DelIcon.Visible = false; //.Attributes.Add("style", "display:none;");

            //取消订单
            this.offIcon.Visible = false; //.Attributes.Add("style", "display:none");

            if (OrderModel.OState == (int)Enums.OrderState.已到货)
            {
                //签收
                this.SingIcon.Visible = false; //  .Attributes.Add("style", "display:none;");
                if (OrderModel.ReturnState == (int)Enums.ReturnState.未退货 || OrderModel.ReturnState == (int)Enums.ReturnState.新增退货)
                {
                    this.removeIcon.Visible = true;  //.Attributes.Add("style", "display:inline-block;");
                    this.copyIcon.Visible = true;   //.Attributes.Add("style", "display:inline-block;");
                    //this.payIcon.Attributes.Add("style", "display:inline-block;"); //未支付
                }
                else
                {
                    //退货
                    this.removeIcon.Visible = false;  //.Attributes.Add("style", "display:none;");
                    this.copyIcon.Visible = false;    //.Attributes.Add("style", "display:none;");
                    this.payIcon.Visible = false;    //.Attributes.Add("style", "display:none;"); 
                }
            }
            else
            {
                //签收
                this.SingIcon.Visible = true;   //.Attributes.Add("style", "display:inline-block;");
                //退货
                this.removeIcon.Visible = false;   //.Attributes.Add("style", "display:none;");
            }

            //支付
            if (OrderModel.Otype == (int)Enums.OType.赊销订单)
            {
                if (OrderModel.PayState == (int)Enums.PayState.未支付 || OrderModel.PayState == (int)Enums.PayState.部分支付)
                {
                    this.payIcon.Visible = true;   //.Attributes.Add("style", "display:inline-block;"); //未支付
                }
                else
                {
                    this.payIcon.Visible = false;   //.Attributes.Add("style", "display:none;"); //已支付
                }
            }
            else
            {
                if (OrderModel.PayState == (int)Enums.PayState.未支付 || OrderModel.PayState == (int)Enums.PayState.部分支付)
                {
                    this.payIcon.Visible = true;   //.Attributes.Add("style", "display:inline-block;"); //未支付
                }
                else
                {
                    this.payIcon.Visible = false;   //.Attributes.Add("style", "display:none;"); //已支付
                }
            }
        }
        if (OrderModel.OState == (int)Enums.OrderState.已作废 || OrderModel.OState == (int)Enums.OrderState.退货处理 || OrderModel.OState == (int)Enums.OrderState.已退货)
        {
            //签收
            this.SingIcon.Visible = false;  //.Attributes.Add("style", "display:none;");
            //提交
            this.prnIcon.Visible = false;   //.Attributes.Add("style", "display:none;");
            //编辑
            this.editIcon.Visible = false;  //.Attributes.Add("style", "display:none;");

            if (OrderModel.OState == (int)Enums.OrderState.已作废)
            {
                //删除
                this.DelIcon.Visible = true;
            }
            else
            {
                //删除
                this.DelIcon.Visible = false;  //.Attributes.Add("style", "display:none;");
            }
            //支付
            if (OrderModel.Otype == (int)Enums.OType.赊销订单)
            {
                //if (OrderModel.PayState == (int)Enums.PayState.未支付)
                //{
                //    this.payIcon.Attributes.Add("style", "display:inline-block;"); //未支付
                //}
                //else
                //{
                this.payIcon.Visible = false;  //.Attributes.Add("style", "display:none;"); //未支付
                //}
            }
            else
            {
                if (OrderModel.PayState == (int)Enums.PayState.未支付 || OrderModel.PayState == (int)Enums.PayState.部分支付)
                {
                    //this.payInfo.Attributes.Add("style", "display:none;"); //未支付
                }
                this.payIcon.Visible = false;  //.Attributes.Add("style", "display:none;"); //未支付
            }
            //取消订单
            this.offIcon.Visible = false;  //.Attributes.Add("style", "display:none");
            this.copyIcon.Visible = false;  //.Attributes.Add("style", "display:none;"); //复制
            //退货
            this.removeIcon.Visible = false;  //.Attributes.Add("style", "display:none;");
        }
    }


    #region 页面事件

    /// <summary>
    /// 提交订单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPrn_Click(object sender,EventArgs e)
    {
        Hi.Model.DIS_Order OrderInfoModel = OrderBll.GetModel(KeyID);

        string GoodsName = string.Empty;
        string GoodMome = string.Empty;

        if (OrderInfoModel != null)
        {
            //判断订单明细是否有商品数据为0的
            List<Hi.Model.DIS_OrderDetail> ld = OrderDetailBll.GetList("", " OrderID=" + KeyID, "");
            if (ld != null)
            {
                if (ld.Count > 0)
                {
                    foreach (Hi.Model.DIS_OrderDetail item in ld)
                    {
                        if (item.GoodsNum <= 0)
                        {
                            //订单存在有商品数量小于等于0的商品
                            JScript.AlertMsgOne(this, "订单明细数据不正确,无法提交！", JScript.IconOption.错误);
                            return;
                        }
                        //判断商品是否可购买
                        if (OrderInfoType.IsGoodsShip(this.DisID, item.GoodsinfoID, item.vdef1.ToInt(0), CompID.ToString(),out GoodsName, out GoodMome) == 1)
                        {
                            JScript.AlertMsgOne(this, "订单商品：" + GoodsName + "，" + GoodMome + "，不能提交!", JScript.IconOption.错误);
                            return;
                        }
                    }
                }
                else
                {
                    //没有商品明细
                    JScript.AlertMsgOne(this, "订单明细数据不正确,无法提交！", JScript.IconOption.错误);
                    return;
                }
            }

            if (OrderInfoModel.OState == (int)Enums.OrderState.未提交 || OrderInfoModel.OState == (int)Enums.OrderState.退回)
            {
                int OState = (int)Enums.OrderState.待审核;

                string sql = string.Empty;

                if (OrderInfoModel.IsAudit == 1)
                {
                    //无需审核
                    OState = (int)Enums.OrderState.已审;
                    sql = " update [DIS_Order] set [OState]=" + OState + ",[AuditDate]='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' where ID=" + KeyID;
                }
                else
                {
                    sql = " update [DIS_Order] set [OState]=" + OState + " where ID=" + KeyID;
                }

                if (OrderBll.UpdateOrderState(sql))
                {
                    Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "订单提交", "");
                    new Common().GetWxService("1", KeyID.ToString(), "1");

                    if (OrderInfoModel.IsAudit == 1)
                    {
                        //无需审核
                        if (OrderInfoModel.Otype == (int)Enums.OType.赊销订单)
                        {
                            ClientScript.RegisterStartupScript(GetType(), "", "<script>location.href=location.href;</script>");
                            return;
                        }
                        else
                        {
                            ClientScript.RegisterStartupScript(GetType(), "", "<script>location.href='pay/Pay.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey) + "';</script>");
                            //Response.Redirect("pay/Pay.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
                            return;
                        }
                    }
                    else
                    {
                        //需审核
                        //Bind();
                        JScript.AlertMethod(this, "您已成功提交，请等待审核！", JScript.IconOption.正确, "function (){ location.replace('" + ("neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey)) + "'); }");

                        //Response.Redirect("OrderInfo.aspx?KeyID=" + KeyID);
                    }
                }
            }
            else
            {
                JScript.AlertMsgOne(this, "订单状态不正确,不能进行提交！", JScript.IconOption.错误);
            }
        }
        else
        {
            JScript.AlertMsgOne(this, "数据不存在！", JScript.IconOption.错误);
        }
    }

    /// <summary>
    /// 删除
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnDel_Click(object sender, EventArgs e)
    {
        Hi.Model.DIS_Order OrderInfoModel = OrderBll.GetModel(KeyID);

        if (OrderInfoModel != null)
        {
            bool del = false;
            //if (OrderInfoModel.AddType == (int)Enums.AddType.企业补单)
            //{
            //    if (OrderInfoModel.OState == (int)Enums.OrderState.已提交)
            //        del = true;
            //}
            //else
            //{
            //}
            //判断订单状态是否能删除
            if (OrderInfoModel.OState == (int)Enums.OrderState.退回 || OrderInfoModel.OState == (int)Enums.OrderState.已作废 || OrderInfoModel.OState == (int)Enums.OrderState.未提交)
                del = true;
            if (del)
            {
                OrderInfoModel.dr = 1;
                bool falg = OrderBll.OrderDel(OrderInfoModel);
                if (falg)
                {
                    Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "订单删除", "");
                    //JScript.AlertMsg(this, "订单删除成功！", "OrderList.aspx");
                    ClientScript.RegisterStartupScript(this.GetType(), "del", "<script>window.location.href = 'OrderList.aspx'</script>");
                }
            }
            else
            {
                JScript.AlertMsgOne(this, "订单处理中，不能删除！", JScript.IconOption.错误);
            }
        }
    }

    /// <summary>
    /// 取消订单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnOff_Click(object sender, EventArgs e)
    {
        Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(KeyID);

        if (OrderModel != null)
        {
            if ((OrderModel.OState == (int)Enums.OrderState.已审 || OrderModel.OState == (int)Enums.OrderState.待审核) && OrderModel.PayState == (int)Enums.PayState.未支付)
            {
                string sql = " update [DIS_Order] set [OState]=" + (int)Enums.OrderState.已作废 + " where ID=" + KeyID;

                if (OrderBll.UpdateOrderState(sql))
                {
                    Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "订单作废", "");
                    //Bind();
                    Response.Redirect("neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
                }
            }
            else
            {
                JScript.AlertMsgOne(this, "数据状态不正确,无法取消！", JScript.IconOption.错误);
            }
        }
    }

    /// <summary>
    /// 复制订单
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnCopy_Click(object sender, EventArgs e)
    {
        decimal TotalAmount = 0; //订单总价
        string LogRemark = string.Empty;  //日志备注
        int Audit = 0;  //是否审核

        Hi.Model.DIS_Order OrderInfoModel = null;
        try
        {
            OrderInfoModel = OrderBll.GetModel(KeyID);

            if (OrderInfoModel != null)
            {
                String guid = Guid.NewGuid().ToString().Replace("-", "");
                OrderInfoModel.GUID = guid;
                OrderInfoModel.ReceiptNo = SysCode.GetNewCode("销售单");

                Audit = OrderInfoType.OrderEnAudit((int)Enums.AddType.正常下单, OrderInfoModel.DisID, OrderInfoModel.Otype);
                OrderInfoModel.IsAudit = Audit;

                //收货地址
                Hi.Model.BD_DisAddr DisAddr = new Hi.BLL.BD_DisAddr().GetModel(OrderInfoModel.AddrID);
                if (DisAddr != null)
                {
                    OrderInfoModel.AddrID = OrderInfoModel.AddrID;
                    OrderInfoModel.Principal = DisAddr.Principal;
                    OrderInfoModel.Phone = DisAddr.Phone;
                    OrderInfoModel.Address = DisAddr.Address;
                }

                OrderInfoModel.PayState = (int)Enums.PayState.未支付;
                OrderInfoModel.OState = (int)Enums.OrderState.未提交;
                OrderInfoModel.PayedAmount = 0;
                OrderInfoModel.ReturnState = 0;
                OrderInfoModel.ArriveDate = DateTime.MinValue;
                OrderInfoModel.DisUserID = this.UserID;
                OrderInfoModel.CreateUserID = this.UserID;
                OrderInfoModel.CreateDate = DateTime.Now;

                OrderInfoModel.AuditUserID = 0;
                OrderInfoModel.AuditDate = DateTime.MinValue;
                OrderInfoModel.AuditRemark = "";
                OrderInfoModel.ReturnMoneyDate = DateTime.MinValue;
                OrderInfoModel.ReturnMoneyUserId = 0;
                OrderInfoModel.ReturnMoneyUser = "";
                OrderInfoModel.ts = DateTime.Now;
                OrderInfoModel.dr = 0;
                OrderInfoModel.modifyuser = 0;

                //订单商品明细
                List<Hi.Model.DIS_OrderDetail> l = OrderDetailBll.GetList("", " OrderId=" + KeyID, "");

                List<Hi.Model.DIS_OrderDetail> dl = new List<Hi.Model.DIS_OrderDetail>();
                
                foreach (Hi.Model.DIS_OrderDetail item in l)
                {
                    //获取商品最新价格
                    decimal Price = SelectGoods.GoodsNewPrice(item.GoodsinfoID, this.DisID, CompID);
                    item.Price = Price;
                    item.AuditAmount = Price;
                    item.sumAmount = Price * item.GoodsNum;

                    TotalAmount += item.sumAmount;

                    item.ts = DateTime.Now;
                    item.dr = 0;
                    item.modifyuser = 0;

                    dl.Add(item);
                }
                OrderInfoModel.TotalAmount = TotalAmount;
                OrderInfoModel.AuditAmount = TotalAmount;
                //OrderInfoModel.OtherAmount = 0;
                LogRemark += " 下单总价：" + TotalAmount.ToString("N");
                int OrderId = OrderInfoType.TansOrder(OrderInfoModel, dl);

                if (OrderId > 0)
                {
                    Utils.AddSysBusinessLog(this.CompID, "Order", OrderId.ToString(), "订单新增", LogRemark);
                    JScript.AlertMethod(this, "复制成功", JScript.IconOption.正确, "function (){ location.replace('" + ("neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(OrderId.ToString(), Common.EncryptKey)) + "'); }");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    /// <summary>
    /// 申请退款
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnRefund_Click(object sender, EventArgs e)
    //{
    //    Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(KeyID);

    //    if (OrderModel != null)
    //    {
    //        if ((OrderModel.OState == (int)Enums.OrderState.已审) && OrderModel.PayState == (int)Enums.PayState.已支付)
    //        {
    //            string sql = " update [DIS_Order] set [PayState]=" + (int)Enums.PayState.申请退款 + ",[ReturnState]=" + (int)Enums.ReturnState.申请退款 + " where ID=" + KeyID;

    //            if (OrderBll.UpdateOrderState(sql))
    //            {
    //                Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "申请退款", "");
    //                //Bind();
    //                Response.Redirect("OrderInfo.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
    //            }
    //        }
    //        else
    //        {
    //            JScript.ShowAlert(this, "数据状态不正确,不能取消!");
    //        }
    //    }
    //}

    /// <summary>
    /// 支付查询
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnPayInfo_Click(object sender, EventArgs e)
    {
        Hi.Model.DIS_Order orderM = OrderBll.GetModel(KeyID);
        Hi.Model.PAY_PrePayment prepayM = new Hi.Model.PAY_PrePayment();
        if (orderM.PayState == (int)Enums.PayState.已支付)
        {
            //支付已成功
            JScript.AlertMsgOne(this, "支付已成功！", JScript.IconOption.笑脸);
            return;
        }
        else
        {
            //支付未成功
            string strWhere5 = string.Empty;
            if (KeyID > 0)
            {
                strWhere5 += " OrderID = " + KeyID;
            }
            strWhere5 += " and isnull(dr,0)=0";
            List<Hi.Model.PAY_PrePayment> prepayL = new Hi.BLL.PAY_PrePayment().GetList("", strWhere5, "");
            if (prepayL.Count > 0 && orderM.AuditAmount == prepayL[0].price * -1)
            {
                //只用企业钱包支付，但是修改状态时出错
                SqlConnection con = new SqlConnection(LocalSqlServer);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                int order = 0;
                int prepay = 0;
                try
                {
                    order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, KeyID, prepayL[0].price * -1, sqlTrans);
                    prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayL[0].ID, sqlTrans);
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
                if (order > 0 && prepay > 0)
                {
                    JScript.AlertMsgOne(this, "支付已成功！", JScript.IconOption.笑脸);
                    return;
                }
                else
                {
                    JScript.AlertMsgOne(this, "支付处理中！", JScript.IconOption.哭脸);
                    return;
                }
            }

            string strWhere = string.Empty;
            if (KeyID > 0)
            {
                strWhere += " OrderID = " + KeyID;
            }
            strWhere += " and vdef3 = '1' and verifystatus = '40' and status = '20' and isnull(dr,0)=0";
            List<Hi.Model.PAY_Payment> payL = new Hi.BLL.PAY_Payment().GetList("", strWhere, "");
            if (payL.Count > 0)
            {
                //使用快捷支付成功，但是修改状态失败
                if (orderM.AuditAmount > payL[0].PayPrice)
                {
                    //是否使用企业钱包
                    string strWhere1 = string.Empty;
                    if (KeyID != 0)
                    {
                        strWhere1 += " OrderID = " + KeyID;
                    }
                    strWhere += " and isnull(dr,0)=0";
                    prepayM = new Hi.BLL.PAY_PrePayment().GetList("", strWhere1, "")[0];
                }
                int order = 0;
                int prepay = 0;
                int pay = 0;
                SqlConnection con = new SqlConnection(LocalSqlServer);
                con.Open();
                SqlTransaction sqlTrans = con.BeginTransaction();
                try
                {
                    order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, orderM.ID, payL[0].PayPrice + (prepayM.price*-1), sqlTrans);

                    if (orderM.AuditAmount == payL[0].PayPrice)
                    {
                        prepay = 1;
                        pay = new Hi.BLL.PAY_Payment().updatePayState(con, payL[0].ID, sqlTrans);
                    }
                    else if (orderM.AuditAmount > payL[0].PayPrice)
                    {
                        prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayM.ID, sqlTrans);
                        pay = new Hi.BLL.PAY_Payment().updatePayState(con, payL[0].ID, sqlTrans);
                    }
                    else
                    {
                        order = 0;
                        prepay = 0;
                        pay = 0;
                        sqlTrans.Rollback();
                    }

                    sqlTrans.Commit();
                }
                catch
                {
                    order = 0;
                    prepay = 0;
                    pay = 0;
                    sqlTrans.Rollback();
                }
                finally
                {
                    con.Close();
                }
                if (order > 0 && prepay > 0 && pay > 0)
                {
                    JScript.AlertMsgOne(this, "支付成功！", JScript.IconOption.笑脸);
                    //Bind();
                    Response.Redirect("neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
                    return;
                }
                else
                {
                    JScript.AlertMsgOne(this, "支付处理中！", JScript.IconOption.哭脸);
                    return;
                }
            }
            else
            {
                //使用快捷支付未成功
                string strWhere2 = string.Empty;
                if (KeyID > 0)
                {
                    strWhere2 += " OrderID = " + KeyID;
                }
                strWhere2 += " and vdef3 = '1' and verifystatus = '40' and status = '10' and isnull(dr,0)=0";
                List<Hi.Model.PAY_Payment> payList = new Hi.BLL.PAY_Payment().GetList("", strWhere2, "ID desc");

                if (payList.Count > 0)
                {
                    //使用快捷支付，处理中
                    int regid = 0;
                    try
                    {
                        Hi.Model.PAY_RegisterLog regModel = new Hi.Model.PAY_RegisterLog();
                        regModel.OrderId = KeyID;
                        regModel.Ordercode = orderM.ReceiptNo;
                        regModel.number = payList[0].guid;
                        regModel.Price = payList[0].PayPrice;
                        regModel.Payuse = "订单支付查询";
                        regModel.PayName = new Hi.BLL.BD_Distributor().GetModel(orderM.DisID).DisName;
                        regModel.DisID = orderM.DisID;
                        regModel.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                        regModel.Remark = orderM.Remark;
                        regModel.DisName = new Hi.BLL.BD_Company().GetModel(orderM.CompID).CompName;
                        regModel.CreateUser = this.UserID;
                        regModel.CreateDate = DateTime.Now;
                        regModel.LogType = 1372;
                        regid = new Hi.BLL.PAY_RegisterLog().Add(regModel);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    if (regid > 0)
                    {
                        if (orderM.AuditAmount > payList[0].PayPrice) {
                            //是否使用企业钱包
                            string strWhere3 = string.Empty;
                            if (KeyID != 0)
                            {
                                strWhere3 += " OrderID = " + KeyID;
                            }
                            strWhere3 += " and isnull(dr,0)=0";
                            prepayM = new Hi.BLL.PAY_PrePayment().GetList("", strWhere3, "")[0];
                        }

                        //调用快捷支付查询接口
                        string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                        PaymentEnvironment.Initialize(configPath);

                        Tx1372Request tx1372Request = new Tx1372Request();
                        tx1372Request.setInstitutionID("001520");
                        tx1372Request.setPaymentNo(payList[0].guid);
                        tx1372Request.process();

                        TxMessenger txMessenger = new TxMessenger();
                        String[] respMsg = txMessenger.send(tx1372Request.getRequestMessage(), tx1372Request.getRequestSignature());

                        Tx1372Response tx1372Response = new Tx1372Response(respMsg[0], respMsg[1]);

                        try
                        {
                            Hi.Model.PAY_RegisterLog regModel = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                            regModel.Start = tx1372Response.getCode();
                            regModel.ResultMessage = tx1372Response.getMessage();
                            new Hi.BLL.PAY_RegisterLog().Update(regModel);
                            Hi.Model.PAY_Payment payM = new Hi.BLL.PAY_Payment().GetModel(payList[0].ID);
                            payM.status = tx1372Response.getStatus();
                            new Hi.BLL.PAY_Payment().Update(payM);
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                        if ("2000".Equals(tx1372Response.getCode()))
                        {
                            int Status = tx1372Response.getStatus();
                            if (Status == 10)
                            {
                                JScript.AlertMsgOne(this, "支付处理中！", JScript.IconOption.哭脸);
                                return;
                            }
                            else if (Status == 20)
                            {
                                int order = 0;
                                int prepay = 0;
                                int pay = 0;
                                SqlConnection con = new SqlConnection(LocalSqlServer);
                                con.Open();
                                SqlTransaction sqlTrans = con.BeginTransaction();
                                try
                                {
                                    order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, KeyID, payList[0].PayPrice + prepayM.price * -1, sqlTrans);

                                    if (orderM.AuditAmount == payList[0].PayPrice)
                                    {
                                        prepay = 1;
                                        pay = 1;
                                    }
                                    else if (orderM.AuditAmount > payList[0].PayPrice)
                                    {
                                        prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepayM.ID, sqlTrans);
                                        pay = new Hi.BLL.PAY_Payment().updatePayState(con, payList[0].ID, sqlTrans);
                                    }
                                    else
                                    {
                                        sqlTrans.Rollback();
                                    }

                                    sqlTrans.Commit();
                                }
                                catch(Exception ex)
                                {
                                    order = 0;
                                    prepay = 0;
                                    pay = 0;
                                    sqlTrans.Rollback();
                                }
                                finally
                                {
                                    con.Close();
                                }

                                if (order > 0 && prepay > 0 && pay > 0)
                                {
                                    JScript.AlertMsgOne(this, "支付成功！", JScript.IconOption.笑脸);
                                    //Bind();
                                    Response.Redirect("neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
                                    return;
                                }
                                else
                                {
                                    JScript.AlertMsgOne(this, "系统繁忙，请稍后！", JScript.IconOption.错误);
                                    return;
                                }
                            }
                            else if (Status == 30)
                            {
                                bool f = false;
                                try
                                {
                                    Hi.Model.PAY_PrePayment prepayModel = new Hi.BLL.PAY_PrePayment().GetModel(prepayL[0].ID);
                                    prepayModel.Start = Convert.ToInt32(Enums.PrePayMentState.失败);
                                    f = new Hi.BLL.PAY_PrePayment().Update(prepayModel);
                                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(KeyID);
                                    orderModel.PayState = Convert.ToInt32(Enums.PayState.未支付);
                                    f = new Hi.BLL.DIS_Order().Update(orderModel);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                if (f)
                                {
                                    JScript.AlertMsgOne(this, "支付失败！", JScript.IconOption.错误);
                                    Bind();
                                    return;
                                }
                                else
                                {
                                    JScript.AlertMsgOne(this, "系统繁忙，请稍后！", JScript.IconOption.错误);
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 签收
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSing_Click(object sender, EventArgs e)
    {
        Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(KeyID);

        if (OrderModel.OState == (int)Enums.OrderState.已发货 && OrderModel.ReturnState == (int)Enums.ReturnState.未退货)
        {
            Hi.Model.DIS_OrderOut OutModel = new Hi.BLL.DIS_OrderOut().GetOutModel(KeyID);

            OutModel.SignDate = DateTime.Now;
            OutModel.SignRemark = "";
            OutModel.SignUser = Common.GetUserName(this.UserID);
            OutModel.SignUserId = this.UserID;
            OutModel.IsSign = 1;
            OutModel.ts = DateTime.Now;
            OutModel.modifyuser = this.UserID;

            if (OrderInfoType.SignOrder(OutModel, OrderModel) > 0)
            {
                Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "订单签收", "");
                new Common().GetWxService("3", KeyID.ToString(), "1");
                if (!string.IsNullOrEmpty(Request["type1"]) && Request["type1"].ToString() == "ReceivingList")
                {
                    Response.Redirect("receivinglist.aspx");
                }
                Response.Redirect("neworder/orderdetail.aspx?KeyID=" + Common.DesEncrypt(KeyID.ToString(), Common.EncryptKey));
                //Bind();
            }
        }
    }
    #endregion

    /// <summary>
    /// 申请退货保存
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void A_AddOrderReturn(object sender, EventArgs e)
    //{
    //    Hi.Model.DIS_Order order = OrderBll.GetModel(KeyID);
    //    if (order != null)
    //    {
            
    //        if (string.IsNullOrEmpty(Request["type"]))
    //        {
    //            Hi.Model.DIS_OrderReturn orderreturn = new Hi.Model.DIS_OrderReturn();
    //            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetDisID(user.UserName);
    //            orderreturn.CompID = dis.CompID;
    //            orderreturn.DisID = dis.ID;
    //            orderreturn.OrderID = order.ID;
    //            orderreturn.CreateDate = DateTime.Now;
    //            orderreturn.CreateUserID = user.ID;
    //            orderreturn.ReturnContent = txtremark.Value;
    //            orderreturn.ReturnState = (int)Enums.AuditState.未提交;
    //            orderreturn.ts = DateTime.Now;
    //            orderreturn.modifyuser = user.ID;

    //            int orderreturnid = OrderInfoType.ReturnOrderAdd(orderreturn, order.ID);
    //            if (orderreturnid > 0)
    //            {
    //                order.ReturnState = 1;
    //                order.ReturnMoneyDate = DateTime.Now;
    //                order.ReturnMoneyUser = user.TrueName;
    //                order.ReturnMoneyUserId = user.ID;
    //                if (new Hi.BLL.DIS_Order().Update(order))
    //                {
    //                    Utils.AddSysBusinessLog(this.CompID, "Order", order.ID.ToString(), "申请退货", "");
    //                    Response.Redirect("returnorderinfo.aspx?KeyID=" + order.ID);
    //                }
    //            }
    //            else
    //            {
    //                JScript.AlertMsg(this, "退货失败,请稍候再试");
    //            }
    //        }
    //        else if (Request["type"].ToString() == "update")
    //        {
    //            Hi.Model.DIS_OrderReturn orderreturn = new Hi.BLL.DIS_OrderReturn().GetModel(KeyID.ToString());
    //            Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetDisID(user.UserName);
    //            orderreturn.CompID = dis.CompID;
    //            orderreturn.DisID = dis.ID;
    //            orderreturn.OrderID = order.ID;
    //            orderreturn.CreateDate = DateTime.Now;
    //            orderreturn.CreateUserID = user.ID;
    //            orderreturn.ReturnContent = txtremark.Value;
    //            orderreturn.ReturnState = (int)Enums.AuditState.未提交;
    //            orderreturn.ts = DateTime.Now;
    //            orderreturn.modifyuser = user.ID;
    //            if (new Hi.BLL.DIS_OrderReturn().Update(orderreturn))
    //            {
    //                Response.Redirect("returnorderinfo.aspx?KeyID=" + order.ID);
    //            }
    //        }

            
            
    //    }
    //}
}