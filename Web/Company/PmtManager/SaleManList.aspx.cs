using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_OrgManage_SaleManList : CompPageBase
{
    public string page = "1";//默认初始页
    public DropDownList allSaleMan = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    public void Bind()
    {
        int pageCount = 0;
        int Counts = 0;

        //每页显示的数据设置
        if (this.txtPageSize.Value.ToString() != "")
        {
            if (this.txtPageSize.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPageSize.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPageSize.Value.Trim().ToInt(0);
            }
        }
        List<Hi.Model.BD_DisSalesMan> LAdminuser = new Hi.BLL.BD_DisSalesMan().GetList(Pager.PageSize, Pager.CurrentPageIndex, "id", false, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Distribute.DataSource = LAdminuser;
        this.Rpt_Distribute.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }
    public void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }
    public void btn_SearchClick(object sender, EventArgs e)
    {
        Bind();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = " and isnull(dr,0)=0 and Compid="+CompID+"";
        if (!string.IsNullOrEmpty(salename.Value.Trim()))
        {
            where += " and (SalesName like '%" +Common.NoHTML( salename.Value.Trim().Replace("'", "''")) + "%' )";
        }
        if (!string.IsNullOrEmpty(salecode.Value.Trim()))
        {
            where += " and (SalesCode like '%" +Common.NoHTML( salecode.Value.Trim().Replace("'", "''")) + "%' )";
        }
        if (!string.IsNullOrEmpty(sltIsAllow.SelectedValue))
        {
            where += " and IsEnabled='" + sltIsAllow.SelectedValue + "'";
        }
        return where;
    }

    public string FindDisNameBySMID(string SMID)
    {
         
        string DisName = "";
        List<Hi.Model.BD_Distributor> dis = new Hi.BLL.BD_Distributor().GetList(""," isnull(dr,0)=0 and CompID="+CompID+" and SMID="+SMID+""," createdate asc");
        foreach (Hi.Model.BD_Distributor model in dis)
        {
            if (string.IsNullOrEmpty(DisName))
            {
                DisName += model.DisName;
            }
            else
            {
                DisName += "，"+model.DisName;
            }
        }
        return DisName;
    }
}