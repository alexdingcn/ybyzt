
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

/***
 * 申请退货：已到货（DIS_Order表 OState=5）可以申请退货。
 *           申请退货时，生成退货单（DIS_OrderReturn表），修改订单退货状态为新增退货（DIS_Order表 ReturnState=1）
 * **/
public partial class Distributor_ReturnOrderAdd : DisPageBase
{
    public string receiptno = "";

    public Hi.Model.DIS_Order order = null;
    public string truename = "";
    public string Otype = "";

    Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
    Hi.BLL.DIS_OrderReturn OrderReturnBll = new Hi.BLL.DIS_OrderReturn();


    protected void Page_Load(object sender, EventArgs e)
    {
        
        if (KeyID != 0)
        {
            if (!Common.PageDisOperable("Order", KeyID, this.DisID))
            {
                Response.Redirect("../NoOperable.aspx");
                return;
            }

            order = OrderBll.GetModel(KeyID);
            if (order != null)
            {
                //receiptno = order.ReceiptNo;
                //DisID = order.DisID;
                //Otype = OrderInfoType.OType(order.Otype);
                //if (user != null)
                //{
                //    //UserID = user.ID;
                //    //CompID = user.CompID;
                //    //DisID = user.DisID;

                //    //DisUser = new Hi.BLL.SYS_Users().GetModel(order.DisUserID);
                //    //if (DisUser != null)
                //    //{
                //    //    truename = DisUser.UserName;
                //    //    BindGoods();
                //    //}

                //}
                //else
                //{
                //    Response.Redirect("orderlist.aspx");
                //}
            }
            else
            {
                Response.Redirect("orderlist.aspx");
            }
        //    if (Request["KeyID"] != null)
        //    {
        //        KeyID = int.Parse(Request["KeyID"]);
        //    }
        //    if (KeyID != 0)
        //    {
        //        order = OrderBll.GetModel(KeyID);
        //        if (order != null)
        //        {
        //            Otype = OrderInfoType.OType(order.Otype);

        //            if (user != null)
        //            {
        //                truename = user.TrueName;
        //                BindGoods();
        //            }

        //        }
        //        else
        //        {
        //            Response.Redirect("orderlist.aspx");
        //        }
        //    }
        //    else
        //    {
        //        Response.Redirect("orderlist.aspx");
        //    }
        }
    }

    //public void BindGoods()
    //{
    //    SelectGoods.Clear(this.CompID);
    //    SelectGoods.OrderDetail(KeyID, DisID, this.CompID);
    //    DataTable dt = Session["GoodsInfo"] as DataTable;
    //    if (dt != null)
    //    {
    //        this.rpDtl.DataSource = dt;
    //        this.rpDtl.DataBind();
    //    }
    //    else
    //    {
    //        this.rpDtl.DataSource = "";
    //        this.rpDtl.DataBind();
    //    }
    //    SelectGoods.Clear(DisID, this.CompID);
    //}


    //protected void A_AddOrderReturn(object sender, EventArgs e)
    //{
    //    if (order != null)
    //    {
    //        if (!string.IsNullOrEmpty(txtremark.Value.Trim()))
    //        {
    //            if (string.IsNullOrEmpty(Request.QueryString["type"]))
    //            {
    //                Hi.Model.DIS_OrderReturn orderreturn = new Hi.Model.DIS_OrderReturn();
    //                Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetDisID(user.UserName);
    //                orderreturn.CompID = dis.CompID;
    //                orderreturn.DisID = dis.ID;
    //                orderreturn.OrderID = order.ID;
    //                orderreturn.CreateDate = DateTime.Now;
    //                orderreturn.CreateUserID = user.ID;
    //                orderreturn.ReturnContent = txtremark.Value;
    //                orderreturn.ReturnState = (int)Enums.AuditState.未提交;
    //                orderreturn.ts = DateTime.Now;
    //                orderreturn.modifyuser = user.ID;

    //                int orderreturnid = OrderInfoType.ReturnOrderAdd(orderreturn, order.ID);
    //                if (orderreturnid > 0)
    //                {
    //                    Utils.AddSysBusinessLog(this.CompID, "Order", order.ID.ToString(), "申请退货", "");
    //                    Response.Redirect("returnorderinfo.aspx?KeyID=" + order.ID);
    //                }
    //                else
    //                {
    //                    JScript.AlertMsg(this, "退货失败,请稍候再试");
    //                }
    //            }
    //            else if (Request.QueryString["type"].ToString() == "update")
    //            {
    //                Hi.Model.DIS_OrderReturn orderreturn = new Hi.BLL.DIS_OrderReturn().GetModel(KeyID.ToString());
    //                Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetDisID(user.UserName);
    //                orderreturn.CompID = dis.CompID;
    //                orderreturn.DisID = dis.ID;
    //                orderreturn.OrderID = order.ID;
    //                orderreturn.CreateDate = DateTime.Now;
    //                orderreturn.CreateUserID = user.ID;
    //                orderreturn.ReturnContent = txtremark.Value;
    //                orderreturn.ReturnState = (int)Enums.AuditState.未提交;
    //                orderreturn.ts = DateTime.Now;
    //                orderreturn.modifyuser = user.ID;
    //                if (new Hi.BLL.DIS_OrderReturn().Update(orderreturn))
    //                {
    //                    Response.Redirect("returnorderinfo.aspx?KeyID=" + order.ID);
    //                }
    //            }
                    
    //        }
    //        else
    //        {
    //            JScript.AlertMsg(this, "请注明退货原因");
    //        }
    //    }
    //}

    //public int GetGoodsID(string goodsinfoid)
    //{
    //    Hi.Model.BD_GoodsInfo goodsinfo = new Hi.BLL.BD_GoodsInfo().GetModel(int.Parse(goodsinfoid));
    //    return goodsinfo.GoodsID;
    //}

    protected void A_AddOrderReturn(object sender, EventArgs e)
    {
        Hi.Model.DIS_Order order = OrderBll.GetModel(KeyID);
        if (order != null)
        {
            if (order.OState == (int)Enums.OrderState.已到货 && (order.ReturnState == (int)Enums.ReturnState.未退货 || order.ReturnState == (int)Enums.ReturnState.拒绝退货))
            {
                if (string.IsNullOrEmpty(Request.QueryString["type"]))
                {
                    Hi.Model.DIS_OrderReturn orderreturn = new Hi.Model.DIS_OrderReturn();
                    //Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
                    orderreturn.ReceiptNo = order.ReceiptNo + SysCode.GetCode("退单", order.ID.ToString());
                    orderreturn.CompID = this.CompID;
                    orderreturn.DisID = this.DisID;
                    orderreturn.OrderID = order.ID;
                    orderreturn.ReturnDate = DateTime.Now;
                    orderreturn.CreateDate = DateTime.Now;
                    orderreturn.CreateUserID = this.UserID;
                    orderreturn.ReturnContent = Common.NoHTML(txtremark.Value);
                    orderreturn.ReturnState = (int)Enums.AuditState.提交;
                    orderreturn.ts = DateTime.Now;
                    orderreturn.modifyuser = this.UserID;

                    int orderreturnid = OrderInfoType.ReturnOrderAdd(orderreturn, order.ID);
                    if (orderreturnid > 0)
                    {
                        order.ts = DateTime.Now;
                        order.ReturnState = (int)Enums.ReturnState.申请退货;
                        order.ReturnMoneyDate = DateTime.Now;
                        order.ReturnMoneyUser = this.UserName;
                        order.ReturnMoneyUserId = this.UserID;
                        if (new Hi.BLL.DIS_Order().Update(order))
                        {
                            //Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "订单修改", orderreturn.ReturnContent);
                            ClientScript.RegisterStartupScript(GetType(), "msg", "<script>window.parent.ResurnOrder();</script>");
                        }
                    }
                    else
                    {
                        JScript.AlertMsgOne(this, "退货失败,请稍候再试！", JScript.IconOption.哭脸);
                    }
                }
                else if (Request.QueryString["type"].ToString() == "update")
                {
                    Hi.Model.DIS_OrderReturn orderreturn = new Hi.BLL.DIS_OrderReturn().GetModel(KeyID.ToString());
                    Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(DisID);
                    orderreturn.CompID = dis.CompID;
                    orderreturn.DisID = dis.ID;
                    orderreturn.OrderID = order.ID;
                    orderreturn.CreateDate = DateTime.Now;
                    orderreturn.CreateUserID = this.UserID;
                    orderreturn.ReturnContent = Common.NoHTML(txtremark.Value);
                    orderreturn.ReturnState = (int)Enums.AuditState.未提交;
                    orderreturn.ts = DateTime.Now;
                    orderreturn.modifyuser = this.UserID;
                    if (new Hi.BLL.DIS_OrderReturn().Update(orderreturn))
                    {
                        Response.Redirect("returnorderinfo.aspx?KeyID=" + Common.DesEncrypt(order.ID.ToString(), Common.EncryptKey));
                    }
                }
            }
            else
                JScript.AlertMsgOne(this, "订单处理中，不能申请退货！", JScript.IconOption.哭脸);
        }
    }
}