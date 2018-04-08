using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UserControl_Tree_IndusTryPage : System.Web.UI.Page
{
    string chidString = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string zNodes = "var zNodes = [";
        TreeData();
        zNodes += chidString + "]";
            ClientScript.RegisterStartupScript(this.GetType(), "ClientScript", "<script>" + zNodes + "</script>");
    }

    public void TreeData() {
        List<Hi.Model.SYS_Industry> list = new Hi.BLL.SYS_Industry().GetList(null, "   isnull(dr,0)=0  and IsEnabled=1 ", "sortindex");
        foreach (Hi.Model.SYS_Industry item in list)
        {
            chidString += "{ id :" + item.ID + ",pId: 0,name:\"" + item.InduName + "\"},";
        }
    }
}