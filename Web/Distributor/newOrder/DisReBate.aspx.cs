using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_newOrder_DisReBate : System.Web.UI.Page
{
    //代理商ID
    public int DisID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            databand();
        }
    }

    public void databand()
    {
        string strwhere = string.Empty;

        if (!string.IsNullOrEmpty(Request["DisID"]))
            DisID = (Request["DisID"] + "").ToInt(0);

        //strwhere += " isnull(dr,0)=0 and DisID='" + DisID + "' and RebateState=1 ";

        strwhere = " disID = " + DisID + " and IsNull(dr,0) = 0 and RebateState = 1 and EnableAmount <> 0 and getdate() between StartDate and dateadd(day,1,EndDate)";

        List<Hi.Model.BD_Rebate> l = new Hi.BLL.BD_Rebate().GetList("", strwhere, " EndDate asc");

        if (l != null)
        {
            if (l.Count > 0)
            {
                this.rptbate.DataSource = l;
                this.rptbate.DataBind();
            }
        }
    }
}