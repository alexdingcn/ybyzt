using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using CFCA.Payment.Api;
using System.Text;
using System.Web.Configuration;

public partial class Company_Pay_PayPre1341 : CompPageBase
{

    Hi.BLL.DIS_Order OrderInfoBLL = new Hi.BLL.DIS_Order();

    public string page = "1";//默认初始页
    public int Id = 0;  //订单Id

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();
        }
    }

    /// <summary>
    /// 绑定数据
    /// </summary>
    public void Bind()
    {
        //查询录入明细
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        //查询全部 转账汇款的记录
        strwhere += " and PreType=1";
        //付款状态是成功的
        strwhere += " and Start=1";
        //付款审核状态是已审的
        strwhere += " and AuditState=2";
        //是否已结算
        strwhere += " and (vdef2  is null or vdef2='') ";

        //所属企业
        strwhere += " and CompID=" + CompID;

        //int disId = Convert.ToInt32(Request.QueryString["keyId"]);
        //if (disId != 0)
        //{
        //    strwhere += " and DisID=" + disId + "";
        //}

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        if (this.txtPager.Value.Trim().ToString() != "")
        {
            Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
        }

        List<Hi.Model.PAY_PrePayment> pay = new Hi.BLL.PAY_PrePayment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreatDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = pay;
        this.rptOrder.DataBind();
    }

    /// <summary>
    /// 分页
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void rptOrder_ItemCommand(object source, RepeaterCommandEventArgs e)
    {

    }

    /// <summary>
    /// 搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = string.Empty;
        string receiptNO = string.Empty;
        decimal price_one = decimal.Zero;
        decimal price_two = decimal.Zero;

        receiptNO = this.txtReceiptNo.Value;
        price_one = Convert.ToDecimal(this.txtAmount1.Value);
        price_two = Convert.ToDecimal(this.txtAmount2.Value);

        if (receiptNO != "")
        {
            strWhere += "and ID like '%" + receiptNO.Trim() + "%'";
        }
        if (price_one > 0 && price_two > 0)
        {

            strWhere += "and Price between " + price_one + " and " + price_two;

        }
        else
        {
            if (price_one > 0)
            {
                strWhere += "and Price >=" + price_one;
            } if (price_two > 0)
            {
                strWhere += "and Price <=" + price_two;
            }
        }

        ViewState["strwhere"] = strWhere;
        Bind();
    }
    /// <summary>
    /// 获取选中的订单
    /// </summary>
    /// <returns></returns>
    public string CB_SelAll()
    {
        string strId = string.Empty;

        foreach (RepeaterItem item in rptOrder.Items)
        {
            CheckBox cb = item.FindControl("CB_SelItem") as CheckBox;
            if (cb != null && cb.Checked == true)
            {
                HiddenField fld = item.FindControl("Hd_Id") as HiddenField;

                if (fld != null)
                {
                    int id = Convert.ToInt32(fld.Value);
                    strId += id + ",";
                }
            }
        }
        if (strId != "")
        {
            strId = strId.Substring(0, strId.Length - 1);
        }
        return strId;
    }

    /// <summary>
    ///  结算
    /// </summary>
    /// <param name="source"></param>
    /// <param name="e"></param>
    protected void btnAudit_Click(object source, EventArgs e)
    {
        int compID = 0;
        int disID = 0;
        long price = 0;//金额
        int ordID = 0;//订单Id
        string ReceiptNo = string.Empty;//订单号
        string guid = string.Empty;//流水号
        string remark = string.Empty;//订单备注

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

        //结算接口日志表
        Hi.Model.PAY_PayLog paylogmodel = new Hi.Model.PAY_PayLog();
        Hi.BLL.PAY_PayLog paylogbll = new Hi.BLL.PAY_PayLog();
        int paylogID = 0;//接口日志返回ID

        string str = CB_SelAll();
        if (string.IsNullOrEmpty(str))
        {
            JScript.AlertMsgOne(this, "请选择要结算的记录！", JScript.IconOption.错误);
            return;
        }


        string[] strArry = str.Split(',');

        foreach (string s in strArry)
        {
            ordID = Convert.ToInt32(s);//订单编号

            DataTable dt_order = new Hi.BLL.PAY_PrePayment().GetdataTable_pre(1, " and PAY_PrePayment.ID =" + ordID);
            if (dt_order.Rows.Count <= 0)
            {
                JScript.AlertMsgOne(this, "支付数据中没有相关的记录,无法进行收款结算！", JScript.IconOption.错误);
                return;
            }
            foreach (DataRow dr in dt_order.Rows)
            {
                compID = Convert.ToInt32(dr["CompID"]);
                disID = Convert.ToInt32(dr["DisID"]);
                price = Convert.ToInt64(Convert.ToDecimal(dr["PayPrice"]) * 100);
                ReceiptNo = Convert.ToString(dr["ReceiptNo"]);
                guid = Convert.ToString(dr["GUID"]);
                remark = Convert.ToString(dr["Remark"]);

                //查找企业银行信息（绑定>默认）
                DataTable dt_bank_bydis = new Hi.BLL.PAY_PrePayment().GetdataTable_pre(2, " and PAY_PaymentAccountdtl.DisID=" + disID);//结算接口，银行信息--已代理商为核心
                DataTable dt_bank_comp = new Hi.BLL.PAY_PrePayment().GetdataTable_pre(3, " and PAY_PaymentBank.CompID=" + compID);//结算接口，银行信息--已企业为主，


                if (dt_bank_bydis.Rows.Count > 0)
                {
                    foreach (DataRow drdis in dt_bank_bydis.Rows)
                    {
                        orgcode = Convert.ToString(drdis["OrgCode"]);//机构代码
                        accountType = Convert.ToInt32(drdis["type"]);//帐号类型
                        paymentaccountname = Convert.ToString(drdis["payName"]);//账户名称
                        paymentaccountnumber = Convert.ToString(drdis["PayCode"]); //账户号码
                        //收款方在银行开立的账户
                        bankId = "700";// Convert.ToString(drdis["BankID"]); ;//银行ID
                        accountname = Convert.ToString(drdis["AccountName"]); //账户名称
                        accountnumber = Convert.ToString(drdis["bankcode"]);//账户号码
                        branchname = Convert.ToString(drdis["bankAddress"]);//开会行地址
                        province = Convert.ToString(drdis["bankprivate"]);//开会所在省
                        city = Convert.ToString(drdis["bankcity"]);//开会所在市
                    }
                }
                else if (dt_bank_comp.Rows.Count > 0)
                {
                    foreach (DataRow drcomp in dt_bank_comp.Rows)
                    {
                        orgcode = Convert.ToString(drcomp["OrgCode"]);//机构代码
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
                    JScript.AlertMsgOne(this, "无法进行收款结算,请在【结算账户管理】中维护收款帐号信息！", JScript.IconOption.错误);
                    return;
                }
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
                    paylogmodel.CreateUser = this.UserID;
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

                            if ("2000".Equals(tx134xResponse.getCode()))
                            {
                                //处理业务

                                //日志记录 接口返回的信息
                                paylogmodel.Start = tx134xResponse.getCode();
                                paylogmodel.ResultMessage = tx134xResponse.getMessage();
                                paylogmodel.ID = paylogID;
                                bool payLog_update = paylogbll.Update(paylogmodel);

                                //修改转账汇款状态
                                Hi.Model.PAY_PrePayment PrepaymentModel = new Hi.BLL.PAY_PrePayment().GetModel(ordID);
                                PrepaymentModel.vdef2 = "1";//修改结算标示为已结算
                                bool fal = new Hi.BLL.PAY_PrePayment().Update(PrepaymentModel);
                                if (fal)
                                {
                                    Utils.AddSysBusinessLog(CompID, "PrePayment", ordID.ToString(), "转账汇款结算", "");
                                    JScript.AlertMsgOne(this, "操作成功！", JScript.IconOption.笑脸);
                                    Bind();
                                }
                            }
                            else
                            {
                                Utils.AddSysBusinessLog(CompID, "PrePayment", ordID.ToString(), strs, "");
                                JScript.AlertMsgOne(this, strs + "!", JScript.IconOption.错误);

                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        Utils.AddSysBusinessLog(CompID, "PrePayment", ordID.ToString(), "系统繁忙，接口日志文件插入失败！", "");
                        JScript.AlertMsgOne(this, "系统繁忙，接口日志文件插入失败！", JScript.IconOption.错误);

                    }

                }

                //支付账户类型判断**************************************************************************************
                else if (accountType == 20 && paymentaccountname != "" && paymentaccountnumber != "")
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
                    paylogmodel.CreateUser = this.UserID;
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

                            if ("2000".Equals(tx134xResponse.getCode()))
                            {
                                //处理业务

                                //日志记录 接口返回的信息
                                paylogmodel.Start = tx134xResponse.getCode();
                                paylogmodel.ResultMessage = tx134xResponse.getMessage();
                                paylogmodel.ID = paylogID;
                                bool payLog_update = paylogbll.Update(paylogmodel);

                                //修改转账汇款状态
                                Hi.Model.PAY_PrePayment PrepaymentModel = new Hi.BLL.PAY_PrePayment().GetModel(ordID);
                                PrepaymentModel.vdef2 = "1";//修改结算标示为已结算
                                bool fal = new Hi.BLL.PAY_PrePayment().Update(PrepaymentModel);
                                if (fal)
                                {
                                    Utils.AddSysBusinessLog(CompID, "PrePayment", ordID.ToString(), "转账汇款结算", "");
                                    JScript.AlertMsgOne(this, "操作成功！", JScript.IconOption.笑脸);
                                    Bind();
                                }
                            }
                            else
                            {
                                Utils.AddSysBusinessLog(CompID, "PrePayment", ordID.ToString(), strs, "");
                                JScript.AlertMsgOne(this, strs + "!", JScript.IconOption.错误);
                            }

                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    else
                    {
                        Utils.AddSysBusinessLog(CompID, "PrePayment", ordID.ToString(), "系统繁忙，接口日志文件插入失败！", "");
                        JScript.AlertMsgOne(this, "系统繁忙，接口日志文件插入失败！", JScript.IconOption.错误);

                    }

                }
                else
                {

                    if (accountType == 11 || accountType == 12)
                        JScript.AlertMsgOne(this, "银行名称、持卡人名称、银行卡号不能为空！", JScript.IconOption.错误, 2500);
                    else
                        JScript.AlertMsgOne(this, "中金账户名称、中金账户号码不能为空！", JScript.IconOption.错误, 2500);
                }
            }
        }

    }

}