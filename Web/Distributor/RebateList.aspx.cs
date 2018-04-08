using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_RebateList : DisPageBase
{
    public string page = "1";//默认初始页
    Hi.Model.BD_Distributor dis = null;
    public decimal ta = 0;
    public decimal tb = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();
        }
    }

    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;

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

        List<Hi.Model.BD_Rebate> orders = new Hi.BLL.BD_Rebate().GetList(Pager.PageSize, Pager.CurrentPageIndex, "EndDate", true, SearchWhere(), out pageCount, out Counts);

        //计算总计 begin

        foreach (Hi.Model.BD_Rebate rebate in orders)
        {
            ta += Convert.ToDecimal(rebate.RebateAmount.ToString() == "" ? "0" : rebate.RebateAmount.ToString());
            if (rebate.RebateState == 1)
                tb += Convert.ToDecimal(rebate.EnableAmount.ToString() == "" ? "0" : rebate.EnableAmount.ToString());

        }
        //计算总计  end


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

    public void btnSearch_Click(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = "  and isnull(dr,0)=0 and disID=" + DisID + "";

        if (!string.IsNullOrEmpty(txtRebateCode.Value.Trim()))
        {
            where += " and (ReceiptNo like '%" + Common.NoHTML(txtRebateCode.Value.Trim().Replace("'", "''")) + "%' )";
        }

        if (!string.IsNullOrEmpty(ddlRebateState.SelectedValue) && ddlRebateState.SelectedValue != "0")
        {
            where += " and RebateState='" + Common.NoHTML(ddlRebateState.SelectedValue) + "'";
        }

        return where;
    }
}