using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Hi.Model;

public partial class findPwd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        object action = Request["action"];
        if (action != null)
        {
            if (action.ToString() == "checktelcode")//检查手机验证码
            {
                checkphonecode();
            }

            if (action.ToString() == "updatepwd")//修改密码
            {

                UpdatPwd();
            }

        }
    }


    /// <summary>
    /// 验证手机验证码
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void checkphonecode()
    {
        string username = Request["username3"].ToString();
        string userphone = Request["userphone3"].ToString();
        string phonecodes = Request["phonecode3"].ToString();
        List<Hi.Model.SYS_PhoneCode> ListPhonecode = new Hi.BLL.SYS_PhoneCode().GetList("top 1 *", " DATEDIFF(minute,CreateDate,GETDATE()) between 0 and 30 and ispast=0 and module='修改登录密码' and username='" + username + "' and Phone='" + userphone + "' and PhoneCode='" + phonecodes + "' and dr=0 ", "");
        if (ListPhonecode.Count > 0)
        {
            //成功
            ListPhonecode[0].Type = 999;
            if (new Hi.BLL.SYS_PhoneCode().Update(ListPhonecode[0]))
            {
                Response.Write("{\"type\":true,\"str\":\"验证成功\"}");
                Response.End();
            }
            else
            {
                Response.Write("{\"type\":false,\"str\":\"验证异常请重试\"}");
                Response.End();
            }
        }
        else
        {
            Response.Write("{\"type\":false,\"str\":\"验证码错误或已失效！\"}");
            Response.End();
            return;
        }
    }


    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void UpdatPwd()
    {
        string pwd1 = Common.NoHTML(Request["pwd1"].ToString());
        string pwd2 = Common.NoHTML(Request["pwd2"].ToString());
        string userid = Request["userid"].ToString();
        string username = Request["username1"].ToString();
        string userphone = Request["userphone"];
        string Phonecode = Request["Phonecode"];
        List<Hi.Model.SYS_PhoneCode> ListPhonecode = new Hi.BLL.SYS_PhoneCode().GetList("top 1 * ", " DATEDIFF(minute,CreateDate,GETDATE()) between 0 and 60 and ispast=0 and Type='999' and module='修改登录密码' and username='" + username + "' and Phone='" + userphone + "' and PhoneCode='" + Phonecode + "' and dr=0 ", "");
        if (ListPhonecode.Count == 0)
        {
            Response.Write("{\"type\":false,\"str\":\"手机验证码校验异常，请重试！\",\"code\":\"error\"}");
            Response.End();
            return;
        }
        if (pwd1 != pwd2 || string.IsNullOrEmpty(pwd2) || string.IsNullOrEmpty(pwd1))
        {
            Response.Write("{\"type\":false,\"str\":\"密码不一致！\"}");
            Response.End();
            return;
        }
        else if (pwd1 == "123456" || pwd2 == "123456")
        {
            Response.Write("{\"type\":false,\"str\":\"不能使用系统默认密码作为新密码！\"}");
            Response.End();
            return;
        }
        else
        {
            List<Hi.Model.SYS_Users> ListUser = new Hi.BLL.SYS_Users().GetListUser("", "Username", username, "");
            if (ListUser.Count==0)
            {
                Response.Write("{\"type\":false,\"str\":\"用户不存在！\"}");
                Response.End();
                return;
            }
            else
            {
                string newpwd = Util.md5(pwd2);
                if (ListUser[0].UserPwd == newpwd.Trim())
                {
                    Response.Write("{\"type\":false,\"str\":\"新密码不可与原密码一致！\"}");
                    Response.End();
                    return;
                }
                if (new Hi.BLL.SYS_Users().UpdatePassWord(newpwd, ListUser[0].ID.ToString()))
                {
                    ListPhonecode[0].IsPast = 1;
                    ListPhonecode[0].ts = DateTime.Now;
                    ListPhonecode[0].modifyuser = userid.ToInt(0);
                    new Hi.BLL.SYS_PhoneCode().Update(ListPhonecode[0]);
                    //修改成功，保存日志
                    Response.Write("{\"type\":true,\"str\":\"\"}");
                    Response.End();
                    return;
                }
                else
                {
                    Response.Write("{\"type\":false,\"str\":\"密码修改失败\"}");
                    Response.End();
                    return;
                }
            }
        }
    }
}