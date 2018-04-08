using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Admin_Systems_PaybankEdit : AdminPageBase
{
    /// <summary>
    /// 命令
    /// </summary>
    private string action;

    public string Action
    {
        get { return action; }
        set { action = value; }
    }

    //代理商Id
    public int DisId;
    //代理商地址Id
    public int AddrId;

    public string page;

    //收款帐号ID
    private int paid;

    public int Paid
    {
        get { return paid; }
        set { paid = value; }
    }

    Hi.BLL.BD_Distributor BDdbutorbll = new Hi.BLL.BD_Distributor();
    public int userid = 0;
    public string username = string.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        //Paid = Convert.ToInt32(Request.QueryString["paid"]);
        if (Request.QueryString["action"] != null)
        {
            Action = Request.QueryString["action"].ToString();
        }

        if (Action == "Addr")
        {
            DisId = Convert.ToInt32(Request["DisId"]);
            AddrId = Convert.ToInt32(Request["AddrId"]);
           
        }
        userid = this.UserID;
        username = this.UserName;
        this.hid_username.Value = username;
        this.hid_userid.Value = userid.ToString();
        if (!IsPostBack)
        {
           
            Bind();
        }
    }

    protected void Bind()
    {
        string str = string.Empty;
        if (KeyID > 0)
        {
            //修改时控制账户类型是否可用
            ddltype.Disabled = false;

            Hi.Model.SYS_PaymentBank bankModel = new Hi.BLL.SYS_PaymentBank().GetModel(KeyID);

            this.txtDisUser.Value = bankModel.AccountName;//账户名称
            this.txtbankcode.Value = bankModel.bankcode;
            this.txtbankAddress.Value = bankModel.bankAddress;
            //this.txtphone.InnerText = bankModel.vdef4;
            this.hid_tel.Value = bankModel.vdef4;
            this.hidProvince.Value = bankModel.bankPrivate;
            this.hidCity.Value = bankModel.bankCity;
            this.hidArea.Value = bankModel.vdef1;
            this.chkIsno.Checked = bankModel.Isno == 1 ? true : false;
            this.txtRemark.Value = bankModel.Remark;

            //现有银行列表
            string bankID_str = "102,103,104,105,301,100,303,305,306,302,310,309,401,403,307,308";
            string BankID = Convert.ToString(bankModel.BankID);

            bool index = bankID_str.Contains(BankID);
            if (index)
                this.ddlbank.Value = Convert.ToString(bankModel.BankID);
            else
            {
                this.ddlbank.Items.Insert(0, new ListItem(new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(BankID), BankID));
                this.ddlbank.SelectedIndex = 0;
            }

            this.ddltype.Value = Convert.ToString(bankModel.type);
            if (bankModel.type == 11)
            {
                this.tbdis.Visible = true;
                this.SltPesontype.Value = bankModel.vdef2;
                this.txtpesoncode.Value = bankModel.vdef3;
            }
            else
            {
                this.tbdis.Visible = false;
            }
        }
        else
        {
            //获取管理员手机号码
            string phone = new Hi.BLL.SYS_AdminUser().GetModel(UserID).Phone.ToString();
           // this.txtphone.InnerText = phone;
            this.hid_tel.Value = phone;
            //设置默认值
            ddltype.Value = "11";
            SltPesontype.Value = "0";

            //判断是否显示默认账户
            List<Hi.Model.SYS_PaymentBank> Sysl = new Hi.BLL.SYS_PaymentBank().GetList("", " Isno=1", "");
            int num = Sysl.Count;
            if (num >= 1)
                this.chkIsno.Checked = false;
        }
    }

    /// <summary>
    /// 银行卡绑定
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //声明变量
        string AccountName = string.Empty;
        string bankcode = string.Empty;
        string bankAddress = string.Empty;
        string bankPrivate = string.Empty;
        string bankCity = string.Empty;
        int Isno = 0;
        string remark = string.Empty;
        string county = string.Empty;//开会所在区县
        int BankId = 0;//银行Id
        int type = 0;//账户类型

        //收集数据
        AccountName = Common.NoHTML(this.txtDisUser.Value.Trim());//账户名称
        bankcode = Common.NoHTML(this.txtbankcode.Value.Replace(" ", ""));
        bankAddress = Common.NoHTML(this.txtbankAddress.Value.Trim());
        bankPrivate = Common.NoHTML(this.hidProvince.Value.Trim());
        bankCity = Common.NoHTML(this.hidCity.Value.Trim());
        county = Common.NoHTML(this.hidArea.Value.Trim());
        Isno = Convert.ToInt32(this.chkIsno.Checked == true ? 1 : 0);

        int is_no = Convert.ToInt32(this.hid_isno.Value);

        if (is_no == 1)
        {
            int success = new Hi.BLL.PAY_PrePayment().Upisno();
        }
        
        remark = Common.NoHTML(this.txtRemark.Value.Trim());
        BankId = Convert.ToInt32(this.ddlbank.Value);
        type = Convert.ToInt32(this.ddltype.Value);

        //实例化对象实体
        Hi.Model.SYS_PaymentBank pbModel = new Hi.Model.SYS_PaymentBank();

        // Paid = Convert.ToInt32(Request.QueryString["paid"]);
        //pbModel.paymentAccountID = Paid;
        pbModel.BankID = BankId;
        pbModel.AccountName = AccountName;
        pbModel.bankcode = bankcode;
        pbModel.bankAddress = bankAddress;
        pbModel.bankPrivate = bankPrivate;
        pbModel.bankCity = bankCity;
        pbModel.Isno = Isno;
        pbModel.Remark = remark;
        pbModel.CreateDate = DateTime.Now;
        pbModel.CreateUser = UserID;
        pbModel.dr = 0;
        pbModel.modifyuser = UserID;
        pbModel.ts = DateTime.Now;
        pbModel.vdef1 = county;
        pbModel.Start = 1;

       // pbModel.vdef4 = Common.NoHTML(this.txtphone.InnerText);
       // pbModel.vdef5 = Common.NoHTML(this.txtphpcode.Value);        
        pbModel.type = type;
        pbModel.vdef2 = Common.NoHTML(this.SltPesontype.Value);
        pbModel.vdef3 = Common.NoHTML(this.txtpesoncode.Value);
        if (KeyID > 0)
        {
            pbModel.ID = KeyID;
            bool result = new Hi.BLL.SYS_PaymentBank().Update(pbModel);

            if (result)
            {
                //判断平台是否有默认账户
                List<Hi.Model.SYS_PaymentBank> Sysl = new Hi.BLL.SYS_PaymentBank().GetList("", " Isno=1", "");
                int num = Sysl.Count;
                if (num == 0)
                    JScript.AlertMsg(this, "平台最好有一个，默认第一收款账户，请检查！", "PaybankInfo.aspx?paid=" + Paid + "&KeyID=" + KeyID);
                else
                    Response.Redirect("PaybankInfo.aspx?paid=" + Paid + "&KeyID=" + KeyID);

            }
        }
        else
        {
            int succes = new Hi.BLL.SYS_PaymentBank().Add(pbModel);
            if (succes > 0)//银行账户保存成功后，进行绑定代理商操作
            {
                //判断平台是否有默认账户
                List<Hi.Model.SYS_PaymentBank> Sysl = new Hi.BLL.SYS_PaymentBank().GetList("", " Isno=1", "");
                int num = Sysl.Count;
                if (num == 0)
                    JScript.AlertMsg(this, "平台最好有一个，默认第一收款账户，请检查！", "PaybankInfo.aspx?paid=" + Paid + "&KeyID=" + succes);
                else
                    Response.Redirect("PaybankInfo.aspx?paid=" + Paid + "&KeyID=" + succes);
            }
        }
    }
   
    /// <summary>
    /// 选择银行返回
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSelectBankReturn_ServerClick(object sender, EventArgs e)
    {
        string bankname = Common.NoHTML(this.txtbandname.Value);
        string bankcode = Common.NoHTML(this.txtbankcodes.Value);

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
}