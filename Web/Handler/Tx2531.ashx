<%@ WebHandler Language="C#" Class="Tx2531" %>

using System;
using System.Web;
using System.Collections.Generic;

using CFCA.Payment.Api;
using System.Web.Configuration;
using System.Web.SessionState;
public class Tx2531 : IHttpHandler, IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        Hi.Model.PAY_FastPayMent fastpayModel = new Hi.Model.PAY_FastPayMent();
        int id = 0;
        //bool flag = false;
        int quickCardNum = 0;
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

            int disid = logUser.DisID;//代理商ID
            if (request["hidBankid"].Trim().ToString() == "")
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"操作有误！\"}";
                context.Response.Write(Josn);
                return;
            }
            int bankid = Convert.ToInt32(request["hidBankid"].Trim().ToString());//银行ID
            string bankcode = Convert.ToString(request["txtBankCode"]);//帐号号码
            bankcode = bankcode.Replace(" ", "");
            string busername = Convert.ToString(request["txtUserName"]);//帐号名称
            string idcard = Convert.ToString(request["txtIDCard"]);//身份证号码
            string phone = Convert.ToString(request["txtPhone"]);//手机号码
            string banklogo = Convert.ToString(request["hidBankLogo"]);//银行logo

            string strWhere = string.Empty;
            if (bankcode == "")
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"银行卡号必须填写！\"}";
                context.Response.Write(Josn);
                return;
            }

            #region  判断用户获取验证码次数，超过次数给出提示。

            string querywhere = string.Format(" UserID={0} and Phone='{1}' and PhoneCode='{2}' and CreateDate='{3}'", logUser.DisID, phone, idcard, DateTime.Now.Date);//根据一个手机不能发多次
            string querywhereUser = string.Format(" UserID={0}  and PhoneCode='{1}' and CreateDate='{2}'", logUser.DisID, idcard, DateTime.Now.Date);//根据一个帐号不能发多次
            List<Hi.Model.SYS_PhoneCode> PhoneCodeList = new Hi.BLL.SYS_PhoneCode().GetList("", querywhere, "");
            //配置文件中配置
            int QuickPaycard = Convert.ToInt32(ConfigCommon.GetNodeValue("Version.xml", "quickcardnum"));
            //系统设置值减去今天历史绑定记录等于0，说明已达到绑定上限
            quickCardNum =QuickPaycard- PhoneCodeList.Count;
            List<Hi.Model.SYS_PhoneCode> PhoneCodeLists = new Hi.BLL.SYS_PhoneCode().GetList("", querywhereUser, "");
            int quickCardNums =QuickPaycard- PhoneCodeLists.Count;
            if (quickCardNum==0||quickCardNums==0)
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"今日银行卡绑定，已达到最大次数，请明日再试！\"}";
                context.Response.Write(Josn);
                return;
            }

            //插入本次绑定的记录
            Hi.Model.SYS_PhoneCode phonecodemodel = new Hi.Model.SYS_PhoneCode();
            phonecodemodel.Type = 0;
            phonecodemodel.Module = "绑定快捷支付获取验证码";
            phonecodemodel.UserID = logUser.DisID;
            phonecodemodel.UserName = logUser.DisName;
            phonecodemodel.Phone = phone;
            phonecodemodel.PhoneCode = idcard;
            phonecodemodel.IsPast = 1;
            phonecodemodel.CreateDate = DateTime.Now.Date;
            phonecodemodel.ts = DateTime.Now;
            phonecodemodel.dr = 0;
            phonecodemodel.modifyuser = logUser.DisID;
            int addnum = new Hi.BLL.SYS_PhoneCode().Add(phonecodemodel);           

            #endregion




            strWhere += " bankcode = '" + bankcode + "' ";
            strWhere += " and DisID = " + logUser.DisID + " and Start = 1 and vdef6 = 0 and isnull(dr,0)=0";
            List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");
            if (fastList.Count > 0)
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"银行卡已绑定，不能重复绑定！\"}";
                context.Response.Write(Josn);
                return;
            }

            fastpayModel.DisID = disid;
            fastpayModel.BankID = bankid;
            fastpayModel.Number = DateTime.Now.ToLongDateString();
            fastpayModel.AccountName = busername;
            fastpayModel.bankcode = bankcode;
            fastpayModel.bankName = new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(bankid.ToString());
            fastpayModel.IdentityCode = idcard;
            fastpayModel.phone = phone;
            fastpayModel.BankLogo = banklogo;
            fastpayModel.Start = 2;
            fastpayModel.vdef6 = "0";
            fastpayModel.CreateUser = Convert.ToInt32(logUser.UserID.ToString());
            fastpayModel.CreateDate = DateTime.Now;
            fastpayModel.ts = DateTime.Now;
            fastpayModel.modifyuser = Convert.ToInt32(logUser.UserID.ToString());
            id = new Hi.BLL.PAY_FastPayMent().Add(fastpayModel);

            if (id <= 0)
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\"}";
                context.Response.Write(Josn);
                return;
            }

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

                Tx2531Request tx2531Request = new Tx2531Request();
                tx2531Request.setInstitutionID(institutionID);
                tx2531Request.setTxSNBinding(WebConfigurationManager.AppSettings["OrgCode"] + id.ToString());//绑定流水号
                tx2531Request.setBankID(bankid.ToString());//银行卡代码
                tx2531Request.setAccountName(busername);//代理商名称
                tx2531Request.setAccountNumber(bankcode);//代理商银行卡号
                tx2531Request.setIdentificationType("0".ToString());//证件类型，0：省份证
                tx2531Request.setIdentificationNumber(idcard);//证件号码
                tx2531Request.setPhoneNumber(phone);//手机号
                tx2531Request.setCardType("10".ToString());//银行卡类型，10：储蓄卡

                tx2531Request.process();

                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx2531Request.getRequestMessage(), tx2531Request.getRequestSignature());

                Tx2531Response tx2531Response = new Tx2531Response(respMsg[0], respMsg[1]);

                try
                {
                    Hi.Model.PAY_FastPayMent fModel = new Hi.BLL.PAY_FastPayMent().GetModel(id);
                    fModel.vdef1 = tx2531Response.getCode();
                    fModel.vdef2 = tx2531Response.getMessage();
                    fModel.ts = DateTime.Now;
                    fModel.modifyuser = logUser.UserID;
                    new Hi.BLL.PAY_FastPayMent().Update(fModel);
                }
                catch { }

                if (!"2000".Equals(tx2531Response.getCode()))//返回Code=2000代表成功
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"" + tx2531Response.getMessage() + "！\"}";
                    context.Response.Write(Josn);
                    return;
                }

            }
            //企业根据自己的业务要求编写相应的业务处理代码
            context.Response.Write("{\"id\":\"" + id + "\",\"msg\":\"验证码发送成功！\"}");
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
            response.End();
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