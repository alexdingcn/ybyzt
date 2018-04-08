using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Distributor_FCmaterials_CompFCmateriarialsList : DisPageBase
{
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public string price = string.Empty;//企业钱包余额

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();

        }
    }


    /// <summary>
    /// 绑定
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strWhere = string.Empty;
        if (!string.IsNullOrWhiteSpace(this.textName.Value.Trim()))
            strWhere += string.Format(" and CompName like '%{0}%'  ", this.textName.Value.Trim());

        Pager.PageSize = this.txtPager.Value.ToInt(0);

         DataTable table = new Hi.BLL.YZT_FCmaterials().getCompDataTable(Pager.PageSize, Pager.CurrentPageIndex, DisID.ToString(), strWhere, out pageCount, out Counts);

        this.rptOrder.DataSource = table;
        this.rptOrder.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptOrder_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {   
        Bind();
    }

  


}