using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Script.Serialization;
using LitJson;
using WXAPPservice;

/// <summary>
///MsgSend 的摘要说明
/// </summary>
public class MsgSend
{
	public MsgSend()
	{
		//
		//TODO: 在此处添加构造函数逻辑
		//
	}

    private Thread thread; //微信线程
    private Thread threadAndroid; //安卓线程
    private Thread threadTimer; //计时线程
    private Thread threadMsg; //微信线程

    private string Type = string.Empty;//发送类型
    private string OrderID = string.Empty;//操作订单
    private string UserType = string.Empty;//操作用户，0：企业，1 经销商
    private string MsgID = string.Empty;//操作订单
    private string MsgType = string.Empty;
    private decimal money_all = 0;

    /// <summary>
    /// 同时推送微信和安卓
    /// </summary>
    /// <param name="sendType">发送类型</param>
    /// <param name="orderID">操作订单</param>
    /// <param name="userType">当前用户</param>
    public void GetWxService(string sendType, string orderID, string userType,decimal money = 0)
    {
        try
        {
            //Common.WriteLog(@"D:\u8\sql.txt", sendType + orderID + userType);

            Type = sendType.Trim();
            OrderID = orderID.Trim();
            UserType = userType.Trim();
            money_all = money;
           
            Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(int.Parse(orderID));
            if (order == null)
                return;
            if (userType != "0" && userType != "1")
                return;
            if (sendType != "1" && sendType != "2" && sendType != "3" && sendType != "4"&&sendType !="5" && sendType != "41" &&
                sendType != "42" && sendType != "43" && sendType != "44" && sendType != "45" && sendType != "46") return;

            //Hi.Model.BD_CompAccessControl compControl = Common.GetCompControl(order.CompID);

            //微信推送线程
            //this.thread = new Thread(new ThreadStart(WechatThread));

            //if (sendType == "1" || sendType == "2" || sendType == "3" || sendType == "4")
            //{
            //    if (compControl == null)
            //        this.threadAndroid = new Thread(new ThreadStart(AndroidThread));
            //    else if (compControl.IsRecApp == 1)
            //        this.threadAndroid = new Thread(new ThreadStart(AndroidThread));
            //}
            //else
            //{
            //    this.threadAndroid = new Thread(new ThreadStart(AndroidThread));
            //}


            AndroidThread();

            //this.threadAndroid = new Thread(new ThreadStart(AndroidThread));
            //this.threadTimer = new Thread(new ThreadStart(ThreadTimeOut));

            //thread.Start();
            //threadAndroid.Start();
            //threadTimer.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
    }

    /// <summary>
    /// 推送消息
    /// </summary>
    /// <param name="msgID">操作单</param>
    public void GetMsgService(string msgID,string type)
    {
        try
        {
            MsgID = msgID.Trim();
            MsgType = type.Trim();
            Hi.Model.BD_CompNews order = new Hi.BLL.BD_CompNews().GetModel(int.Parse(MsgID));
            if (order == null)
                return;

            MsgThread();

            //this.threadMsg = new Thread(new ThreadStart(MsgThread));
            //this.threadTimer = new Thread(new ThreadStart(ThreadTimeOut));

            //threadMsg.Start();
            //threadTimer.Start();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

    }

    /// <summary>
    /// 计时线程
    /// </summary>
    internal void ThreadTimeOut()
    {
        Thread.Sleep(3000);
        
        //Thread.CurrentThread.Abort();
        threadTimer.Abort();
        threadAndroid.Abort();
        thread.Abort();
        threadMsg.Abort();
    }

    #region 微信推送

    /// <summary>
    /// 推送线程
    /// </summary>
    internal void WechatThread()
    {
        string r = string.Empty;
        string JX = string.Empty;
        StreamReader StReder = null;
        string Json = string.Empty;

        try
        {
            if (Type != "" && OrderID != "")
            {
                Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(int.Parse(OrderID));
                List<Hi.Model.SYS_Users> userList = null;
                if (order != null)
                {
                    if (UserType.Trim() == "0")//推送给经销商
                        userList = new Hi.BLL.SYS_Users().GetList("",
                            " DisID='" + order.DisID + "' and dr=0 and IsEnabled=1 and CompID='" + order.CompID +
                            "' and (type=1 or type =5)", "");
                    else if (UserType.Trim() == "1")//推送给企业
                        userList = new Hi.BLL.SYS_Users().GetList("",
                            " compID='" + order.CompID + "' and dr=0 and IsEnabled=1 and type between 3 and 4", "");
                    else
                        r = "";
                    if (userList != null && userList.Count > 0)
                    {
                        foreach (Hi.Model.SYS_Users user in userList)
                        {
                            if (user.OpenID.ToString() != "")
                            {
                                JX = GetWXType(Type, user.ID.ToString(), OrderID);
                                if (!string.IsNullOrEmpty(JX))
                                {
                                    #region zxz  历史   PHP微信推送 
                                    //string strGet = "http://wx.shangyijiu.com/wx/WxService/notice&data=" + JX + "";
                                    //StReder = VisitWebService(strGet, Encoding.UTF8);
                                    //Json += StReder.ReadToEnd();
                                    #endregion

                                    #region  ggh  现在 .net微信推送
                                    WxServiceController wx = new WxServiceController();
                                    wx.NoticeAction(JX);


                                    //测试微信推送接口
                                    //TestWx.WxServiceController testwx = new TestWx.WxServiceController();
                                    //testwx.NoticeAction(JX);

                                    #endregion
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (StReder != null) StReder.Close();
        }

        if (r != "")
        {
            thread.Abort();
        }
    }

    /// <summary>
    /// 访问WebService方法
    /// </summary>
    /// <param name="Url">访问的Url</param>
    /// <param name="Coding">规定返回的编码格式（可不填）</param>
    /// <returns></returns>
    public static StreamReader VisitWebService(string Url, Encoding Coding = null)
    {
        StreamReader StReder = null;
        try
        {
            if (string.IsNullOrEmpty(Url.Trim()))
            {
                throw new Exception("URL地址不能为空");
            }
            HttpWebRequest Requst = (HttpWebRequest)HttpWebRequest.Create(Url.Trim());
            HttpWebResponse Respons = (HttpWebResponse)Requst.GetResponse();
            if (Coding != null)
                StReder = new StreamReader(Respons.GetResponseStream(), Coding);
            else
                StReder = new StreamReader(Respons.GetResponseStream());
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return StReder;
    }

    /// <summary>
    /// 获取推送类型
    /// </summary>
    /// <returns></returns>
    public static string GetWXType(string sendType, string userID, string orderID)
    {
        string res = string.Empty;
        decimal num = 0;
        string orderInfo = string.Empty;
        string orderOut = String.Empty;
        Hi.Model.DIS_OrderOut outModel = null;
        Hi.Model.DIS_OrderReturn returnModel = null;
        List<Hi.Model.DIS_Logistics> list_log= null;
        Hi.Model.DIS_Logistics log = null;
        Hi.BLL.DIS_Logistics bll_log = new Hi.BLL.DIS_Logistics();

        Hi.Model.SYS_Users user = new Hi.BLL.SYS_Users().GetModel(int.Parse(userID));
        if (user == null || string.IsNullOrEmpty(user.OpenID))
            return "";
        Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(int.Parse(orderID));
        if (order == null)
            return "";
        List<Hi.Model.DIS_OrderDetail> orderList = new Hi.BLL.DIS_OrderDetail().GetList("",
            " orderID=" + int.Parse(orderID), "");
        if (orderList == null || orderList.Count == 0)
            return "";
        foreach (Hi.Model.DIS_OrderDetail detail in orderList)
        {
            Hi.Model.BD_GoodsInfo goodsInfo = new Hi.BLL.BD_GoodsInfo().GetModel(detail.GoodsinfoID);
            if (goodsInfo != null)
            {
                Hi.Model.BD_Goods goods = new Hi.BLL.BD_Goods().GetModel(goodsInfo.GoodsID);
                orderInfo += " " + goods.GoodsName + "*" + detail.GoodsNum;
            }
            num += detail.GoodsNum;
        }
        Hi.Model.BD_DisAddr addr = new Hi.BLL.BD_DisAddr().GetModel(order.AddrID);
        if (addr != null)
        {
            if (addr.Phone != "")
                orderOut += addr.Principal + ":" + addr.Phone + " ";
            else
                orderOut += addr.Principal + ":" + addr.Tel + " ";
        }
        List<Hi.Model.DIS_OrderOut> orderout = new Hi.BLL.DIS_OrderOut().GetList("",
            " orderID=" + int.Parse(orderID), "");
        if (orderout != null && orderout.Count != 0)
        {
            foreach (Hi.Model.DIS_OrderOut detail in orderout)
            {
                list_log = bll_log.GetList("","OrderOutID = "+detail.ID+" and isnull(dr,0) == 0","");
                if (list_log != null && list_log.Count >= 0)
                {
                    log = list_log[0];
                    //orderOut += log.ExpressPerson + ":" + log.ExpressTel + " ";
                }
               
                outModel = detail;
            }
        }
        List<Hi.Model.DIS_OrderReturn> returnlist = new Hi.BLL.DIS_OrderReturn().GetList("",
            " orderID=" + int.Parse(orderID), "");
        if (returnlist != null && returnlist.Count != 0)
        {
            foreach (Hi.Model.DIS_OrderReturn detail in returnlist)
            {
                returnModel = detail;
            }
        }
        //string sqlstr = string.Format("select * from dis_order where receiptno like ('%{0}%') and dr=0", ReceiptNo);

        switch (sendType)
        {
            case "1": //下单通知
                StringBuilder strSql = new StringBuilder();
                strSql.Append("{\"type\":\"ORDERADD\",\"openid\":\"");
                strSql.Append(user.OpenID);
                strSql.Append("\",\"msg\":[{\"title\":\"尊敬的");
                strSql.Append(user.TrueName);
                strSql.Append("，您好\",\"orderno\":\"");
                strSql.Append(order.ReceiptNo);
                strSql.Append("\",\"orderamount\":\"");
                strSql.Append(order.TotalAmount.ToString("N"));
                strSql.Append("元\",\"extra1\":\"状态:待付款\",\"extra2\":\"数量：");
                strSql.Append(num.ToString("0.00"));
                strSql.Append("\",\"remark\":\"");
                strSql.Append(order.Remark);
                strSql.Append("\"}]}");
                res = strSql.ToString();
                break;
            case "2": //订单支付通知
                StringBuilder str = new StringBuilder();
                str.Append("{\"type\":\"ORDERPAY\",\"openid\":\"");
                str.Append(user.OpenID);
                str.Append("\",\"msg\":[{\"title\":\"尊敬的");
                str.Append(user.TrueName);
                str.Append("，您好\",\"orderpay\":\"");
                str.Append(order.TotalAmount.ToString("N"));
                str.Append("元\",\"orderinfo\":\"");
                str.Append(orderInfo);
                str.Append("\",\"receiveinfo\":\"");
                str.Append(orderOut);
                str.Append("\",\"orderno\":\"");
                str.Append(order.ReceiptNo);
                str.Append("\",\"remark\":\"");
                str.Append("\"}]}");
                res = str.ToString();
                break;
            case "3": //42、签收提醒
                StringBuilder SIGN = new StringBuilder();
                SIGN.Append("{\"type\":\"SIGN\",\"openid\":\"");
                SIGN.Append(user.OpenID);
                SIGN.Append("\",\"msg\":[{\"title\":\"您好，您的订单");
                SIGN.Append(order.ReceiptNo);
                SIGN.Append("已签收\",\"orderno\":\"");
                SIGN.Append(order.ReceiptNo);
                SIGN.Append("\",\"qianshouren\":\"");
                SIGN.Append(outModel.SignUser);
                SIGN.Append("\",\"qianshoushijian\":\"");
                SIGN.Append(outModel.SignDate);
                SIGN.Append("\",\"remark\":\"");
                SIGN.Append(order.Remark);
                SIGN.Append("\"}]}");
                res = SIGN.ToString();
                break;
            case "4": //退货申请
                StringBuilder RETGOODS = new StringBuilder();
                RETGOODS.Append("{\"type\":\"RETGOODS\",\"openid\":\"");
                RETGOODS.Append(user.OpenID);
                RETGOODS.Append("\",\"msg\":[{\"title\":\"买家申请退货");
                RETGOODS.Append("\",\"orderno\":\"");
                RETGOODS.Append(order.ReceiptNo);
                RETGOODS.Append("\",\"shangpinxinxi\":\"");
                RETGOODS.Append(orderInfo);
                RETGOODS.Append("\",\"orderjine\":\"");
                RETGOODS.Append(order.TotalAmount.ToString("N"));
                RETGOODS.Append("元\",\"remark\":\"");
                RETGOODS.Append(order.Remark);
                RETGOODS.Append("\"}]}");
                res = RETGOODS.ToString();
                break;
            case "41": //代人下单通知
                StringBuilder ORDERADD = new StringBuilder();
                ORDERADD.Append("{\"type\":\"ORDERADD\",\"openid\":\"");
                ORDERADD.Append(user.OpenID);
                ORDERADD.Append("\",\"msg\":[{\"title\":\"尊敬的");
                ORDERADD.Append(user.TrueName);
                ORDERADD.Append("，您好\",\"orderno\":\"");
                ORDERADD.Append(order.ReceiptNo);
                ORDERADD.Append("\",\"orderamount\":\"");
                ORDERADD.Append(order.TotalAmount.ToString("N"));
                ORDERADD.Append("元\",\"extra1\":\"状态:待付款\",\"extra2\":\"数量：");
                ORDERADD.Append(num.ToString());
                ORDERADD.Append("\",\"remark\":\"");
                ORDERADD.Append(order.Remark);
                ORDERADD.Append("\"}]}");
                res = ORDERADD.ToString();
                break;
            case "42": //订单审批
                StringBuilder sql = new StringBuilder();
                sql.Append("{\"type\":\"ORDERSTATUS\",\"openid\":\"");
                sql.Append(user.OpenID);
                sql.Append("\",\"msg\":[{\"title\":\"尊敬的");
                sql.Append(user.TrueName);
                sql.Append("，您好\",\"orderno\":\"");
                sql.Append(order.ReceiptNo);
                if (order.OState.ToString() == "-1")
                {
                    sql.Append("\",\"orderstatus\":\"订单退回\",\"remark\":\"");
                }
                else
                {
                    sql.Append("\",\"orderstatus\":\"待发货、审批通过\",\"remark\":\"");
                }
                sql.Append(order.Remark);
                sql.Append("\"}]}");
                res = sql.ToString();
                break;
            case "43": //订单发货
                StringBuilder sqlSend = new StringBuilder();
                sqlSend.Append("{\"type\":\"ORDERSEND\",\"openid\":\"");
                sqlSend.Append(user.OpenID);
                sqlSend.Append("\",\"msg\":[{\"title\":\"尊敬的");
                sqlSend.Append(user.TrueName);
                sqlSend.Append("，您好\",\"orderno\":\"");
                sqlSend.Append(order.ReceiptNo);
                sqlSend.Append("\",\"wuliuname\":\"");
                sqlSend.Append(log.ComPName);
                sqlSend.Append("\",\"wuliucode\":\"");
                sqlSend.Append(log.LogisticsNo);
                sqlSend.Append("\",\"remark\":\"");
                sqlSend.Append(outModel.Remark);
                sqlSend.Append("\"}]}");
                res = sqlSend.ToString();
                break;
            case "44": //6、退货审核
                StringBuilder RETAUDIT = new StringBuilder();
                RETAUDIT.Append("{\"type\":\"RETAUDIT\",\"openid\":\"");
                RETAUDIT.Append(user.OpenID);
                RETAUDIT.Append("\",\"msg\":[{\"title\":\"您好，您的退货审核");
                if (returnModel.ReturnState == -1)
                {
                    RETAUDIT.Append("已退回");
                }
                else
                {
                    RETAUDIT.Append("已审核");
                }
                RETAUDIT.Append("\",\"shenhejieguo\":\"");
                if (returnModel.ReturnState == -1)
                {
                    RETAUDIT.Append("已退回");
                }
                else
                {
                    RETAUDIT.Append("已审核");
                }
                RETAUDIT.Append("\",\"shangpinxinxi\":\"");
                RETAUDIT.Append(orderInfo);
                RETAUDIT.Append("\",\"tuihuojine\":\"");
                RETAUDIT.Append(order.PayedAmount.ToString("N"));
                RETAUDIT.Append("元\",\"shenheshuoming\":\"");
                RETAUDIT.Append(returnModel.AuditRemark);
                RETAUDIT.Append("\",\"shenheshijian\":\"");
                RETAUDIT.Append(returnModel.AuditDate);
                RETAUDIT.Append("\",\"orderno\":\"");
                RETAUDIT.Append(order.ReceiptNo);
                RETAUDIT.Append("\",\"remark\":\"");
                RETAUDIT.Append("\"}]}");
                res = RETAUDIT.ToString();
                break;
            case "45": //5、退款
                StringBuilder REFUND = new StringBuilder();
                REFUND.Append("{\"type\":\"REFUND\",\"openid\":\"");
                REFUND.Append(user.OpenID);
                REFUND.Append("\",\"msg\":[{\"title\":\"您好，您的订单");
                REFUND.Append(order.ReceiptNo);
                REFUND.Append("，已退款。\",\"reason\":\"");
                REFUND.Append(returnModel.ReturnContent);
                REFUND.Append("\",\"refund\":\"");
                REFUND.Append(order.PayedAmount.ToString("N"));
                REFUND.Append("元\",\"orderno\":\"");
                REFUND.Append(order.ReceiptNo);
                REFUND.Append("\",\"remark\":\"");
                REFUND.Append("\"}]}");
                res = REFUND.ToString();
                break;
        }
        return res;
    }

    #endregion

    #region 安卓推送

    /// <summary>
    /// 安卓推送线程
    /// </summary>
    internal void AndroidThread()
    {
        string Key = string.Empty;
        string MasterSecret = string.Empty;
        if (UserType == "0")//推送给经销商
        {
            Key = ConfigurationManager.AppSettings["DisKey"].ToString();
            MasterSecret = ConfigurationManager.AppSettings["DisMasterSecret"].ToString();
        }
        else//推送给企业
        {
            Key = ConfigurationManager.AppSettings["CompKey"].ToString();
            MasterSecret = ConfigurationManager.AppSettings["CompMasterSecret"].ToString();
        }
        JPushClient push = new JPushClient(Key, MasterSecret);
        PushPayload payload = new MsgSend().GetSendUserModel(UserType, OrderID, Type,money_all);
        if (payload != null)
        {
            MessageResult msgResult = push.SendPush(payload);
            //if (msgResult!=null)
            //threadAndroid.Abort();
        }
    }

    internal void MsgThread()
    {
        string Key = string.Empty;
        string MasterSecret = string.Empty;
        
        Key = ConfigurationManager.AppSettings["DisKey"].ToString();
        MasterSecret = ConfigurationManager.AppSettings["DisMasterSecret"].ToString();
        
        JPushClient push = new JPushClient(Key, MasterSecret);
        PushPayload payload = new PushPayload();
        string type = MsgType == "" || MsgType == "-1" ? "-1" : MsgType;
        payload =  new MsgSend().GetMsgUserModel(MsgID,type);
        if (payload != null)
        {
            MessageResult msgResult = push.SendPush(payload);
            //if (msgResult != null)
            //    threadMsg.Abort();
        }
    }

    /// <summary>
    /// 获取推送用户，返回推送类
    /// </summary>
    /// <param name="userType">0 （当前用户类型是企业，即推送至经销商） 1 （当前用户类型是经销商，即推送至企业） 2 全部 </param>
    /// <param name="sendType">类型同微信推送</param>
    /// <returns></returns>
    public PushPayload GetMsgUserModel(string orderID,string type)
    {
        Hi.Model.BD_CompNews order = new Hi.BLL.BD_CompNews().GetModel(int.Parse(orderID));
        if (order == null)
            return null;

        //推送设置
        PushPayload payload = new PushPayload();

        payload.platform = Platform.android_ios();//推送平台

        //推送用户，0：推送给经销商 1：推送给企业
        //payload.audience = Audience.all();
        List<Hi.Model.SYS_CompUser> userList = new Hi.BLL.SYS_CompUser().GetList("",
                    " compID='" + order.CompID + "' and IsAudit = 2 and disID!=0 and dr=0 and IsEnabled=1 and (Utype=1 or Utype =5)", "");
        if (userList == null || userList.Count == 0)
            return null;
        string[] arr = new String[userList.Count];
        for (int i = 0; i < userList.Count; i++)
        {
            //arr[i] = AESHelper.Encrypt_android(userList[i].ID.ToString());
            //arr[i] = AESHelper.Encrypt_android_old(userList[i].ID.ToString());
            arr[i] = AESHelper.Encrypt_MD5(userList[i].ID.ToString());//MD5加密
        }
        payload.audience = Audience.s_alias(arr);
        //payload.audience = Audience.s_alias(AESHelper.Encrypt_android("1304"));

        //推送通知
        string str = "{\"Type\":\"" + (type == "" || type == "-1" ? "-1" : order.NewsType.ToString()) + 
            "\",\"Title\":\"" + order.NewsTitle +
            "\",\"ID\":\"" + order.ID.ToString() + "\",\"Date\":\"" + DateTime.Now.ToString("yyyy-MM-dd") + "\"}";

        // SendType 0：推送给经销商 1：推送给企业
        string sendStr = "{\"SendType\":\"0\",\"CompID\":\"" + order.CompID + "\"}";
        
        //AndroidNotification android = new AndroidNotification();
        //android.setAlert(GetAlertContent(order, sendType));//推送内容
        //payload.notification = new Notification().setAndroid(android);//推送内容
        payload.notification = new Notification().setAlert("您有一条新的信息公告！")
                               .setAndroid(new AndroidNotification()//设置Android的notification
                                             .AddExtra("Type", "2")
                                             .AddExtra("CheckUser", AESHelper.Encrypt_android(sendStr))
                                             .AddExtra("Json", AESHelper.Encrypt_android(str)))
                                             .setIos(new IosNotification()//设置IOS的notification
                                             .setAlert("您有一条新的信息公告")
                                             .autoBadge()
                                             .AddExtra("Type", "2")
                                             .AddExtra("CheckUser", AESHelper.Encrypt_android(sendStr))
                                             .AddExtra("Json", AESHelper.Encrypt_android(str)));
        
        //推送消息
        payload.message = Message.content("您有一条新的短消息！")
                              .AddExtras("Type", "2")
                              .AddExtras("CheckUser", AESHelper.Encrypt_android(sendStr))
                              .AddExtras("Json", AESHelper.Encrypt_android(str));
        Options op = new Options();
        op.apns_production = false;
        payload.options = op;

        return payload;
    }

    /// <summary>
    /// 获取推送用户，返回推送类
    /// </summary>
    /// <param name="userType">0 （当前用户类型是企业，即推送至经销商） 1 （当前用户类型是经销商，即推送至企业） 2 全部 </param>
    /// <param name="sendType">类型同微信推送</param>
    /// <returns></returns>
    public PushPayload GetSendUserModel(string userType,string orderID,string sendType,decimal money)
    {
        Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(int.Parse(orderID));
        if (order == null)
            return null;

        //推送设置
        PushPayload payload = new PushPayload();

        payload.platform = Platform.android_ios();//推送平台
        string str = "{\"OrderType\":\"" + sendType + "\",\"ReceiptNo\":\"" + order.ReceiptNo + "\",\"Date\":\"" + DateTime.Now.ToString("yyyy年MM月dd日") + "\",\"Money\":\"" + money.ToString("F2") + "\",\"AllDate\":\"" + DateTime.Now.ToString() + "\"}";
        // SendType 0：推送给经销商 1：推送给企业
        string sendStr = "{\"SendType\":\"" + userType + "\",\"DisID\":\"" + order.DisID + "\",\"CompID\":\"" + order.CompID + "\"}";

        //推送通知
        //AndroidNotification android = new AndroidNotification();
        //android.setAlert(GetAlertContent(order, sendType));//推送内容
        //payload.notification = new Notification().setAndroid(android);//推送内容
        payload.notification = new Notification().setAlert(GetAlertContent(order, sendType))
                               .setAndroid(new AndroidNotification()
                                             .AddExtra("Type", "1")
                                             .AddExtra("CheckUser", AESHelper.Encrypt_android(sendStr))
                                             .AddExtra("Json", AESHelper.Encrypt_android(str)))
                                             .setIos(new IosNotification()//设置IOS的notification
                                             .setAlert(GetAlertContent(order, sendType))
                                             .autoBadge()
                                             .AddExtra("Type", "1")
                                             .AddExtra("CheckUser", AESHelper.Encrypt_android(sendStr))
                                             .AddExtra("Json", AESHelper.Encrypt_android(str)));
        //推送消息
        payload.message = Message.content(GetAlertContent(order, sendType))
                              .AddExtras("Type", "1")
                              .AddExtras("CheckUser", AESHelper.Encrypt_android(sendStr))
                              .AddExtras("Json", AESHelper.Encrypt_android(str));
        //推送用户，0：推送给经销商 1：推送给企业
        //payload.audience = Audience.all();
        payload.audience = Audience.s_alias(GetUserAlias(order, userType));
        Options op = new Options();
        op.apns_production = false;
        payload.options = op;


        return payload;
    }

    /// <summary>
    /// 获取推送用户的别名
    /// </summary>
    /// <param name="order"></param>
    /// <param name="userType">0：推送给经销商 1：推送给企业</param>
    /// <returns></returns>
    public string[] GetUserAlias(Hi.Model.DIS_Order order, string userType)
    {
        List<Hi.Model.SYS_CompUser> userList = new List<Hi.Model.SYS_CompUser>();
        switch (userType.Trim())
        {
            case "0":
                userList = new Hi.BLL.SYS_CompUser().GetList("",
                    " compID='" + order.CompID + "' and IsAudit = 2 and disID='" + order.DisID + "' and dr=0 and IsEnabled=1 and (Utype=1 or Utype =5)", "");
                break;
            case "1":
                userList = new Hi.BLL.SYS_CompUser().GetList("",
                    " compID='" + order.CompID + "' and IsAudit = 2 and dr=0 and IsEnabled=1 and Utype between 3 and 4", "");
                break;
            case "2":
                userList = new Hi.BLL.SYS_CompUser().GetList("", " dr=0 and IsAudit = 2 and IsEnabled=1 and Utype in (1,3,4,5)", "");
                break;
            default:
                return null;
        }

        if (userList == null || userList.Count == 0)
            return null;
        
        string[] arr = new String[userList.Count];
        for (int i = 0; i < userList.Count; i++)
        {
            arr[i] = AESHelper.Encrypt_MD5(userList[i].ID.ToString());
        }
        return arr;
    }

    /// <summary>
    /// 获取推送内容
    /// </summary>
    /// <param name="user"></param>
    /// <param name="order"></param>
    /// <param name="sendType"></param>
    /// <returns></returns>
    public string GetAlertContent(Hi.Model.DIS_Order order, string sendType)
    {
        string res = string.Empty;
        switch (sendType)
        {
            case "1": //下单通知
                StringBuilder strSql = new StringBuilder();
                strSql.Append("您有一条新的订单消息。");
                
               
                //strSql.Append("您好，您有一条新的订单（");
                //strSql.Append(order.ReceiptNo);
                //strSql.Append("）。");
                res = strSql.ToString();
                break;
            case "2": //订单支付通知
                StringBuilder str = new StringBuilder();
                if (order.Otype == 9)
                {
                    str.Append("您收到一笔账单付款。");
                }
                else
                {
                    str.Append("您收到一笔订单付款。");
                }
                
                //str.Append("您好，您有一条订单（");
                //str.Append(order.ReceiptNo);
                //str.Append("）已支付。");
                res = str.ToString();
                break;
            case "3": //签收提醒
                StringBuilder SIGN = new StringBuilder();
                //SIGN.Append("您好，您有一条订单（");
                //SIGN.Append(order.ReceiptNo);
                //SIGN.Append("）已签收。");
                SIGN.Append("您有一张订单已签收。");
                res = SIGN.ToString();
                break;
            case "4": //退货申请
                StringBuilder RETGOODS = new StringBuilder();
                //RETGOODS.Append("您好，您有一条订单（");
                //RETGOODS.Append(order.ReceiptNo);
                //RETGOODS.Append("）申请退货。");
                RETGOODS.Append("您有一张订单申请退货。");
                res = RETGOODS.ToString();
                break;
            case "5"://经销商修改订单
                StringBuilder UPDATEORDER =  new StringBuilder();
                UPDATEORDER.Append("您有一张订单已更新。");
                res = UPDATEORDER.ToString();
                break;
            case "41": //代人下单通知
                StringBuilder ORDERADD = new StringBuilder();
                //ORDERADD.Append("您好，您有一条新的订单（");
                //ORDERADD.Append(order.ReceiptNo);
                //ORDERADD.Append("）。");
                ORDERADD.Append("核心企业代您生成一条新的订单。");
                res = ORDERADD.ToString();
                break;
            case "42": //订单审批
                StringBuilder sql = new StringBuilder();
                //sql.Append("您好，您有一条订单（");
                //sql.Append(order.ReceiptNo);
                //sql.Append("）已审批。");
                sql.Append("您有一张订单已审核。");
                res = sql.ToString();
                break;
            case "43": //订单发货
                StringBuilder sqlSend = new StringBuilder();
                //sqlSend.Append("您好，您有一条订单（");
                //sqlSend.Append(order.ReceiptNo);
                //sqlSend.Append("）已发货。");
                sqlSend.Append("您有一张订单已发货。");
                res = sqlSend.ToString();
                break;
            case "44": //退货审核
                StringBuilder RETAUDIT = new StringBuilder();
                //RETAUDIT.Append("您好，您有一条退货订单（");
                //RETAUDIT.Append(order.ReceiptNo);
                //RETAUDIT.Append("）已审核。");
                RETAUDIT.Append("您的退货申请已审核");
                res = RETAUDIT.ToString();
                break;
            case "45": //退款
                StringBuilder REFUND = new StringBuilder();
                //REFUND.Append("您好，您有一条订单（"); 
                StringBuilder COMUPDATEORDER = new StringBuilder();
                COMUPDATEORDER.Append("您有一张订单已更新。");
                res = COMUPDATEORDER.ToString();
                break;
            case "46"://经销商修改订单
                StringBuilder UPDATEORDER_comp = new StringBuilder();
                UPDATEORDER_comp.Append("您有一张订单已更新。");
                res = UPDATEORDER_comp.ToString();
                break;
        }
        return res;
    }
    /// <summary>
    /// 同时推送微信和安卓
    /// </summary>
    /// <param name="sendType">发送类型</param>
    /// <param name="orderID">操作订单</param>
    /// <param name="userType">当前用户</param>
    //public void GetWxService(string sendType, string orderID, string userType, decimal money)
    //{
    //    try
    //    {
    //        //Common.WriteLog(@"D:\u8\sql.txt", sendType+orderID+userType);

    //        Type = sendType.Trim();
    //        OrderID = orderID.Trim();
    //        UserType = userType.Trim();
    //        money_all = money;

    //        Hi.Model.DIS_Order order = new Hi.BLL.DIS_Order().GetModel(int.Parse(orderID));
    //        if (order == null)
    //            return;
    //        if (userType != "0" && userType != "1")
    //            return;
    //        if (sendType != "1" && sendType != "2" && sendType != "3" && sendType != "4" && sendType != "5" && sendType != "41" &&
    //            sendType != "42" && sendType != "43" && sendType != "44" && sendType != "45" && sendType != "46") return;

    //        //Hi.Model.BD_CompAccessControl compControl = Common.GetCompControl(order.CompID);

    //        //微信推送线程
    //        //this.thread = new Thread(new ThreadStart(WechatThread));

    //        //if (sendType == "1" || sendType == "2" || sendType == "3" || sendType == "4")
    //        //{
    //        //    if (compControl == null)
    //        //        this.threadAndroid = new Thread(new ThreadStart(AndroidThread));
    //        //    else if (compControl.IsRecApp == 1)
    //        //        this.threadAndroid = new Thread(new ThreadStart(AndroidThread));
    //        //}
    //        //else
    //        //{
    //        //    this.threadAndroid = new Thread(new ThreadStart(AndroidThread));
    //        //}


    //        AndroidThread();

    //        //this.threadAndroid = new Thread(new ThreadStart(AndroidThread));
    //        //this.threadTimer = new Thread(new ThreadStart(ThreadTimeOut));

    //        //thread.Start();
    //        //threadAndroid.Start();
    //        //threadTimer.Start();
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine(ex.Message);
    //    }

    //}

    #endregion

    public delegate void Jpushdega(string sendType, string orderID, string userType, decimal money = 0);
}