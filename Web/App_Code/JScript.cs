using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Reflection;

/// <summary>
///JScript 的摘要说明
/// </summary>
public class JScript
{
    public JScript()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }
    /// <summary>
    /// 刷新父页面并且弹出提示信息
    /// </summary>
    /// <param name="page"></param>
    /// <param name="msg"></param>
    public static void ShowParentRefresh(System.Web.UI.Page page, string msg)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "msg", "<script>parent.location.reload();alert('" + msg + "。');</script>");
    }
    public static void ShowAlert(System.Web.UI.Page page, string msg)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "msg", "<script>alert('" + msg + "');</script>");
    }
    public static void ShowAlert(System.Web.UI.Page page, string msg, string script)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "msg", "<script>alert('" + msg + "');" + script + ";</script>");
    }
    /// <summary>
    /// 弹出JavaScript小窗口,并转向指定的页面
    /// </summary>
    /// <param name="message">弹出信息</param>
    /// <param name="toURL">专转向的网页</param>
    public static void AlertAndRedirect(string message, string toURL)
    {
        string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
        HttpContext.Current.Response.Write(string.Format(js, message, toURL));
    }

    public static void AlertMsg(Page page, string Msg, string url = "")
    {
        //page.ClientScript.RegisterClientScriptBlock(page.GetType(), "msg", "<script>errMsg('提示','" + Msg + "','','" + url + "');</script>");
        if (url == "")
        {
            page.ClientScript.RegisterClientScriptBlock(page.GetType(), "msg", "<script>alert('" + Msg + "');</script>");
        }
        else
        {
            string js = "<script language=javascript>alert('{0}');window.location.replace('{1}')</script>";
            HttpContext.Current.Response.Write(string.Format(js, Msg, url));
        }

    }

    public static void AlertMsgMo(Page page, string Msg, string method = "")
    {
        page.ClientScript.RegisterClientScriptBlock(page.GetType(), "msg", "<script>errMsgMo('提示','" + Msg + "'," + method + ");</script>");
    }

    public static void AlertMethod(Page page, string Msg, IconOption icon, string method = "")
    {
        if (method == "")
        {
            method = "undefined";
        }
        page.ClientScript.RegisterClientScriptBlock(page.GetType(), "msg", "<script>layerCommon.alert('" + Msg + "'," + StringEnum.GetStringValue(icon) + ",undefined," + method + ",0);</script>");
    }

    public static void AlertMsgOne(Page page, string Msg, IconOption icon, int CMillisecond = 1500)
    {
        page.ClientScript.RegisterClientScriptBlock(page.GetType(), "msg", "<script>layerCommon.msg('" + Msg + "'," + StringEnum.GetStringValue(icon) + "," + CMillisecond + ");</script>");
    }

    /// <summary>
    /// 刷新父页面并且弹出提示信息
    /// </summary>
    /// <param name="page"></param>
    /// <param name="msg"></param>
    public static void AlertMsgParentRefresh(System.Web.UI.Page page, string Msg, IconOption icon, int CMillisecond = 1500)
    {
        page.ClientScript.RegisterStartupScript(page.GetType(), "msg", "<script>layerCommon.alert('" + Msg + "'," + StringEnum.GetStringValue(icon) + "," + CMillisecond + ",function(){ parent.location.reload(); },0);</script>");
    }

    public enum IconOption
    {
        [StringValue("IconOption.不显示图标")]
        不显示图标 = -1,
        [StringValue("IconOption.感叹")]
        感叹 = 0,
        [StringValue("IconOption.正确")]
        正确 = 1,
        [StringValue("IconOption.错误")]
        错误 = 2,
        [StringValue("IconOption.询问")]
        询问 = 3,
        [StringValue("IconOption.锁定")]
        锁定 = 4,
        [StringValue("IconOption.哭脸")]
        哭脸 = 4,
        [StringValue("IconOption.笑脸")]
        笑脸 = 6
    }

    public class StringValue : Attribute
    {
        private string _value;

        public StringValue(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }
    }

    public static class StringEnum
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetField(value.ToString());
            StringValue[] attrs =
               fi.GetCustomAttributes(typeof(StringValue),
                                       false) as StringValue[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }
}