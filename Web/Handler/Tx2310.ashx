<%@ WebHandler Language="C#" Class="Tx2310" %>

using System;
using System.Web;
using System.Collections.Generic;

using CFCA.Payment.Api;
using System.Web.Configuration;

public class Tx2310 : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        string Josn = string.Empty;
        Hi.Model.PAY_FastPayMent fastpayModel = new Hi.Model.PAY_FastPayMent();
        int kind = -1;//区分平台或者企业

        int ddltype = Convert.ToInt32(request["ddltype"]); //账户类型
        int ddlbank = Convert.ToInt32(request["ddlbank"]);//开户银行
        string SltPesontype = Convert.ToString(request["SltPesontype"]);//证件类型
        string txtpesoncode = Convert.ToString(request["txtpesoncode"]);//身份证号码
        string txtDisUser = Convert.ToString(request["txtDisUser"]);//帐号名称
        string txtbankcode = Convert.ToString(request["txtbankcode"]);//帐号号码
        txtbankcode = txtbankcode.Replace(" ", "");

        int compid = Convert.ToInt32(request["txtcompId"]);

        int chkisno = Convert.ToInt32(request["chkisno"]);

        int id = Convert.ToInt32(request["id"]);
        kind = Convert.ToInt32(request["kind"]);



        try
        {
          

            int num = -1;

            //测试接口代码
            if (System.Configuration.ConfigurationManager.AppSettings["Paytest_zj"] == "1")
            {
                #region 判断是否有多个默认账户
                
                
              
                if (chkisno == 1)
                {
                    //判断是否有多个默认账户
                    if (kind == 1)
                        num = new Hi.BLL.PAY_PrePayment().GetDate("count(ID)", "sys_paymentbank", " isno=1 and dr=0").Rows.Count;
                    else
                        num = new Hi.BLL.PAY_PrePayment().GetBankBYCompID(compid);
                  
                    if (id > 0)
                    {
                        if (num >= 1)
                        {
                            Josn = "{\"ID\":\"1qaz\",\"msg\":\"一个企业不能同时存在两个第一收款账户，是否确定修改此账户为第一收款账户！\"}";
                            context.Response.Write(Josn);
                            return;
                        }

                    }
                    else
                    {
                        if (num >= 1)
                        {
                            Josn = "{\"ID\":\"1qaz\",\"msg\":\"一个企业不能同时存在两个第一收款账户，是否确定修改此账户为第一收款账户！\"}";
                            context.Response.Write(Josn);
                            return;
                        }


                    }

                }
                #endregion
                
                
                Josn = "{\"ID\":\"2000\",\"msg\":\"身份验证成功！\"}";
                context.Response.Write(Josn);
                return;
            }
            
            

            //只能验证账户类型是个人的收款账户
            if ( ddltype == 11)
            {

                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                PaymentEnvironment.Initialize(configPath);

                string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码



                Tx2310Request tx2310Request = new Tx2310Request();
                tx2310Request.setInstitutionID(institutionID);
                tx2310Request.setAccountType(ddltype);
                tx2310Request.setBankID(ddlbank.ToString());
                tx2310Request.setAccountName(txtDisUser);
                tx2310Request.setAccountNumber(txtbankcode);
                tx2310Request.setIdentificationType(SltPesontype);
                tx2310Request.setIdentificationNumber(txtpesoncode);
                tx2310Request.process();

                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx2310Request.getRequestMessage(), tx2310Request.getRequestSignature());

                Tx2310Response tx2310Response = new Tx2310Response(respMsg[0], respMsg[1]);

                if ("2000".Equals(tx2310Response.getCode()))
                {
                    if (tx2310Response.getStatus() == 20)
                    {
                        string xml = tx2310Response.getResponsePlainText();
                        var doc = new System.Xml.XmlDocument(); //实例化XmlDocument,怎么用这个,网上去查查      
                        doc.LoadXml(xml);//加载数据         
                        var node = doc.SelectSingleNode("Response/Body/IssueBankID"); // 查询节点      
                        int bankid = Convert.ToInt32(node.InnerText);

                        if (bankid == ddlbank)
                        {

                            //身份验证成功，校验是否多个默认账户
                            //Josn = "{\"ID\":\"2000\",\"msg\":\"身份验证成功！\"}";
                            //context.Response.Write(Josn);
                            #region 判断是否有多个默认账户

                            if (chkisno == 1)
                            {
                                //判断是否有多个默认账户
                                if (kind == 1)
                                    num = new Hi.BLL.PAY_PrePayment().GetDate("count(ID)", "sys_paymentbank", " isno=1 and dr=0").Rows.Count;
                                else
                                    num = new Hi.BLL.PAY_PrePayment().GetBankBYCompID(compid);

                                if (id > 0)
                                {
                                    if (num >= 1)
                                    {
                                        Josn = "{\"ID\":\"1qaz\",\"msg\":\"一个企业不能同时存在两个第一收款账户，是否确定修改此账户为第一收款账户！\"}";
                                        context.Response.Write(Josn);
                                        return;
                                    }

                                }
                                else
                                {
                                    if (num >= 1)
                                    {
                                        Josn = "{\"ID\":\"1qaz\",\"msg\":\"一个企业不能同时存在两个第一收款账户，是否确定修改此账户为第一收款账户！\"}";
                                        context.Response.Write(Josn);
                                        return;
                                    }


                                }

                            }
                            else
                            {
                                Josn = "{\"ID\":\"2000\",\"msg\":\"身份验证成功！\"}";
                                context.Response.Write(Josn);
                            }
                            #endregion


                        }
                        else
                        {
                            Josn = "{\"ID\":\"2001\",\"msg\":\"身份验证失败:开户银行、证件号码、账户名称、账户号码不匹配！\"}";
                            context.Response.Write(Josn);
                        }
                    }
                    else
                    {
                        //企业根据自己的业务要求编写相应的业务处理代码
                        Josn = "{\"ID\":\"2001\",\"msg\":\"身份验证失败:开户银行、证件号码、账户名称、账户号码不匹配！\"}";
                        context.Response.Write(Josn);
                        //context.Response.End();
                    }
                }
                else
                {

                    string mes = "接口调用失败！";
                    Josn = "{\"ID\":\"1\",\"msg\":\"" + mes + "！\"}";
                    context.Response.Write(Josn);
                    // context.Response.End();
                }
            }
            else if (ddltype == 12)
            {
                //if (chkisno == 1)
                //{
                //    //判断是否有多个默认账户
                //     num = new Hi.BLL.PAY_PrePayment().GetBankBYCompID(compid);
                //    if (id > 0)
                //    {
                //        if (num >= 1)
                //        {
                //            Josn = "{\"ID\":\"1qaz\",\"msg\":\"一个企业不能同时存在两个第一收款账户，是否确定修改此账户为第一收款账户！\"}";
                //            context.Response.Write(Josn);
                //        }
                //    }
                //    else
                //    {
                //        if (num >= 1)
                //        {
                //            Josn = "{\"ID\":\"1qaz\",\"msg\":\"一个企业不能同时存在两个第一收款账户，是否确定修改此账户为第一收款账户！\"}";
                //            context.Response.Write(Josn);
                //        }
                //    }
                //}
                Josn = "{\"ID\":\"2000\",\"msg\":\"身份验证成功！\"}";
                context.Response.Write(Josn);
            }
            else
            {
                Josn = "{\"ID\":\"2000\",\"msg\":\"身份验证成功！\"}";
                context.Response.Write(Josn);
                //context.Response.End();
            }
        }
        catch (Exception ex)
        {
            string mes = ex.Message;
            Josn = "{\"ID\":\"1\",\"msg\":\"" + mes + "！\"}";
            context.Response.Write(Josn);
        }
        finally
        {
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