using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_ButtonToExcel : System.Web.UI.UserControl
{

    private string _contect = "";
    public string contect
    {
        get
        {
            return _contect;
        }
        set
        {
            _contect = value;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
}