using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Pay_PayAccountList : CompPageBase
{
    
    Hi.BLL.PAY_PaymentBank PAbll = new Hi.BLL.PAY_PaymentBank();
    public string bankid = string.Empty; //银行卡收款帐号
    public string wx_aliId = string.Empty;//微信支付收款帐号
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            string action = Request["action"] + "";
            if (action.Equals("sett"))
            {
                btnSave_Click();
            }
            bind();
        }
    }
    /// <summary>
    /// 绑定信息
    /// </summary>
    public void bind()
    {

        List<Hi.Model.PAY_PaymentBank> paymentbanklist = new Hi.BLL.PAY_PaymentBank().GetList("", "  CompID=" + this.CompID, "");

        if (paymentbanklist.Count > 0)
        {
            Hi.Model.PAY_PaymentBank paymentbankmodel = paymentbanklist[0];
            bankid = paymentbankmodel.ID.ToString();

            this.txtDisUser.Value = paymentbankmodel.AccountName;//账户名称
            this.txtDisUser.Attributes.Add("class", "box noBox");
            this.txtDisUser.Attributes.Add("disabled", "true");

            this.txtbankcode.Value = paymentbankmodel.bankcode;//账户号码
            this.txtbankcode.Attributes.Add("class", "box noBox");
            this.txtbankcode.Attributes.Add("disabled", "true");

            this.txtbankAddress.Value = paymentbankmodel.bankAddress;//开户行地址
            this.txtbankAddress.Attributes.Add("class", "box noBox");
            this.txtbankAddress.Attributes.Add("disabled", "true");


            this.hidProvince.Value = paymentbankmodel.bankPrivate;//省
            this.ddlProvince.Attributes.Add("class", "prov select1 l xz2 noBox");
            this.ddlProvince.Attributes.Add("disabled", "true");

            this.hidCity.Value = paymentbankmodel.bankCity;//市
            this.ddlCity.Attributes.Add("class", "city select xz2 noBox");
            this.ddlCity.Attributes.Add("disabled", "true");

            this.hidArea.Value = paymentbankmodel.vdef1;//区
            this.ddlArea.Attributes.Add("class", "dist select  xz2 noBox");
            this.ddlArea.Attributes.Add("disabled", "true");

            this.chkIsno.Checked = paymentbankmodel.Isno == 1 ? true : false;//是否启用
            this.chkIsno.Attributes.Add("class", "fx noBox");
            this.chkIsno.Attributes.Add("disabled", "true");

            //如果在下拉列表中不存在的银行id，重新到银行表中查询加载
            ListItem list = ddlbank.Items.FindByValue(paymentbankmodel.BankID.ToString());
            if (list == null)
            {
                DataTable dt = new Hi.BLL.PAY_PrePayment().GetDate("BankCode ,BankName", "PAY_BankInfo", "BankCode=" + paymentbankmodel.BankID);
                if (dt.Rows.Count > 0)
                {
                    this.ddlbank.Items.Insert(0, new ListItem(Convert.ToString(dt.Rows[0]["BankName"]),Convert.ToString(dt.Rows[0]["BankCode"])));
                    this.ddlbank.SelectedIndex = 0;
                }
            }
            else
            {
                this.ddlbank.Value = paymentbankmodel.BankID.ToString();//银行Id
                
                 DataTable dt = new Hi.BLL.PAY_PrePayment().GetDate("BankCode ,BankName", "PAY_BankInfo", "BankCode=" + paymentbankmodel.BankID);
                if (dt.Rows.Count > 0)
                {
                    this.ddlbank.Name =Convert.ToString(dt.Rows[0]["BankName"]);
                }
             }

            this.ddlbank.Attributes.Add("class", "xz noBox");
            this.ddlbank.Attributes.Add("disabled", "true");



            this.ddltype.Value = paymentbankmodel.type.ToString();//账户类型
            this.ddltype.Attributes.Add("class", "xz noBox");
            this.ddltype.Attributes.Add("disabled", "true");
            if (paymentbankmodel.type == 12) 
                this.tbdis.Attributes.Add("style", "display:none;");

            this.SltPesontype.Value = paymentbankmodel.vdef2;//证件类型
            this.SltPesontype.Attributes.Add("class", "xz noBox");
            this.SltPesontype.Attributes.Add("disabled", "true");

            this.txtpesoncode.Value = paymentbankmodel.vdef3;//证件号码
            this.txtpesoncode.Attributes.Add("class", "box noBox");
            this.txtpesoncode.Attributes.Add("disabled", "true");

            
            //微信、支付宝不可编辑，需点击修改按钮
            Wx_ali_Disable();


            this.btnSave.Attributes.Add("style", "display:none;");
        }
        else
        {
            this.txtDisUser.Value ="";//账户名称
            this.txtbankcode.Value = "";//账户号码
            this.txtbankAddress.Value = "";//开户行地址
            this.hidProvince.Value = "";//省
            this.hidCity.Value = "";//市
            this.hidArea.Value = "";//区
            this.chkIsno.Checked = false;//是否启用
            this.ddlbank.Value = "";//银行Id
            this.ddltype.Value = "";//账户类型
            this.SltPesontype.Value = "";//证件类型
            this.txtpesoncode.Value = "";//证件号码

            this.btnUpdate.Attributes.Add("style", "display:none;");
            this.btnSave.Attributes.Add("style", "display:block;");

        }


        //查询该企业的设置
        List<Hi.Model.Pay_PayWxandAli> Sysl = new Hi.BLL.Pay_PayWxandAli().GetList("", " CompID=" + this.CompID, "");
        if (Sysl.Count > 0)
        {
            wx_aliId = Sysl[0].ID.ToString();
            //微信
            if (Sysl[0].wx_Isno == "1")
                this.wx_chisno.Checked = true;
            this.wx_chisno.Attributes.Add("class", "fx noBox");
            this.wx_chisno.Attributes.Add("disabled", "true");

            this.appid.Value = Convert.ToString(Sysl[0].wx_appid);
            this.appid.Attributes.Add("class", "box noBox");
            this.appid.Attributes.Add("disabled", "true");

            this.appsecret.Value = Convert.ToString(Sysl[0].wx_appsechet);
            this.appsecret.Attributes.Add("class", "box noBox");
            this.appsecret.Attributes.Add("disabled", "true");

            this.mchid.Value = Convert.ToString(Sysl[0].wx_mchid);
            this.mchid.Attributes.Add("class", "box noBox");
            this.mchid.Attributes.Add("disabled", "true");

            this.key.Value = Convert.ToString(Sysl[0].wx_key);
            this.key.Attributes.Add("class", "box noBox");
            this.key.Attributes.Add("disabled", "true");


            //支付宝
            if (Sysl[0].ali_isno == "1")
                this.ali_chisno.Checked = true;
            this.ali_chisno.Attributes.Add("class", "fx noBox");
            this.ali_chisno.Attributes.Add("disabled", "true");

            this.seller_email.Value = Convert.ToString(Sysl[0].ali_seller_email);
            this.seller_email.Attributes.Add("class", "box noBox");
            this.seller_email.Attributes.Add("disabled", "true");

            this.partner.Value = Convert.ToString(Sysl[0].ali_partner);
            this.partner.Attributes.Add("class", "box noBox");
            this.partner.Attributes.Add("disabled", "true");

            this.PayKey.Value = Convert.ToString(Sysl[0].ali_key);
            this.PayKey.Attributes.Add("class", "box noBox");
            this.PayKey.Attributes.Add("disabled", "true");
            this.alirsa.Value = Convert.ToString(Sysl[0].ali_RSAkey);
            this.alirsa.Attributes.Add("class", "box noBox");
            this.alirsa.Attributes.Add("disabled", "true");
            //收款帐号不可编辑，需点击修改按钮
            Paymentbank_disable();

            this.btnUpdate.Attributes.Add("style", "display:block;");

        }
        else //默认值显示
        {
            //微信
            this.appid.Value = "";
            this.appsecret.Value = "";
            this.mchid.Value = "";
            this.key.Value = "";
            //支付宝
            this.seller_email.Value = "";
            this.partner.Value = "";
            this.PayKey.Value = "";
        }

    }

    /// <summary>
    /// 保存方法操作
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click()
    {
        string bind = string.Empty;
        //声明变量
        string AccountName = string.Empty;
        string bankcode = string.Empty;
        string bankAddress = string.Empty;
        string bankPrivate = string.Empty;
        string bankCity = string.Empty;
        int Isno = 0;
        string county = string.Empty;//开户所在区县
        int BankId = 0;//银行Id
        int type = 0;//账户类型

        try
        {
            //收集数据
            AccountName =Common.NoHTML( Convert.ToString(Request["txtDisUser"]));//账户名称
            bankcode = Common.NoHTML(Convert.ToString(Request["txtbankcode"]));//账户号码
            bankAddress = Common.NoHTML(Convert.ToString(Request["txtbankAddress"]));//开户行地址
            bankPrivate = Common.NoHTML(Convert.ToString(Request["hidProvince"]));//省
            bankCity = Common.NoHTML(Convert.ToString(Request["hidCity"]));//市
            county = Common.NoHTML(Convert.ToString(Request["hidArea"]));//区
            Isno = Convert.ToInt32(Request["chkIsno"]);//是否启用
            BankId =Convert.ToInt32(Request["ddlbank"]);//银行Id
            type = Convert.ToInt32(Request["ddltype"]);//账户类型

            string SltPesontype = Convert.ToString(Request["SltPesontype"]);//证件类型
            string txtpesoncode = Common.NoHTML(Convert.ToString(Request["txtpesoncode"]));//证件号码

            //查询收款帐号设置
            List<Hi.Model.PAY_PaymentBank> pbModellist = new Hi.BLL.PAY_PaymentBank().GetList("", " CompID=" + this.CompID, "");

            if (pbModellist.Count > 0)
            {
                Hi.Model.PAY_PaymentBank pbModel =pbModellist[0];
                pbModel.BankID = BankId;
                pbModel.AccountName = AccountName;
                pbModel.bankcode = bankcode;
                pbModel.bankAddress = bankAddress;
                pbModel.bankPrivate = bankPrivate;
                pbModel.bankCity = bankCity;
                pbModel.Isno = Isno;
                pbModel.modifyuser = UserID;
                pbModel.ts = DateTime.Now;
                pbModel.vdef1 = county;
                pbModel.type = type;
                pbModel.vdef2 = SltPesontype;
                pbModel.vdef3 = txtpesoncode;

                bool result = new Hi.BLL.PAY_PaymentBank().Update(pbModel);
                if (result)
                {
                    string strmessage = string.Format("帐号类型：{0};户名：{1};开户行：{2};帐号：{3};开户所在省市：{4}/{5};开户地址：{6}", type, AccountName, BankId, bankcode, bankPrivate, bankCity, bankAddress);
                    Utils.AddSysBusinessLog(CompID, "paymentbank", pbModel.ID.ToString(), "收款账户修改", strmessage, this.UserID.ToString());
                     
                }
            }
            else
            {

                //实例化对象实体
                Hi.Model.PAY_PaymentBank pbModel = new Hi.Model.PAY_PaymentBank();
                pbModel.paymentAccountID = 0;
                pbModel.BankID = BankId;
                pbModel.AccountName = AccountName;
                pbModel.bankcode = bankcode;
                pbModel.bankAddress = bankAddress;
                pbModel.bankPrivate = bankPrivate;
                pbModel.bankCity = bankCity;
                pbModel.Isno = Isno;
                pbModel.CreateDate = DateTime.Now;
                pbModel.CreateUser = UserID;
                pbModel.dr = 0;
                pbModel.modifyuser = UserID;
                pbModel.ts = DateTime.Now;
                pbModel.vdef1 = county;
                pbModel.Start = 1;
                pbModel.CompID = CompID;
                pbModel.type = type;
                pbModel.vdef2 = SltPesontype;
                pbModel.vdef3 = txtpesoncode;

                 int succes = new Hi.BLL.PAY_PaymentBank().Add(pbModel);
                if(succes>0){
                     string strmessage = string.Format("帐号类型：{0};户名：{1};开户行：{2};帐号：{3};开户所在省市：{4}/{5};开户地址：{6}", type, AccountName, BankId, bankcode, bankPrivate,bankCity,bankAddress);             
                     Utils.AddSysBusinessLog(CompID, "paymentbank", succes.ToString(), "收款账户新增",strmessage, this.UserID.ToString());
                     
                }
            }

            #region  微信、支付宝 -支付

            //微信
            string wx_chkisno =Common.NoHTML( Convert.ToString(Request["wx_chisno"]));
            string appid = Common.NoHTML(Convert.ToString(Request["appid"]));
            string appsecret = Common.NoHTML(Convert.ToString(Request["appsecret"]));
            string mchid = Common.NoHTML(Convert.ToString(Request["mchid"]));
            string key = Common.NoHTML(Convert.ToString(Request["key"]));

            //支付宝
            string ali_chkisno = Common.NoHTML(Convert.ToString(Request["ali_chkisno"]));
            string seller_email = Common.NoHTML(Convert.ToString(Request["seller_email"]));
            string partner = Common.NoHTML(Convert.ToString(Request["partner"]));
            string PayKey = Common.NoHTML(Convert.ToString(Request["PayKey"]));
            string alirsa = Common.NoHTML(Convert.ToString(Request["alirsa"]));

            bool fal = false;

            //查询该企业的设置
            List<Hi.Model.Pay_PayWxandAli> Sysl = new Hi.BLL.Pay_PayWxandAli().GetList("", " CompID=" + this.CompID, "");

            //判断企业的是否有设置
            if (Sysl.Count > 0)
            {
                Hi.Model.Pay_PayWxandAli paywxandaliModel = Sysl[0];
                //微信
                paywxandaliModel.wx_Isno = wx_chkisno;
                paywxandaliModel.wx_appid = appid;
                paywxandaliModel.wx_appsechet = appsecret;
                paywxandaliModel.wx_mchid = mchid;
                paywxandaliModel.wx_key = key;
                //支付宝
                paywxandaliModel.ali_isno = ali_chkisno;
                paywxandaliModel.ali_seller_email = seller_email;
                paywxandaliModel.ali_partner = partner;
                paywxandaliModel.ali_key = PayKey;
                paywxandaliModel.ali_RSAkey = alirsa;

                fal = new Hi.BLL.Pay_PayWxandAli().Update(paywxandaliModel);
                if(fal)
                {
                    if (wx_chkisno.Equals("1"))
                    {
                        string strmessage = string.Format("ApppID(应用ID)：{0};AppSecrect(应用秘钥)：{1};Mchid(商户号)：{2};APPKey(API秘钥)：{3};是否启用：{4}", appid, appsecret, mchid, key, wx_chkisno);
                        Utils.AddSysBusinessLog(CompID, "paymentbank", paywxandaliModel.ID.ToString(), "微信收款账户修改", strmessage, this.UserID.ToString());
                    }
                    if (ali_chkisno.Equals("1"))
                    {
                        string strmessage = string.Format("支付宝企业账户：{0};合作者身(Partner ID)：{1};安全校验码（Key）：{2};RSA加密(RSA秘钥)：{3};是否启用：{4}", seller_email, partner, PayKey, alirsa, ali_chkisno);
                        Utils.AddSysBusinessLog(CompID, "paymentbank", paywxandaliModel.ID.ToString(), "支付宝收款账户修改", strmessage, this.UserID.ToString());
          
                    }

                }

            }
            else
            {
                Hi.Model.Pay_PayWxandAli paywxandaliModel = new Hi.Model.Pay_PayWxandAli();
                //微信
                paywxandaliModel.wx_Isno = wx_chkisno;
                paywxandaliModel.wx_appid = appid;
                paywxandaliModel.wx_appsechet = appsecret;
                paywxandaliModel.wx_mchid = mchid;
                paywxandaliModel.wx_key = key;

                //支付宝
                paywxandaliModel.ali_isno = ali_chkisno;
                paywxandaliModel.ali_seller_email = seller_email;
                paywxandaliModel.ali_partner = partner;
                paywxandaliModel.ali_key = PayKey;
                paywxandaliModel.ali_RSAkey = alirsa;

                paywxandaliModel.CompID = this.CompID;
                int num = new Hi.BLL.Pay_PayWxandAli().Add(paywxandaliModel);
                if (num > 0)
                {
                    if (wx_chkisno.Equals("1"))
                    {
                        string strmessage = string.Format("ApppID(应用ID)：{0};AppSecrect(应用秘钥)：{1};Mchid(商户号)：{2};APPKey(API秘钥)：{3};是否启用：{4}", appid, appsecret, mchid, key, wx_chkisno);
                        Utils.AddSysBusinessLog(CompID, "paymentbank", paywxandaliModel.ID.ToString(), "微信收款账户新增", strmessage, this.UserID.ToString());
                    }
                    if (ali_chkisno.Equals("1"))
                    {
                        string strmessage = string.Format("支付宝企业账户：{0};合作者身(Partner ID)：{1};安全校验码（Key）：{2};RSA加密(RSA秘钥)：{3};是否启用：{4}", seller_email, partner, PayKey, alirsa, ali_chkisno);
                        Utils.AddSysBusinessLog(CompID, "paymentbank", paywxandaliModel.ID.ToString(), "支付宝收款账户新增", strmessage, this.UserID.ToString());

                    }
                }
            }

            #endregion

            bind = "{\"ds\":\"1\",\"prompt\":\"提交成功！\"}";
        }
        catch (Exception ex)
        {
            bind = "{\"ds\":\"1\",\"prompt\":\"提交失败！\"}";
        }
        finally
        {
            Response.Write(bind);
            Response.End();
        }
    }




    /// <summary>
    /// 选择银行返回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSelectBankReturn_ServerClick(object sender, EventArgs e)
    {

        string bankname = this.txtbandname.Value;
        string bankcode = this.txtbankcodes.Value;

        ListItem list = ddlbank.Items.FindByValue(bankcode);
        if (list == null)
        {
            this.ddlbank.Items.Insert(0, new ListItem(bankname, bankcode));
            this.ddlbank.SelectedIndex = 0;
        }
        else
        {
            int index = ddlbank.Items.IndexOf(list);
            this.ddlbank.SelectedIndex = index;
        }

        //判断证件类型层的显示
        if (this.ddltype.Value == "12")
            this.tbdis.Visible = false;
        else
            this.tbdis.Visible = true;
    }

    /// <summary>
    /// 修改按钮
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnUpdate_ServerClick(object sender, EventArgs e) 
    {
        this.btnSave.Attributes.Add("style", "display:block;");
       
    }

    /// <summary>
    /// 支付宝、微信编辑显示
    /// </summary>
     public  void Wx_ali_Disable()
    {
        this.wx_chisno.Attributes.Add("class", "fx noBox");
        this.wx_chisno.Attributes.Add("disabled", "true");

        this.appid.Attributes.Add("class", "box noBox");
        this.appid.Attributes.Add("disabled", "true");

        this.appsecret.Attributes.Add("class", "box noBox");
        this.appsecret.Attributes.Add("disabled", "true");

        this.mchid.Attributes.Add("class", "box noBox");
        this.mchid.Attributes.Add("disabled", "true");

        this.key.Attributes.Add("class", "box noBox");
        this.key.Attributes.Add("disabled", "true");


        //支付宝
        this.ali_chisno.Attributes.Add("class", "fx noBox");
        this.ali_chisno.Attributes.Add("disabled", "true");

        this.seller_email.Attributes.Add("class", "box noBox");
        this.seller_email.Attributes.Add("disabled", "true");

        this.partner.Attributes.Add("class", "box noBox");
        this.partner.Attributes.Add("disabled", "true");

        this.PayKey.Attributes.Add("class", "box noBox");
        this.PayKey.Attributes.Add("disabled", "true");

        this.alirsa.Attributes.Add("class", "box noBox");
        this.alirsa.Attributes.Add("disabled", "true");
    }

    /// <summary>
    /// 收款帐号不可编辑
    /// </summary>
     public void Paymentbank_disable() 
     {
         this.txtDisUser.Attributes.Add("class", "box noBox");
         this.txtDisUser.Attributes.Add("disabled", "true");

         //账户号码
         this.txtbankcode.Attributes.Add("class", "box noBox");
         this.txtbankcode.Attributes.Add("disabled", "true");

         //开户行地址
         this.txtbankAddress.Attributes.Add("class", "box noBox");
         this.txtbankAddress.Attributes.Add("disabled", "true");


        //省
         this.ddlProvince.Attributes.Add("class", "prov select1 l xz2 noBox");
         this.ddlProvince.Attributes.Add("disabled", "true");

         //市
         this.ddlCity.Attributes.Add("class", "city select xz2 noBox");
         this.ddlCity.Attributes.Add("disabled", "true");

         //区
         this.ddlArea.Attributes.Add("class", "dist select  xz2 noBox");
         this.ddlArea.Attributes.Add("disabled", "true");

         //是否启用
         this.chkIsno.Attributes.Add("class", "fx noBox");
         this.chkIsno.Attributes.Add("disabled", "true");


         //银行Id
         this.ddlbank.Attributes.Add("class", "xz noBox");
         this.ddlbank.Attributes.Add("disabled", "true");



         //账户类型
         this.ddltype.Attributes.Add("class", "xz noBox");
         this.ddltype.Attributes.Add("disabled", "true");

         //证件类型
         this.SltPesontype.Attributes.Add("class", "xz noBox");
         this.SltPesontype.Attributes.Add("disabled", "true");

         //证件号码
         this.txtpesoncode.Attributes.Add("class", "box noBox");
         this.txtpesoncode.Attributes.Add("disabled", "true");
     }

}