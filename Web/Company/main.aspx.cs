using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_main : CompPageBase
{
    public string mainstr = "index.aspx";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Common.TypeID() != 3 && Common.TypeID() != 4)
        {
            //ClientScript.RegisterStartupScript(this.GetType(), "msg", "<script>alert('');</script>");
            //Session.Remove("UserModel");
            Session["UserModel"] = null;
            Session.Clear();
            Session.Abandon();
            Response.Clear();
            JScript.AlertAndRedirect("您不是商家用户，或已登录平台，请查看！", "../index.aspx");
        }

        if (Session["UserModel"] is LoginModel)
        {
            LoginModel model = Session["UserModel"] as LoginModel;
            if (model != null)
            {
                //获得用户类型
                Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(model.UserID);
                if (User != null && Common.HasAdminRole(User.ID))
                {
                    if (Request["main"] != null)
                    {
                        if (Request["main"].ToString() == "11")
                        {
                            mainstr = "Order/OrderCreateList.aspx";
                        }
                        else if (Request["main"].ToString() == "12")
                        {
                            mainstr = "Report/CompCollection.aspx";
                        }
                        else if (Request["main"].ToString() == "13")
                        {
                            mainstr = "Order/OrderShipList.aspx";
                        }
                        else if (Request["main"].ToString() == "14")
                        {
                            mainstr = "SysManager/DisList.aspx";
                        }
                        rightFrame.Attributes.Add("src", mainstr);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(Request["type"]))
                        {
                            mainstr = "jsc.aspx";
                            rightFrame.Attributes.Add("src", mainstr);
                        }
                    }
                }

            }
        }

    }
}