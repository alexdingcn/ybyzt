using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_CMerchants_SelectFC : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.hidCompID.Value = this.CompID.ToString();
            this.hidPageAction.Value = Request["PageAction"] + "";
        }
    }
}