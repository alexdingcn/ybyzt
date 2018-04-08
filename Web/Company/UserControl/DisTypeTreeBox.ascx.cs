using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_UserContro_DisTypeTreeBox : System.Web.UI.UserControl
{


    private string id;
    public string Id  
    {
        get { return hid_TypeId.Value.Trim(); }
        set { id = value; }
    }
    private string compid;
    public string CompID
    {
        get { return compid; }
        set { compid = value; }
    }
    private string comphdid;
    public string CompHDID
    {
        get { return comphdid + ""; }
        set { comphdid = value; }
    }
    public string Hid_ID
    {
        get { return hid_TypeId.ClientID; }
    }
    public string Class {
        get {return txt_txtTypename.Attributes["class"]; }
        set { txt_txtTypename.Attributes.Add("class", value); }
    }
    /// <summary>
    /// 分类名称
    /// </summary>
    public string Name
    {
        get { return txt_txtTypename.Value.Trim(); }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            if (!string.IsNullOrEmpty(id)) {
                Hi.Model.BD_DisType type = new Hi.BLL.BD_DisType().GetModel(id.ToInt(0));
                if (type != null) {
                    txt_txtTypename.Value = type.TypeName;
                    hid_TypeId.Value = id;
                }
            }
        }
    }
}