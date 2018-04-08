using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void btnLogin_Click(object sender, EventArgs e)
    {
        try
        {
            string dt = DateTime.Now.ToString();
            string adminName = Common.NoHTML(this.txtLoginId.Value.Trim());
            string adminPwd = this.txtPwd.Value.Trim();
            string admintcode = Common.NoHTML(this.txtcode.Value.Trim());
            string Chckcode = Session["CheckCode"] != null ? Session["CheckCode"].ToString() : "";
            if (Util.IsEmpty(adminName))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script>alert('请输入用户名！');</script>");
                this.txtLoginId.Focus();
                return;
            }

            if (Util.IsEmpty(adminPwd))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script>alert('请输入密码！');</script>");
                this.txtPwd.Focus();
                return;
            }

            if (admintcode == "")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script>alert('请输入验证码！');</script>");
                return;
            }
            if (admintcode != Chckcode)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script>alert('验证码输入错误！');</script>");
                this.txtcode.Value = "";
                return;
            }
            if (DBHelper.IsOpen() == false)
            {
                JScript.ShowAlert(this, "系统无法连接数据库服务器，请联系管理员！");
                return;
            }

            Hi.Model.SYS_AdminUser model = new Hi.BLL.SYS_AdminUser().GetModelByName(adminName);

            if (model == null)
            {
                //登录录日志
                //Utils.EditLog("安全日志",adminName, "用户" + adminName + "登录管理系统失败，该用户不存在。", "系统安全模块", "Admin/login.aspx", 0, 0);

                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script>alert('用户不存在！');</script>");
                this.txtLoginId.Focus();
                return;
            }
            else
            {
                if (Util.SHA1Encrypt(Util.SHA1Encrypt(model.LoginPwd)) == adminPwd)
                {
                    if (model.IsEnabled == 0)
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script>alert('您的账户目前处在禁用状态,不能登录！');</script>");
                        return;
                    }
                    else
                    {
                        //保存登录信息

                        //保存Session信息
                        Session["AdminUser"] = model;
                        Session["AdminUserDate"] = dt;
                        Session["UserType"] = model.UserType;

                        //登录成功记录日志
                        Utils.EditLog("安全日志", adminName, "用户" + adminName + "登录管理系统成功。", "系统安全模块", "Admin/login.aspx", 0, 1, 0);
                        //Response.Redirect("index.aspx");

                        //Cookie记录登录名
                        HttpCookie cookie = new HttpCookie("LoginId", model.LoginName);
                        cookie.Expires = DateTime.Now.AddDays(7);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);

                        //添加token验证
                        string MyTokenKey = "__MYToken";
                        string MyUserNameKey = "__MYUserName";
                        var myTokenValue = Guid.NewGuid().ToString("N");
                        var responseCookie = new HttpCookie(MyTokenKey)
                        {
                            HttpOnly = true,
                            Value = myTokenValue,
                            Expires = DateTime.Now.AddDays(7)
                        };
                        Response.Cookies.Set(responseCookie);
                        Session[MyUserNameKey] = Util.md5(adminName + Util._salt);
                        Session[MyTokenKey] = myTokenValue;

                        Response.Redirect("index.aspx");
                    }
                }
                else
                {
                    //登录录日志
                    Utils.EditLog("安全日志", adminName, "用户" + adminName + "登录管理系统失败，输入的密码错误。", "系统安全模块", "Admin/login.aspx", 0, 0, 1);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "<script>alert('用户名或密码错误！');</script>");
                    this.txtPwd.Focus();
                    return;
                }
            }
        }
        catch (System.Threading.ThreadAbortException)
        {
            //捕捉线程终止异常   不处理
        }
        catch (Exception ex)
        {
            Tiannuo.LogHelper.LogHelper.Error("Error", ex);
            return;
        }
    }

}