

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Distributor_PhoneEdit : DisPageBase
{
    public Hi.Model.SYS_Users user = null;
    protected void Page_Load(object sender, EventArgs e)
    {
        user = new Hi.BLL.SYS_Users().GetModel(this.UserID);
        if (!string.IsNullOrEmpty(Request["type"]) && Request["type"].ToString() == "erphone" && !string.IsNullOrEmpty(Request["phone"]))
        {
            A_Erphone(Request["phone"].ToString());
        }
        if (!string.IsNullOrEmpty(Request["types"]) && Request["types"].ToString() == "aphone" && !string.IsNullOrEmpty(Request["phone"]))
        {
            AESPhone(Request["phone"].ToString());
        }
        if (!string.IsNullOrEmpty(Request["type"]) && Request["type"] == "hf")
        {
            afh.Visible = true;
        }
        else 
        {
            afh.Visible = false;
        }
    }

    protected void Btn_Update(object sender, EventArgs e)
    {
        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(DisID);
        Hi.Model.SYS_PhoneCode phonecode = new Hi.BLL.SYS_PhoneCode().GetModel("修改绑定手机", txtphone.Value, txtcode.Value);
        //if (Util.md5(txtpaypwd.Value) == dis.Paypwd)
        //{
        if (phonecode != null)
        {

            user.Phone = txtphone.Value;
            user.ts = DateTime.Now;
            user.modifyuser = user.ID;
            if (new Hi.BLL.SYS_Users().Update(user))
            {
                phonecode.IsPast = 1;
                phonecode.ts = DateTime.Now;
                phonecode.modifyuser = user.ID;
                if (new Hi.BLL.SYS_PhoneCode().Update(phonecode))
                {
                    JScript.AlertMethod(this, "您的绑定手机已经修改成功！", JScript.IconOption.正确, "function (){ location.href = 'UserIndex.aspx'; }");
                    return;
                }
            }
        }
        else
        {
            spancode.Attributes.CssStyle.Value = "display:inline-block;color:Red;";
            spancode.InnerText = "-验证码错误";
            return;
        }
        //}
        //else
        //{
        //    spancode.Attributes.CssStyle.Value = "display:inline-block;color:Red;";
        //    spancode.InnerText = "-支付密码错误";
        //    return;
        //}
    }

    public void A_Erphone(string phone)
    {
        if (Common.GetUserExists("Phone", phone))
        {
            Response.Write("该手机已被注册请重新填写");
            Response.End();
        }
        else
        {
            Response.Write("");
            Response.End();
        }
    }

    /// <summary>
    /// 手机号码加密
    /// </summary>
    /// <param name="phone">手机号码</param>
    public void AESPhone(string phone)
    {

        if (phone != "")
        {
            Regex reg = new Regex("^0?1[0-9]{10}$");
            if (reg.IsMatch(phone))
            {
                string phones = AESHelper.Encrypt_php(phone);
                Response.Write(phones);
                Response.End();
            }
            else
            {
                Response.Write("手机号码格式不正确！");
                Response.End();
            }

        }
        else
        {
            Response.Write("");
            Response.End();
        }
    }
}
