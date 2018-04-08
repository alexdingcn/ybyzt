using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Admin_UserControl_SelectBankList : System.Web.UI.Page
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            DataBinds();
        }
    }

    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
        List<Hi.Model.BF_ZD_PAYBANK> LDis = new Hi.BLL.BF_ZD_PAYBANK().GetList(Pager.PageSize, Pager.CurrentPageIndex, "FQHHO2", false, SearchWhere(), out pageCount, out Counts, " FKHMC1,FQHHO2 ");
        this.Rpt_Bank.DataSource = LDis;
        this.Rpt_Bank.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = "";
        if (!string.IsNullOrEmpty(txtBankName.Value.Trim()))
        {
            where += " and (FKHMC1 like '%" + Common.NoHTML(txtBankName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (!string.IsNullOrEmpty(txtbankid.Value.Trim()))
        {
            where += " and ( FQHHO2 like '%" + Common.NoHTML(txtbankid.Value.Trim().Replace("'", "''")) + "%')";
        }
        return where;
    }

    protected void btn_SearchClick(object sender, EventArgs e)
    {
        DataBinds();
    }

    public void Pager_PageChanged(object sender, EventArgs e)
    {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }
}