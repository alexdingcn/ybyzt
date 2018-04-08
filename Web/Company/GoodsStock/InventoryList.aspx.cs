using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_GoodsStock_InventoryList : CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataBinds();
            
        }
    }

    public void DataBinds()
    {
        int Counts = 0;
        ////每页显示的数据设置
        //if (this.txtPageSize.Value.ToString() != "")
        //{
        //    if (this.txtPageSize.Value.Trim().Length >= 5)
        //    {
        //        Pager.PageSize = 100;
        //        this.txtPageSize.Value = "100";
        //    }
        //    else
        //    {
        //        Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
        //    }
        //}
        DataTable LUser = new Hi.BLL.DIS_StockOrder().GetListPage(Pager.PageSize, Convert.ToInt32(page), SearchWhere());
        this.Rpt_Company.DataSource = LUser;
        this.Rpt_Company.DataBind();
        Counts = new Hi.BLL.DIS_StockOrder().GetPageCount(SearchWhere());
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    public void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        DataBinds();
    }

    public void btnSearch_Click(object sender, EventArgs e)
    {
        DataBinds();
    }

    public string SearchWhere()
    {
        string where = " and CompID=" + CompID + " and Type=3  and dr=0 ";
        if (this.txtCreateDate.Value != "" && this.txtEndCreateDate.Value == "")
        {
            where += "and ChkDate>='" + Convert.ToDateTime(this.txtCreateDate.Value).AddHours(0.00000) + "'";
        }
        if (this.txtEndCreateDate.Value != "" && this.txtCreateDate.Value == "")
        {
            where += "and ChkDate<='" + Convert.ToDateTime(this.txtEndCreateDate.Value).AddHours(23.99999) + "'";
        }
        if (this.txtCreateDate.Value!=""&&this.txtEndCreateDate.Value != "")
        {
            where += "and ChkDate>='" + Convert.ToDateTime(this.txtCreateDate.Value).AddHours(0.00000) + "'  and ChkDate<='" + Convert.ToDateTime(this.txtEndCreateDate.Value).AddHours(23.99999) + "'";
        }
        if (!string.IsNullOrWhiteSpace(this.txtReceiptNo.Value.Trim()))
        {
            where += " and OrderNO like '%" + this.txtReceiptNo.Value.Trim() + "%'";
        }
        if (this.DropDownList2.Text != "1")
        {
            where += " and State='" + this.DropDownList2.Text + "'";
        }
        return where;
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