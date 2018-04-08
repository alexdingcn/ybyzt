using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Configuration;
using CFCA.Payment.Api;
using System.Configuration;

public partial class Distributor_Pay_PayQuickly :DisPageBase
{
    public string username = "haiyu";
    public int KeyID = 0;
    public int SumNumber = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            KeyID = Convert.ToInt32(Request.QueryString["KeyID"]);
            Bind();
        }
    }

    /// <summary>
    /// 绑定事件
    /// </summary>
    public void Bind()
    {
        Hi.Model.DIS_Order orderModel = new Hi.BLL.DIS_Order().GetModel(KeyID);

        this.hidUserName.Value = this.UserName;
        string strWhere = string.Empty;
        if (this.DisID != 0)
        {
            strWhere += " DisID = '" + this.DisID + "' ";
        }
        else
        {
            JScript.AlertMsgOne(this, "操作员没有对应的代理商！", JScript.IconOption.错误, 2500);
        }
        strWhere += " and Start = 1 and vdef6='0'  and isnull(dr,0)=0";
        List<Hi.Model.PAY_FastPayMent> fastList = new Hi.BLL.PAY_FastPayMent().GetList("", strWhere, "");
        this.rptQpay.DataSource = fastList;
        this.rptQpay.DataBind();
        SumNumber = fastList.ToArray().Length;
        List<Hi.Model.PAY_BankInfo> BankL = new Hi.BLL.PAY_BankInfo().GetList("", " vdef1=0", "");
        this.rptOtherBank.DataSource = BankL;
        this.rptOtherBank.DataBind();
    }

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnClose_Click(object sender, EventArgs e)
    {
        int restID = 0;//接口日志表生成Id
        int BankID = Convert.ToInt32(this.hidFastBankid.Value);

        bool fal = false;

        string TxSNBinding = string.Empty;//原绑定流水号
        string TxSNUnBinding = string.Empty;// 解绑流水号

        try
        {

            //老的绑定记录
            Hi.Model.PAY_FastPayMent fastpayModel_old = new Hi.BLL.PAY_FastPayMent().GetModel(BankID);

            //调用接口前，先生成一条解绑，信息在绑定表中
            Hi.Model.PAY_FastPayMent fastpayModel_new = new Hi.Model.PAY_FastPayMent();

            TxSNUnBinding = ConfigurationManager.AppSettings["OrgCode"].ToString().Trim() +Common.Number_repeat("");//解绑流水号
            TxSNBinding = ConfigurationManager.AppSettings["OrgCode"].ToString().Trim() + fastpayModel_old.ID.ToString();//原绑定流水号

            //收集信息
            fastpayModel_new.DisID = fastpayModel_old.DisID;
            fastpayModel_new.BankID = fastpayModel_old.BankID;
            fastpayModel_new.Number = fastpayModel_old.Number;
            fastpayModel_new.AccountName = fastpayModel_old.AccountName;
            fastpayModel_new.bankcode = fastpayModel_old.bankcode;
            fastpayModel_new.bankName = fastpayModel_old.bankName;
            fastpayModel_new.IdentityCode = fastpayModel_old.IdentityCode;
            fastpayModel_new.phone = fastpayModel_old.phone;
            fastpayModel_new.BankLogo = fastpayModel_old.BankLogo;
            fastpayModel_new.Start = 2;
            fastpayModel_new.CreateUser = fastpayModel_old.CreateUser;
            fastpayModel_new.CreateDate = DateTime.Now;
            fastpayModel_new.ts = DateTime.Now;
            fastpayModel_new.modifyuser = this.UserID;//获取当前登录ID
            fastpayModel_new.vdef5 = TxSNUnBinding;//解绑流水号
            restID = new Hi.BLL.PAY_FastPayMent().Add(fastpayModel_new);

            string strs = string.Empty;
            string resetcode="2000";
            int succes = 20;
         //是否屏蔽中金支付接口0是不屏蔽，1，屏蔽
            if (ConfigurationManager.AppSettings["Paytest_zj"] == "0")
            {
                //调用接口
                string configPath = WebConfigurationManager.AppSettings["payment.config.path"];
                PaymentEnvironment.Initialize(configPath);

                string institutionID = WebConfigurationManager.AppSettings["PayOrgCode"];//机构号码

                Tx2503Request tx2503Request = new Tx2503Request();
                tx2503Request.setInstitutionID(institutionID);
                tx2503Request.setTxSNUnBinding(TxSNUnBinding);//解绑流水号
                tx2503Request.setTxSNBinding(TxSNBinding);//原绑定流水号
                tx2503Request.process();

                TxMessenger txMessenger = new TxMessenger();
                String[] respMsg = txMessenger.send(tx2503Request.getRequestMessage(), tx2503Request.getRequestSignature());

                Tx2503Response tx2503Response = new Tx2503Response(respMsg[0], respMsg[1]);

                strs = tx2503Response.getMessage();// tx2503Response.getCode() + "," + tx2503Response.getMessage();
                resetcode = tx2503Response.getCode();
                succes = Convert.ToInt32(tx2503Response.getStatus());
            }   
                //消息提示
            //JScript.ShowAlert(this, strs);
            if ("2000".Equals(resetcode))
            {

               
                if (succes == 20)
                {
                    //修改原始记录的状态为解绑
                    fastpayModel_old.vdef6 = "1";
                    bool fal_old = new Hi.BLL.PAY_FastPayMent().Update(fastpayModel_old);

                    //修改，回填快捷支付表中的记录状态信息。
                    Hi.Model.PAY_FastPayMent fastpayModel = new Hi.BLL.PAY_FastPayMent().GetModel(restID);
                    fastpayModel.vdef6 = "1";
                    fastpayModel.ID = restID;
                    fal = new Hi.BLL.PAY_FastPayMent().Update(fastpayModel);

                }
                else 
                {
                    fal = false;
                }
            }
            else if ("30250311".Equals(resetcode))
            {
                //修改原始记录的状态为解绑
                fastpayModel_old.vdef6 = "1";
                bool fal_old = new Hi.BLL.PAY_FastPayMent().Update(fastpayModel_old);

                //修改，回填快捷支付表中的记录状态信息。
                Hi.Model.PAY_FastPayMent fastpayModel = new Hi.BLL.PAY_FastPayMent().GetModel(restID);
                fastpayModel.vdef6 = "1";
                fastpayModel.ID = restID;
                fal = new Hi.BLL.PAY_FastPayMent().Update(fastpayModel);
            }
            else 
            {
                JScript.AlertMsgOne(this, strs + "！", JScript.IconOption.错误);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }

        //if (fal)
        //    JScript.ShowAlert(this, "操作成功！");
        //else
        //    JScript.ShowAlert(this, "操作失败！");

        //重新绑定
        Bind();
    }
}