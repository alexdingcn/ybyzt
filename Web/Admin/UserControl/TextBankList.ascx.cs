using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_UserControl_TextBankList : System.Web.UI.UserControl
{
    public string BankId {
        get { return txt_txtBankid.Value.Trim(); }
        set { txt_txtBankid.Value = value; }
    }
    public string Class
    {
        get { if (!string.IsNullOrWhiteSpace(Class)) { return txt_txtBankid.Attributes["class"]; } else { return ""; } }
        set { if (!string.IsNullOrWhiteSpace(value)) { txt_txtBankid.Attributes.Add("class", value); } }
    }
    public string txtBankID {
        get { return txt_txtBankid.ClientID; }
    }
    public string SetNameFc
    {
        set;
        get;
    }

    
    protected void Page_Load(object sender, EventArgs e)
    {
        
    }
}