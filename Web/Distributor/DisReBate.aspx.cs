using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_DisReBate : DisPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    public void Bind()
    {
        string strwhere = string.Empty;

        strwhere += " isnull(dr,0)=0 and DisID='" + DisID + "' and RebateState=1 ";

        List<Hi.Model.BD_Rebate> l = new Hi.BLL.BD_Rebate().GetList("", strwhere, " EndDate asc");

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