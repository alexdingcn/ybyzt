

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UserControl_SelectCompList : System.Web.UI.Page
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
        List<Hi.Model.BD_Company> LDis = new Hi.BLL.BD_Company().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, SearchWhere(), out pageCount, out Counts);
        this.Rpt_Comp.DataSource = LDis;
        this.Rpt_Comp.DataBind();
        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere()
    {
        string Ishow = "";
        if (Request.UrlReferrer != null)
        {
            if (System.IO.Path.GetFileName(Request.UrlReferrer.ToString()) == "Disregister.aspx")
            {
                Ishow = " and hotshow=1 ";
            }
        }
        string where = "  and auditstate=2 and IsEnabled=1 and isnull(dr,0)=0 " + Ishow + " ";
        if (!string.IsNullOrEmpty(txtCompName.Value.Trim()))
        {
            where += " and (CompName like '%" + Common.NoHTML(txtCompName.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (!string.IsNullOrEmpty(txtPerson.Value.Trim()))
        {
            where += " and ( Principal like '%" + Common.NoHTML(txtPerson.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (!string.IsNullOrEmpty(txtPerPhone.Value.Trim()))
        {
            where += " and (Phone like '%" + Common.NoHTML(txtPerPhone.Value.Trim().Replace("'", "''")) + "%')";
        }
        if (HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser != null) {
            if ((HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser).UserType == 3 || (HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser).UserType == 4) {
                where += " and orgid=" + (HttpContext.Current.Session["AdminUser"] as Hi.Model.SYS_AdminUser).OrgID + "";
            }
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