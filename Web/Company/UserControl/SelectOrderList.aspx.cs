using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_UserControl_SelectOrderList : System.Web.UI.Page
{
   public string page = "1";//默认初始页
   string Compid
   {
       get { return Request["compid"] + ""; }
   }
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
        List<Hi.Model.DIS_Order> LDis = new Hi.BLL.DIS_Order().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Dis.DataSource = LDis;
        this.Rpt_Dis.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {

        string where = "  and OState=2 and PayState=0 and isnull(dr,0)=0 and Otype!=9 and CompID=" + Compid + "";
        if (!string.IsNullOrEmpty(txtOrdercode.Value.Trim()))
        {
            where += " and ( receiptno like '%" + txtOrdercode.Value.Trim() + "%')";
        }       

        //if (!string.IsNullOrEmpty(txtDisSname.Value.Trim()))
        //{
        //    where += " and ShortName like'%" + txtDisSname.Value.Trim() + "%'";
        //}
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