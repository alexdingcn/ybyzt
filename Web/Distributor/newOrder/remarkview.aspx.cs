using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hi.BLL;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services;
using System.Web.Script.Serialization;
public partial class Distributor_newOrder_remarkview : System.Web.UI.Page
{
    //订单、订单明细ID
    public int KeyID = 0;
    //商品ID
    public int infoid = 0;
    //代理商ID
    public int DisID = 0;
    //修改类型  0、订单备注， 1、商品备注 2、下单商品备注
    public int type = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(Request["type"]))
                type = Request["type"].ToString().ToInt(0);

            this.hidDisId.Value = DisID.ToString();
            this.hidType.Value = type.ToString();

            if (type == 0)
                KeyID = Common.DesDecrypt((Request["KeyID"] + ""), Common.EncryptKey).ToInt(0);
            else
                KeyID = (Request["KeyID"] + "").ToInt(0);
            this.hidGoodsInfo.Value = KeyID.ToString();
            this.hidIndex.Value = Request["index"] != null ? Request["index"].ToString() : "";

            if (type == 0)
            {
                Hi.Model.DIS_Order o = new Hi.BLL.DIS_Order().GetModel(KeyID);
                this.txtremark.Value = o.Remark;
                txtremark.Attributes.Add("placeholder", "选填：对本次交易的说明（限200字）。");
                txtremark.Attributes.Add("maxlength", "400");

            }
            else if (type == 1)
            {
                Hi.Model.DIS_OrderDetail od = new Hi.BLL.DIS_OrderDetail().GetModel(KeyID);
                this.txtremark.Value = od.Remark;
                txtremark.Attributes.Add("placeholder", "选填：对本次交易商品的说明（限150字）。");
                txtremark.Attributes.Add("maxlength", "150");
            }
            else
            {
                //DataTable dt = HttpContext.Current.Session["GoodsInfo"] as DataTable;
                txtremark.Attributes.Add("placeholder", "选填：对本次交易商品的说明（限150字）。");
                txtremark.Attributes.Add("maxlength", "150");
                ClientScript.RegisterStartupScript(this.GetType(), "121", "<script>type3();</script>");

                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    DataRow[] dr = dt.Select(" GoodsInfoID=" + KeyID);
                //    if (dr.Length > 0)
                //    {
                //        this.txtremark.Value = dr[0]["Remark"].ToString();
                //    }
                //}
                //this.txtremark.Value = Request["remark"] != null ? Request["remark"].ToString() : "";
            }
        }

    }

    //public void Bind() 
    //{


    //    if (KeyID!=0)
    //    {
    //        //判断改该条数据代理商是否有操作权限
    //        if (!Common.PageDisOperable("newOrder", KeyID, DisID))
    //        {
    //            Response.Redirect("../NoOperable.aspx");
    //            return;
    //        }
    //        Hi.Model.DIS_Order orderModel = orderBll.GetModel(KeyID);
    //        if (orderModel != null)
    //        {
    //            this.hidDisId.Value = orderModel.DisID.ToString();
    //            this.hidType.Value =orderModel.AddType.ToString();
    //            this.txtremark.Value = orderModel.Remark;
    //        }


    //        Response.Write("<script>alert(" + KeyID + ")</script>");
    //    }

    //        //JScript.AlertMsgOne(this, "请选择数据!", JScript.IconOption.错误, 2500);

    //}
    [WebMethod]
    public static string Edit(string KeyID, string type, string remark)
    {
        remark = Common.NoHTML(remark);
        Common.ResultMessage Msg = new Common.ResultMessage();

        if (type == "0")
        {
            Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
            int oID = Common.DesDecrypt(KeyID, Common.EncryptKey).ToInt(0);
            Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(oID);
            if (OrderModel != null)
            {
                OrderModel.Remark = Common.NoHTML(remark);
                OrderModel.ts = DateTime.Now;
                if (OrderBll.Update(OrderModel))
                {
                    Msg.result = true;
                    Msg.code = OrderModel.ts.ToString("yyyy-MM-dd HH:mm");
                }
            }
            else
            {
                Msg.code = "未查找到数据";
            }
        }
        else if (type == "1")
        {
            Hi.BLL.DIS_OrderDetail OrderBllDetail = new Hi.BLL.DIS_OrderDetail();
            Hi.Model.DIS_OrderDetail OrderModelDetail = OrderBllDetail.GetModel(KeyID.ToInt(0));
            if (OrderModelDetail != null)
            {
                OrderModelDetail.Remark = Common.NoHTML(remark);
                OrderModelDetail.ts = DateTime.Now;
                if (OrderBllDetail.Update(OrderModelDetail))
                {
                    Msg.result = true;
                }
            }
        }
        else
        {
            Msg.result = true;
        }
        return new JavaScriptSerializer().Serialize(Msg);
    }

}