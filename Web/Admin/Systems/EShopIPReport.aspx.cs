using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class EShopLogReport : System.Web.UI.Page
{
    public string page = "1";  //默认初始页

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //this.txtDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToShortDateString();
            this.txtDate.Value = DateTime.Now.ToShortDateString();
            this.txtEndDate.Value = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).ToShortDateString();
            Bind();
        }
    }

    public void Bind()
    {
        string strwhere = " and 1=1";

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        else
            strwhere += Where();

        LoginLog login = new LoginLog();
        //Pager.RecordCount = login.getDataCount();
        //DataTable dt = login.getData(Pager.PageSize, Pager.StartRecordIndex - 1);

        DataTable dt = login.getDataSet(Returnsql(strwhere));
        this.rtp_loginLog.DataSource = dt;
        this.rtp_loginLog.DataBind();
    }

    public string Returnsql(string sqlwhere)
    {
        string sql = string.Format("select top 30 COUNT(*) as num,LogUIp from (select * from A_SystemLog where 1=1 {0} ) a group by LogUIp order by num desc", sqlwhere);
        return sql;
    }

    public void Pager_PageChanged(object sender, EventArgs e)
    {
        Bind();
    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        ViewState["strwhere"] = Where();
        Bind();
    }

    /// <summary>
    /// 搜索条件
    /// </summary>
    /// <returns></returns>
    public string Where()
    {
        string strWhere = "";

        if (this.txtIPName.Value != "")
        {
            strWhere += " and LogUIp like '%" + Common.NoHTML(this.txtIPName.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (this.txtDate.Value.Trim() != "")
        {
            strWhere += " and LogTime>='" + this.txtDate.Value.Trim().ToDateTime() + "'";
        }
        if (this.txtEndDate.Value.Trim() != "")
        {
            strWhere += " and LogTime<='" + this.txtEndDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        return strWhere;
    }

    /// <summary>
    /// 登录类型
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string Login(object obj)
    {
        //1厂商、2代理商、3平台工作人员、4游客
        if (Convert.ToInt32(obj) == 1)
        {
            return "厂商";
        }
        if (Convert.ToInt32(obj) == 2)
        {
            return "代理商";
        }
        if (Convert.ToInt32(obj) == 3)
        {
            return "平台工作人员";
        }
        if (Convert.ToInt32(obj) == 4)
        {
            return "游客";
        }
        return "";
    }
}