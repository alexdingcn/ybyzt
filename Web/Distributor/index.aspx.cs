using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using DBUtility;

public partial class Distributor_index : DisPageBase
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["UserModel"] is LoginModel)
        {
            LoginModel model = Session["UserModel"] as LoginModel;
            if (model != null)
            {
                //获得用户类型
                Hi.Model.SYS_Users User = new Hi.BLL.SYS_Users().GetModel(model.UserID);
                if (User != null && IsDisAdmin(User.ID))
                {
                    Response.Redirect("UserIndex.aspx");
                }
            }
        }
    }

    //判断是否为企业管理员登录
    public bool IsDisAdmin(int ID)
    {
        string sql = "select id from SYS_Users where isnull(dr,0)=0 and type=5 and ID=" + ID;
        DataTable dt = SqlHelper.Query(SqlHelper.LocalSqlServer, sql).Tables[0];
        if (dt != null && dt.Rows.Count > 0)
            return true;
        else
            return false;
    }
}