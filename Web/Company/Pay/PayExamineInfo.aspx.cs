using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Web.Configuration;
using CFCA.Payment.Api;
using System.Text;

public partial class Company_Pay_PayExamineInfo : CompPageBase
{

    Hi.BLL.PAY_PrePayment PAbll = new Hi.BLL.PAY_PrePayment();
    public static bool Auditstatr = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    public void Bind()
    {
        if (KeyID > 0)
        {

            Hi.Model.PAY_PrePayment Ppmodel = PAbll.GetModel(KeyID);
            this.lbldis.InnerText = Common.GetDis(Ppmodel.DisID, "DisName");
            this.lblcreatetime.InnerText = Convert.ToDateTime(Ppmodel.CreatDate).ToString("yyyy-MM-dd");
            this.lblauditstate.InnerText = Common.GetNameBYPreStart(Ppmodel.AuditState);
            this.lblcreateuser.InnerText = Common.GetUserName(Ppmodel.CrateUser);
            this.lblprice.InnerText = Convert.ToDecimal(Ppmodel.price).ToString("0.00");
            this.lblpaytype.InnerText = Common.GetPrePayStartName(Ppmodel.PreType);
            this.lblRemark.InnerText = Ppmodel.vdef1;
            this.Audit.Visible = Ppmodel.AuditState == 2 ? false : true;

        }


    }


    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAudit_Click(object sender, EventArgs e)
    {
        #region  接口字段整理

        int ordID = 0;//订单Id
        string ReceiptNo = string.Empty;//订单号
        string guid = string.Empty;//流水号
        string remark = string.Empty;//订单备注

        //-----银行信息
        string orgcode = string.Empty;//机构代码
        string paymentaccountname = string.Empty;//账户名称
        string paymentaccountnumber = string.Empty;//账户号码
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
        #endregion




        Hi.Model.PAY_PrePayment PAmodel = this.PAbll.GetModel(KeyID);




        if (PAmodel != null)
        {
            if (PAmodel.AuditState != Convert.ToInt32(Enums.PrePayState.已审) && (Convert.ToInt32(Enums.PrePayType.企业钱包冲正) == PAmodel.PreType || Convert.ToInt32(Enums.PrePayType.企业钱包补录) == PAmodel.PreType))
            {
                PAmodel.AuditState = 2;
                PAmodel.IsEnabled = 1;
                PAmodel.ID = KeyID;
                if (PAbll.Update(PAmodel))
                {
                    #region//修改代理商的企业钱包金额---作废
                    ////sum代理商全部补录，企业钱包冲正金额
                    //decimal sums = new Hi.BLL.PAY_PrePayment().sums(PAmodel.DisID, PAmodel.CompID);


                    ////调用model,对属性进行赋值
                    //Hi.Model.BD_Distributor dismodel = new Hi.BLL.BD_Distributor().GetModel(PAmodel.DisID);
                    //dismodel.DisAccount = sums;
                    //dismodel.ID = PAmodel.DisID;
                    ////调用修改方法    
                    //Hi.BLL.BD_Distributor disupdate = new Hi.BLL.BD_Distributor();
                    //bool disup = disupdate.Update(dismodel);
                    #endregion

                    //if (disup)
                    //{
                    if (Convert.ToInt32(Enums.PrePayType.企业钱包冲正) == PAmodel.PreType)
                        //系统日志记录
                        Utils.AddSysBusinessLog(this.CompID, "PrePayment", KeyID.ToString(), "企业钱包冲正审核", PAmodel.vdef1);
                    else
                        //系统日志记录
                        Utils.AddSysBusinessLog(this.CompID, "PrePayment", KeyID.ToString(), "企业钱包补录审核", PAmodel.vdef1);

                    JScript.AlertMsgOne(this, "操作成功！", JScript.IconOption.笑脸);
                    Bind();
                    // }


                }
            }
            #region  结算作废代码----
            //if (PAmodel.AuditState != Convert.ToInt32(Enums.PrePayState.已审) && Convert.ToInt32(Enums.PrePayType.转账汇款) == PAmodel.PreType)
            //{
            //    ordID = KeyID;//订单编号

            //    DataTable dt_order = new Hi.BLL.PAY_PrePayment().GetdataTable_pre(1, " and PAY_PrePayment.ID =" + ordID);

            //    if (dt_order.Rows.Count <= 0)
            //    {
            //        JScript.ShowAlert(this, "支付表中没有相关的记录,无法进行审批结算！");
            //        return;
            //    }

            //    foreach (DataRow dr in dt_order.Rows)
            //    {
            //        compID = Convert.ToInt32(dr["CompID"]);
            //        disID = Convert.ToInt32(dr["DisID"]);
            //        price = Convert.ToInt64(Convert.ToDecimal(dr["PayPrice"]) * 100);
            //        ReceiptNo = Convert.ToString(dr["ReceiptNo"]);
            //        guid = Convert.ToString(dr["GUID"]);
            //        remark = Convert.ToString(dr["Remark"]);

            //        //查找企业银行信息（绑定>默认）
            //        DataTable dt_bank_bydis = new Hi.BLL.PAY_PrePayment().GetdataTable(2, " and PAY_PaymentAccountdtl.DisID=" + disID, 0);//结算接口，银行信息--已代理商为核心
            //        DataTable dt_bank_comp = new Hi.BLL.PAY_PrePayment().GetdataTable(3, " and PAY_PaymentAccount.CompID=" + compID, 0);//结算接口，银行信息--已企业为主，


            //        if (dt_bank_bydis.Rows.Count > 0)
            //        {
            //            foreach (DataRow drdis in dt_bank_bydis.Rows)
            //            {
            //                orgcode = Convert.ToString(drdis["OrgCode"]);//机构代码
            //                accountType = Convert.ToInt32(drdis["type"]);//帐号类型
            //                paymentaccountname = Convert.ToString(drdis["payName"]);//账户名称
            //                paymentaccountnumber = Convert.ToString(drdis["PayCode"]); //账户号码
            //                //收款方在银行开立的账户
            //                bankId =  Convert.ToString(drdis["BankID"]); ;//银行ID
            //                accountname = Convert.ToString(drdis["AccountName"]); //账户名称
            //                accountnumber = Convert.ToString(drdis["bankcode"]);//账户号码
            //                branchname = Convert.ToString(drdis["bankAddress"]);//开会行地址
            //                province = Convert.ToString(drdis["bankprivate"]);//开会所在省
            //                city = Convert.ToString(drdis["bankcity"]);//开会所在市
            //            }
            //        }
            //        else if (dt_bank_comp.Rows.Count > 0)
            //        {
            //            foreach (DataRow drcomp in dt_bank_comp.Rows)
            //            {
            //                orgcode = Convert.ToString(drcomp["OrgCode"]);//机构代码
            //                accountType = Convert.ToInt32(drcomp["type"]);//帐号类型
            //                paymentaccountname = Convert.ToString(drcomp["payName"]);//账户名称
            //                paymentaccountnumber = Convert.ToString(drcomp["PayCode"]); //账户号码
            //                //收款方在银行开立的账户
            //                bankId = Convert.ToString(drcomp["BankID"]); ;//银行ID
            //                accountname = Convert.ToString(drcomp["AccountName"]); //账户名称
            //                accountnumber = Convert.ToString(drcomp["bankcode"]);//账户号码
            //                branchname = Convert.ToString(drcomp["bankAddress"]);//开会行地址
            //                province = Convert.ToString(drcomp["bankprivate"]);//开会所在省
            //                city = Convert.ToString(drcomp["bankcity"]);//开会所在市
            //            }
            //        }

            //        //判断参数收款银行是否维护
            //        if (accountType == 0)
            //        {
            //            JScript.ShowAlert(this, "无法进行收款结算,请在【结算账户管理】中维护收款帐号信息！");
            //            return;
            //        }
            //        else if ((accountType == 11 || accountType == 12) && bankId != "" && accountname != "" && accountnumber != "")
            //        {

            //            //先插入日志表，
            //            paylogmodel.OrderId = ordID;
            //            paylogmodel.Ordercode = ReceiptNo;
            //            paylogmodel.number = guid;
            //            paylogmodel.CompID = compID;
            //            paylogmodel.OrgCode = orgcode;
            //            paylogmodel.MarkName = paymentaccountname;
            //            paylogmodel.MarkNumber = paymentaccountnumber;
            //            paylogmodel.AccountName = accountname;
            //            paylogmodel.bankcode = accountnumber;
            //            paylogmodel.bankAddress = branchname;
            //            paylogmodel.bankPrivate = province;
            //            paylogmodel.bankCity = city;
            //            paylogmodel.Price = price;
            //            paylogmodel.Remark = remark;
            //            paylogmodel.CreateDate = DateTime.Now;
            //            paylogmodel.CreateUser = this.UserID;
            //            paylogID = paylogbll.Add(paylogmodel);


            //            if (paylogID > 0)//日志插入成功
            //            {

            //                //调用中金接口，做结算处理-------------------------------
            //                try
            //                {
            //                    string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
            //                    PaymentEnvironment.Initialize(configPath);

            //                    // 2.创建交易请求对象
            //                    Tx1341Request tx1341Request = new Tx1341Request();
            //                    tx1341Request.setInstitutionID(orgcode);
            //                    tx1341Request.setSerialNumber(guid);
            //                    tx1341Request.setOrderNo(ReceiptNo);
            //                    tx1341Request.setAmount(price);
            //                    tx1341Request.setRemark(remark);
            //                    tx1341Request.setAccountType(accountType);
            //                    tx1341Request.setPaymentAccountName(paymentaccountname);
            //                    tx1341Request.setPaymentAccountNumber(paymentaccountnumber);

            //                    BankAccount bankAccount = new BankAccount();
            //                    bankAccount.setBankID(bankId);
            //                    bankAccount.setAccountName(accountname);
            //                    bankAccount.setAccountNumber(accountnumber);
            //                    bankAccount.setBranchName(branchname);
            //                    bankAccount.setProvince(province);
            //                    bankAccount.setCity(city);
            //                    tx1341Request.setBankAccount(bankAccount);

            //                    // 3.执行报文处理
            //                    tx1341Request.process();

            //                    //2个信息参数
            //                    HttpContext.Current.Items["txCode"] = "1341";
            //                    HttpContext.Current.Items["txName"] = "市场订单结算（结算）";

            //                    // 与支付平台进行通讯
            //                    TxMessenger txMessenger = new TxMessenger();
            //                    String[] respMsg = txMessenger.send(tx1341Request.getRequestMessage(), tx1341Request.getRequestSignature());// 0:message; 1:signature
            //                    String plaintext = XmlUtil.formatXmlString(Encoding.UTF8.GetString(Convert.FromBase64String(respMsg[0])));
            //                    //string plantext= tx1341Request.getRequestPlainText;
            //                    Console.WriteLine("[message] = [" + respMsg[0] + "]");
            //                    Console.WriteLine("[signature] = [" + respMsg[1] + "]");
            //                    Console.WriteLine("[plaintext] = [" + plaintext + "]");

            //                    Tx134xResponse tx134xResponse = new Tx134xResponse(respMsg[0], respMsg[1]);
            //                    HttpContext.Current.Items["plainText"] = tx134xResponse.getResponsePlainText();
            //                    string strs = tx134xResponse.getCode() + "," + tx134xResponse.getMessage();
            //                    //消息提示
            //                   // JScript.ShowAlert(this, strs);

            //                    if ("2000".Equals(tx134xResponse.getCode()))
            //                    {
            //                        //修改转账汇款状态
            //                        Hi.Model.PAY_PrePayment PrepaymentModel = new Hi.BLL.PAY_PrePayment().GetModel(ordID);
            //                        PrepaymentModel.AuditState = 2;//修改审批状态
            //                        PrepaymentModel.vdef2 ="1"; //支付状态改为已结算
            //                        bool fal = new Hi.BLL.PAY_PrePayment().Update(PrepaymentModel);
            //                        if (fal)
            //                        {
            //                            //系统日志记录
            //                            Utils.AddSysBusinessLog(this.CompID, "PrePayment", ordID.ToString(), "转账汇款审核", PAmodel.vdef1);

            //                            JScript.ShowAlert(this, "操作成功！");
            //                        }

            //                        //日志记录 接口返回的信息
            //                        paylogmodel.Start = tx134xResponse.getCode();
            //                        paylogmodel.ResultMessage = tx134xResponse.getMessage();
            //                        paylogmodel.ID = paylogID;
            //                        bool payLog_update = paylogbll.Update(paylogmodel);
            //                        if (payLog_update)
            //                            Utils.AddSysBusinessLog(this.CompID, "PrePayment", ordID.ToString(), "转账汇款结算", PAmodel.vdef1);

            //                        #region//修改代理商的企业钱包金额--作废

            //                        ////sum代理商全部补录，企业钱包冲正金额
            //                        //decimal sums = new Hi.BLL.PAY_PrePayment().sums(PAmodel.DisID, PAmodel.CompID);
            //                        ////调用model,对属性进行赋值
            //                        //Hi.Model.BD_Distributor dismodel = new Hi.BLL.BD_Distributor().GetModel(PAmodel.DisID);
            //                        //dismodel.DisAccount = sums;
            //                        //dismodel.ID = PAmodel.DisID;
            //                        ////调用修改方法    
            //                        //Hi.BLL.BD_Distributor disupdate = new Hi.BLL.BD_Distributor();
            //                        //bool disup = disupdate.Update(dismodel);

            //                        #endregion
            //                        Bind();
            //                    }


            //                    else
            //                    {
            //                        Utils.AddSysBusinessLog(this.CompID, "PrePayment", ordID.ToString(),strs , PAmodel.vdef1);

            //                        JScript.ShowAlert(this, "系统繁忙，请稍后再试！");
            //                    }

            //                }
            //                catch (Exception ex)
            //                {
            //                    throw ex;
            //                }
            //            }

            //        }
            //    }
            //}
            #endregion
            //else
            //{
            //    Utils.AddSysBusinessLog(this.CompID, "PrePayment", ordID.ToString(), "数据状态不正确,不能进行审核!", PAmodel.vdef1);

            //    JScript.ShowAlert(this, "数据状态不正确,不能进行审核!");
            //}
        }
        else
        {
            Utils.AddSysBusinessLog(this.CompID, "PrePayment", ordID.ToString(), "数据不存在!", PAmodel.vdef1);

            JScript.AlertMsgOne(this, "数据不存在！", JScript.IconOption.错误);
        }

    }


    /// <summary>
    /// 退回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnReturn_Click(object sender, EventArgs e)
    {
        //JScript.ShowAlert(this, "数据不存在!");

    }


}