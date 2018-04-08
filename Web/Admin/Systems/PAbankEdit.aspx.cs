using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Admin_Systems_PAbankEdit : AdminPageBase
{
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
    //厂商id
    public int CompID = 0;

    protected void Page_Load(object sender, EventArgs e)
    {
        userid = this.UserID;
        username = this.UserName;
        this.hid_username.Value = username;
        this.hid_userid.Value = userid.ToString();

        //修改时的主键ID
        paid = Convert.ToInt32(Request.QueryString["paid"]);//收款帐号ID

        if (!IsPostBack)
        {
            //新增copmid
            CompID = Convert.ToInt32(Request["KeyID"]);//厂商ID

            Bind();
        }
    }

    protected void Bind()
    {
        string str = string.Empty;

        if (paid > 0)
        {
            ////修改时控制账户类型是否可用
            ddltype.Disabled = false;

            Hi.Model.PAY_PaymentBank bankModel = new Hi.BLL.PAY_PaymentBank().GetModel(paid);

            this.txtDisUser.Value = bankModel.AccountName;//账户名称
            this.txtbankcode.Value = bankModel.bankcode;
            this.txtbankAddress.Value = bankModel.bankAddress;
            this.hid_tel.Value = bankModel.vdef4 == "" ? new Hi.BLL.PAY_PrePayment().GetPhoneBYCompID(bankModel.CompID).ToString() : bankModel.vdef4;

            this.hidProvince.Value = bankModel.bankPrivate;
            this.hidCity.Value = bankModel.bankCity;
            this.hidArea.Value = bankModel.vdef1;

            //给隐藏域赋值
            this.txtcompId.Value = bankModel.CompID.ToString();
            CompID = bankModel.CompID;

            this.chkIsno.Checked = bankModel.Isno == 1 ? true : false;

            if (bankModel.Isno == 1)
            {
                this.btnDis.Attributes.Add("style", "display:none");
                div_grid.Attributes.Add("style", "display:none");
            }
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
                //this.tbdis.Visible = false;
                this.tbdis.Attributes.Add("style", "display:none");
            }

        }
        else
        {
            //给隐藏域赋值  厂商ID
            this.txtcompId.Value = CompID.ToString();

            this.hid_tel.Value = new Hi.BLL.PAY_PrePayment().GetPhoneBYCompID(CompID).ToString();
            //设置默认值
            ddltype.Value = "11";
            SltPesontype.Value = "0";

            int num = new Hi.BLL.PAY_PrePayment().GetBankBYCompID(CompID);
            if (num >= 1)
            {
                this.chkIsno.Checked = false;

                this.gvDtl.Visible = true;
                this.btnDis.Visible = true;

            }
            else
            {
                this.gvDtl.Visible = false;
                this.btnDis.Visible = false;
            }
        }
        str = new Hi.BLL.PAY_PrePayment().GetDisIDBYCompID(CompID);
        if (str != "")
        {
            this.txtGoodsCodes.Value = new Hi.BLL.PAY_PrePayment().GetDisIDBYCompID(CompID) + ",";// str;
        }
    }

    /// <summary>
    /// 银行卡绑定、关联代理商
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
            int success = new Hi.BLL.PAY_PrePayment().UpisnoBYCompID(Convert.ToInt32(this.txtcompId.Value));
        }

        remark = Common.NoHTML(this.txtRemark.Value.Trim());
        BankId = Convert.ToInt32(this.ddlbank.Value);
        type = Convert.ToInt32(this.ddltype.Value);

        //实例化对象实体
        Hi.Model.PAY_PaymentBank pbModel = new Hi.Model.PAY_PaymentBank();
        pbModel.paymentAccountID = Paid;
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
        pbModel.type = type;
        pbModel.vdef2 = Common.NoHTML(this.SltPesontype.Value);
        pbModel.vdef3 = Common.NoHTML(this.txtpesoncode.Value);
        if (paid > 0)
        {
            pbModel.CompID = Convert.ToInt32(this.txtcompId.Value);// 厂商ID
            pbModel.ID = paid;
            bool result = new Hi.BLL.PAY_PaymentBank().Update(pbModel);

            if (result)
            {
                Response.Redirect("PayBankAuditInfo.aspx?KeyID=" + paid);
            }
        }
        else
        {
            pbModel.CompID = Convert.ToInt32(this.txtcompId.Value);// 厂商ID

            int succes = new Hi.BLL.PAY_PaymentBank().Add(pbModel);
            if (succes > 0)//银行账户保存成功
            {
                Response.Redirect("PayBankAuditInfo.aspx?KeyID=" + succes);
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

        //判断关联代理商按钮是否可用
        if (this.chkIsno.Checked)
        {
            this.btnDis.Attributes.Add("style", "display:none;");
            div_grid.Attributes.Add("style", "display:none;");
        }
        else
        {
            this.btnDis.Attributes.Add("style", "display:block;");
            div_grid.Attributes.Add("style", "display:block;");
        }
    }
}