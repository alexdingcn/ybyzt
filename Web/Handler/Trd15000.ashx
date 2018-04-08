<%@ WebHandler Language="C#" Class="Trd15000" %>

using System;
using System.Web;
using System.Collections.Generic;
using LitJson;
using FinancingReferences;

public class Trd15000 : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;
        string Paypwd = context.Request["Paypwd"];
        //string userid = Common.DesDecrypt(request["userid"].ToString(), Common.EncryptKey);
        //Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(Common.DesDecrypt(request["userid"].ToString(), Common.EncryptKey).ToInt(0));
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;

        if (logUser == null)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(logUser.DisID);
        if (dis != null)
        {
            if (!dis.Paypwd.Equals(Util.md5(Paypwd)))
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"支付密码错误！\"}";
                context.Response.Write(Josn);
                context.Response.End();
            }
        }
        else
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        Hi.Model.PAY_Financing Fin = new Hi.Model.PAY_Financing();
        Fin.DisID = logUser.DisID;
        Fin.CompID = logUser.CompID;
        Fin.State = 2;
        List<Hi.Model.PAY_OpenAccount> LOpen = new Hi.BLL.PAY_OpenAccount().GetList("", "DisID=" + Fin.DisID + " and State=1 and dr=0", "");
        if (LOpen.Count > 0)
        {
            Fin.OpenAccID = LOpen[0].ID;
        }
        else
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"请先添加开销账户！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        int result = 0;
        string guid = Common.Number_repeat("1");
        Fin.OrderID = 0;
        Fin.AclAmt = Convert.ToDecimal(request["aclamt"]);
        Fin.Guid = guid;
        Fin.Type = 1;
        Fin.CcyCd = "CNY";
        Fin.vdef1 = request["txtRemark"];
        Fin.ts = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd"));
        Fin.modifyuser = logUser.UserID;
        result = new Hi.BLL.PAY_Financing().Add(Fin);

        if (result > 0)
        {
            //Response.Redirect("FinancingDetailList.aspx");
            String msghd_rspcode = "";
            String msghd_rspmsg = "";
            String actionUrl = "";
            String key1 = "";
            String val1 = "";
            String key2 = "";
            String val2 = "";
            String Json = "";
            try
            {
                //trd15000(日期，交易流水号，付款方代理商号，付款方账户名称，入金金额);
                IPubnetwk ipu = new IPubnetwk();
                Json = ipu.trd15000("{\"msghd_trdt\":\"" + DateTime.Now.ToString("yyyyMMdd") + "\",\"srl_ptnsrl\":\"" + guid + "\",\"cltacc_cltno\":\"" + LOpen[0].AccNumver + "\",\"cltacc_cltnm\":\"" + LOpen[0].AccName + "\",\"amt_aclamt\":\"" + (int)(Convert.ToDecimal(request["aclamt"]) * 100) + "\"}");
            }
            catch
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"接口连接失败！\"}";
                context.Response.Write(Josn);
                context.Response.End();
            }
            //String Json = "{\"msghd_rspcode\":\"000000\",\"msghd_rspmsg\":\"成功！\",\"actionUrl\":\"https://test.cpcn.com.cn/Gateway/InterfaceI\",\"key1\":\"message\",\"val1\":\"PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9Im5vIj8+CjxSZXF1ZXN0IHZlcnNpb249IjIuMSI+CjxIZWFkPgo8VHhDb2RlPjEzMTI8L1R4Q29kZT4KPC9IZWFkPgo8Qm9keT4KPEluc3RpdHV0aW9uSUQ+MDAxNTk4PC9JbnN0aXR1dGlvbklEPgo8T3JkZXJObz4xNTI4N1RaMDAwMDEzMzExPC9PcmRlck5vPgo8UGF5bWVudE5vPjE1Mjg3VFowMDAwMTMzMTE8L1BheW1lbnRObz4KPEFtb3VudD4xPC9BbW91bnQ+CjxQYXllcklEPjgwMDIxMDIwPC9QYXllcklEPgo8UGF5ZXJOYW1lPua1i+ivleaVsOaNruS8muWRmOS4gDwvUGF5ZXJOYW1lPgo8VXNhZ2U+5YWF5YC8LeiuouWNleaUr+S7mDwvVXNhZ2U+CjxSZW1hcmsvPgo8Tm90ZS8+CjxOb3RpZmljYXRpb25VUkw+aHR0cDovLzExMS4yMDUuOTguMTg0OjkzNjAvemhqbm90aWNlL2ludGVyZmFjZVBhZ2VOb3RpY2UuaHRtPC9Ob3RpZmljYXRpb25VUkw+CjxQYXllZUxpc3QvPgo8L0JvZHk+CjwvUmVxdWVzdD4=\",\"key2\":\"signature\",\"val2\":\"2D8B978EB9E5F80EB1D3530DED71E45EC9ED36F5E841C79B4817A4B1455285A8CD24C00D3B625198C90266FC3FD6C28905D584F86B13D2C7118ED679BB215B87CC041A494587730C4D8931F809CD2F6E5761EBA0B41D2A028220086BDD39B9D3EE6A33906E15549F16E97E6C014AC84CCF0A8EC0156F517E9244780082DB26A2\"}";
            try
            {
                JsonData Params = JsonMapper.ToObject(Json);
                msghd_rspcode = Params["msghd_rspcode"].ToString();
                msghd_rspmsg = Params["msghd_rspmsg"].ToString();
                actionUrl = Params["actionUrl"].ToString();
                key1 = Params["key1"].ToString();
                val1 = Params["val1"].ToString();
                key2 = Params["key2"].ToString();
                val2 = Params["val2"].ToString();
            }
            catch { }
            if ("000000".Equals(msghd_rspcode))
            {
                try
                {
                    Hi.Model.PAY_Financing finaM = new Hi.BLL.PAY_Financing().GetModel(result);
                    finaM.State = 3;
                    finaM.ts = DateTime.Now;
                    new Hi.BLL.PAY_Financing().Update(finaM);
                }
                catch { }
                string Josn = "{\"success\":\"1\",\"msg\":\"成功！\",\"js\":\"var tempForm = document.createElement('form');tempForm.id = 'tempForm1';tempForm.method = 'post';tempForm.action = '" + actionUrl + "';tempForm.target = '_blank';var hideInput2 = document.createElement('input');hideInput2.type = 'hidden';hideInput2.name = '" + key1 + "';hideInput2.value = '" + val1 + "';var hideInput3 = document.createElement('input');hideInput3.type = 'hidden';hideInput3.name = '" + key2 + "';hideInput3.value = '" + val2 + "';tempForm.appendChild(hideInput2);tempForm.appendChild(hideInput3);document.body.appendChild(tempForm);tempForm.submit();document.body.removeChild(tempForm);\"}";
                context.Response.Write(Josn);
                context.Response.End();
            }
            else
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"" + msghd_rspmsg + "！\"}";
                context.Response.Write(Josn);
                context.Response.End();
            }
        }
        else
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}