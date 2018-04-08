using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using FinancingReferences;

public partial class Company_Report_Report4 : CompPageBase
{
    public string url = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            
            if (ConfigurationManager.AppSettings["PayType"] == "0")
                url = "http://114.55.140.161:8080/WebReport/ReportServer?formlet=mwbproduct.frm&iEtlversion=1&icompid=" + this.CompID;
            else
                url = "http://114.55.140.161:8080/WebReport/ReportServer?formlet=mwbproduct.frm&iEtlversion=1&icompid=" + this.CompID;
           
        }
    }

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        
    }
}