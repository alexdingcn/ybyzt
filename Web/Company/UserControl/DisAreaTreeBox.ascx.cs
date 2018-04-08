using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_UserContro_DisAreaTreeBox : System.Web.UI.UserControl
{

    private string id;
    private string compid;
    public string CompID
    {
        get { return compid; }
        set { compid = value; }
    }
    private string comphdid;
    public string CompHDID
    {
        get { return comphdid+""; }
        set { comphdid = value; }
    }
    /// <summary>
    /// 区域Id
    /// </summary>
    public string Id
    {
        get { return hid_AreaId.Value.Trim(); }
        set { id = value; }
    }



    public string Hid_ID {
        get { return hid_AreaId.ClientID; }
    }
    /// <summary>
    /// 区域名称
    /// </summary>
    public string Name
    {
        get { return txt_txtAreaname.Value.Trim(); }
    }
    public string Class
    {
        get { return txt_txtAreaname.Attributes["class"]; }
        set { txt_txtAreaname.Attributes.Add("class", value); }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Hi.Model.BD_DisArea type = new Hi.BLL.BD_DisArea().GetModel(id.ToInt(0));
                if (type != null)
                {
                    txt_txtAreaname.Value = type.AreaName;
                    hid_AreaId.Value = id;
                }
            }
        }
    }
}