using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Distributor_ReceivingList1 : DisPageBase
{
    public string page = "1";//默认初始页
    //Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        //user = LoginModel.IsLogined(this);
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();
        }
        if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "sing")
        {
            btnSing_Click(Convert.ToInt32(Request["orderid"].ToString()));
        }
    }

    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        strwhere += " and disid=" + this.DisID + " and isnull(dr,0)=0 and IsSign=1";

        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length < 4)
            {
                Pager.PageSize = int.Parse(this.txtPager.Value.Trim());
            }
            else
            {
                this.txtPager.Value = "12";
                Pager.PageSize = 12;
            }
        }

        List<Hi.Model.DIS_OrderOut> orders = new Hi.BLL.DIS_OrderOut().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", true, strwhere, out pageCount, out Counts);
        this.rptving.DataSource = orders;
        this.rptving.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    public string GetOrderID(int orderid)
    {
        Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(orderid);
        if (order != null)
        {
            return order.ReceiptNo;
        }
        return "";
    }

    public string GetOrderdatetime(int orderid)
    {
        Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(orderid);
        if (order != null)
        {
            return order.CreateDate.ToString("yyyy-MM-dd");
        }
        return "";
    }

    public decimal GetOrderdatemy(int orderid)
    {
        Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(orderid);
        if (order != null)
        {
            return Math.Round(order.AuditAmount, 2);
        }
        return 0;
    }

    public void A_Seek(object sender, EventArgs e)
    {
        string strwhere = string.Empty;
        if (!string.IsNullOrEmpty(txtorderReceiptNo.Value.Trim()))
        {
            DataSet order = new Hi.BLL.DIS_Order().GetData(txtorderReceiptNo.Value.Trim());
            if (order.Tables[0].Rows.Count > 0)
            {
                string str = "";
                //for (int i = 0; i < order.Tables[0].Rows.Count; i++)
                //{
                //    str += order.Tables[0].Rows[i]["id"] + ",";
                //}
                // strwhere += " and OrderID in (" + str.TrimEnd(',') + ")";
                strwhere += " and orderid in(select id from dis_order where receiptno like '%" + Common.NoHTML(txtorderReceiptNo.Value.Trim().Replace("'", "''")) + "%' and dr=0)";
            }
            else
            {
                strwhere += " and OrderID=0";
            }
        }
        //if (!string.IsNullOrEmpty(txtname.Value.Trim()))
        //{
        //    strwhere += " and SignUser like '%" + txtname.Value.Trim() + "%'";
        //}
        //if (txtArriveDate.Value.Trim() != "")
        //{
        //    strwhere += " and orderid in(select id from dis_order where createDate >= '" + txtArriveDate.Value.Trim() + "' and dr=0)";
        //}
        //if (txtArriveDate1.Value.Trim() != "")
        //{
        //    strwhere += " and orderid in(select id from dis_order where createDate < '" + Convert.ToDateTime(txtArriveDate1.Value.Trim()).AddDays(1) + "' and dr=0)";
        //}

        if (txtArriveDate.Value.Trim() != "")
        {
            strwhere += " and  SignDate>= '" + txtArriveDate.Value.Trim() + "'";
        }
        if (txtArriveDate1.Value.Trim() != "")
        {
            strwhere += " and SignDate < '" + Convert.ToDateTime(txtArriveDate1.Value.Trim()).AddDays(1) + "'";
        }

        //if (IsSign.Value != "-1")
        //{
        //    strwhere += " and IsSign=" + IsSign.Value.Trim();
        //}
        ViewState["strwhere"] = strwhere;
        Bind();
    }

    /// <summary>
    /// 签收
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void btnSing_Click(int KeyID)
    {
        Hi.BLL.DIS_Order OrderBll = new Hi.BLL.DIS_Order();
        Hi.Model.DIS_Order OrderModel = OrderBll.GetModel(KeyID);

        if (OrderModel.OState == (int)Enums.OrderState.已发货 && OrderModel.ReturnState == (int)Enums.ReturnState.未退货)
        {
            Hi.Model.DIS_OrderOut OutModel = new Hi.BLL.DIS_OrderOut().GetOutModel(KeyID);

            OutModel.SignDate = DateTime.Now;
            OutModel.SignRemark = "";
            OutModel.SignUser = this.UserName;
            OutModel.SignUserId = this.UserID;
            OutModel.IsSign = 1;
            OutModel.ts = DateTime.Now;
            OutModel.modifyuser = this.UserID;

            if (OrderInfoType.SignOrder(OutModel, OrderModel) > 0)
            {
                Utils.AddSysBusinessLog(this.CompID, "Order", KeyID.ToString(), "订单签收", "");
                string str = "\"str\":true";
                str = "{" + str + "}";
                Response.Write(str);
                Response.End();
                //Bind();
            }
        }
    }
}