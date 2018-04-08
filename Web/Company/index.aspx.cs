using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_index : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserModel"] is  LoginModel)
        {
            LoginModel model = Session["UserModel"] as LoginModel;
            if (model != null)
            {
                //获得用户类型
                Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(model.UserID);
                if (User != null && Common.HasAdminRole(User.ID))
                {
                    if (Request["type"] != null)
                    {
                        Response.Redirect("jsc.aspx?type=" + Request["type"]);
                    }
                    else
                    {
                        Response.Redirect("jsc.aspx");
                    }
                }
            }

        }

    }
}