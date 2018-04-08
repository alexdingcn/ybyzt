using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_UserControl_TreeDisType : System.Web.UI.UserControl
{
    private string typeid;
    public string typeId
    {
        get { return this.hid_TypeId.Value; }
        set { typeid = value; }
    }
    private string typename;
    public string typeName
    {
        get { return this.txt_txtTypename.Value; }
        set { typename = value; }
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
            if (!string.IsNullOrEmpty(typeid))
            {
                Hi.Model.BD_DisType type = new Hi.BLL.BD_DisType().GetModel(typeid.ToInt(0));
                if (type != null)
                {
                    txt_txtTypename.Value = type.TypeName;
                    hid_TypeId.Value = typeid;
                }
            }
        }
    }
}