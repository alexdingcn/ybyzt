using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Distributor_Storage_selectHtList : System.Web.UI.Page
{
    public string page = "1";//默认初始页
    public int PageSize = 6;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.index.Value = Request.QueryString["index"];
            Bind();
        }
    }

    /// <summary>
    /// 绑定销售订单信息列表
    /// </summary>
    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;
        string strwhere =  Where();
        List<Hi.Model.SYS_Hospital> hospital = new Hi.BLL.SYS_Hospital().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = hospital;
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
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {

        Bind();
    }

    public string Where()
    {
        string strWhere = " and dr=0";

        if (this.txtHtName.Value.Trim() != "")
        {
            strWhere += " and HospitalName like '%" + this.txtHtName.Value.Trim() + "%'";
        }
        return strWhere;
    }
}