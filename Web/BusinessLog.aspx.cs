using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class BusinessLog : System.Web.UI.Page
{
    public string LogClass = string.Empty; //业务
    public int CompId = 0;  //企业
    public int ApplicationId = 0; //业务Id

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["LogClass"] != null)
            {
                LogClass =Common.NoHTML( Request.QueryString["LogClass"].ToString());
            }
            if (Request.QueryString["CompId"] != null)
            {
                CompId = Convert.ToInt32(Request.QueryString["CompId"]);
            }
            if (Request.QueryString["ApplicationId"] != null)
            {
                ApplicationId = Convert.ToInt32(Request.QueryString["ApplicationId"]);
            }
            Bind();
        }
    }

    public void Bind()
    {
        string strwhere = string.Empty;

        if (LogClass.Equals("paymentbank"))
            strwhere += " isnull(dr,0)=0 and CompId=" + CompId + " and LogClass='" + LogClass+"'";
        else
            strwhere += " isnull(dr,0)=0 and CompId=" + CompId + " and LogClass='" + LogClass + "' and ApplicationId= " + ApplicationId;

        List<Hi.Model.SYS_SysBusinessLog> l = new Hi.BLL.SYS_SysBusinessLog().GetList("", strwhere, " LogTime asc");

        if (l != null)
        {
            if (l.Count > 0)
            {
                this.rptLog.DataSource = l;
                this.rptLog.DataBind();
            }
        }
    }
}