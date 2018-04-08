using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DBUtility;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web.Mail;
//using System.Web.UI.HtmlControls;

/// <summary>
///PayInfoType 支付
/// </summary>
public class PayInfoType
{
    public static Hi.BLL.PAY_PrePayment OrderBll = new Hi.BLL.PAY_PrePayment();

    public static readonly string LocalSqlServer = SqlHelper.LocalSqlServer;

    public PayInfoType()
    {
        //
        //TODO: 在此处添加构造函数逻辑
        //
    }

    /// <summary>
    /// 款项类型
    /// </summary>
    /// <param name="type">1,转账汇款2,手工录入3,录入冲正4,退款</param>
    /// <returns></returns>
    public static string PreType(int type)
    {
        string PreType = string.Empty;

        if (type == 1)
        {
            PreType = "转账汇款";
        }
        else if (type == 2)
        {
            PreType = "手工补录";
        }
        else if (type == 3)
        {
            PreType = "补录冲正";
        }
        else if (type == 4)
        {
            PreType = "退款";
        }
        return PreType;
    }
    /// <summary>
    /// 审批状态
    /// </summary>
    /// <param name="type">2,已审 1,提交 0,未审</param>
    /// <returns></returns>
    public static string AuditState(int type)
    {
        string AuditState = string.Empty;

        if (type == 0)
        {
            AuditState = "未审";
        }
        else if (type == 1)
        {
            AuditState = "提交";
        }
        else if (type == 2)
        {
            AuditState = "已审";
        }
        return AuditState;
    }

    /// <summary>
    /// 邮件发送最终方法
    /// </summary>
    /// <param name="message">中金返回信息</param>
    /// <param name="ReceiptNo">支付流水号</param>
    public static void SendFinsh(string message, string ReceiptNo)
    {
        //string[] emails = ConfigCommon.GetNodeValue("Version.xml", "emails").Split(',');//new string[] { "genggh@haiyusoft.com" }
        //return  Send("smtp.exmail.qq.com", 465, MailPriority.Normal, "servicedesk@moreyou.cn", "陌远科技运营服务", "F606d906", "servicedesk@moreyou.cn", "清算", null, null, emails, "关于后台清算结果的通知-YZT", message, new string[] { }, true, false, Encoding.GetEncoding(936), NullSmtp_SendCompleted);
         Sends(message);
    }

    /// <summary>
    /// 发送邮件
    /// </summary>
    /// <param name="host"></param>
    /// <param name="port"></param>
    /// <param name="mailPriority"></param>
    /// <param name="userid"></param>
    /// <param name="displayName"></param>
    /// <param name="password"></param>
    /// <param name="replyTo"></param>
    /// <param name="replyToDisplayName"></param>
    /// <param name="tos"></param>
    /// <param name="ccs"></param>
    /// <param name="bccs"></param>
    /// <param name="subject"></param>
    /// <param name="body"></param>
    /// <param name="attachments"></param>
    /// <param name="isBodyHtml"></param>
    /// <param name="isAsync"></param>
    /// <param name="encoding"></param>
    /// <param name="smtp_SendCompleted"></param>
    /// <returns></returns>
    //public static string Send(string host, int port, MailPriority mailPriority, string userid, string displayName, string password, string replyTo, string replyToDisplayName, string[] tos, string[] ccs, string[] bccs, string subject, string body, string[] attachments, bool isBodyHtml, bool isAsync, Encoding encoding, SendCompletedEventHandler smtp_SendCompleted)
    //{
    //    try
    //    {
    //        SmtpClient smtp = new SmtpClient();                     //实例化一个SmtpClient
    //        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;       //将smtp的出站方式设为 Network
    //        smtp.EnableSsl =false;                                 //smtp服务器是否启用SSL加密
    //        smtp.Host = host;                                       //指定 smtp 服务器地址
    //        smtp.Port = port;                                       //指定 smtp 服务器的端口，默认是25，如果采用默认端口，可省去
    //        smtp.UseDefaultCredentials = true;
    //        smtp.Credentials = new NetworkCredential(userid, password);
    //        MailMessage mm = new MailMessage();                     //实例化一个邮件类
    //        mm.Priority = mailPriority;
    //        mm.From = new MailAddress(userid, displayName, encoding);
    //        mm.ReplyTo = new MailAddress(replyTo, replyToDisplayName, encoding);
    //        mm.Sender = new MailAddress(userid, "邮件发送者1", encoding);
    //        mm.Subject = subject;                                   //邮件标题
    //        mm.SubjectEncoding = encoding;
    //        mm.IsBodyHtml = isBodyHtml;
    //        mm.BodyEncoding = encoding;
    //        mm.Body = body;
    //        mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnSuccess;
    //        if (null != ccs)                                        //抄送
    //            foreach (string cc in ccs)
    //            {
    //                mm.CC.Add(cc);
    //            }
    //        if (null != bccs)                                        //密送
    //            foreach (string bcc in bccs)
    //            {
    //                mm.Bcc.Add(bcc);
    //            }
    //        if (null != tos)                                        //收件人
    //            foreach (string to in tos)
    //            {
    //                mm.To.Add(to);
    //            }
    //        if (null != attachments)                                //附件
    //        {
    //            foreach (string attachment in attachments)
    //            {
    //                mm.Attachments.Add(new Attachment(attachment, System.Net.Mime.MediaTypeNames.Application.Octet));
    //            }
    //        }
    //        if (isAsync)
    //        {
    //            smtp.SendCompleted += new SendCompletedEventHandler(smtp_SendCompleted);
    //            smtp.SendAsync(mm, tos);
    //        }
    //        else
    //        {
    //            smtp.Send(mm);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.ToString());
    //        return ex.ToString();// false;
    //    }
    //    return "";// true;
    //}

    /// <summary>
    /// 测试邮件发送
    /// </summary>
    public static void Sends(string message)
    {
     
        try
        {
            System.Web.Mail.MailMessage mail = new System.Web.Mail.MailMessage();
            mail.To = "ggh@moreyou.cn";
            mail.From = new MailAddress("servicedesk@moreyou.cn","陌远科技运营服务",Encoding.GetEncoding(936)).ToString();// "servicedesk@moreyou.cn";
            mail.Subject = "关于后台清算结果的通知-YZT";
            mail.BodyFormat = System.Web.Mail.MailFormat.Html;
            mail.Body = message;
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpauthenticate", "1"); //basic authentication
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendusername", "servicedesk@moreyou.cn"); //set your username here
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/sendpassword", "F606d906"); //set your password here
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpserverport", 465);//set port
            mail.Fields.Add("http://schemas.microsoft.com/cdo/configuration/smtpusessl", "true");//set is ssl
            System.Web.Mail.SmtpMail.SmtpServer = "smtp.exmail.qq.com";
            System.Web.Mail.SmtpMail.Send(mail);
            
        }
        catch (Exception ex)
        {
           // return ex.Message;
        }
    }


    /// <summary>
    /// 邮件发送空处理
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public static void NullSmtp_SendCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
    {

    }


}

