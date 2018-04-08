

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_PayPWDEdit1 : DisPageBase
{
    public Hi.Model.SYS_Users user = null;
    public Hi.Model.BD_Distributor dis = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = new Hi.BLL.SYS_Users().GetModel(this.UserID);
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (Request["code"] != null)
        {
            Hi.Model.SYS_PhoneCode phonecode = new Hi.BLL.SYS_PhoneCode().GetModel("支付密码找回", user.Phone, Common.DesDecrypt(Request.QueryString["code"], Common.EncryptKey));
            if (phonecode == null)
            {
                Response.Redirect("PayPWDEdit.aspx",true);
            }
        }
        else
        {
            Response.Redirect("PayPWDEdit.aspx",true);
        }
    }

    protected void A_Save(object sender, EventArgs e)
    {
        if (dis.Paypwd == Util.md5(Password.Value))
        {
            JScript.AlertMsgOne(this, "新密码不能与原密码相同！", JScript.IconOption.错误);
            return;
        }
        dis.Paypwd = Util.md5(Password.Value);
        dis.ts = DateTime.Now;
        dis.modifyuser = user.ID;
        if (new Hi.BLL.BD_Distributor().Update(dis))
        {
            if (user.IsFirst == 0 || user.IsFirst == 1)
            {
                if (user.IsFirst == 0)
                {
                    user.IsFirst = 2;
                }
                else
                {
                    user.IsFirst = 3;
                }
                user.modifyuser = user.ID;
                user.ts = DateTime.Now;
                new Hi.BLL.SYS_Users().Update(user);
            }
            Hi.Model.SYS_PhoneCode phonecode = new Hi.BLL.SYS_PhoneCode().GetModel("支付密码找回", user.Phone, Common.DesDecrypt(Request.QueryString["code"], Common.EncryptKey));
            phonecode.IsPast = 1;
            phonecode.ts = DateTime.Now;
            phonecode.modifyuser = user.ID;
            if (new Hi.BLL.SYS_PhoneCode().Update(phonecode))
            {
                spanpwd1.Attributes.Add("style", "display:none");
                spanpwd2.Attributes.Add("style", "display:none");
                JScript.AlertMethod(this, "您的支付密码已经修改成功！", JScript.IconOption.笑脸, "function (){ location.href ='UserIndex.aspx'; }");
                return;
            }
        }
    }
}