using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UserControl_Tree_Industry : System.Web.UI.UserControl
{
    private string id;
    /// <summary>
    /// 区域Id
    /// </summary>
    public string Id
    {
        get { return hid_IndusId.Value.Trim(); }
        set { id = value; }
    }

    public string Class {
        get { return txt_txtIndusname.Attributes["class"]; }
        set { txt_txtIndusname.Attributes.Add("class", value); }
    }

    private string Utype;

    public string UType
    {
        get { return Utype; }
        set { Utype = value; }
    }

    public string Hid_ID
    {
        get { return hid_IndusId.ClientID; }
    }
    /// <summary>
    /// 区域名称
    /// </summary>
    public string Name
    {
        get { return txt_txtIndusname.Value.Trim(); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (UType == "label")
            {
                txt_txtIndusname.Visible = false;
                lblIndusName.Visible = true;
            }
            if (!string.IsNullOrEmpty(id))
            {
                Hi.Model.SYS_GType type = new Hi.BLL.SYS_GType().GetModel(id.ToInt(0));
                if (type != null)
                {
                    txt_txtIndusname.Value = type.TypeName;
                    lblIndusName.InnerText = type.TypeName;
                    hid_IndusId.Value = id;
                }
            }
        }
    }
}