using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_GoodsStock_OutStorageList :CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.hid_type.Value = "0";
            SearchWhere();
        }
    }

    public void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        SearchWhere();
    }

    public void btnSearch_Click(object sender, EventArgs e)
    {
        SearchWhere();
    }

    public void SearchWhere()
    {
        int Counts = 0;
        DataTable dt = new Hi.BLL.DIS_StockInOut().GetListPage(Pager.PageSize, Pager.CurrentPageIndex,where());
        this.Rpt_Company.DataSource = dt;
        this.Rpt_Company.DataBind();
        Counts = new Hi.BLL.DIS_StockInOut().GetPageCount(where());
        Pager.RecordCount =Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    public string where()
    {
        string sql = " DIS_StockInOut.CompID=" + CompID + "  and  DIS_StockInOut.dr=0  and  DIS_StockOrder.dr=0 ";
        if (this.txtReceiptNo.Value != "")
        {
            sql += " and  (BD_GoodsInfo.BarCode LIKE '%" + Common.NoHTML(this.txtReceiptNo.Value.Trim()) + "%' OR BD_Goods.GoodsName LIKE '%" + Common.NoHTML(this.txtReceiptNo.Value.Trim()) + "%') ";
        }
        if (this.txtCreateDate.Value != "" && this.txtEndCreateDate.Value == "")
        {
            sql += "and DIS_StockInOut.CreateDate>='" + Convert.ToDateTime(this.txtCreateDate.Value).AddHours(0.00000) + "'";
        }
        if (this.txtEndCreateDate.Value != "" && this.txtCreateDate.Value == "")
        {
            sql += "and DIS_StockInOut.CreateDate<='" + Convert.ToDateTime(this.txtEndCreateDate.Value).AddHours(23.99999) + "'";
        }
        if (this.txtCreateDate.Value != "" && this.txtEndCreateDate.Value != "")
        {
            sql += "and DIS_StockInOut.CreateDate>='" + Convert.ToDateTime(this.txtCreateDate.Value).AddHours(0.00000) + "'  and DIS_StockInOut.CreateDate<='" + Convert.ToDateTime(this.txtEndCreateDate.Value).AddHours(23.99999) + "'";
        }
        if (this.DropDownList1.Text != "0")
        {
            sql += "and DIS_StockOrder.Type='" + this.DropDownList1.Text + "'";
        }
        return sql;
    }


    public string GetName(int ID)
    {
        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(ID);
        if (user != null)
        {
            return user.TrueName;
        }
        else
        {
            return ID.ToString();
        }
    }
}