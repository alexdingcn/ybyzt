using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_TreeDisType : System.Web.UI.Page
{

    string Compid {
        get { return Request["compid"] + ""; }
    }
    string chidString = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        string zNodes = "var zNodes = [";
        TreeData(0);
        zNodes += chidString + "]";
        ClientScript.RegisterStartupScript(this.GetType(), "ClientScript", "<script>" + zNodes + "</script>");
    }

    public void TreeData(int ParentId, string str = "")
    {
        chidString = str;
        List<Hi.Model.BD_DisType> list = new Hi.BLL.BD_DisType().GetList(null, "  CompID=" + Compid.ToInt(0) + "  and isnull(dr,0)=0  and ParentId=" + ParentId.ToString(), "sortindex");
        foreach (Hi.Model.BD_DisType item in list)
        {
            chidString += "{ id :" + item.ID + ",pId:" + ParentId + ",name:\"" + item.TypeName + "\",isParent:\"" + (ParentId == 0 ? "true" : "false") + "\"},";
            TreeData(item.ID, chidString);
        }
    }
}