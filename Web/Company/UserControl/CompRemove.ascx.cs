using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_UserControl_CompRemove : System.Web.UI.UserControl
{
    string moduleID;
    public string ModuleID
    {
        get { return moduleID; }
        set { moduleID = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
            if (ModuleID == "1")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script> $(\".list\").find(\"li\").removeClass(\"hover\");$(\"#szgwqx\").addClass(\"hover\"); </script>");
            }
            if (ModuleID == "2")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script> $(\".list\").find(\"li\").removeClass(\"hover\");$(\"#xzjxs\").addClass(\"hover\"); </script>");
            }
            if (ModuleID == "3")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script> $(\".list\").find(\"li\").removeClass(\"hover\");$(\"#xzsp\").addClass(\"hover\"); </script>");
            }
            if (ModuleID == "4")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script> $(\".list\").find(\"li\").removeClass(\"hover\");$(\"#bdskzh\").addClass(\"hover\"); </script>");
            }
            if (ModuleID == "5")
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script> $(\".list\").find(\"li\").removeClass(\"hover\");$(\"#zxsc\").addClass(\"hover\"); </script>");
            }
    }
}