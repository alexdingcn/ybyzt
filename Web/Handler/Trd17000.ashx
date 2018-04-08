<%@ WebHandler Language="C#" Class="Trd17000" %>

using System;
using System.Web;
using System.Collections.Generic;
using LitJson;
using FinancingReferences;

public class Trd17000 : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        Hi.Model.PAY_OpenAccount openAccM = null;

        // 1.取得参数
        string username = Convert.ToString(request.Form["hidUserName"]);//用户名称
       // Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(username);//用户对象
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;

        if (logUser == null) {
            string Josn = "{\"error\":\"1\",\"msg\":\"请先登录！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        
        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(logUser.DisID);//代理商对象
        int disid = logUser.DisID;//代理商ID
        string a4 = Convert.ToString(request["hida4"]);//订单支付
        decimal txtPayOrder = Convert.ToDecimal(request["txtPayOrder"]);//支付金额
        
        List<Hi.Model.PAY_OpenAccount> openAccL = new Hi.BLL.PAY_OpenAccount().GetList("", "DisID=" + disid + " and State=1 and isnull(dr,0)=0", "");
        if (openAccL.Count > 0)
        {
            openAccM = openAccL[0];
        }
        else
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"请先开户！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }

        List<Hi.Model.PAY_OpenAccount> CompOAcc = new Hi.BLL.PAY_OpenAccount().GetList("", " CompID=" + logUser.CompID + " and DisID=0 and isnull(dr,0)=0", "");
        if (CompOAcc.Count == 0)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"企业未开通在线融资！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }

        int orderid = Common.DesDecrypt(request["KeyID"], Common.EncryptKey).ToInt(0);//订单id
        if (orderid == 0)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"无效的订单！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }

        List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + logUser.DisID + " and OrderID=" + orderid + " and State=3 and isnull(dr,0)=0", "");
        
        Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(orderid);
        if (Convert.ToDecimal(request["txtPayOrder"].Trim().ToString()) > orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"支付金额大于未支付金额，不能支付！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        if (!((
            (orderModel.Otype == (int)Enums.OType.赊销订单 && (orderModel.OState != (int)Enums.OrderState.退回 && orderModel.OState != (int)Enums.OrderState.未提交 && orderModel.OState != (int)Enums.OrderState.待审核) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付)) ||
            (orderModel.Otype != (int)Enums.OType.赊销订单 && orderModel.Otype != (int)Enums.OType.推送账单 && (orderModel.OState == (int)Enums.OrderState.已审 || orderModel.OState == (int)Enums.OrderState.已发货 || orderModel.OState == (int)Enums.OrderState.已到货) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付))
            )
            && orderModel.OState != (int)Enums.OrderState.已作废))
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"订单异常，不能支付！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        if (a4 == "1")//是否使用融资
        {
            int finaID = 0;
            string guid = Common.Number_repeat("0");
            try
            {
                Hi.Model.PAY_Financing finaM = new Hi.Model.PAY_Financing();
                finaM.DisID = disid;
                finaM.CompID = logUser.CompID;
                finaM.State = 2;
                finaM.OpenAccID = openAccM.ID;
                finaM.OrderID = orderid;
                finaM.AclAmt = Convert.ToDecimal(request["txtPayOrder"].Trim().ToString());
                finaM.Guid = guid;
                finaM.Type = 3;
                finaM.CcyCd = "CNY";
                finaM.ts = DateTime.Now;
                finaM.dr = 0;
                finaM.modifyuser = logUser.UserID;
                finaID = new Hi.BLL.PAY_Financing().Add(finaM);
            }
            catch 
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\"}";
                context.Response.Write(Josn);
                context.Response.End();
            }
            if (finaID > 0)
            {
                String msghd_rspcode = "";
                String msghd_rspmsg = "";
                String Jsons = "";
                try
                {
                    BLL.Service.OrderPay op = new BLL.Service.OrderPay();
                    op.msghd_trdt = DateTime.Now.ToString("yyyyMMdd");//交易日期
                    op.srl_ptnsrl = guid;//合作方交易流水号 
                    op.billinfo_pcltno = openAccM.AccNumver;//付款方代理商号
                    op.billinfo_pnm = openAccM.AccName;//付款方账户名称
                    op.billinfo_rcltno = CompOAcc[0].AccNumver;//收款方代理商号
                    op.billinfo_rcltnm = CompOAcc[0].AccName;//收款方账户名称 
                    op.billinfo_orderno = orderModel.ReceiptNo;//业务单号 
                    op.billinfo_billno = guid;//支付流水号（唯一）
                    op.billinfo_aclamt = ((int)(Convert.ToDecimal(request["txtPayOrder"].Trim().ToString()) * 100)).ToString();//本次支付金额
                    op.billinfo_payfee = "0";//付款方手续费,暂定0
                    op.billinfo_payeefee = "0";//收款方手续费,暂定0
                    op.billinfo_usage = "订单支付";//资金用途 
                    op.trsflag = "A00";//业务标示 A00 普通订单支付 B00 收款方支付冻结 [付款冻结] PS：冻结失败，资金回滚 B01 付款方解冻支付 [解冻退款]
                    op.suminfo_sum1 = "";//备注1
                    op.suminfo_sum2 = "";//备注2
                    Jsons = JsonMapper.ToJson(op);
                }
                catch { }
                String Json = "";
                try
                {
                    IPubnetwk ipu = new IPubnetwk();
                    Json = ipu.trd17000(Jsons);//调用java接口，返回msghd_rspcode msghd_rspmsg
                }
                catch
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"接口连接失败！\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
                //String Json = "{'msghd_rspcode','000000','msghd_rspmsg':'成功','msghd_trcd':'guid'}";
                try
                {
                    JsonData Params = JsonMapper.ToObject(Json);
                    msghd_rspcode = Params["msghd_rspcode"].ToString();
                    msghd_rspmsg = Params["msghd_rspmsg"].ToString();
                }
                catch
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"返回参数有误！\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
                if ("000000".Equals(msghd_rspcode))//msghd_rspcode=000000，融资接口调用成功
                {
                    try
                    {
                        Hi.Model.PAY_Financing Fin = new Hi.Model.PAY_Financing();
                        Fin = new Hi.BLL.PAY_Financing().GetModel(finaID);
                        Fin.State = 1;
                        Hi.Model.DIS_Order orderM = new Hi.BLL.DIS_Order().GetModel(orderid);
                        orderM.PayedAmount += Fin.AclAmt;
                        orderM.ts = DateTime.Now;
                        if (orderM.AuditAmount + orderM.OtherAmount == orderM.PayedAmount)
                        {
                            orderM.PayState = (int)Enums.PayState.已支付;
                        }
                        else
                        {
                            orderM.PayState = (int)Enums.PayState.部分支付;
                        }
                        if (new Hi.BLL.PAY_Financing().Update(Fin) && new Hi.BLL.DIS_Order().Update(orderM))
                        {
                            string Josn = "{\"success\":\"1\",\"msg\":\"成功！\",\"js\":\"window.location='PaySuccess.aspx?KeyID=" + Common.DesEncrypt(orderid.ToString(), Common.EncryptKey) + "&Pid=" + Common.DesEncrypt("0", Common.EncryptKey) + "&PPid=" + Common.DesEncrypt("0", Common.EncryptKey) + "&Fid=" + Common.DesEncrypt(finaID.ToString(), Common.EncryptKey) + "'\"}";
                            try
                            {
                                Utils.AddSysBusinessLog(orderModel.CompID, "Order", orderModel.ID.ToString(), "订单支付", "支付：" + (Fin.AclAmt).ToString("0.00") + "元(融资余额支付)", orderModel.CreateUserID.ToString());
                                if (orderModel.Otype != 9)
                                {
                                    OrderInfoType.AddIntegral(orderModel.CompID, orderModel.DisID, "1", 1, orderModel.ID, Fin.AclAmt, "订单支付", "", orderModel.CreateUserID);
                                }
                            }
                            catch { }
                            context.Response.Write(Josn);
                            context.Response.End();
                        }
                    }
                    catch { }
                }
                else
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"" + msghd_rspmsg + "！\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
            }
        }
        else
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"系统繁忙，请稍后！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}