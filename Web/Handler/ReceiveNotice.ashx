<%@ WebHandler Language="C#" Class="ReceiveNotice" %>

using System;
using System.Web;
using System.Text;

using CFCA.Payment.Api;

public class ReceiveNotice : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        Console.WriteLine("---------- Begin [ReceiveNotice] process......");

        HttpRequest request = context.Request;
        HttpResponse response = context.Response;

        //1 获得参数message和signature
        String message = request.Form["message"];
        String signature = request.Form["signature"];

        Console.WriteLine("[message]=[" + message + "]");
        Console.WriteLine("[signature]=[" + signature + "]");

        //2 生成交易结果对象
        NoticeRequest noticeRequest = new NoticeRequest(message, signature);
        string a = noticeRequest.getTxCode();
        //3 业务处理
        if ("1118".Equals(noticeRequest.getTxCode()))
        {
            Notice1118Request nr = new Notice1118Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
            //以下为演示代码
            Console.WriteLine("[TxName]       = [企业订单支付状态变更通知]");
            Console.WriteLine("[TxCode]       = [1118]");
            Console.WriteLine("[InstitutionID]= [" + nr.getInstitutionID() + "]");
            Console.WriteLine("[PaymentNo]    = [" + nr.getPaymentNo() + "]");
            Console.WriteLine("[Amount]       = [" + nr.getAmount() + "]");
            Console.WriteLine("[Status]       = [" + nr.getStatus() + "]");
            Console.WriteLine("[BankNotificationTime]       = [" + nr.getBankNotificationTime() + "]");

        }
        else if ("1119".Equals(noticeRequest.getTxCode()))
        {
            Notice1119Request nr = new Notice1119Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
            //以下为演示代码
            Console.WriteLine("[TxName]       = [企业订单支付状态变更（通知收款行）]");
            Console.WriteLine("[TxCode]       = [1119]");
            Console.WriteLine("[BranchID]= [" + nr.getBranchID() + "]");
            Console.WriteLine("[InstitutionID]      = [" + nr.getInstitutionID() + "]");
            Console.WriteLine("[PaymentNo] = [" + nr.getPaymentNo() + "]");
            Console.WriteLine("[PayerID]      = [" + nr.getPayerID() + "]");
            Console.WriteLine("[PayerName] = [" + nr.getPayerName() + "]");
            Console.WriteLine("[Amount]       = [" + nr.getAmount() + "]");
            Console.WriteLine("[Status]       = [" + nr.getStatus() + "]");
            Console.WriteLine("[Usage]       = [" + nr.getUsage() + "]");
            Console.WriteLine("[Remark]       = [" + nr.getRemark() + "]");
            Console.WriteLine("[PaidTime]       = [" + nr.getPaidTime() + "]");

            // 响应支付平台
            Notice1119Response notice1119Response = new Notice1119Response();
            notice1119Response.setPaymentNo(nr.getPaymentNo());
            String response1119 = Convert.ToBase64String(Encoding.UTF8.GetBytes(notice1119Response.process()));
            response.Write(response1119);
            response.Flush();
            response.Close();
            return;

        }
        else if ("1138".Equals(noticeRequest.getTxCode()))
        {
            Notice1138Request nr = new Notice1138Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
            //以下为演示代码
            Console.WriteLine("[TxName]       = [企业订单退款结算状态变更通知]");
            Console.WriteLine("[TxCode]       = [1138]");
            Console.WriteLine("[InstitutionID]      = [" + nr.getInstitutionID() + "]");
            Console.WriteLine("[SerialNumber] = [" + nr.getSerialNumber() + "]");
            Console.WriteLine("[PaymentNo]      = [" + nr.getPaymentNo() + "]");
            Console.WriteLine("[Amount]       = [" + nr.getAmount() + "]");
            Console.WriteLine("[Status]       = [" + nr.getStatus() + "]");
            Console.WriteLine("[RefundTime]       = [" + nr.getRefundTime() + "]");

        }
        else if ("1318".Equals(noticeRequest.getTxCode()))
        {
            Notice1318Request nr = new Notice1318Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
            //以下为演示代码
            Console.WriteLine("[TxName]       = [市场订单支付状态变更通知]");
            Console.WriteLine("[TxCode]       = [1318]");
            Console.WriteLine("[InstitutionID]= [" + nr.getInstitutionID() + "]");
            Console.WriteLine("[PaymentNo]    = [" + nr.getPaymentNo() + "]");
            Console.WriteLine("[Amount]       = [" + nr.getAmount() + "]");
            Console.WriteLine("[Status]       = [" + nr.getStatus() + "]");
            Console.WriteLine("[BankNotificationTime]       = [" + nr.getBankNotificationTime() + "]");
        }
        else if ("1348".Equals(noticeRequest.getTxCode()))
        {
            Notice1348Request nr = new Notice1348Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
            //以下为演示代码
            Console.WriteLine("[TxName]       = [市场订单结算状态变更通知]");
            Console.WriteLine("[TxCode]       = [1348]");
            Console.WriteLine("[InstitutionID]= [" + nr.getInstitutionID() + "]");
            Console.WriteLine("[OrderNo]      = [" + nr.getOrderNo() + "]");
            Console.WriteLine("[SerialNumber] = [" + nr.getSerialNumber() + "]");
            Console.WriteLine("[Amount]       = [" + nr.getAmount() + "]");
            Console.WriteLine("[Status]       = [" + nr.getStatus() + "]");
            Console.WriteLine("[TransferTime]       = [" + nr.getTransferTime() + "]");
        }
        else if ("1363".Equals(noticeRequest.getTxCode()))
        {
            Notice1363Request nr = new Notice1363Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
            //以下为演示代码
            Console.WriteLine("[TxName]       = [市场订单单笔代收状态变更通知]");
            Console.WriteLine("[TxCode]       = [1363]");
        }
        else if ("1438".Equals(noticeRequest.getTxCode()))
        {
            Notice1438Request nr = new Notice1438Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("1712".Equals(noticeRequest.getTxCode()))
        {
            Notice1712Request nr = new Notice1712Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("1722".Equals(noticeRequest.getTxCode()))
        {
            Notice1722Request nr = new Notice1722Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("1732".Equals(noticeRequest.getTxCode()))
        {
            Notice1732Request nr = new Notice1732Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("2018".Equals(noticeRequest.getTxCode()))
        {
            Notice2018Request nr = new Notice2018Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("2118".Equals(noticeRequest.getTxCode()))
        {
            Notice2118Request nr = new Notice2118Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("2138".Equals(noticeRequest.getTxCode()))
        {
            Notice2138Request nr = new Notice2138Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("2218".Equals(noticeRequest.getTxCode()))
        {
            Notice2218Request nr = new Notice2218Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("3118".Equals(noticeRequest.getTxCode()))
        {
            Notice3118Request nr = new Notice3118Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("3218".Equals(noticeRequest.getTxCode()))
        {
            Notice3218Request nr = new Notice3218Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("4203".Equals(noticeRequest.getTxCode()))
        {
            Notice4203Request nr = new Notice4203Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("4218".Equals(noticeRequest.getTxCode()))
        {
            //Notice4218Request nr = new Notice4218Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("4233".Equals(noticeRequest.getTxCode()))
        {
            Notice4233Request nr = new Notice4233Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("4243".Equals(noticeRequest.getTxCode()))
        {
            Notice4243Request nr = new Notice4243Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("4247".Equals(noticeRequest.getTxCode()))
        {
            Notice4247Request nr = new Notice4247Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("4253".Equals(noticeRequest.getTxCode()))
        {
            Notice4253Request nr = new Notice4253Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("4257".Equals(noticeRequest.getTxCode()))
        {
            Notice4257Request nr = new Notice4257Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("4263".Equals(noticeRequest.getTxCode()))
        {
            //Notice4263Request nr = new Notice4263Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else if ("4322".Equals(noticeRequest.getTxCode()))
        {
            Notice4322Request nr = new Notice4322Request(noticeRequest.getDocument());
            //！！！ 在这里添加企业处理逻辑！！！
        }
        else
        {
            Console.WriteLine("！！！ 错误的通知 ！！！");
            Console.WriteLine("[txCode]       = [????]");
            Console.WriteLine("[txName]       = [未知通知类型]");
        }

        Console.WriteLine("[plainText]=[" + noticeRequest.getPlainText() + "]");

        //4 响应支付平台
        String messageResponse = Convert.ToBase64String(Encoding.UTF8.GetBytes(new NoticeResponse().getMessage()));

        response.Clear();
        response.Write(messageResponse);
        //response.Flush();
        response.End();
        response.Close();

        Console.WriteLine("---------- End [ReceiveNotice] process.");
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}