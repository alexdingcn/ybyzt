using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LitJson;
using System.Data;
using System.Text;
using System.Web.Configuration;

/// <summary>
///Pay 的摘要说明
/// </summary>
public class Pay_Update
{
	public Pay_Update()
	{

	}
    #region//微信支付
    public ResultWX Result_WX(string JSon,string version)
    {
        string disID = string.Empty;
        string UserID = string.Empty;
        Hi.BLL.BD_Distributor bll_dis = new Hi.BLL.BD_Distributor();
        Hi.BLL.BD_Company bll_comp = new Hi.BLL.BD_Company();
        ResultWX result = new ResultWX();
        string appid = string.Empty;
        string mchid = string.Empty;
        string key = string.Empty;
       
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString().Trim() != "" && JInfo["ResellerID"].ToString().Trim() != "")
            {
                disID = JInfo["ResellerID"].ToString();
                UserID = JInfo["UserID"].ToString();

            }
            else
            {
                return new ResultWX() { Result = "F",Description = "参数异常"};
            }

            #endregion
            //判断登录信息是否正确
            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out user, 0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultWX() { Result = "F", Description = "用户异常" };
            //判断经销商信息是否异常
            Hi.Model.BD_Distributor dis = bll_dis.GetModel(Convert.ToInt32(disID));
            if (dis == null || dis.dr == 1 || dis.AuditState == 0 || dis.IsEnabled == 0)
                return new ResultWX() { Result = "F",Description = "经销商异常"};
            //判断经销商对应的核心企业是否异常
            Hi.Model.BD_Company comp = bll_comp.GetModel(dis.CompID);
            if (comp == null || comp.dr == 1 || comp.AuditState == 0 || comp.IsEnabled == 0)
                return new ResultWX() { Result = "F",Description = "核心企业异常"};
            //获取Pay_PayWxandAli表的实体
            Hi.Model.Pay_PayWxandAli paywx = Common.GetPayWxandAli(comp.ID);
            if (paywx == null || ClsSystem.gnvl(paywx.wx_Isno, "0") == "0")
                return new ResultWX() { Result = "F",Description = "核心企业无可用的微信收款账户"};
            #region //返回参数
            result.Result = "T";
            result.Description = "返回成功";
            //对数据库中取出的值进行解密
            appid = Common.DesDecrypt(paywx.wx_appid,Common.EncryptKey);
            mchid = Common.DesDecrypt(paywx.wx_mchid,Common.EncryptKey);
            key = Common.DesDecrypt(paywx.wx_key,Common.EncryptKey);
            //对解密过的字段，用我们这边的方法重新加密
            appid = AESHelper.Encrpt_string(appid);
            mchid = AESHelper.Encrpt_string(mchid);
            key = AESHelper.Encrpt_string(key);
            //将加密完的值赋给返回实体
            result.AppID = appid;
            result.Mchid = mchid;
            //result.AppSecret = paywx.wx_appsechet;
            result.APPkey = key;

            #endregion
            //return result;
            }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "Result_WX:" + JSon);
            return new ResultWX() { Result = "F", Description = "参数异常" };
        }
        return result;
    }
    #endregion

    #region//支付宝支付
    public ResultAli Result_Ali(string JSon,string version)
    {
        string disID = string.Empty;
        string UserID = string.Empty;
        Hi.BLL.BD_Distributor bll_dis = new Hi.BLL.BD_Distributor();
        Hi.BLL.BD_Company bll_comp = new Hi.BLL.BD_Company();
        ResultAli result = new ResultAli();
        string partner = string.Empty;
        string seller = string.Empty;
        string private_key = string.Empty;
        try
        {
            #region//JSon取值
            JsonData JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString().Trim() != "" && JInfo["ResellerID"].ToString().Trim() != "")
            {
                disID = JInfo["ResellerID"].ToString();
                UserID = JInfo["UserID"].ToString();

            }
            else
            {
                return new ResultAli() { Result = "F", Description = "参数异常" };
            }

            #endregion
            //判断登录信息是否正确
            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out user, 0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultAli() { Result = "F", Description = "用户异常" };
            //判断经销商信息是否异常
            Hi.Model.BD_Distributor dis = bll_dis.GetModel(Convert.ToInt32(disID));
            if (dis == null || dis.dr == 1 || dis.AuditState == 0 || dis.IsEnabled == 0)
                return new ResultAli() { Result = "F", Description = "经销商异常" };
            //判断经销商对应的核心企业是否异常
            Hi.Model.BD_Company comp = bll_comp.GetModel(dis.CompID);
            if (comp == null || comp.dr == 1 || comp.AuditState == 0 || comp.IsEnabled == 0)
                return new ResultAli() { Result = "F", Description = "核心企业异常" };
            //获取Pay_PayWxandAli表的实体
            Hi.Model.Pay_PayWxandAli payali = Common.GetPayWxandAli(comp.ID);
            if (payali == null || ClsSystem.gnvl(payali.ali_isno, "0") == "0")
                return new ResultAli() { Result = "F", Description = "核心企业无可用的支付宝收款账户" };
           #region //返回参数

            result.Result = "T";
            result.Description = "返回成功";
            //对数据库中数据进行解密
            partner = Common.DesDecrypt(payali.ali_partner,Common.EncryptKey);
            seller = Common.DesDecrypt(payali.ali_seller_email,Common.EncryptKey);
            private_key = Common.DesDecrypt(payali.ali_key,Common.EncryptKey);
            //对解密过的数据，再用我们的加密方法进行加密
            partner = AESHelper.Encrpt_string(partner);
            seller = AESHelper.Encrpt_string(seller);
            private_key = AESHelper.Encrpt_string(private_key);
            //将我们这边加密完的数据，赋值给返回实体
            result.PARTNER = partner;
            result.SELLER = seller;
            result.RSA_PRIVATE = private_key;
           #endregion

        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "Result_Ali:" + JSon);
            return new ResultAli() { Result = "F", Description = "参数异常" };
        }
        return result;
    }



    #endregion


    /// <summary>
    /// 查看是否启用微信支付或者支付宝支付
    /// </summary>
    public ResultPayInfo GetPayInfo(string JSon, string version)
    {
        string disID = string.Empty;
        string UserID = string.Empty;
        string OrderType = string.Empty;
        string ReceiptNo = string.Empty;
        string PayPrice = string.Empty;
        string PayIDJSon = string.Empty;
        string partner = string.Empty;
        string seller = string.Empty;
        string private_key = string.Empty;
        string subject = string.Empty;//提示内容
        string GoodsName = string.Empty;//订单所有商品的名称
        string OrderNumberJSon = string.Empty;//订单支付流水号的json
        string OrderNumber = string.Empty;//订单支付流水号
        string orderInfo = string.Empty;//订单信息
        string OrderCode = string.Empty;//企业订单号
        DataTable dt_order = null;
        

        Hi.BLL.BD_Distributor bll_dis = new Hi.BLL.BD_Distributor();
        Hi.BLL.BD_Company bll_comp = new Hi.BLL.BD_Company();
        Hi.BLL.DIS_Order bll_order = new Hi.BLL.DIS_Order();
        ResultPayInfo payinfo = new ResultPayInfo();
        Common comm = new Common();
        try
        {

            #region//JSon取值
            JsonData JInfo = null;
            JInfo = JsonMapper.ToObject(JSon);
            if (JInfo.Count > 0 && JInfo["UserID"].ToString().Trim() != "" && JInfo["ResellerID"].ToString().Trim() != "" && JInfo["OrderType"].ToString().Trim() != "" &&
                JInfo["PayPrice"].ToString().Trim() != "" && JInfo["PayIDJson"].ToString().Trim() != "")
            {
                UserID = JInfo["UserID"].ToString();
                disID = JInfo["ResellerID"].ToString();
                OrderType = JInfo["OrderType"].ToString();
                PayPrice = JInfo["PayPrice"].ToString();
                PayIDJSon = JInfo["PayIDJson"].ToString();

            }
            else
            {
                return new ResultPayInfo() { Result = "F", Description = "参数异常" };
            }

            if (OrderType == "0")
            {
                if (JInfo["ReceiptNo"].ToString().Trim() != "")
                {
                    ReceiptNo = JInfo["ReceiptNo"].ToString();
                }
                else
                {
                    return new ResultPayInfo() { Result = "F", Description = "参数异常" };
                }
            }
            #endregion

            //判断登录信息是否正确
            Hi.Model.SYS_Users user = new Hi.Model.SYS_Users();
            if (!new Common().IsLegitUser(int.Parse(UserID), out user, 0, int.Parse(disID == "" ? "0" : disID)))
                return new ResultPayInfo() { Result = "F", Description = "用户异常" };
            //判断经销商信息是否异常
            Hi.Model.BD_Distributor dis = bll_dis.GetModel(Convert.ToInt32(disID));
            if (dis == null || dis.dr == 1 || dis.AuditState == 0 || dis.IsEnabled == 0)
                return new ResultPayInfo() { Result = "F", Description = "经销商异常" };
            //判断经销商对应的核心企业是否异常
            Hi.Model.BD_Company comp = bll_comp.GetModel(dis.CompID);
            if (comp == null || comp.dr == 1 || comp.AuditState == 0 || comp.IsEnabled == 0)
                return new ResultPayInfo() { Result = "F", Description = "核心企业异常" };
            //判断此订单的信息是否异常，是否可支付
            //根据订单号，取出订单数据放入dt中
            if (OrderType == "0")
            {
                dt_order = bll_order.GetData(ReceiptNo).Tables[0];
                if (Convert.ToInt32(dt_order.Rows[0]["OState"]) == (int)Enums.OrderState.待审核 ||
                    Convert.ToInt32(dt_order.Rows[0]["OState"]) == (int)Enums.OrderState.已作废 ||
                    Convert.ToInt32(dt_order.Rows[0]["PayState"]) == (int)Enums.PayState.已支付)
                    return new ResultPayInfo() { Result = "F", Description = "订单信息异常" };
            }

            //获取Pay_PayWxandAli表的实体
            Hi.Model.Pay_PayWxandAli payali = Common.GetPayWxandAli(comp.ID);
            if (payali == null || ClsSystem.gnvl(payali.ali_isno, "0") == "0")
                return new ResultPayInfo() { Result = "F", Description = "核心企业无可用的支付宝收款账户" };
            //对数据库中数据进行解密
            //partner = Common.DesDecrypt(payali.ali_partner, Common.EncryptKey);
            //seller = Common.DesDecrypt(payali.ali_seller_email, Common.EncryptKey);
            //private_key = Common.DesDecrypt(payali.ali_key, Common.EncryptKey);
            partner = payali.ali_partner;
            seller = payali.ali_seller_email;
            private_key = payali.ali_RSAkey;
            //订单支付跟预付款充值的提示信息（两种请款下的提示信息是不同的）
            if (OrderType == "0")
            {
                subject = "医站通订单-" + ReceiptNo;
            }
            else
            {
                subject = "医站通预付款充值";
            }
            //订单所有商品明细

            if (OrderType == "0")
            {
                DataTable l = new Hi.BLL.DIS_OrderDetail().GetOrderDe("", " IsNUll(o.dr,0)=0 and o.OrderId=" + Convert.ToInt32(dt_order.Rows[0]["ID"]));
                foreach (DataRow dr in l.Rows)
                {
                    GoodsName += dr["GoodsName"] + ",";
                }
                GoodsName = GoodsName.Substring(0, GoodsName.Length - 1);//去掉最后一个逗号
                if (GoodsName.Length > 15)
                    GoodsName = GoodsName.Substring(0, 10) + "...";
            }

            //获取订单支付时的支付流水号
            if (OrderType == "0")
            {
                OrderNumberJSon = new OrderPay().GetPayID(PayIDJSon);
                //解析返回的订单支付流水号的JSon
                JInfo = JsonMapper.ToObject(OrderNumberJSon);
                if (JInfo["Result"].ToString().Trim() == "F" || JInfo["PayNumb"].ToString().Trim() == "" || JInfo["Result"].ToString().Trim() == ""||
                    JInfo["OrderCode"].ToString().Trim() == "")
                {
                    return new ResultPayInfo() { Result = "F", Description = "获取支付流水号异常" };
                }
                else
                {
                    OrderNumber = JInfo["PayNumb"].ToString();
                    OrderCode = JInfo["OrderCode"].ToString();
                }
            }
            else//钱包充值时的流水号跟企业订单号
            {
                List<string> list = new List<string>();
                list = Getpayidyfk(user.ID, comp.ID, dis.ID, PayPrice);
                OrderNumber = list[0];
                OrderCode = list[1];
                if(OrderNumber == "" || OrderCode =="")
                    return new ResultPayInfo() { Result = "F", Description = "获取支付流水号异常" };

            }
            //创建订单信息
            orderInfo = comm.getOrderInfo(subject, GoodsName, PayPrice, OrderNumber, partner, seller);
            // 对订单做RSA 签名
            //private_key = "MIICdwIBADANBgkqhkiG9w0BAQEFAASCAmEwggJdAgEAAoGBANDptrV3pWWdbnr6wT8lPkZ6kbRgsjf8CfXpQNrJyOsmktOaX8DQRh5nd0lSFTtbfeGV4oWXi26bv+1Vt+CfAsYssf+futRj0l6Cxx0psJapK7QEXq3HqjB0tM11M9ym75WFFvTSZs1DysUkRV35g5rXuRtBOWR2sqm9JRAen+29AgMBAAECgYEAzZ+L1xb5c4e960uOE1Hb9tDDQs/9+j6XqzQ3QmFj4Zeo4p9KaeRVb62U6lThUvgdcYDuYWEkuuyPvtEk1/CKb61AvEW69ehwLeDXOy9AzEgQpGFPb1bgJ+kU8YCpgcOGR9G55iVc0ZW7B2iyx111Wvij8pc+A2ZeuByAG1f8PoECQQD7wvCyBJRNb7Gv7iHF+zx2lDxG6LTX2rCAZdr8FyWVjZEBfL6uPI+/2J2AqtFPaLa25+jQc3b74r4wuGKsvxBhAkEA1G4aDQovfB9RO/c4I+NX4mmitpNt66IuqKp0a9pOL/YfpNtr5GBgmK4LMVASqIG74bw5wAV7zJkunlGGPusK3QJBALLiUm/KvS1AXbqpsymfV9jRfvrLQiPVaW/x72ULdVMMIaoy3rGiqmkgGtlfhhWsS5cutMfYIwTamVS4zrP7lkECQFTvDJVoHCI5d0ZNivG2ZR4OdFMhURKkTpl7RX8V0qsUcgR9An9WFWkWNT1rMXqUHGWd100yJBRirqP4Hn+rhDUCQBPVgm4jNd5WjKD0Oj39FmF5D89OviTgQK4xYxYUIfqLBhuGZ3kWDvAmeE6uRgVXjQncv/c68W8pS2huT0syom4=";
            //private_key = "MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";
            string sign = Com.Alipay.RSAFromPkcs8.sign(orderInfo, private_key, "utf-8");
            //string sign = RSAFromPkcs8.sign(orderInfo, private_key, "utf-8");
            sign = HttpUtility.UrlEncode(sign, Encoding.UTF8);
            //返回实体
            // 完整的符合支付宝参数规范的订单信息
            payinfo.Result = "T";
            payinfo.Description = "返回成功";
            payinfo.InfoString = orderInfo + "&sign=\"" + sign + "\"&"
                  + comm.getSignType();
            payinfo.OrderCode = OrderCode;
        }
        catch (Exception ex)
        {
            Common.CatchInfo(ex.Message + ":" + ex.StackTrace, "GetPayInfo:" + JSon);
            return new ResultPayInfo() { Result = "F", Description = "参数异常" };
        }
        return payinfo;
    }

    ///<summary>
    ///获取钱包充值流水号
    ///</summary>
    public List<string> Getpayidyfk(int UserID,int CompID, int DisID, string price)
    {
        int prepayid=0 ;
        int payid = 0;
        int regid = 0;
        List<string> list = new List<string>();
        Hi.Model.PAY_PrePayment Prepay = new Hi.Model.PAY_PrePayment();
        Hi.BLL.PAY_PrePayment bll_prepay = new Hi.BLL.PAY_PrePayment();
        Hi.BLL.PAY_Payment bll_pay = new Hi.BLL.PAY_Payment();
        Hi.BLL.PAY_RegisterLog bll_reg = new Hi.BLL.PAY_RegisterLog();
        Hi.Model.BD_Distributor dis = new Hi.BLL.BD_Distributor().GetModel(DisID);
        try
        {
            //在表PAY_PrePayment中生成一条数据
            Prepay.CompID = CompID;
            Prepay.DisID = DisID;
            Prepay.OrderID = 0;
            Prepay.Start = 2;
            Prepay.PreType = 1;
            Prepay.price = Convert.ToDecimal(price);
            Prepay.Paytime = DateTime.Now;
            Prepay.CreatDate = DateTime.Now;
            Prepay.OldId = 0;
            Prepay.CrateUser = UserID;
            Prepay.AuditState = 2;
            Prepay.AuditUser = 0;
            Prepay.IsEnabled = 1;
            Prepay.ts = DateTime.Now;
            Prepay.modifyuser = UserID;
            prepayid = bll_prepay.Add(Prepay);
            if (prepayid > 0)
            {
                //如果pay_prepayment表中插入数据成功的话，在pay_payment表中插入一条数据
                int keyID = prepayid;
                Hi.Model.PAY_PrePayment Prepay_M = bll_prepay.GetModel(prepayid);
                string guid = Guid.NewGuid().ToString().Replace("-","");
                Hi.Model.PAY_Payment pay = new Hi.Model.PAY_Payment();
                pay.OrderID = keyID;
                pay.DisID = DisID;
                pay.PayUser = dis.DisName;
                pay.PayPrice = Convert.ToDecimal(price);
                pay.guid = Common.Number_repeat(guid);
                pay.IsAudit = 2;
                pay.vdef3 = "2";
                pay.CreateDate = DateTime.Now;
                pay.CreateUserID = UserID;
                pay.ts = DateTime.Now;
                pay.modifyuser = UserID;
                pay.Channel = "6";
                payid = bll_pay.Add(pay);
                //如果pay_prepayment表中插入数据成功的话，在PAY_RegisterLog表中插入一条数据
                Hi.Model.PAY_RegisterLog reg = new Hi.Model.PAY_RegisterLog();
                reg.OrderId = keyID;
                reg.Ordercode = WebConfigurationManager.AppSettings["OrgCode"] + Convert.ToString(keyID);
                reg.number = pay.guid;
                reg.Price = Convert.ToDecimal(price);
                reg.Payuse = "企业钱包充值";
                reg.PayName = dis.DisName;
                reg.DisID = DisID;
                reg.PayTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                reg.BankID = "支付宝";
                reg.CreateUser = UserID;
                reg.CreateDate = DateTime.Now;
                reg.LogType = 1375;
                regid = bll_reg.Add(reg);
                if (payid > 0 && regid > 0)
                {
                    //返回的list第一行放支付流水号，第二行放企业订单号（支付时生成）
                    list.Add(ClsSystem.gnvl(pay.guid,""));
                    list.Add(ClsSystem.gnvl(reg.Ordercode,""));
                    return list;
                }
            }
            
        }
        catch
        {
        }
        return null;
    }



    //微信支付的返回实体
    public class ResultWX
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String AppID { get; set; }//绑定支付的APPID
        public String Mchid { get; set; }//商户号
        //public String AppSecret { get; set; }
        public String APPkey { get; set; }//商户支付密钥
    }
    //支付宝支付的返回实体
    public class ResultAli
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String PARTNER { get; set; }//合作身份者ID，以2088开头由16位纯数字组成的字符串。
        public String SELLER { get; set; }//支付宝收款账号，手机号码或邮箱格式。
        public String RSA_PRIVATE { get; set; }//商户方的私钥，pkcs8格式。
    }
    //支付宝支付信息返回实体
    public class ResultPayInfo
    {
        public String Result { get; set; }
        public String Description { get; set; }
        public String InfoString { get; set; }//支付拼接字串
        public String OrderCode { get;set;}//企业订单号（支付时候生成的）
    }
}