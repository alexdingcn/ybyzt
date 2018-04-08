using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_UserControl_TreeDisAddr : System.Web.UI.UserControl
{
    private string treeid;
    public string treeId
    {
        get { return this.hid_product_class.Value; }
        set { treeid = value; }
    }
    private string treename;
    public string treeName
    {
        get { return this.txt_product_class.Value; }
        set { treename = value; }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (!string.IsNullOrEmpty(treeid))
            {
                Hi.Model.SYS_GType type = new Hi.BLL.SYS_GType().GetModel(treeid.ToInt(0));
                if (type != null)
                {
                    this.txt_product_class.Value = type.TypeName;
                    this.hid_product_class.Value = type.ID.ToString();
                }
            }
        }
    }
}