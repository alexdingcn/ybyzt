using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

public partial class Admin_Systems_CompAuditList : AdminPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        string Action = Request["Action"] + "";
        string OrgID =Common.NoHTML( Request["OrgID"]) + "";
        if (Action == "Action")
        {
            Response.Write(Common.getsaleman(OrgID));
            Response.End();
        }
        
        if (!IsPostBack) {
            Common.BindOrgSale(Org, SaleMan, "全部");
            if (Request.QueryString["page"] != null)
            {
                page = Request.QueryString["page"].ToString();
                Pager.CurrentPageIndex = Convert.ToInt32(page);
            }
            Common.BindIndDDL(txtIndusName);
            DataBinds();
        }
    }




    public void DataBinds()
    {
        int pageCount = 0;
        int Counts = 0;
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
        List<Hi.Model.BD_Company> LDis = new Hi.BLL.BD_Company().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Comp.DataSource = LDis;
        this.Rpt_Comp.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
        this.Org.SelectedValue = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
    }


    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string where = " and auditstate=0  and isnull(dr,0)=0 ";
        if (SalesManID > 0)
        {
            where += " and SalesManID=" + SalesManID + "";
        }
        if (OrgID>0)
        {
            where += " and OrgID=" + OrgID + " ";
        }
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            where += " and (CompCode like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%' or CompName like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (txtIndusName.SelectedValue != "-1" && txtIndusName.SelectedValue != "")
        {
            where += " and indid=" + txtIndusName.SelectedValue;
        }
        if (!string.IsNullOrEmpty(txtMinfo.Value.Trim()))
        {
            where += " and manageinfo like '%" + Common.NoHTML(txtMinfo.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (!string.IsNullOrEmpty(txtTel.Value.Trim()))
        {
            where += " and tel like '%" + Common.NoHTML(txtTel.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (Org.SelectedValue != "-1")
        {
            string org = this.OrgID == 0 ? Org.SelectedValue : this.OrgID.ToString();
            where += " and OrgID='" +Common.NoHTML( org) + "'";
        }
        if (salemanid.Value != "-1" && salemanid.Value != "")
        {
            where += " and SalesManID='" + salemanid.Value + "'";
        }
        if (!string.IsNullOrWhiteSpace(hidProvince.Value.Trim()) && hidProvince.Value.Trim() != "选择省")
            where += " and CompAddr like'%" + hidProvince.Value.Trim() + "%'";
        if (!string.IsNullOrWhiteSpace(hidCity.Value.Trim()) && hidCity.Value.Trim() != "选择市")
            where += " and CompAddr like'%" + hidCity.Value.Trim() + "%'";
        if (!string.IsNullOrWhiteSpace(hidArea.Value.Trim()) && hidArea.Value.Trim() != "选择区")
            where += " and CompAddr like'%" + hidArea.Value.Trim() + "%'";
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