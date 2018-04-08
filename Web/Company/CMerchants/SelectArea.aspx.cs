using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_CMerchants_SelectArea : CompPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.hidCompID.Value = this.CompID.ToString();
            this.hidPageAction.Value = Request["PageAction"] + "";

            DataBinds();
        }
    }

    public void DataBinds()
    {

        string str = " CompanyID=" + this.CompID + " and isnull(dr,0)=0 and ParentId=0";
        if (!"".Equals(this.txtName.Value))
        {
            str += " and AreaName like '%" + this.txtName.Value + "%'";
        }
        
         List<Hi.Model.BD_DisArea> l = new Hi.BLL.BD_DisArea().GetList("", str, "");
         this.rptDisAreaList.DataSource = l;
         this.rptDisAreaList.DataBind();
    }

    string ChildString = string.Empty;
    /// <summary>
    /// 查询父类下所有子集
    /// </summary>
    /// <param name="ParentId">父Id</param>
    /// <returns></returns>
    public string FindChild(string ParentId, string Str = "")
    {
        ChildString = Str;
        List<Hi.Model.BD_DisArea> LArea = new Hi.BLL.BD_DisArea().GetList("", "  CompanyID=" + CompID + " and ParentId='" + ParentId + "' and isnull(dr,0)=0", " sortindex asc");
        foreach (Hi.Model.BD_DisArea model in LArea)
        {
            ChildString += "<tr id='" + model.ID + "' parentid='" + model.ParentID + "' bgcolor='#fcfeff' style='height: 26px;width: 100%;display: none;'>";
            ChildString += " <td><div class=\"tc\"><input type=\"checkbox\" class=\"\" value=\"" + model.ID + "\"/></div></td>";
            ChildString += "<td><div class=\"tcle\"><img id='Openimg' height='9' src='" + Simage(model.ID) + "' width='9' border='0' />&nbsp; <span class='span'>" + model.AreaName + "</span></div></td>";
            ChildString += "</tr>";
            FindChild(model.ID.ToString(), ChildString);
        }
        return ChildString;
    }

    protected string Simage(object obj)
    {
        string image = "../images/menu_plus.gif";
        if (obj != null)
        {

            List<Hi.Model.BD_DisArea> l = new Hi.BLL.BD_DisArea().GetList(null, " CompanyID='" + this.CompID + "' and isnull(dr,0)=0 and  ParentId=" + obj.ToString(), null);
            if (l.Count > 0)
            {
                image = "../images/menu_plus.gif";
            }
            else
            {
                image = "../images/menu_minus.gif";
            }
            return image;
        }
        return image;
    }
}