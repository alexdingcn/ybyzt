using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_UserControl_TreeDisArea : System.Web.UI.UserControl
{
    private string areaid;
    public string areaId
    {
        get { return this.hid_AreaId.Value; }
        set { areaid = value; }
    }
    private string areaname;
    public string areaName
    {
        get { return this.txt_txtAreaname.Value; }
        set { areaname = value; }
    }
    private string compid;
    public string CompID
    {
        get { return compid; }
        set { compid = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(areaid))
            {
                Hi.Model.BD_DisArea type = new Hi.BLL.BD_DisArea().GetModel(areaid.ToInt(0));
                if (type != null)
                {
                    txt_txtAreaname.Value = type.AreaName;
                    hid_AreaId.Value = areaid;
                }
            }
        }
    }
}