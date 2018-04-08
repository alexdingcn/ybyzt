using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using LitJson;
using System.Data;
using System.Web.Configuration;
using CFCA.Payment.Api;
using System.Text;
using System.Configuration;
using System.Xml;
using System.Net.Mail;
using System.Collections;
using System.Data.SqlClient;
using DBUtility;

/// <summary>
///WXService 的摘要说明
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
//若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消对下行的注释。 
// [System.Web.Script.Services.ScriptService]
public class TimeService : System.Web.Services.WebService
{

    public TimeService()
    {

        //如果使用设计的组件，请取消注释以下行 
        //InitializeComponent(); 
    }

    [WebMethod(Description = "定时结算")]
    public void Test(string Type)
    {
        ////本地日志记录
        LogManager.LogFielPrefix = "Order_time";
        LogManager.LogPath = "D:/订单结算日志Order_log/";

        LogManager.WriteLog(LogFile.Trace.ToString(), DateTime.Now.ToString() + "-订单结算程序启动>>>>>>>>>>>...\r\n");

        string strMsg = string.Empty;
        string pre_strMsg = string.Empty;
        string fw_strMsg = string.Empty;
        string head = "各位,您好!<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" + DateTime.Now.Year + "年" + DateTime.Now.Month + "月" + DateTime.Now.Day + "日" + " 结算情况如下,请查看："; ;

        string topMsg = string.Empty;
        string pre_topMsg = string.Empty;
        string fw_topMsg = string.Empty;
        string downMsg = string.Empty;
        string pre_downMsg = string.Empty;
        string fw_downMsg = string.Empty;

        int ord_errornum = 0;//记录订单支付错误的记录
        int pre_errornum = 0;//记录企业钱包错误的记录
        int fw_errornum = 0;//记录服务错误的记录

        int db_num = 0;//担保支付记录
        string db_meg = string.Empty;//担保支付信息


        //订单结算方法
        if (Type == "Order")
        {
            string LocalSqlServer = SqlHelper.LocalSqlServer;

            int compID = 0;
            int disID = 0;
            long price = 0;//金额
            int ordID = 0;//订单Id
            string ReceiptNo = string.Empty;//订单号
            string guid = string.Empty;//流水号
            string remark = string.Empty;//订单备注
            int paymentID = 0;//支付表ID
            int Channel = 0;//支付渠道（1，快捷支付，2，银联支付 ，3，网银支付，4，B2B网银支付）
            decimal payment_zfsxf = 0;//支付表计算的手续费
            int Paymen_Type = 0;//支付类型
            int OState = 0;//判断是否已确认到货
            int jsxf_no = 0;//判断手续费是否结算成功
            string hc_flag = string.Empty;//结算回冲标准

            decimal Old_price = 0;//原支付金额
            //-----银行信息
            string orgcode = ConfigurationManager.AppSettings["PayOrgCode"].ToString().Trim();// string.Empty;//机构代码
            int accountType = 0;//帐号类型
            string paymentaccountname = string.Empty;//账户名称（支付平台账户 当 AccountType=20 时，该项必填）
            string paymentaccountnumber = string.Empty;//账户号码（支付平台账户 当 AccountType=20 时，该项必填）
            string bankaccount = string.Empty;//收款方在银行开立的账户
            string bankId = string.Empty;//银行ID
            string accountname = string.Empty;//账户名称
            string accountnumber = string.Empty;//账户号码
            string branchname = string.Empty;//开会行地址
            string province = string.Empty;//开会所在省
            string city = string.Empty;//开会所在市

            //结算接口日志表
            Hi.Model.PAY_PayLog paylogmodel = new Hi.Model.PAY_PayLog();
            Hi.BLL.PAY_PayLog paylogbll = new Hi.BLL.PAY_PayLog();
            int paylogID = 0;//接口日志返回ID



            #region 订单结算
            string sxfsq = "-1";

            DataTable dt_order = new Hi.BLL.PAY_PrePayment().GetdataTable(1, "", 1);
            if (dt_order.Rows.Count > 0)
            {
                foreach (DataRow dr in dt_order.Rows)
                {
                    compID = 0;
                    disID = 0;
                    price = 0;//金额
                    ordID = 0;//订单Id
                    ReceiptNo = string.Empty;//订单号
                    guid = string.Empty;//流水号
                    remark = string.Empty;//订单备注
                    paymentID = 0;//支付表ID
                    Channel = 0;//支付渠道（1，快捷支付，2，银联支付 ，3，网银支付，4，B2B网银支付）
                    payment_zfsxf = 0;//支付表计算的手续费
                    Paymen_Type = 0;//支付类型
                    OState = 0;//判断是否已确认到货
                    jsxf_no = 0;//判断手续费是否结算成功
                    hc_flag = string.Empty;//结算回冲标准
                    Old_price = 0;//原支付金额
                    //-----银行信息
                    accountType = 0;//帐号类型
                    paymentaccountname = string.Empty;//账户名称（支付平台账户 当 AccountType=20 时，该项必填）
                    paymentaccountnumber = string.Empty;//账户号码（支付平台账户 当 AccountType=20 时，该项必填）
                    bankaccount = string.Empty;//收款方在银行开立的账户
                    bankId = string.Empty;//银行ID
                    accountname = string.Empty;//账户名称
                    accountnumber = string.Empty;//账户号码
                    branchname = string.Empty;//开会行地址
                    province = string.Empty;//开会所在省
                    city = string.Empty;//开会所在市


                    ////本地日志记录
                    LogManager.LogFielPrefix = "Order_time";
                    LogManager.LogPath = "D:/订单结算日志Order_log/";
                    LogManager.WriteLog(LogFile.Trace.ToString(), DateTime.Now.ToString() + "-订单结算服务启动...");

                    try
                    {
                        compID = Convert.ToInt32(dr["CompID"]);
                        disID = Convert.ToInt32(dr["DisID"]);
                        //原支付金额
                        Old_price = Convert.ToDecimal(dr["PayPrice"]) - Convert.ToDecimal(dr["vdef5"] + "" == "" ? "0" : dr["vdef5"]);


                        Channel = Convert.ToInt32(dr["Channel"] + "" == "" ? "-1" : dr["Channel"]);
                        payment_zfsxf = Convert.ToDecimal(dr["vdef5"]);
                        jsxf_no = Convert.ToInt32(dr["jsxf_no"]);
                        hc_flag = dr["vdef6"] + "";

                        #region     计算支付手续费 start

                        decimal sxf = 0;
                        long js_sxf = 0;
                        //查询该企业的设置
                        List<Hi.Model.Pay_PaymentSettings> Sysl = new Hi.BLL.Pay_PaymentSettings().GetList("", " CompID=" + compID, "");
                        if (Sysl.Count > 0)
                        {
                            //手续费收取
                            sxfsq = Convert.ToString(Sysl[0].pay_sxfsq);

                            //支付方式--线上or线下
                            string zffs = Convert.ToString(Sysl[0].pay_zffs);

                            //免手续费支付次数
                            int mfcs = Convert.ToInt32(Sysl[0].Pay_mfcs);

                            //快捷支付比例
                            decimal kjzfbl = Convert.ToDecimal(Sysl[0].pay_kjzfbl) / 1000;
                            decimal kjzfstart = Convert.ToDecimal(Sysl[0].pay_kjzfstart);
                            decimal kjzfend = Convert.ToDecimal(Sysl[0].pay_kjzfend);

                            //银联支付比例
                            decimal ylzfbl = Convert.ToDecimal(Sysl[0].pay_ylzfbl) / 1000;
                            decimal ylzfstart = Convert.ToDecimal(Sysl[0].pay_ylzfstart);
                            decimal ylzfend = Convert.ToDecimal(Sysl[0].pay_ylzfend);

                            //B2c网银支付比例

                            decimal b2cwyzfbl = Convert.ToDecimal(Sysl[0].pay_b2cwyzfbl) / 1000;
                            decimal b2cwyzfstart = Convert.ToDecimal(Sysl[0].vdef1);

                            //B2b网银支付比例

                            decimal b2bwyzfbl = Convert.ToDecimal(Sysl[0].pay_b2bwyzf);

                            //手续费收取方
                            if (sxfsq == "0")
                                sxf = 0;
                            else
                            {
                                //手续费 (没有免支付次数时，才计算手续费)
                                if (mfcs <= 0)
                                {
                                    switch (Channel)
                                    {
                                        case 1://快捷支付手续费
                                            sxf = Old_price * kjzfbl;
                                           // if (sxf <= kjzfstart)
                                           //     sxf = kjzfstart;
                                           // else if (sxf >= kjzfend)
                                           //     sxf = kjzfend;
                                            break;
                                        case 2://银联支付手续费
                                            sxf = Old_price * ylzfbl;
                                            //if (sxf <= ylzfstart)
                                            //    sxf = ylzfstart;
                                            //else if (sxf >= ylzfend)
                                             //   sxf = ylzfend;
                                            break;
                                        case 3://B2C网银支付手续费
                                            sxf = Old_price * b2cwyzfbl;
                                            //if (sxf <= b2cwyzfstart)
                                             //   sxf = b2cwyzfstart;
                                            break;
                                        case 8://信用卡支付手续费
                                            sxf = Old_price * b2cwyzfbl;
                                            break;
                                        case 4://B2B网银支付手续费
                                            sxf = b2bwyzfbl;
                                            break;
                                    }
                                }

                                //计算手续费
                                sxf = Math.Round(sxf, 2) * 100;//转化成分                              

                            }
                        }

                        #endregion  计算支付手续费 end

                        price = Convert.ToInt64(Old_price * 100);

                        bool sxf_fal = false;//判断结算时的手续费，是否和支付时手续费一致

                        #region  根据手续费收取方，判断清算金额 start

                        if (sxfsq == "1") // 1,代理商 
                        {
                            if (sxf == payment_zfsxf * 100)
                            {
                                //price = price;//最终清算时的订单金额
                                js_sxf = Convert.ToInt64(sxf);//最终清算时的手续费
                                sxf_fal = true;
                            }
                        }
                        else if (sxfsq == "2")//2,企业
                        {
                            js_sxf = Convert.ToInt64(sxf);//最终清算时的手续费
                            // price = price;//最终清算时的订单金额
                            sxf_fal = true;
                        }
                        else
                            sxf_fal = true;

                        #endregion 根据手续费收取方，判断清算金额 end

                        if (sxf_fal)
                        {
                            ReceiptNo = Convert.ToString(dr["ReceiptNo"]);
                            //回冲操作处理重新生成Guid，paylog表通过createuserid字段和payment表关联
                            if ("1".Equals(hc_flag))
                                guid = Common.Number_repeat(Guid.NewGuid().ToString().Replace("-", ""));
                            else
                                guid = Convert.ToString(dr["GUID"]);

                            ordID = Convert.ToInt32(dr["ID"]);
                            paymentID = Convert.ToInt32(dr["paymentID"]);

                            Paymen_Type = Convert.ToInt32(dr["Type"]);//判断是否是担保支付
                            OState = Convert.ToInt32(dr["OState"]);//判断订单是否已确认到货
                            string disname = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(Common.GetOrderValue(ordID, "DisID"))).DisName;
                            remark = "订单结算-" + disname + "-" + ReceiptNo;// Convert.ToString(dr["Remark"]);



                            //支付类型为正常支付，或者支付类型是担保支付，但是已确认到货的订单，进行结算操作
                            if (Paymen_Type == 0 || (Paymen_Type == 1 && OState == 5))
                            {
                                DataTable dt_bank_comp = new Hi.BLL.PAY_PrePayment().GetdataTable(3, "  and Isno=1 and PAY_PaymentBank.CompID=" + compID, 0);//结算接口，银行信息--已企业为主，

                                if (dt_bank_comp.Rows.Count > 0)
                                {
                                    foreach (DataRow drcomp in dt_bank_comp.Rows)
                                    {
                                        accountType = Convert.ToInt32(drcomp["type"]);//帐号类型
                                        paymentaccountname = Convert.ToString(drcomp["payName"]);//账户名称
                                        paymentaccountnumber = Convert.ToString(drcomp["PayCode"]); //账户号码
                                        //收款方在银行开立的账户
                                        bankId = Convert.ToString(drcomp["BankID"]); ;//银行ID
                                        accountname = Convert.ToString(drcomp["AccountName"]); //账户名称
                                        accountnumber = Convert.ToString(drcomp["bankcode"]);//账户号码
                                        branchname = Convert.ToString(drcomp["bankAddress"]);//开会行地址
                                        province = Convert.ToString(drcomp["bankprivate"]);//开会所在省
                                        city = Convert.ToString(drcomp["bankcity"]);//开会所在市
                                    }
                                }



                                //判断参数收款银行是否维护
                                if (accountType == 0)
                                {
                                    ord_errornum++;
                                    strMsg += "<br>订单结算无法进行收款,请维护收款帐号信息:" + DateTime.Now.ToString() + "；" + "错误流水Guid：" + guid + "；" + "企业ID:&nbsp;(" + new Hi.BLL.BD_Company().GetModel(compID).CompName + ")";
                                }
                                //个人账户、企业账户
                                else if ((accountType == 11 || accountType == 12) && bankId != "" && accountname != "" && accountnumber != "")
                                {

                                    //先插入日志表，
                                    paylogmodel.OrderId = ordID;
                                    paylogmodel.Ordercode = ReceiptNo;
                                    paylogmodel.number = guid;
                                    paylogmodel.CompID = compID;
                                    paylogmodel.OrgCode = orgcode;
                                    paylogmodel.MarkName = paymentaccountname;
                                    paylogmodel.MarkNumber = paymentaccountnumber;
                                    paylogmodel.AccountName = accountname;
                                    paylogmodel.bankcode = accountnumber;
                                    paylogmodel.bankAddress = branchname;
                                    paylogmodel.bankPrivate = province;
                                    paylogmodel.bankCity = city;
                                    paylogmodel.Price = price;
                                    paylogmodel.Remark = remark;
                                    paylogmodel.CreateDate = DateTime.Now;
                                    paylogmodel.CreateUser = paymentID;//关联支付表
                                    paylogID = paylogbll.Add(paylogmodel);


                                    if (paylogID > 0)//日志插入成功
                                    {

                                        //调用中金接口，做结算处理-------------------------------
                                        try
                                        {
                                            string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                                            PaymentEnvironment.Initialize(configPath);

                                            // 2.创建交易请求对象
                                            Tx1341Request tx1341Request = new Tx1341Request();
                                            tx1341Request.setInstitutionID(orgcode);
                                            tx1341Request.setSerialNumber(guid);
                                            tx1341Request.setOrderNo(ReceiptNo);
                                            tx1341Request.setAmount(price);
                                            tx1341Request.setRemark(remark);
                                            tx1341Request.setAccountType(accountType);
                                            tx1341Request.setPaymentAccountName(paymentaccountname);
                                            tx1341Request.setPaymentAccountNumber(paymentaccountnumber);

                                            BankAccount bankAccount = new BankAccount();
                                            bankAccount.setBankID(bankId);
                                            bankAccount.setAccountName(accountname);
                                            bankAccount.setAccountNumber(accountnumber);
                                            bankAccount.setBranchName(branchname);
                                            bankAccount.setProvince(province);
                                            bankAccount.setCity(city);
                                            tx1341Request.setBankAccount(bankAccount);

                                            // 3.执行报文处理
                                            tx1341Request.process();

                                            //2个信息参数
                                            HttpContext.Current.Items["txCode"] = "1341";
                                            HttpContext.Current.Items["txName"] = "市场订单结算（结算）";

                                            // 与支付平台进行通讯
                                            TxMessenger txMessenger = new TxMessenger();
                                            String[] respMsg = txMessenger.send(tx1341Request.getRequestMessage(), tx1341Request.getRequestSignature());// 0:message; 1:signature
                                            String plaintext = XmlUtil.formatXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(respMsg[0])));
                                            Console.WriteLine("[message] = [" + respMsg[0] + "]");
                                            Console.WriteLine("[signature] = [" + respMsg[1] + "]");
                                            Console.WriteLine("[plaintext] = [" + plaintext + "]");

                                            Tx134xResponse tx134xResponse = new Tx134xResponse(respMsg[0], respMsg[1]);
                                            HttpContext.Current.Items["plainText"] = tx134xResponse.getResponsePlainText();
                                            string strs = tx134xResponse.getCode() + "," + tx134xResponse.getMessage();
                                            //消息提示
                                            //JScript.ShowAlert(this, strs);

                                            //日志记录 接口返回的信息
                                            paylogmodel.Start = tx134xResponse.getCode();
                                            paylogmodel.ResultMessage = tx134xResponse.getMessage();
                                            paylogmodel.ID = paylogID;
                                            bool payLog_update = paylogbll.Update(paylogmodel);

                                            #region  清算手续费 start
                                            bool bol = false;
                                            //收取代理商或企业手续费，并且手续费未结算的，且手续费大于0
                                            //if ((sxfsq == "1" || sxfsq == "2") && jsxf_no != 1 && js_sxf > 0)
                                            //    bol = JS_sxfWay(ordID, ReceiptNo, guid, compID, paymentID, js_sxf);
                                            //else
                                            bol = true;

                                            #endregion 清算手续费 end

                                            if ("2000".Equals(tx134xResponse.getCode()))
                                            {


                                                //查询该条订单相关的支付记录是否都已经结算
                                                DataTable dt = new Hi.BLL.PAY_PrePayment().GetDate("ID", "PAY_Payment", "PrintNum=0 and IsAudit=1 and OrderID=" + ordID);
                                                int n_row = dt.Rows.Count;


                                                SqlConnection con = new SqlConnection(LocalSqlServer);
                                                con.Open();
                                                SqlTransaction sqlTrans = con.BeginTransaction();

                                                int paymentnum = 0;//支付表修改记录
                                                int ordernum = 0;//订单结算状态修改记录

                                                try
                                                {
                                                    //修改支付记录的结算状态
                                                    Hi.BLL.PAY_Payment paymentbll = new Hi.BLL.PAY_Payment();
                                                    if (bol)
                                                        paymentnum = paymentbll.updatePayState_JS(con, paymentID, sqlTrans);

                                                    //修改订单
                                                    Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(ordID);
                                                    if (orderModel.AuditAmount == orderModel.PayedAmount)
                                                    {
                                                        if (n_row == 1)
                                                        {
                                                            ordernum = paymentbll.UpdateOrderPaystate_JS(con, ordID, "1", sqlTrans);
                                                            if (ordernum > 0)
                                                            {
                                                                LogManager.WriteLog(LogFile.Trace.ToString(), "订单操作成功:" + DateTime.Now.ToString() + "----" + ReceiptNo);
                                                                //strMsg += "<br>订单结算操作成功:" + DateTime.Now.ToString() + "流水号：" + guid + " 订单号：" + ReceiptNo;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            ordernum = paymentbll.UpdateOrderPaystate_JS(con, ordID, "2", sqlTrans);
                                                            if (ordernum > 0)
                                                            {
                                                                LogManager.WriteLog(LogFile.Trace.ToString(), "订单操作成功:" + DateTime.Now.ToString() + "----" + ReceiptNo);
                                                                //strMsg += "<br>订单结算操作成功:" + DateTime.Now.ToString() + "流水号：" + guid + " 订单号：" + ReceiptNo;
                                                            }
                                                        }


                                                    }
                                                    else
                                                    {
                                                        //orderModel.vdef9 = "2";//部分结算
                                                        ordernum = paymentbll.UpdateOrderPaystate_JS(con, ordID, "2", sqlTrans);
                                                        if (ordernum > 0)
                                                        {
                                                            LogManager.WriteLog(LogFile.Trace.ToString(), "订单操作成功:" + DateTime.Now.ToString() + "----" + ReceiptNo);
                                                            // strMsg += "<br>订单结算操作成功:" + DateTime.Now.ToString() + "流水号：" + guid + " 订单号：" + ReceiptNo;
                                                        }
                                                    }

                                                    //执行修改成功后，提交事务，否则回滚
                                                    if (paymentnum > 0 && ordernum > 0)
                                                        sqlTrans.Commit();
                                                    else
                                                        sqlTrans.Rollback();

                                                }
                                                catch
                                                {
                                                    paymentnum = 0;
                                                    ordernum = 0;
                                                    sqlTrans.Rollback();
                                                }
                                                finally
                                                {
                                                    con.Close();
                                                }

                                            }
                                            else
                                            {
                                                //清算出现问题，发送邮件通知
                                                strMsg += "<br><font color='red'>订单结算接口返回:</font>" + strs + "流水号：" + guid + "&nbsp;(" + new Hi.BLL.BD_Company().GetModel(compID).CompName + ")";
                                                ord_errornum++;

                                                LogManager.WriteLog(LogFile.Trace.ToString(), "订单结算接口返回非2000编码:" + strs + "\r\n" + "订单编号：" + ReceiptNo);

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                            LogManager.WriteLog(LogFile.Trace.ToString(), "订单调用中金结算接口出错:" + DateTime.Now.ToString() + "\r\n" + "订单编号：" + ReceiptNo);

                                            ord_errornum++;
                                            strMsg += "<br><font color='red'>订单调用中金结算接口出错:</font>" + DateTime.Now.ToString() + "流水号：" + guid + " 订单编号：" + ReceiptNo + "&nbsp;(" + new Hi.BLL.BD_Company().GetModel(compID).CompName + ")";
                                        }
                                    }
                                    else
                                    {
                                        LogManager.WriteLog(LogFile.Trace.ToString(), "订单结算接口日志表插入出错:" + DateTime.Now.ToString() + "\r\n" + "订单编号：" + ReceiptNo);
                                        ord_errornum++;
                                        strMsg += "<br><font color='red'>订单结算接口日志表插入出错:</font>" + DateTime.Now.ToString() + "流水号：" + guid + " 订单编号：" + ReceiptNo + "&nbsp;(" + new Hi.BLL.BD_Company().GetModel(compID).CompName + ")";
                                    }

                                }
                                else
                                {
                                    if (accountType == 11 || accountType == 12)
                                        LogManager.WriteLog(LogFile.Trace.ToString(), "银行名称、持卡人名称、银行卡号不能为空！" + "\r\n");
                                    else
                                        LogManager.WriteLog(LogFile.Trace.ToString(), "中金账户名称、中金账户号码不能为空！" + "\r\n");

                                    ord_errornum++;
                                    strMsg += "<br><font color='red'>订单结算银行名称、持卡人名称、银行卡号不能为空 或 中金账户名称、中金账户号码不能为空</font>&nbsp;(" + new Hi.BLL.BD_Company().GetModel(compID).CompName + ")";
                                }
                            }
                            else
                            {
                                db_num++;
                                db_meg = "担保支付需要代理商确认收货，才能进结算";

                            }
                        }
                        else
                        {
                            ord_errornum++;
                            strMsg += "<br><font color='red'>订单结算出错(结算手续费不一致):</font>" + DateTime.Now.ToString() + "流水号" + guid + "订单编号：" + ReceiptNo + "&nbsp;(" + new Hi.BLL.BD_Company().GetModel(compID).CompName + ")";
                            LogManager.WriteLog(LogFile.Trace.ToString(), "订单结算出错(结算手续费不一致):" + DateTime.Now.ToString() + "\r\n" + "订单编号：" + ReceiptNo);

                        }
                    }
                    catch (Exception ex)
                    {
                        ord_errornum++;
                        strMsg += "<br><font color='red'>订单结算出错:</font>" + DateTime.Now.ToString() + "流水号：" + guid + " 订单编号：" + ReceiptNo + "&nbsp;(" + new Hi.BLL.BD_Company().GetModel(compID).CompName + ")";
                        LogManager.WriteLog(LogFile.Trace.ToString(), "订单结算出错:" + DateTime.Now.ToString() + "\r\n" + "订单编号：" + ReceiptNo);

                    }
                    LogManager.WriteLog(LogFile.Trace.ToString(), "订单结算结束时间" + DateTime.Now.ToString() + "\r\n");
                }

                topMsg += "<br><strong>今天未清算订单共 " + dt_order.Rows.Count + "条，成功" + (dt_order.Rows.Count - ord_errornum - db_num) + "条，失败" + ord_errornum + "条</strong>";
                //担保支付信息记录（暂时不提示）
                if (!string.IsNullOrEmpty(db_meg))
                    topMsg += "(" + db_num + " 条 " + db_meg + ")";


                if (!string.IsNullOrEmpty(strMsg))
                {
                    strMsg += "<br>订单结算结束时间" + DateTime.Now.ToString() + "<br>===================================================<br>";
                }
            }
            else
                downMsg = "<br>今天没有需要结算的订单;";

            #endregion


            //最后输出
            string sum_mes = head;

            if (!string.IsNullOrEmpty(topMsg))
            {
                sum_mes += topMsg + strMsg;
            }
            else
                sum_mes += downMsg;

            if (!string.IsNullOrEmpty(fw_topMsg))
            {
                sum_mes += fw_topMsg + fw_strMsg;
            }
            else
                sum_mes += fw_downMsg;

            if (!string.IsNullOrEmpty(pre_topMsg))
            {
                sum_mes += pre_topMsg + pre_strMsg;
            }
            else
                sum_mes += pre_downMsg;

            PayInfoType.SendFinsh(sum_mes + @"谢谢！" + strDownMesg, ReceiptNo);

            #region 每天清算一次手续费
            string orderNo = WebConfigurationManager.AppSettings["OrgCode"] + DateTime.Now.ToString("yyyyMMdd");
            DataTable dt_sxf = new Hi.BLL.PAY_PrePayment().GetdataTable_sxf(orderNo);
            long price_sxf = 0;
            string receiptno_sxf = string.Empty;
            if (dt_sxf.Rows.Count > 0)
            {
                price_sxf = Convert.ToInt64(Math.Round(Convert.ToDecimal(dt_sxf.Rows[0]["price_sumsxf"]), 2) * 100);
                receiptno_sxf = Convert.ToString(dt_sxf.Rows[0]["ReceiptNo"]);
                if (price_sxf > 0)
                {
                    bool fal_jssxf = JS_sxfWay(0, receiptno_sxf, Common.Number_repeat(""), 0, 0, price_sxf);
                    //if (fal_jssxf)
                     ///   resultMesage = resultMesage + "(手续费清算成功)";


                }
            }
            #endregion

        }
    }
    //签名
    public static string strDownMesg = string.Empty;
    string str_log = @"<br><br> 
        <img src='http://www.my1818.com/Company/images/70f2abef-c1ed-4a(11-29-09-16-14).png' />
<img src='http://www.my1818.com/Company/images/1eb0b97e-f005-40(11-29-09-16-14).png' /><br> 
================================================<br> 
  电话：40077-40088 <br> 
  地址：上海市浦东新区耀华路488号信建大厦8楼";


    /// <summary>
    /// 清算手续费
    /// </summary>
    /// <param name="sxf">sxf金额</param>
    /// <returns></returns>
    public bool JS_sxfWay(int ordID, string ReceiptNo, string guid, int compID, int paymentid, long price)
    {
        bool fal = false;
        //-----银行信息
        string orgcode = string.Empty;//机构代码
        int accountType = 0;//帐号类型
        string paymentaccountname = string.Empty;//账户名称
        string paymentaccountnumber = string.Empty;//账户号码
        string bankaccount = string.Empty;//收款方在银行开立的账户
        string bankId = string.Empty;//银行ID
        string accountname = string.Empty;//账户名称
        string accountnumber = string.Empty;//账户号码
        string branchname = string.Empty;//开会行地址
        string province = string.Empty;//开会所在省
        string city = string.Empty;//开会所在市
        string remark = "手续费清算-" + ReceiptNo;//

        int paylogID = 0;//记录日志Id
        //结算接口日志表
        Hi.Model.PAY_PayLog paylogmodel = new Hi.Model.PAY_PayLog();
        Hi.BLL.PAY_PayLog paylogbll = new Hi.BLL.PAY_PayLog();
        //获取平台银行收款账户
        DataTable dt_bank = new Hi.BLL.PAY_PrePayment().GetDate(@"'001520' as  OrgCode,'' as payName,'' as PayCode,SYS_PaymentBank.type,
 SYS_PaymentBank.BankID,SYS_PaymentBank.AccountName,SYS_PaymentBank.bankcode
 ,SYS_PaymentBank.bankAddress,SYS_PaymentBank.bankprivate,SYS_PaymentBank.bankcity", "SYS_PaymentBank", "  SYS_PaymentBank.Isno=1  and SYS_PaymentBank.dr=0");
        //查询银行信息
        if (dt_bank.Rows.Count > 0)
        {

            foreach (DataRow drdis in dt_bank.Rows)
            {
                orgcode = WebConfigurationManager.AppSettings["PayOrgCode"];// Convert.ToString(drdis["OrgCode"]);//机构代码
                accountType = Convert.ToInt32(drdis["type"]);//帐号类型
                paymentaccountname = Convert.ToString(drdis["payName"]);//账户名称
                paymentaccountnumber = Convert.ToString(drdis["PayCode"]); //账户号码
                //收款方在银行开立的账户
                bankId = Convert.ToString(drdis["BankID"]); ;//银行ID
                accountname = Convert.ToString(drdis["AccountName"]); //账户名称
                accountnumber = Convert.ToString(drdis["bankcode"]);//账户号码
                branchname = Convert.ToString(drdis["bankAddress"]);//开会行地址
                province = Convert.ToString(drdis["bankprivate"]);//开会所在省
                city = Convert.ToString(drdis["bankcity"]);//开会所在市
            }


            //先插入日志表，
            paylogmodel.OrderId = ordID;
            paylogmodel.Ordercode = ReceiptNo;
            paylogmodel.number = guid + "_sxf";
            paylogmodel.CompID = compID;
            paylogmodel.OrgCode = orgcode;
            paylogmodel.MarkName = paymentaccountname;
            paylogmodel.MarkNumber = paymentaccountnumber;
            paylogmodel.AccountName = accountname;
            paylogmodel.bankcode = accountnumber;
            paylogmodel.bankAddress = branchname;
            paylogmodel.bankPrivate = province;
            paylogmodel.bankCity = city;
            paylogmodel.Price = price;
            paylogmodel.Remark = remark;
            paylogmodel.CreateDate = DateTime.Now;
            paylogmodel.CreateUser = 0;
            paylogID = paylogbll.Add(paylogmodel);


            if (paylogID > 0)//日志插入成功
            {

                //调用中金接口，做结算处理-------------------------------
                try
                {
                    string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                    PaymentEnvironment.Initialize(configPath);

                    // 2.创建交易请求对象
                    Tx1341Request tx1341Request = new Tx1341Request();
                    tx1341Request.setInstitutionID(orgcode);
                    tx1341Request.setSerialNumber(guid + "_sxf");
                    tx1341Request.setOrderNo(ReceiptNo);
                    tx1341Request.setAmount(price);
                    tx1341Request.setRemark(remark);
                    tx1341Request.setAccountType(accountType);
                    tx1341Request.setPaymentAccountName(paymentaccountname);
                    tx1341Request.setPaymentAccountNumber(paymentaccountnumber);

                    BankAccount bankAccount = new BankAccount();
                    bankAccount.setBankID(bankId);
                    bankAccount.setAccountName(accountname);
                    bankAccount.setAccountNumber(accountnumber);
                    bankAccount.setBranchName(branchname);
                    bankAccount.setProvince(province);
                    bankAccount.setCity(city);
                    tx1341Request.setBankAccount(bankAccount);

                    // 3.执行报文处理
                    tx1341Request.process();

                    //2个信息参数
                    HttpContext.Current.Items["txCode"] = "1341";
                    HttpContext.Current.Items["txName"] = "市场订单结算（结算）";

                    // 与支付平台进行通讯
                    TxMessenger txMessenger = new TxMessenger();
                    String[] respMsg = txMessenger.send(tx1341Request.getRequestMessage(), tx1341Request.getRequestSignature());// 0:message; 1:signature
                    String plaintext = XmlUtil.formatXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(respMsg[0])));
                    Console.WriteLine("[message] = [" + respMsg[0] + "]");
                    Console.WriteLine("[signature] = [" + respMsg[1] + "]");
                    Console.WriteLine("[plaintext] = [" + plaintext + "]");

                    Tx134xResponse tx134xResponse = new Tx134xResponse(respMsg[0], respMsg[1]);
                    HttpContext.Current.Items["plainText"] = tx134xResponse.getResponsePlainText();
                    string strs = tx134xResponse.getCode() + "," + tx134xResponse.getMessage();

                    //日志记录 接口返回的信息
                    paylogmodel.Start = tx134xResponse.getCode();
                    paylogmodel.ResultMessage = tx134xResponse.getMessage();
                    paylogmodel.ID = paylogID;
                    bool payLog_update = paylogbll.Update(paylogmodel);

                    if ("2000".Equals(tx134xResponse.getCode()))
                    {
                        //手续费收取成功，修改标记手续费清算状态 ,//手续费结算成功

                        int num = new Hi.BLL.PAY_PrePayment().Updatejsxf_no(ReceiptNo);
                        if (num > 0)
                            fal = true;
                    }
                    else
                    {
                        //清算出现问题，发送邮件通知
                        try
                        {
                            strs = "手续费结算失败:" + DateTime.Now.ToString() + "\r\n" + "错误流水号：" + ReceiptNo + "\r\n" + "企业ID:" + compID;
                            PayInfoType.SendFinsh(strs, ReceiptNo);
                        }
                        catch { }
                    }

                }
                catch (Exception ex)
                {
                    LogManager.WriteLog(LogFile.Trace.ToString(), "手续费结算接口出错:" + DateTime.Now.ToString() + "\r\n" + "订单编号：" + ReceiptNo);

                }
            }
            else
            {
                LogManager.WriteLog(LogFile.Trace.ToString(), "手续费结算日志表插入出错:" + DateTime.Now.ToString() + "\r\n" + "订单编号：" + ReceiptNo);

            }

        }
        else
        {
            //清算出现问题，发送邮件通知
            try
            {
                string mes = "<br>手续费结算失败,请维护收款帐号信息:" + DateTime.Now.ToString() + "<br>" + "错误流水号：" + ReceiptNo + "<br>" + "企业ID:" + compID;
               // PayInfoType.SendFinsh(mes, ReceiptNo);

            }
            catch { }
        }


        return fal;
    }



    //[WebMethod(Description = "AES解密")]
    //public void AESstring(string str)
    //{

    //    string mes = AESHelper.Decrypt_android(str);
    //    //本地日志记录
    //    LogManager.LogFielPrefix = "Order_time";
    //    LogManager.LogPath = "D:/TestAES/";

    //    LogManager.WriteLog(LogFile.Trace.ToString(), mes + "-解密成功...\r\n");

    //}

    //[WebMethod(Description = "AES加密")]
    //public void Jmstring(string str)
    //{

    //    string mes = AESHelper.Encrypt_android(str);
    //    //本地日志记录
    //    LogManager.LogFielPrefix = "Order_time";
    //    LogManager.LogPath = "D:/TestJM/";

    //    LogManager.WriteLog(LogFile.Trace.ToString(), mes + "-加密成功...\r\n");

    //}


    [WebMethod(Description = "快捷支付处理中情况处理")]
    public void Tx1372()
    {
        LogManager.LogFielPrefix = "tx1372";
        LogManager.LogPath = "D:/Tx1372Order_log/";
        LogManager.WriteLog(LogFile.Trace.ToString(), DateTime.Now + "进入Tx372程序\r");
        string LocalSqlServer = SqlHelper.LocalSqlServer;
        DataTable dt = new Hi.BLL.PAY_PrePayment().GetDate("guid,id,OrderID", "pay_payment", "  verifystatus=40 and status =10 and IsAudit =2 ");
        foreach (DataRow dr in dt.Rows)
        {
            string number = Convert.ToString(dr["guid"]);//支付交易流水号
            int paymentid = Convert.ToInt32(dr["id"]);//支付表id
            int Orderid = Convert.ToInt32(dr["OrderID"]);//支付表关联ID

            string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
            PaymentEnvironment.Initialize(configPath);
            string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码

            Tx1372Request tx1372 = new Tx1372Request();
            tx1372.setInstitutionID(institutionID);
            tx1372.setPaymentNo(number);
            tx1372.process();
            TxMessenger txMessenger = new TxMessenger();
            String[] respMsg = txMessenger.send(tx1372.getRequestMessage(), tx1372.getRequestSignature());
            Tx1372Response tx1372Response = new Tx1372Response(respMsg[0], respMsg[1]);
            //接口调用成功
            if ("2000".Equals(tx1372Response.getCode()))
            {
                LogManager.WriteLog(LogFile.Trace.ToString(), "流水号：" + number + "    状态:" + tx1372Response.getStatus() + "\r");
                //支付成功
                if (20 == tx1372Response.getStatus())
                {
                    //支付成功,修改状态
                    //企业钱包修改状态
                    int order = 0;
                    int pay = 0;
                    int prepay = 0;
                    SqlConnection con = new SqlConnection(LocalSqlServer);
                    con.Open();
                    SqlTransaction sqlTrans = con.BeginTransaction();
                    try
                    {
                        Hi.Model.PAY_Payment paymentmodel = new Hi.BLL.PAY_Payment().GetModel(paymentid);
                        Hi.Model.DIS_Order ordermodel = new Hi.BLL.DIS_Order().GetModel(Orderid);
                        Hi.Model.PAY_PrePayment prepaymentmodel = new Hi.BLL.PAY_PrePayment().GetModel(Orderid);
                        if (ordermodel != null)//订单支付
                        {
                            order = new Hi.BLL.DIS_Order().UpdateOrderPstate(con, Orderid, paymentmodel.PayPrice - Convert.ToDecimal(paymentmodel.vdef5 == "" ? "0" : paymentmodel.vdef5), sqlTrans);//修改订单状态
                            pay = new Hi.BLL.PAY_Payment().updatePayState(con, paymentid, sqlTrans);//修改支付表状态

                            if (order > 0 && pay > 0)
                                sqlTrans.Commit();
                            else
                                sqlTrans.Rollback();
                        }
                        else if (prepaymentmodel != null)//预付款充值
                        {
                            pay = new Hi.BLL.PAY_Payment().updatePayState(con, paymentid, sqlTrans);
                            prepay = new Hi.BLL.PAY_PrePayment().updatePrepayState(con, prepaymentmodel.ID, sqlTrans);
                            if (pay > 0 && prepay > 0)
                                sqlTrans.Commit();
                            else
                                sqlTrans.Rollback();
                        }
                    }
                    catch
                    {
                        order = 0;
                        pay = 0;
                        sqlTrans.Rollback();
                    }
                    finally
                    {
                        con.Close();
                    }
                }
                else if (30 == tx1372Response.getStatus())//支付失败
                {
                    Hi.Model.PAY_Payment payment = new Hi.BLL.PAY_Payment().GetModel(paymentid);
                    payment.status = 30;//支付失败
                    new Hi.BLL.PAY_Payment().Update(payment);

                    LogManager.WriteLog(LogFile.Trace.ToString(), "失败：" + tx1372Response.getStatus() + "\r");
                }

            }

        }
    }

    //[WebMethod(Description = "订单同步到ERP")]
    //public void SYJInsert(string TranType = "自动")
    //{
    //    if (string.IsNullOrWhiteSpace(TranType))
    //    {
    //        TranType = "自动";
    //    }
    //    new ImportDisProD().TransFerOrder(TranType);
    //}

}
