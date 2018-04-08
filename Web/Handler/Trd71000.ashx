<%@ WebHandler Language="C#" Class="Trd71000" %>

using System;
using System.Web;
using System.Collections.Generic;
using LitJson;
using FinancingReferences;
using System.Text;

public class Trd71000 : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        Hi.Model.PAY_OpenAccount openAccM = null;

        // 1.取得参数
        //登录信息
        LoginModel logUser = context.Session["UserModel"] as LoginModel;
        if (logUser == null)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"请先登录！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }

        Hi.Model.BD_Distributor disModel = new Hi.BLL.BD_Distributor().GetModel(logUser.DisID);//代理商对象
        int disid = logUser.DisID;//代理商ID
        string a4 = Convert.ToString(request["hida4"]);//订单支付
        decimal txtPayOrder = Convert.ToDecimal(request["txtPayOrder"]);//借款金额

        List<Hi.Model.PAY_OpenAccount> CompOAcc = new Hi.BLL.PAY_OpenAccount().GetList("", " CompID=" + logUser.CompID + " and DisID=0 and isnull(dr,0)=0", "");
        if (CompOAcc.Count == 0)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"企业未开通在线融资！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        List<Hi.Model.BD_Ereceipt> eL = new Hi.BLL.BD_Ereceipt().GetList("", " CompID=" + logUser.CompID + " and isnull(dr,0)=0", "");
        if (eL.Count == 0)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"企业未维护仓单信息！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        if (disModel.FinancingRatio == 0)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"企业未维护融资比例！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }

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
        int orderid = Common.DesDecrypt(request["KeyID"], Common.EncryptKey).ToInt(0);//订单id
        if (orderid == 0)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"无效的订单！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        List<Hi.Model.PAY_Financing> PList = new Hi.BLL.PAY_Financing().GetList("", "DisID=" + logUser.DisID + " and OrderID=" + orderid + " and Type=4 and State in (1,3) and isnull(dr,0)=0", "");
        if (PList.Count > 0)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"每个订单只能借款一次！\",\"js\":\"$('#FinancingA').show();\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(orderid);
        if (!((
            (orderModel.Otype == (int)Enums.OType.赊销订单 && (orderModel.OState != (int)Enums.OrderState.退回 && orderModel.OState != (int)Enums.OrderState.未提交 && orderModel.OState != (int)Enums.OrderState.待审核) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付)) ||
            (orderModel.Otype != (int)Enums.OType.赊销订单 && orderModel.Otype != (int)Enums.OType.推送账单 && (orderModel.OState == (int)Enums.OrderState.已审 || orderModel.OState == (int)Enums.OrderState.已发货 || orderModel.OState == (int)Enums.OrderState.已到货) && (orderModel.PayState == (int)Enums.PayState.未支付 || orderModel.PayState == (int)Enums.PayState.部分支付))
            )
            && orderModel.OState != (int)Enums.OrderState.已作废))
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"订单异常，不能申请借款！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        if (Convert.ToDecimal(request["txtPayOrder"].Trim().ToString()) > orderModel.AuditAmount + orderModel.OtherAmount - orderModel.PayedAmount)
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"支付金额大于未支付金额，不能申请借款！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        List<Hi.Model.PAY_Withdrawals> Lwithdrawals = new Hi.BLL.PAY_Withdrawals().GetList("", " isnull(dr,0)=0  and DisID=" + logUser.DisID, "");
        if (openAccL.Count > 0 && Lwithdrawals.Count > 0)
        {
            decimal price = Convert.ToDecimal(request["txtPayOrder"].Trim().ToString()) * 
                Convert.ToDecimal(disModel.FinancingRatio) / 100;//可融资金额最高金额
          
            if (price < 100)
            {
                //disModel.FinancingRatio融资比例，例：70
                //x * (70/100) = 100
                string Josn = "{\"error\":\"1\",\"msg\":\"本次支付金额少于" + Math.Ceiling(10000.00 / disModel.FinancingRatio).ToString("0.00") + "元，不能申请借款！\"}";
                context.Response.Write(Josn);
                context.Response.End();
            }
            /*
            string msghd_rspcode = "";
            string msghd_rspmsg = "";
            decimal money = 0;
            try
            {
                IPubnetwk IPT = new IPubnetwk();
                StringBuilder Sjson = new StringBuilder();
                Sjson.Append("{\"msghd_trdt\":\"" + DateTime.Now.ToString("yyyyMMdd") + "\",\"srl_ptnsrl\":\"" + Common.Number_repeat("1") + "\",");
                Sjson.Append("\"cltacc_cltno\":\"" + openAccL[0].AccNumver + "\",\"cltacc_cltnm\":\"" + openAccL[0].AccName + "\",");
                Sjson.Append("\"bkacc_accno\":\"" + Lwithdrawals[0].AccNo + "\",\"bkacc_accnm\":\"" + openAccL[0].AccName + "\"}");
                string json = IPT.trd13010(Sjson.ToString());
                JsonData Jdata = JsonMapper.ToObject(json);
                msghd_rspcode = Jdata["msghd_rspcode"].ToString();
                msghd_rspmsg = Jdata["msghd_rspmsg"].ToString();
                money = Jdata["amt_balamt"].ToString().ToDecimal(0) / 100;
            }
            catch
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"接口不通，请稍后再试！\"}";
                context.Response.Write(Josn);
                context.Response.End();
            }
            if ("000000".Equals(msghd_rspcode))
            {
                
            }
            else
            {
                string Josn = "{\"error\":\"1\",\"msg\":\"" + msghd_rspmsg + "\"}";
                context.Response.Write(Josn);
                context.Response.End();
            }
             * */
        }
        else
        {
            string Josn = "{\"error\":\"1\",\"msg\":\"请先维护结算用户！\"}";
            context.Response.Write(Josn);
            context.Response.End();
        }
        if (a4 == "1")
        {
            int finaID = 0;
            string guid = Common.Number_repeat("0");
            decimal price = Convert.ToDecimal(request["txtPayOrder"].Trim().ToString()) * Convert.ToDecimal(disModel.FinancingRatio) / 100;//可融资最高金额
            //price = price - price % 100;
            try
            {
                //融资记录表
                Hi.Model.PAY_Financing finaM = new Hi.Model.PAY_Financing();
                finaM.DisID = disid;
                finaM.CompID = logUser.CompID;
                finaM.State = 2;
                finaM.OpenAccID = openAccM.ID;
                finaM.OrderID = orderid;
                finaM.AclAmt = price;
                finaM.Guid = guid;
                finaM.Type = 4;
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
                String loan_loanno = "";
                String actionUrl = "";
                String ptncode = "";
                String trdcode = "";
                String message = "";
                String signature = "";
                String Jsons = "";
                try
                {
                    BLL.Service.FinancingOrder fo = new BLL.Service.FinancingOrder();
                    fo.msghd_trdt = DateTime.Now.ToString("yyyyMMdd");//交易日期
                    fo.srl_ptnsrl = guid;//合作方交易流水号
                    fo.cltacc_cltno = openAccM.AccNumver;//代理商号
                    fo.cltacc_cltnm = openAccM.AccName;//代理商名称
                    fo.loan_loancd = "L07";//借款品种
                    decimal loanamt = (int)(Convert.ToDecimal(request["txtPayOrder"].Trim().ToString()) * 100 * Convert.ToDecimal(disModel.FinancingRatio) / 100);
                    loanamt = loanamt - loanamt % 10000;
                    fo.loan_totalamt = ((int)(Convert.ToDecimal(request["txtPayOrder"].Trim().ToString()) * 100)).ToString();//订单金额合计，单位：分
                    fo.loan_loanamt = loanamt.ToString();//申请借款金额
                    fo.loan_selfamt = ((int)(Convert.ToDecimal(request["txtPayOrder"].Trim().ToString()) * 100) - loanamt).ToString();//代理商自筹金额，单位：分
                    fo.useage = "用于支付订单";

                    fo.billInfoList = new List<BLL.Service.BillInfo>();
                    Hi.BLL.DIS_OrderDetail OrderDetailBll = new Hi.BLL.DIS_OrderDetail();  //订单明细表
                    List<Hi.Model.DIS_OrderDetail> l = OrderDetailBll.GetList("", " isnull(dr,0)=0 and OrderId=" + orderid, "Id desc");
                    //订单信息
                    Hi.Model.BD_GoodsInfo goodsinfoM = new Hi.BLL.BD_GoodsInfo().GetModel(l[0].GoodsinfoID);
                    BLL.Service.BillInfo billInfo = new BLL.Service.BillInfo();
                    billInfo.pcustno = openAccM.AccNumver;// 付款方代理商号
                    billInfo.pcustnm = openAccM.AccName;// 付款方账户名称
                    billInfo.pecustno = CompOAcc[0].AccNumver;// 收款方代理商号
                    billInfo.pecustnm = CompOAcc[0].AccName;// 收款方账户名称
                    billInfo.billtype = "0";// 订单类型0订单类;1应收账款类
                    billInfo.orderno = orderModel.ReceiptNo;// 支付单号(若订单则为订单编号、若应收账款则为应收账款编号)
                    billInfo.totalamt = ((int)(Convert.ToDecimal(request["txtPayOrder"].Trim().ToString()) * 100)).ToString();// 总金额(单位：分
                    billInfo.paidamt = "0";// 已付金额(单位：分)应收账款必填
                    billInfo.unpaidamt = ((int)(Convert.ToDecimal(request["txtPayOrder"].Trim().ToString()) * 100)).ToString();// 未付金额(单位：分)应收账款必填
                    billInfo.gdsnm = SelectGoods.GoodsName(goodsinfoM.GoodsID, "GoodsName");// 商品名称（应收账款则填写“应收账款”）应收账款必填
                    billInfo.gdsdic = l[0].GoodsInfos == "" ? Common.GetGoodsMemo(goodsinfoM.GoodsID) : l[0].GoodsInfos;// 商品描述（商品属性的综合描述以“；”分割。）

                    Hi.Model.BD_DisAddr DisAddr = new Hi.BLL.BD_DisAddr().GetModel(orderModel.AddrID);
                    if (DisAddr != null)
                    {
                        billInfo.dly = DisAddr.Address;// 交货方式；交货地（分号隔开）
                    }
                    else
                    {
                        billInfo.dly = "默认";
                    }
                    billInfo.sgndt = orderModel.CreateDate.ToString("yyyyMMdd");// 签订日期
                    billInfo.duedate = "30000101";// 截止日
                    fo.billInfoList.Add(billInfo);
                    //仓单信息
                    fo.ereceiptList = new List<BLL.Service.Ereceipt>();
                    if (l.Count > 0)
                    {
                        foreach (Hi.Model.DIS_OrderDetail item in l)
                        {
                            Hi.Model.BD_GoodsInfo goodsinfoModel = new Hi.BLL.BD_GoodsInfo().GetModel(item.GoodsinfoID);
                            if (goodsinfoModel != null)
                            {
                                BLL.Service.Ereceipt ereceipt = new BLL.Service.Ereceipt();
                                ereceipt.rtbill = orderModel.CreateDate.ToString("yyyyMMdd") + getIDLen(item.ID.ToString());//编号
                                ereceipt.kd = goodsinfoModel.CreateDate.ToString("yyyyMMdd") + goodsinfoModel.ID.ToString(); //品种
                                ereceipt.nm = SelectGoods.GoodsName(goodsinfoModel.GoodsID, "GoodsName");//名称
                                ereceipt.std = eL[0].ereceipt_std;//规格
                                ereceipt.grd = eL[0].ereceipt_grd;//等级
                                ereceipt.unit = SelectGoods.GoodsName(goodsinfoModel.GoodsID, "Unit");//单位
                                ereceipt.num = item.GoodsNum.ToString("0.00");//数量
                                ereceipt.price = ((int)item.Price * 100).ToString();//参考价格
                                ereceipt.value = ((int)(item.GoodsNum * item.Price * 100)).ToString();//参考价值
                                ereceipt.brd = eL[0].ereceipt_brd;//品牌
                                ereceipt.chkbill = eL[0].ereceipt_chkbill;//检验证书编号
                                ereceipt.duedate = eL[0].ereceipt_duedate.ToString("yyyyMMdd"); //失效日期
                                ereceipt.gdsdic = eL[0].ereceipt_gdsdic;//商品描述
                                ereceipt.hder = eL[0].ereceipt_hder;//货物归属方
                                ereceipt.gds = eL[0].ereceipt_gds;//货位
                                ereceipt.whnm = eL[0].ereceipt_whnm;//仓库名称
                                ereceipt.whno = eL[0].ereceipt_whno;//仓库编号
                                ereceipt.batchno = eL[0].ereceipt_batchno;//批次号
                                ereceipt.sgndt = eL[0].ereceipt_sgndt.ToString("yyyyMMdd");//签发日期
                                ereceipt.mfters = eL[0].ereceipt_mfters;//生成厂家
                                fo.ereceiptList.Add(ereceipt);
                            }
                        }
                    }
                    Jsons = JsonMapper.ToJson(fo);
                }
                catch { }
                String Json = "";
                try
                {
                    IPubnetwk ipu = new IPubnetwk();
                    Json = ipu.trd71000(Jsons);
                }
                catch
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"接口连接失败！\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
                //String Json = "{\"msghd_rspcode\":\"000000\",\"msghd_rspmsg\":\"\",\"actionUrl\":\"https://111.205.98.184:80/netwk/interfaceI.htm\",\"ptncode\":\"HYJR0026\",\"trdcode\":\"71000\",\"message\":\"PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iR0JLIj8+PE1TRyB2ZXJzaW9uPSIxLjAiPjxNU0dIRD48VHJDZD43MTAwMDwvVHJDZD48VHJEdD4yMDE1MTAxMzwvVHJEdD48VHJUbT4xNDUxMjI8L1RyVG0+PFRyU3JjPkY8L1RyU3JjPjxQdG5DZD5IWUpSMDAyNjwvUHRuQ2Q+PEJrQ2Q+WkpZSDAwMDA8L0JrQ2Q+PC9NU0dIRD48U3JsPjxQdG5Tcmw+MTQ0NDcxODgxNTA1MDwvUHRuU3JsPjwvU3JsPjxDbHRBY2M+PENsdE5vPjgwMDIxMDIwPC9DbHRObz48Q2x0Tm0+5rWL6K+V5pWw5o2u5Lya5ZGY5LiAPC9DbHRObT48L0NsdEFjYz48bG9hbj48TG9hbkNkPkwwNzwvTG9hbkNkPjxUb3RhbEFtdD41MjAwMDAwPC9Ub3RhbEFtdD48TG9hbkFtdD4zMDAwMDAwPC9Mb2FuQW10PjxTZWxmQW10PjIyMDAwMDA8L1NlbGZBbXQ+PENjeUNkPkNOWTwvQ2N5Q2Q+PC9sb2FuPjxVc2VBZ2U+55So5LqO5pSv5LuY5ZCI5ZCM5qy+PC9Vc2VBZ2U+PGJpbGxJbmZvPjxQQ3VzdE5vPjgwMDIxMDIwPC9QQ3VzdE5vPjxQQ3VzdE5tPua1i+ivleaVsOaNruS8muWRmOS4gDwvUEN1c3RObT48UGVDdXN0Tm8+ODAwMjEwMjE8L1BlQ3VzdE5vPjxQZUN1c3RObT7mtYvor5XmlbDmja7kvJrlkZjkuow8L1BlQ3VzdE5tPjxCaWxsVHlwZT4wPC9CaWxsVHlwZT48T3JkZXJObz5aRkRfMTQ0NDcxODgxNTA1MDwvT3JkZXJObz48Q2N5Q2Q+Q05ZPC9DY3lDZD48VG90YWxBbXQ+NTIwMDAwMDwvVG90YWxBbXQ+PFBhaWRBbXQ+MDwvUGFpZEFtdD48VW5wYWlkQW10PjUyMDAwMDA8L1VucGFpZEFtdD48R2RzTm0+5Lic5YyX5aSn57GzPC9HZHNObT48R2RzRGljPuivpeWkp+exs+W+iOWlveWQgzwvR2RzRGljPjxEbHk+5Lic5YyX5rKI6ZizPC9EbHk+PFNnbkR0PjIwMTUxMDEzPC9TZ25EdD48RHVlRGF0ZT4yMDE2MDgwMTwvRHVlRGF0ZT48L2JpbGxJbmZvPjxlcmVjZWlwdD48UnRCaWxsPkNEXzE0NDQ3MTg4MTUwNTE8L1J0QmlsbD48S2Q+RE1fMDI8L0tkPjxObT7lpKfnsbMwMuWPtzwvTm0+PFN0ZD4yKjIqMTAwPC9TdGQ+PEdyZD7kuoznuqc8L0dyZD48VW5pdD7lkKg8L1VuaXQ+PE51bT4yMC4wPC9OdW0+PFByaWNlPjI2MDAwMDwvUHJpY2U+PFZhbHVlPjUyMDAwMDA8L1ZhbHVlPjxCcmQ+5Lic5YyX5aSn57GzPC9CcmQ+PENoa0JpbGw+WkpfMTQ0NDcxODgxNTA1MTwvQ2hrQmlsbD48RHVlRGF0ZT4yMDE2MDcwNjwvRHVlRGF0ZT48R2RzRGljPuivpeWkp+exs+W+iOWlveWQgzwvR2RzRGljPjxIZGVyPum7kem+meaxn+WMl+aWueaBkumYs+mjn+WTgeaciemZkOWFrOWPuDwvSGRlcj48R2RzPjnlj7fkvY08L0dkcz48V2hObT7pu5HpvpnmsZ8y5Y+35bqTPC9XaE5tPjxXaE5vPkhMSl8yOTEyPC9XaE5vPjxCYXRjaE5vPlBDXzE0NDQ3MTg4MTUwNTE8L0JhdGNoTm8+PE1mdGVycz7pu5HpvpnmsZ88L01mdGVycz48Q2N5Q2Q+Q05ZPC9DY3lDZD48L2VyZWNlaXB0PjwvTVNHPg==\",\"signature\":\"A3081EF94696F38A3F79FB8474D81D7FFF55F049BACA323AF1F8277AB5E42436E1CD9705B12133400CA5764BD5862252EF57C02FEFDE3D058173350279E26D5E317F1A66659792E508F96F97BE3EA3D31F6B154310F99B7E53DD8C6E47970743C8C514858C986F91A10D477C7DE1772505B53515ADDC5BFADE757B1EAD7649EC\"}";
                try
                {
                    JsonData Params = JsonMapper.ToObject(Json);
                    msghd_rspcode = Params["msghd_rspcode"].ToString();
                    msghd_rspmsg = Params["msghd_rspmsg"].ToString();
                    loan_loanno = Params["loan_loanno"].ToString();//借款单号
                    actionUrl = Params["actionUrl"].ToString();
                    ptncode = Params["ptncode"].ToString();
                    trdcode = Params["trdcode"].ToString();
                    message = Params["message"].ToString();
                    signature = Params["signature"].ToString();
                }
                catch
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"返回参数有误！\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
                if ("000000".Equals(msghd_rspcode))
                {
                    try
                    {
                        Hi.Model.PAY_Financing finaM = new Hi.BLL.PAY_Financing().GetModel(finaID);
                        finaM.State = 3;
                        finaM.vdef2 = loan_loanno;//借款单号
                        finaM.ts = DateTime.Now;
                        new Hi.BLL.PAY_Financing().Update(finaM);
                    }
                    catch { }
                    //form提交数据到融资平台
                    string Josn = "{\"success\":\"1\",\"msg\":\"成功！\",\"js\":\"$('#divFinaMsg').show();var tempForm = document.createElement('form');tempForm.id = 'tempForm1';tempForm.method = 'post';tempForm.action = '" + actionUrl + "';tempForm.target = '_blank';var hideInput2 = document.createElement('input');hideInput2.type = 'hidden';hideInput2.name = 'ptncode';hideInput2.value = '" + ptncode + "';var hideInput3 = document.createElement('input');hideInput3.type = 'hidden';hideInput3.name = 'trdcode';hideInput3.value = '" + trdcode + "';var hideInput4 = document.createElement('input');hideInput4.type = 'hidden';hideInput4.name = 'message';hideInput4.value = '" + message + "';var hideInput5 = document.createElement('input');hideInput5.type = 'hidden';hideInput5.name = 'signature';hideInput5.value = '" + signature + "';tempForm.appendChild(hideInput2);tempForm.appendChild(hideInput3);tempForm.appendChild(hideInput4);tempForm.appendChild(hideInput5);document.body.appendChild(tempForm);tempForm.submit();document.body.removeChild(tempForm);\"}";
                    context.Response.Write(Josn);
                    context.Response.End();
                }
                else
                {
                    string Josn = "{\"error\":\"1\",\"msg\":\"" + msghd_rspmsg + "\"}";
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

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    public string getIDLen(string ID)
    {
        string newID = ID;
        for (int i = 0; i < 6; i++)
        {
            if (newID.Length == 6 || newID.Length > 6)
            {
                break;
            }
            else
            {
                newID = "0" + newID;
            }
        }
        return newID;
    }
}