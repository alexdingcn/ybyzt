using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_PayPWDEdit : DisPageBase
{
    public Hi.Model.SYS_Users user = null;
    public Hi.Model.BD_Distributor dis = null;
    public string OrderID = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = new Hi.BLL.SYS_Users().GetModel(this.UserID);
        dis = new Hi.BLL.BD_Distributor().GetModel(this.DisID);
        if (!IsPostBack)
        {
            if (Request.QueryString["type"]!=null && Request.QueryString["type"].ToString() == "zhifu" && Request["ordid"]!=null)
            {
                updatepay.Style["display"] = "none";
                PhonePay.Style["display"] = "block";
                hidfh.Value = Request.QueryString["ordid"].ToString();
                OrderID = Request.QueryString["ordid"].ToString();
            }
            if (Request.QueryString["type"] != null && Request.QueryString["type"] == "1")
            {
                A_Affirm();
            }
            if ((user.IsFirst == 0 || user.IsFirst == 1) && TypeID == 5)
            {
                paypwdli.Visible = false;
            }
        }
    }

    protected void A_Save(object sender, EventArgs e)
    {
        string txtpaypwd = "";
        spanpwd1.InnerText = "";
        if ((user.IsFirst == 0 || user.IsFirst == 1) && TypeID == 5)
        {
            txtpaypwd = dis.Paypwd;
        }
        else
        {
            txtpaypwd = Util.md5(paypwd.Value.Trim());
        }
        if (dis.Paypwd == txtpaypwd)
        {
            if (paypwd.Value == paypwd1.Value)
            {
                JScript.AlertMsgOne(this, "新密码不能与原密码相同！", JScript.IconOption.错误);
                return;
            }
            if (paypwd1.Value == paypwd2.Value)
            {
                dis.Paypwd = Util.md5(paypwd1.Value);
                dis.ts = DateTime.Now;
                dis.modifyuser = user.ID;
                if (new Hi.BLL.BD_Distributor().Update(dis))
                {
                    spanpwd1.Attributes.CssStyle.Value = "display:none;";
                    if (user.IsFirst == 1 || user.IsFirst == 0)
                    {
                        user.IsFirst = 3;
                        user.modifyuser = user.ID;
                        user.ts = DateTime.Now;
                        if (new Hi.BLL.SYS_Users().Update(user))
                        {
                            JScript.AlertMethod(this, "恭喜您，您已经完成了首次登录验证！", JScript.IconOption.正确, "function (){ location.href = 'UserIndex.aspx'; }");
                            return;
                        }
                    }
                    JScript.AlertMethod(this, "您的支付密码已经修改成功！", JScript.IconOption.笑脸, "function (){ location.href ='UserIndex.aspx'; }");
                    return;
                }
                else
                {
                    spanpwd1.Attributes.CssStyle.Value = "display:inline-block;color:Red;";
                    spanpwd1.InnerText = "修改失败请重试";
                    return;
                }
            }
            else
            {
                spanpwd2.Attributes.CssStyle.Value = "display:inline-block;color:Red;";
                spanpwd2.InnerText = "两次输入的密码不一致";
                return;
            }
        }
        else
        {
            spanpwd1.Attributes.CssStyle.Value = "display:inline-block;color:Red;";
            spanpwd1.InnerText = "支付密码错误";
            return;
        }
    }

    protected void A_Affirm()
    {
        Hi.Model.SYS_PhoneCode phonecode = new Hi.BLL.SYS_PhoneCode().GetModel("支付密码找回", user.Phone, Request["code"].ToString());
        if (phonecode != null)
        {
            string str = "\"str\":\"" + Common.DesEncrypt(Request["code"].ToString(), Common.EncryptKey) + "\",\"type\":true";
            str = "{" + str + "}";
            Response.Write(str);
            Response.End();
        }
        else
        {
            string str = "\"str\":\"-验证码错误\",\"type\":false";
            str = "{" + str + "}";
            Response.Write(str);
            Response.End();
        }
    }
}