

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

/***
 * 退货单提交：退货单状态为未提交（DIS_OrderReturn表 ReturnState=0）可以提交订单，修改退货单状态为待审核（DIS_OrderReturn表                             ReturnState=1）、修改订单退货状态（DIS_Order表 ReturnState=2）
 * 退货单退回：退回的退货单不能从新提交
 * ***/
public partial class Distributor_ReturnOrderInfo : DisPageBase
{
    public string ReceiptNo = "";
    public string ReturnState = "";
    public string AuditUserName = "";
    public string CreateDate = "";
    public string AuditDate = "";
    public string ReturnContent = "";
    public string AuditRemark = "";
    public string ProID = "0";
    public string ProPrice = "";
    public string ProIDD = "0";
    public string ProType = "";

    Hi.Model.DIS_Order order = null;
    Hi.Model.DIS_OrderReturn orderreturn = null;
    //public Hi.Model.SYS_Users user = null;

    //public int KeyID = 0;
    //public int CompID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        //if (Request["KeyID"] != null)
        //{
        //    string Id = Common.DesDecrypt(Request["KeyID"].ToString(), Common.EncryptKey);
        //    KeyID = Id.ToInt(0);
        //    CompID = user.CompID;
        //}
        if (KeyID != 0)
        //user = LoginModel.IsLogined(this);
        //if (user != null)
        //{
            if (KeyID != 0)
            {
                if (!Common.PageDisOperable("Order", KeyID, this.DisID))
                {
                    Response.Redirect("../NoOperable.aspx");
                    return;
                }

                order = new Hi.BLL.DIS_Order().GetModel(KeyID);
                orderreturn = new Hi.BLL.DIS_OrderReturn().GetModel(order.ID.ToString());
                if (orderreturn == null)
                {
                    Response.Redirect("OrderList.aspx");
                }

                ReceiptNo = orderreturn.ReceiptNo;

                ProID = OrderInfoType.getOrderExt(orderreturn.OrderID, "ProID");
                ProPrice = OrderInfoType.getOrderExt(orderreturn.OrderID, "ProAmount");
                ProIDD = OrderInfoType.getOrderExt(orderreturn.OrderID, "ProDID");
                ProType = OrderInfoType.getOrderExt(orderreturn.OrderID, "Protype");

                this.lblTotalPrice.InnerText = order.AuditAmount.ToString("N");

                Hi.Model.SYS_Users disuser = new Hi.BLL.SYS_Users().GetModel(orderreturn.AuditUserID);
                switch (orderreturn.ReturnState)
                {
                    case -1: ReturnState = "已拒绝";
                        AuditUserName = disuser == null ? "" : disuser.TrueName;
                        AuditDate = orderreturn.AuditDate.ToString("yyyy-MM-dd");
                        AuditRemark = orderreturn.AuditRemark;
                        A_btn.InnerHtml = "<a href=\"#\" onclick=\"returnLog();\" class=\"btnBl\"><i class=\"dailyIcon\"></i>日志</a><a href=\"returnorderlist.aspx\" class=\"btnBl\"><i class=\"returnIcon\"></i>返回</a>";
                        //增加修改,取消退货按钮代码,把后面一截拷进去就OK了   <a href=\"returnorderadd.aspx?KeyID="+KeyID+"&type=update\" class=\"btnBl\"><i class=\"editIcon\"></i>修改退货单</a><a href=\"#\" onclick=\"offIcon();\" class=\"btnBl\"><i class=\"offIcon\"></i>取消退货单</a>
                        break;
                    case 0: ReturnState = "未提交"; A_btn.InnerHtml = "<a href=\"#\" onclick=\"editIcon();\" class=\"btnOr\"><i class=\"editIcon\"></i>提交退货单</a><a href=\"#\" onclick=\"offIcon();\" class=\"btnBl\"><i class=\"offIcon\"></i>取消退货单</a><a href=\"#\" onclick=\"returnLog();\" class=\"btnBl\"><i class=\"dailyIcon\"></i>日志</a><a href=\"#\" onclick=\"areturn()\" class=\"btnBl\"><i class=\"returnIcon\"></i>返回</a>"; break;
                    case 1: ReturnState = "待审核"; A_btn.InnerHtml = "<a href=\"#\" onclick=\"returnLog();\" class=\"btnBl\"><i class=\"dailyIcon\"></i>日志</a><a href=\"ReturnOrderList.aspx\" class=\"btnBl\"><i class=\"returnIcon\"></i>返回</a>"; break;
                    case 2: ReturnState = "已退货";
                        AuditUserName = disuser == null ? "" : disuser.TrueName;
                        AuditDate = orderreturn.AuditDate.ToString("yyyy-MM-dd");
                        AuditRemark = orderreturn.AuditRemark;
                        A_btn.InnerHtml = "<a href=\"#\" onclick=\"returnLog();\" class=\"btnBl\"><i class=\"dailyIcon\"></i>日志</a><a href=\"ReturnOrderList.aspx\" class=\"btnBl\"><i class=\"returnIcon\"></i>返回</a>";
                        break;
                    case 4: ReturnState = "已退货款";
                        AuditUserName = disuser == null ? "" : disuser.TrueName;
                        AuditDate = orderreturn.AuditDate.ToString("yyyy-MM-dd");
                        AuditRemark = orderreturn.AuditRemark;
                        A_btn.InnerHtml = "<a href=\"#\" onclick=\"returnLog();\" class=\"btnBl\"><i class=\"dailyIcon\"></i>日志</a><a href=\"ReturnOrderList.aspx\" class=\"btnBl\"><i class=\"returnIcon\"></i>返回</a>";
                        break;
                }
                CreateDate = orderreturn.CreateDate.ToString("yyyy-MM-dd");
                ReturnContent = orderreturn.ReturnContent;
                BindGoods();
            }
            else
            {
                Response.Redirect("orderlist.aspx");
            }
            if (!string.IsNullOrEmpty(Request["OffIcon"]) && Request["OffIcon"].ToString() == "true")
            {
                A_OffIcon(null, null);
            }
            if (!string.IsNullOrEmpty(Request["editIcon"]) && Request["editIcon"].ToString() == "true")
            {
                A_EditIcon(null, null);
            }
        //}
    }

    protected void A_OffIcon(object sender, EventArgs e)
    {
        orderreturn.dr = 1;
        orderreturn.ts = DateTime.Now;
        orderreturn.modifyuser = this.UserID;
        if (new Hi.BLL.DIS_OrderReturn().Update(orderreturn))
        {
            order.ReturnState = 0;
            order.ts = DateTime.Now;
            order.modifyuser = this.UserID;
            if (new Hi.BLL.DIS_Order().Update(order))
            {
                string str = "\"str\":true";
                str = "{" + str + "}";
                Response.Write(str);
                Response.End();
            }
        }
    }

    protected void A_EditIcon(object sender, EventArgs e)
    {
        Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(this.UserName);
        orderreturn.ReturnState = (int)Enums.AuditState.提交;
        orderreturn.ReturnDate = DateTime.Now;
        orderreturn.ReturnUserID = User.ID;
        orderreturn.ts = DateTime.Now;
        orderreturn.modifyuser = User.ID;
        if (new Hi.BLL.DIS_OrderReturn().Update(orderreturn))
        {
            order.ReturnState = (int)Enums.ReturnState.申请退货;
            order.ReturnMoneyUser = User.TrueName;
            order.ReturnMoneyUserId = User.ID;
            order.ReturnMoneyDate = DateTime.Now;
            order.ts = DateTime.Now;
            order.modifyuser = User.ID;
            if (new Hi.BLL.DIS_Order().Update(order))
            {
                Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "申请退货", orderreturn.ReturnContent);
                new Common().GetWxService("4", KeyID.ToString(), "1");
                string str = "\"str\":true";
                str = "{" + str + "}";
                Response.Write(str);
                Response.End();
            }
        }
    }

    public void BindGoods()
    {
        SelectGoods.Clear(this.CompID);
        SelectGoods.OrderDetail(KeyID, this.DisID, this.CompID);
        DataTable dt = Session["GoodsInfo"] as DataTable;
        if (dt != null)
        {
            this.rptgoods.DataSource = dt;
            this.rptgoods.DataBind();
        }
        else
        {
            this.rptgoods.DataSource = "";
            this.rptgoods.DataBind();
        }
        SelectGoods.Clear(this.DisID, this.CompID);
    }

    
}