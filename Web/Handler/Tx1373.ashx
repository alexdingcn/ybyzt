<%@ WebHandler Language="C#" Class="Tx1373" %>

using System;
using System.Web;
using System.Collections.Generic;
using DBUtility;
using System.Data.SqlClient;

using CFCA.Payment.Api;
using System.Web.Configuration;

using System.Web.SessionState;
public class Tx1373 : IHttpHandler, IRequiresSessionState
{

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        Hi.Model.PAY_PrePayment prepayModel = new Hi.Model.PAY_PrePayment();

        try
        {
            //订单ID
            int ordID = 2268;//Convert.ToInt32(request["ordID"]);
            //登录信息
            LoginModel logUser = context.Session["UserModel"] as LoginModel;
            if (logUser == null)
            {
                string Josn = ErrMessage("请先登录");
                context.Response.Write(Josn);
                return;
            }
           
           
            
            decimal txtPayOrder = 0;//订单已支付金额
           

            Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(ordID);
            if (orderModel == null)
            {
                string Josn = ErrMessage("数据有误");
                context.Response.Write(Josn);
                return;
            }
            if (!(
                (orderModel.PayState == (int)Enums.PayState.已支付 || orderModel.PayState == (int)Enums.PayState.部分支付)
                 && orderModel.OState != (int)Enums.OrderState.已作废))
            {
                string Josn = string.Empty;
                if (orderModel.Otype == (int)Enums.OType.推送账单)
                    Josn = ErrMessage("账单异常，不能支付");
                else
                    Josn = ErrMessage("订单异常，不能支付");
                context.Response.Write(Josn);
                return;
            }

            txtPayOrder = orderModel.PayedAmount;
            if (txtPayOrder <= 0)
            {
                string Josn = ErrMessage("数据异常！");
                context.Response.Write(Josn);
                return;
            }




            String institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构代码
            
            int regid = 0;

            List<Hi.Model.PAY_Payment> payModel_list = new Hi.BLL.PAY_Payment().GetList("", "ISNULL(dr,0)=0 and OrderID=" + orderModel.ID, "");

            foreach (Hi.Model.PAY_Payment paymodel in payModel_list)
            {
                // 2.创建交易请求对象
                Tx1373Request tx1373Request = new Tx1373Request();
                tx1373Request.setInstitutionID(institutionID);
                tx1373Request.setTxSN(paymodel.guid);//退款交易流水号
                tx1373Request.setOrderNo(paymodel.vdef4 + paymodel.ID.ToString());//支付订单号 = 支付订单号前半部分 + 支付表ID;
                tx1373Request.setPaymentNo(paymodel.guid);//支付交易流水号
                decimal Return_priec = paymodel.PayPrice;
                decimal sxf = Convert.ToDecimal(paymodel.vdef5);
                long payEd =Convert.ToInt32((Return_priec - sxf) * 100);
                tx1373Request.setAmount(payEd);//退款金额
                tx1373Request.setRemark("快捷支付退款");

                // 3.执行报文处理
                tx1373Request.process();
                
                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx1373Request.getRequestMessage(), tx1373Request.getRequestSignature());

                Tx1373Response tx1373Response = new Tx1373Response(respMsg[0], respMsg[1]);
                try
                {
                    //regModel = new Hi.BLL.PAY_RegisterLog().GetModel(regid);
                    //regModel.PlanMessage = tx1375Request.getRequestPlainText();
                    //regModel.Start = tx1375Response.getCode();
                    //regModel.ResultMessage = tx1375Response.getMessage();
                    //new Hi.BLL.PAY_RegisterLog().Update(regModel);
                }
                catch (Exception ex) { }

                if (!("2000".Equals(tx1373Response.getCode())))//返回Code=2000代表成功
                {
                    //ClientScript.RegisterStartupScript(this.GetType(), "pay", "<script>$('.opacity').fadeIn(200);$('.tip').fadeIn(200);$('#paying').hide();$('#txtPhoneNum').show();$('#msgtwo').hide();$('#msgone').show();msgTime(120);</script>");
                    string Josn1 = ErrMessage(tx1373Response.getMessage());
                    context.Response.Write(Josn1);
                    return;
                }

            }
           

               
                        
           // string Josn2 = "{\"success\":\"1\",\"payid\":\"" + payid + "\",\"prepayid\":\"" + prepayid + "\",\"js\":\"$('#phone').html('" + phoneHtml + "');$('.opacity').fadeIn(200);$('.tip').fadeIn(200);$('#paying').hide();$('#txtPhoneNum').show();$('#msgtwo').hide();$('#msgone').show();msgTime(120);\"}";
            string Josn2 = "";
            context.Response.Write(Josn2);
            return;
        }
        catch (Exception ex)
        {
            string Josn1 = ErrMessage(ex.Message);
            context.Response.Write(Josn1);
            return;
        }
        finally
        {
            context.Response.End();
        }
    }

    public string ErrMessage(string msg)
    {
        return "{\"error\":\"1\",\"msg\":\""+msg+"！\"}";
    }
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}