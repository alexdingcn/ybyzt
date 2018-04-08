using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Distributor_Payment_PaymentList : DisPageBase
{
    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id
    public int PageSize = 12;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = Common.PageSize;
          

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
        string strwhere = Where();

        List<Hi.Model.YZT_Payment> list = new Hi.BLL.YZT_Payment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreateDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = list;
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
        string strWhere = " and DisID="+DisID+" and dr=0";

        if (this.PaymentNO.Value.Trim() != "")
        {
            strWhere += " and PaymentNO like '%" + this.PaymentNO.Value.Trim() + "%'";
        }
        if (this.ddrState.Value != "")
        {
            strWhere += " and IState =" + this.ddrState.Value.Trim();
        }
        if (this.txtCreateDate.Value != "")
        {
            strWhere += " and PaymentDate>='" + this.txtCreateDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndCreateDate.Value != "")
        {
            strWhere += "and PaymentDate<'" + this.txtEndCreateDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        return strWhere;
    }


    public string getHname(string id)
    {
        Hi.Model.SYS_Hospital h = new Hi.BLL.SYS_Hospital().GetModel(Convert.ToInt32(id));
        if (h == null)
            return id;
        else
            return h.HospitalName;
    }

}