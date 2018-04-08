using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Systems_LoginLog : AdminPageBase
{
    public string page = "1";  //默认初始页
    public static string Connection = System.Configuration.ConfigurationManager.AppSettings["ConnectionString_Log"];

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
           
            txtEndDate.Value = DateTime.Now.ToString("yyyy-MM-dd");
            txtDate.Value = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
            //初始化数据
            DataTable dt = new LoginLog().getDataSet("select top 300 * from A_LoginLog where Module='系统安全模块'  order by LoginStartDate desc");
            foreach (DataRow dr in dt.Rows)
            {
                string str = "";
                int logintype = Convert.ToInt32(dr["LoginUserType"]);
                int loginid = Convert.ToInt32(dr["LoginId"]);
                if (logintype == 0)//系统管理员
                {
                    str = "系统管理员";
                }
                if (logintype == 1)//代理商
                {
                    List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and UType=" + logintype + "  and UserID=" + loginid + "", "");
                    if (ListCompUser.Count > 0)
                    {
                        List<Hi.Model.BD_Distributor> ListDIS = new Hi.BLL.BD_Distributor().GetList("", " dr=0 AND ID=" + ListCompUser[0].DisID + " ", "");
                        if (ListDIS != null)
                        {
                            str = ListDIS[0].DisName;
                        }
                        else
                        {
                            str = "";
                        }
                    }

                }
                if (logintype == 3)//厂商
                {
                    List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and UType=" + logintype + "  and UserID=" + loginid + " ", "");
                    if (ListCompUser.Count > 0)
                    {
                        List<Hi.Model.BD_Company> ListComp = new Hi.BLL.BD_Company().GetList("", " dr=0 AND ID=" + ListCompUser[0].CompID + " ", "");
                        if (ListComp.Count > 0)
                        {
                            str = ListComp[0].CompName;
                        }
                        else
                        {
                            str = "";
                        }
                    }
                }
                if (logintype == 4)//厂商
                {
                    List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and UType=" + logintype + "  and UserID=" + loginid + " ", "");
                    if (ListCompUser.Count > 0)
                    {
                        List<Hi.Model.BD_Company> ListComp = new Hi.BLL.BD_Company().GetList("", " dr=0 AND ID=" + ListCompUser[0].CompID + " ", "");
                        if (ListComp.Count > 0)
                        {
                            str = ListComp[0].CompName;
                        }
                        else
                        {
                            str = "";
                        }
                    }
                }
                if (logintype == 5)//代理商
                {
                    List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and UType=" + logintype + "  and UserID=" + loginid + "", "");
                    if (ListCompUser.Count > 0)
                    {
                        List<Hi.Model.BD_Distributor> ListDIS = new Hi.BLL.BD_Distributor().GetList("", " dr=0 AND ID=" + ListCompUser[0].DisID + " ", "");
                        if (ListDIS.Count > 0)
                        {
                            str = ListDIS[0].DisName;
                        }
                        else
                        {
                            str = "";
                        }
                    }
                }
                //执行update sql
                DBUtility.SqlHelper.ExecuteSql(Connection, "update A_LoginLog set Module='" + str + "' where Id= " + dr["Id"].ToString());
            }

            Bind();
        }
    }

    public void Bind()
    {
        string CreateDate = txtDate.Value + " 0:0:0";
        string EndCreateDate = txtEndDate.Value + " 23:59:59";
        string strwhere = "1=1 and LoginStartDate between '" + CreateDate + "' and '" + EndCreateDate + "' ";

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
        string sql = string.Format("select * from A_LoginLog where {0} order by [LoginStartDate] desc", sqlwhere);
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

        //企业/代理商名称
        if (this.txtCompName.Value != "")
        {
            strWhere += " and [Module] like '%" + Common.NoHTML(this.txtCompName.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (this.txtLoginName.Value != "")
        {
            strWhere += " and [LoginName] like '%" + Common.NoHTML(this.txtLoginName.Value.Trim().Replace("'", "''")) + "%'";
        }
        if (this.ddlloginUserType.Value != "-1")
        {
            strWhere += " and [LoginUserType]=" + Common.NoHTML(this.ddlloginUserType.Value);
        }
        if (this.txtDate.Value.Trim() != "")
        {
            strWhere += " and [LoginStartDate]>='" + this.txtDate.Value.Trim().ToDateTime() + "' and [LoginStartDate]<='" + this.txtEndDate.Value.Trim().ToDateTime().AddDays(1) + "'";
        }

        return strWhere;
    }

    /// <summary>
    /// 获取登录帐号的厂商名称
    /// </summary>
    /// <param name="id">登录帐号ID</param>
    /// <param name="type">登录帐号类型</param>
    /// <returns></returns>
    public string GetEnterpriseName(object id,object type)
    {
        int loginid = Convert.ToInt32(id);//登录帐号ID
        int logintype = Convert.ToInt32(type);//登录帐号类型
        string str = "";
        if (loginid!=0)
        {
            if (logintype == 0)//系统管理员
            {
                str = "系统管理员";
            }
            if (logintype == 1)//代理商
            {
                List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and UType=" + logintype + "  and UserID=" + loginid + "", "");
                if (ListCompUser.Count>0)
                {
                    List<Hi.Model.BD_Distributor> ListDIS = new Hi.BLL.BD_Distributor().GetList("", " dr=0 AND ID=" + ListCompUser[0].DisID + " ", "");
                    if (ListDIS!=null)
                    {
                        str = ListDIS[0].DisName;
                    }
                    else
                    {
                        str = "";
                    }
                }
                
            }
            if (logintype == 3)//厂商
            {
                List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and UType=" + logintype + "  and UserID=" + loginid + " ", "");
                if (ListCompUser.Count > 0)
                {
                    List<Hi.Model.BD_Company> ListComp = new Hi.BLL.BD_Company().GetList("", " dr=0 AND ID=" + ListCompUser[0].CompID + " ", "");
                    if (ListComp.Count>0)
                    {
                        str = ListComp[0].CompName;
                    }
                    else
                    {
                        str = "";
                    }
                }               
            }
            if (logintype == 4)//厂商
            {
                List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and UType=" + logintype + "  and UserID=" + loginid + " ", "");
                if (ListCompUser.Count > 0)
                {
                    List<Hi.Model.BD_Company> ListComp = new Hi.BLL.BD_Company().GetList("", " dr=0 AND ID=" + ListCompUser[0].CompID + " ", "");
                    if (ListComp.Count > 0)
                    {
                        str = ListComp[0].CompName;
                    }
                    else
                    {
                        str = "";
                    }
                }  
            }
            if (logintype == 5)//代理商
            {
                List<Hi.Model.SYS_CompUser> ListCompUser = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and UType=" + logintype + "  and UserID=" + loginid + "", "");
                if (ListCompUser.Count > 0)
                {
                    List<Hi.Model.BD_Distributor> ListDIS = new Hi.BLL.BD_Distributor().GetList("", " dr=0 AND ID=" + ListCompUser[0].DisID + " ", "");
                    if (ListDIS.Count > 0)
                    {
                        str = ListDIS[0].DisName;
                    }
                    else
                    {
                        str = "";
                    }
                }
            }
        }
        else
        {
            str = "";
        }
        return str;
    }


    /// <summary>
    /// 登录类型
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public string Login(object obj)
    {
        if (Convert.ToInt32(obj) == 0) {
            return "平台后台管理员登录";
        }
        if (Convert.ToInt32(obj) == 1)
        {
            return "代理商用户登录";
        }
        if (Convert.ToInt32(obj) == 2)
        {
            return "公共用户登录";
        }
        if (Convert.ToInt32(obj) == 3)
        {
            return "企业用户登录";
        }
        if (Convert.ToInt32(obj) == 4)
        {
            return "企业管理员登录";
        }
        if (Convert.ToInt32(obj) == 5)
        {
            return "代理商管理员登录";
        }
        return "";
    }
}