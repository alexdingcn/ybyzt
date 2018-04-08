using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_UserControl_TreeDis : System.Web.UI.UserControl
{
    private string compid;
    public string CompID
    {
        get { return compid; }
        set { compid = value; }
    }
    private string disid;
    public string Disid
    {
        get { return hid_DisId.Value; }
        set { disid = value; }
    }
    private string name;
    public string Name
    {
        get { return txt_txtDisName.Value.Trim(); }
        set { name = value; }
    }
    public string Hid_Id
    {
        get { return hid_DisId.ClientID; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(disid))
            {
                object value = Common.GetDisValue(disid.ToInt(0), "DisName");
                if (value != null)
                {
                    txt_txtDisName.Value = value.ToString();
                    hid_DisId.Value = Disid;
                }
            }
        }
    }
}