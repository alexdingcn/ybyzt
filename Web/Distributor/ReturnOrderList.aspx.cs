

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_ReturnOrderList : DisPageBase
{
    public string page = "1";//默认初始页
    //Hi.Model.SYS_Users user = null;
    Hi.Model.BD_Distributor dis = null;
    protected void Page_Load(object sender, EventArgs e)
    {

        //user = LoginModel.IsLogined(this);
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
       
        if (!IsPostBack)
        {
            txtCreateDate.Value = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-dd");
            txtEndCreateDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            txtPager.Value = "12";
            Bind();
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
        strwhere += "and Otype!=9 and isnull(dr,0)=0 and ReturnState <>0 and disid=" + this.DisID;
        
        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length < 4)
            {
                Pager.PageSize = int.Parse(this.txtPager.Value.Trim());
            }
            else
            {
                this.txtPager.Value = "100";
                Pager.PageSize = 100;
            }
        }

        List<Hi.Model.DIS_Order> orders = new Hi.BLL.DIS_Order().GetList(Pager.PageSize, Pager.CurrentPageIndex, "ReturnMoneyDate", true, strwhere, out pageCount, out Counts);
        this.rptOrder.DataSource = orders;
        this.rptOrder.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    public string GetROrder(string orderid)
    {
        Hi.Model.DIS_OrderReturn rorder = new Hi.BLL.DIS_OrderReturn().GetModel(orderid);
        if (rorder == null)
        {
            return "";
        }
        string str = "";
        switch (rorder.ReturnState)
        {
            case -1: str = "已拒绝"; break;
            case 0: str = "未提交"; break;
            case 1: str = "待审核"; break;
            case 2: str = "已退货"; break;
            case 4: str = "已退货款"; break;
        }
        return str;
    }

    public string GetReceiptNo(string orderid)
    {
        Hi.Model.DIS_OrderReturn rorder = new Hi.BLL.DIS_OrderReturn().GetModel(orderid);
        if (rorder == null)
        {
            return "";
        }
        return rorder.ReceiptNo;
    }

    public void A_Seek(object sender, EventArgs e)
    {
        string strwhere = string.Empty;
        if (!string.IsNullOrEmpty(orderid.Value.Trim()))
        {
            //strwhere += " and receiptno like ('%" + orderid.Value.Trim().Replace("'", "''") + "%')";
            strwhere += " and id in (select orderid from DIS_OrderReturn where dr=0 and receiptno like '%" + Common.NoHTML(orderid.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (styte.Value != "-2")
        {
            strwhere += " and id in (select orderid from DIS_OrderReturn where dr=0 and ReturnState=" + Common.NoHTML(styte.Value) + ")";
        }
        if (this.ddrOtype.Value != "-1")
        {
            strwhere += " and Otype=" + Common.NoHTML(this.ddrOtype.Value);
        }
        if (txtCreateDate.Value.Trim() != "")
        {
            strwhere += " and Id in (select OrderID from DIS_OrderReturn where CreateDate>='" + txtCreateDate.Value + "')";
        }
        if (txtEndCreateDate.Value.Trim() != "")
        {
            strwhere += " and Id in (select OrderID from DIS_OrderReturn where CreateDate<'" + Convert.ToDateTime(txtEndCreateDate.Value).AddDays(1) + "')";
        }
        ViewState["strwhere"] = strwhere;
        Bind();
    }
}