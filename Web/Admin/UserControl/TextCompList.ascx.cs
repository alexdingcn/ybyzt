using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UserControl_TextCompList : System.Web.UI.UserControl
{
    private string compid=string.Empty;
    public string Name {
        get { return txt_txtCompName.Value.Trim(); }
    }
    public string Class {
        get { return txt_txtCompName.Attributes["class"]; }
        set { txt_txtCompName.Attributes.Add("class", value); }
    }
    public string Compid {
        get { return hid_CompId.Value; }
        set { compid = value; }
    }
    public string Hid_Id {
        get { return hid_CompId.ClientID; }
    }

    public string text_id {
        get { return txt_txtCompName.ClientID; }
    }
    public string rtFunc;
    public string RTFunc {
        get { return rtFunc+""; }
        set { rtFunc = value; }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
            if (!string.IsNullOrEmpty(compid))
            {
                object value = Common.GetCompValue(compid.ToInt(0), "CompName");
                if (value != null) {
                    txt_txtCompName.Value = value.ToString();
                    hid_CompId.Value = compid;
                }
            }
        }
    }
}