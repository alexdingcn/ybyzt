using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Data.SqlClient;
using ICSharpCode.SharpZipLib;


public partial class Company_SysManager_DisAuditList : CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>$(\".txt_txtAreaname\").css(\"width\", \"150px\");</script>");
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

    public void btn_SearchClick(object sender, EventArgs e)
    {
        DataBinds();
    } 
    /// <summary>
    /// 数据绑定
    /// </summary>
    public void DataBinds() {

        int pageCount = 0;
        int Counts = 0;
        string disName = this.txtDisName.Value.Trim();

        Pager.PageSize = txtPageSize.Value.ToInt(0);

        DataTable dt = new Hi.BLL.YZT_FCmaterials().getDataTable(Pager.PageSize, Pager.CurrentPageIndex, CompID.ToString(), disName, out pageCount, out Counts);
        this.Rpt_Distribute.DataSource = dt;
        this.Rpt_Distribute.DataBind();
        Pager.RecordCount = dt.Rows.Count;
        page = Pager.CurrentPageIndex.ToString();
    }


    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Pager_PageChanged(object sender, EventArgs e) {
        DataBinds();
        page = Pager.CurrentPageIndex.ToString();
    }




}