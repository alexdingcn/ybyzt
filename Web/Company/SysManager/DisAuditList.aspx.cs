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
        //List<Hi.Model.BD_Distributor> LDis = new Hi.BLL.BD_Distributor().GetList(Pager.PageSize, Pager.CurrentPageIndex, "Createdate", false, SearchWhere(), out pageCount, out Counts);
        
        DataTable LDis = new Hi.BLL.BD_Distributor().getDataTable(Pager.PageSize, Pager.CurrentPageIndex, SearchWhere(), out pageCount, out Counts, Pager.RecordCount);

        this.Rpt_Distribute.DataSource = LDis;
        this.Rpt_Distribute.DataBind();
        Pager.RecordCount = LDis.Rows.Count;
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


    /// <summary>
    /// 查询条件
    /// </summary>
    /// <returns></returns>
    public string SearchWhere() {
        string where = " and cu.compid=" + CompID + " and cu.IsAudit=0  and isnull(cu.dr,0)=0 and isnull(dis.dr,0)=0";
        if (DisSalesManID != 0)
        {
            where += "and cu.DisID in(select ID from BD_Distributor where smid = " + DisSalesManID + ")";
        }
        if (!string.IsNullOrEmpty(txtDisName.Value.Trim())) {
            where += " and  DisName like '%" + Common.NoHTML(txtDisName.Value.Trim().Replace("'", "''")) + "%'";
        }
        //if (!string.IsNullOrEmpty(BoxArea.areaId))
        //{
        //    where += " and areaid='" + BoxArea.areaId + "'";
        //}
        //if (ddlDisLevel.SelectedValue != "-1")
        //{
        //    where += " and DisLevel='" + ddlDisLevel.SelectedItem.Text + "'";
        //}
        if (!string.IsNullOrEmpty(txtPhone.Value.Trim()))
        {
            where += " and Phone like '%" + Common.NoHTML(txtPhone.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (!string.IsNullOrEmpty(txtPrincipal.Value.Trim()))
        {
            where += " and Principal like '%" + Common.NoHTML(txtPrincipal.Value.Trim().Replace("'", "''")) + "%'";
        }
        return where;
    }


    //导入Eecel返回Dataset的方法
    public static DataSet ReaderExcelToDataset(string filePath)
    {
        string connStr = "";
        string fileType = System.IO.Path.GetExtension(filePath);
        if (string.IsNullOrEmpty(fileType)) return null;
        //根据文件的类型定义链接映射字段
        //if (fileType == ".xls")
        //    connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + "Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
        //else
            connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + "Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
        string sql_F = "Select * FROM [{0}]";

        OleDbConnection conn = null;
        OleDbDataAdapter da = null;
        DataTable dtSheetName = null;

        DataSet ds = new DataSet();
        try
        {
            // 初始化连接，并打开 
            conn = new OleDbConnection(connStr);
            conn.Open();

            // 获取数据源的表定义元数据                        
            string SheetName = "";
            dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

            // 初始化适配器
            da = new OleDbDataAdapter();
            for (int i = 0; i < dtSheetName.Rows.Count; i++)
            {
                SheetName = (string)dtSheetName.Rows[i]["TABLE_NAME"];

                if (SheetName.Contains("$") && !SheetName.Replace("'", "").EndsWith("$"))
                {
                    continue;
                }

                da.SelectCommand = new OleDbCommand(String.Format(sql_F, SheetName), conn);
                DataSet dsItem = new DataSet();
                da.Fill(dsItem, SheetName.Replace("$", ""));
                ds.Tables.Add(dsItem.Tables[0].Copy());
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
        finally
        {
            // 关闭连接
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
                da.Dispose();
                conn.Dispose();
            }
        }

        return ds;
    }
}