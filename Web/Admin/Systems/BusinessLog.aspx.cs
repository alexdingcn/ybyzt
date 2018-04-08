using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Systems_BusinessLog : System.Web.UI.Page
{
    public string page = "1";  //默认初始页

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            txtEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            txtDate.Value = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            Bind();
        }
    }

    public void Bind()
    {
        string CreateDate = txtDate.Value + " 0:0:0";
        string EndCreateDate = txtEndDate.Value + " 23:59:59";
        string strwhere = "1=1 and LogTime between '" + CreateDate + "' and '" + EndCreateDate + "'";

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }

        LoginLog login = new LoginLog(Returnsql(strwhere));
        Pager.RecordCount = login.getDataCount();
        DataTable dt = login.getData(Pager.PageSize, Pager.StartRecordIndex - 1);

        this.rtp_loginLog.DataSource = dt;
        this.rtp_loginLog.DataBind();
    }

    public string Returnsql(string sqlwhere)
    {
        string sql = string.Format("select * from A_SystemLog where {0} order by LogTime desc", sqlwhere);
        return sql;
    }

    public void Pager_PageChanged(object sender, EventArgs e)
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
        ViewState["strwhere"] = Where();
        Bind();
    }

    /// <summary>
    /// 搜索条件
    /// </summary>
    /// <returns></returns>
    public string Where()
    {
        string strWhere = string.Empty;

        if (this.txtLoginName.Value != "")
        {
            strWhere += " and LogUName like '%" + Common.NoHTML(this.txtLoginName.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (this.txtModule.Value!="")
        {
            strWhere += " and Module like '%" + Common.NoHTML(this.txtModule.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (this.txtCompName.Value != "")
        {
            strWhere += " and CompName like '%" + Common.NoHTML(this.txtCompName.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (this.ddlloginUserType.Value != "-1")
        {
            strWhere += " and UserType=" + Common.NoHTML(this.ddlloginUserType.Value);
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