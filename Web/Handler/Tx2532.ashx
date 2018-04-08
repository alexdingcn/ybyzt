<%@ WebHandler Language="C#" Class="Tx2532" %>

using System;
using System.Web;
using System.Collections.Generic;

using CFCA.Payment.Api;
using System.Web.Configuration;
using System.Web.SessionState;
public class Tx2532 : IHttpHandler, IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        try
        {
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;

            if (logUser == null)
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"请先登录！\"}";
                context.Response.Write(Josn);
                context.Response.End();
            }
            
            string bankcode = Convert.ToString(request["hidBankCode"]);//帐号号码
            string phoneCode = Convert.ToString(request["txtPhoneCode"]);
            string hidFas = Convert.ToString(request["hidFas"]);
            if (hidFas == "")
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"请先获取验证码！\"}";
                context.Response.Write(Josn);
                return;
            }

            string id = hidFas;
            if (WebConfigurationManager.AppSettings["Paytest_zj"] != "1")//是否是测试，测试不走支付接口
            {

                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                try
                {
                    PaymentEnvironment.Initialize(configPath);
                }
                catch
                {
                    throw new Exception("支付配置不正确");
                }
                string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码
                Tx2532Request tx2532Request = new Tx2532Request();
                tx2532Request.setInstitutionID(institutionID);
                tx2532Request.setTxSNBinding(WebConfigurationManager.AppSettings["OrgCode"] + id);//绑定流水号
                tx2532Request.setSMSValidationCode(phoneCode);

                tx2532Request.process();

                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx2532Request.getRequestMessage(), tx2532Request.getRequestSignature());

                Tx2532Response tx2532Response = new Tx2532Response(respMsg[0], respMsg[1]);

                Hi.Model.PAY_FastPayMent fModel = new Hi.BLL.PAY_FastPayMent().GetModel(Convert.ToInt32(id.Trim().ToString()));
                fModel.vdef3 = tx2532Response.getCode();
                fModel.vdef4 = tx2532Response.getMessage();
                fModel.verifystatus = tx2532Response.getVerifyStatus();
                fModel.status = tx2532Response.getStatus();
                fModel.ts = DateTime.Now;
                fModel.modifyuser = logUser.UserID;
                new Hi.BLL.PAY_FastPayMent().Update(fModel);

                if (!"2000".Equals(tx2532Response.getCode()))//返回Code=2000代表成功
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"" + tx2532Response.getMessage() + "\"}";
                    context.Response.Write(Josn);
                    return;
                }
                //绑定成功，修改状态
                int VerifyStatus = tx2532Response.getVerifyStatus();
                int Status = tx2532Response.getStatus();
                if (VerifyStatus != 40)//验证码检验，40：检验通过
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"" + tx2532Response.getResponseMessage() + "\"}";
                    context.Response.Write(Josn);
                    return;
                }
                if (Status != 30)//支付检验，30：支付成功
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"" + tx2532Response.getResponseMessage() + "\"}";
                    context.Response.Write(Josn);
                    return;
                }
            }
            bool falg = false;
            Hi.Model.PAY_FastPayMent fastModel = new Hi.BLL.PAY_FastPayMent().GetModel(Convert.ToInt32(id));
            fastModel.Start = 1;
            fastModel.ts = DateTime.Now;
            fastModel.modifyuser = logUser.UserID;
            falg = new Hi.BLL.PAY_FastPayMent().Update(fastModel);
            if (falg)
            {
                string Josn = "{\"error\":\"0\",\"msg\":\"绑定成功！\"}";
                context.Response.Write(Josn);
                return;
            }
            context.Response.Write("{\"error\":\"1\",\"msg\":\"绑定失败！\"}");
            return;
        }
        catch (Exception ex)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"" + ex.Message + "\"}";
            context.Response.Write(Josn);
            return;
        }
        finally
        {
            context.Response.End();
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }
}