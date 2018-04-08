using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_SysManager_TreeDisArea : System.Web.UI.Page
{
    string chidString = string.Empty;

    string Compid
    {
        get { return Request["compid"] + ""; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        string zNodes = "var zNodes = [";
        TreeData(0);
        zNodes += chidString + "]";
        ClientScript.RegisterStartupScript(this.GetType(), "ClientScript", "<script>" + zNodes + "</script>");
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ParentId"></param>
    /// <param name="str"></param>
    public void TreeData(int ParentId,string str="") {
        chidString = str;
        List<Hi.Model.BD_DisArea> list = new Hi.BLL.BD_DisArea().GetList(null, "  CompanyId=" + Compid.ToInt(0) + "  and isnull(dr,0)=0  and ParentId=" + ParentId.ToString(), "sortindex");
        foreach (Hi.Model.BD_DisArea item in list)
        {
            chidString += "{ id :" + item.ID + ",pId:" + ParentId + ",name:\"" + item.AreaName + "\",isParent:\"" + (ParentId == 0 ? "true" : "false") + "\"},";
            TreeData(item.ID, chidString);
        }
    }
}