using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


public partial class Admin_Systems_PaybankInfo : AdminPageBase
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

    //关联银行卡表ID
    private string paid;
    public string Paid
    {
        get { return paid; }
        set { paid = value; }
    }

    //代理商Id
    public int DisId;
    //代理商地址Id
    public int AddrId;

    public string page;



    Hi.BLL.BD_Distributor BDdbutorbll = new Hi.BLL.BD_Distributor();

    protected void Page_Load(object sender, EventArgs e)
    {
        // Paid = Request.QueryString["paid"].ToString();

        if (Request.QueryString["action"] != null)
        {
            Action = Request.QueryString["action"].ToString();
        }
        if (Action == "Addr")
        {
            DisId = Convert.ToInt32(Request["DisId"]);
            AddrId = Convert.ToInt32(Request["AddrId"]);
           
        }

        if (!IsPostBack)
        {
           
            Bind();
           
        }
    }

    protected void Bind()
    {
        if (KeyID > 0)
        {
            Hi.Model.SYS_PaymentBank bankModel = new Hi.BLL.SYS_PaymentBank().GetModel(KeyID);

            this.lblDisUser.InnerText = bankModel.AccountName;//账户名称
            this.lblbankcode.InnerText = bankModel.bankcode;
            this.lblbankAddress.InnerText = bankModel.bankAddress;
            this.lblprivateCity.InnerText = bankModel.bankPrivate + "/" + bankModel.bankCity + "/" + bankModel.vdef1;
            this.lblisno.InnerText = bankModel.Isno == 1 ? "是" : "否";
            this.lblremake.InnerText = bankModel.Remark;
            this.lblType.InnerText = GetType(bankModel.type);
            if (bankModel.type == 11)
            {
                this.tbdis.Visible = true;
                this.lblpesontype.InnerText = GetPesonType(bankModel.vdef2);
                this.lblpesoncode.InnerText = bankModel.vdef3;
            }
            else
                this.tbdis.Visible = false;


            this.lblddlbank.InnerText = new Hi.BLL.PAY_PrePayment().GetBankNameBYbankID(bankModel.BankID.ToString());
            //this.lblstart.InnerText = bankModel.Start == 1 ? "已复核" : "未复核";
           
        }

    }

    /// <summary>
    /// 复核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAudit_Click(object sender, EventArgs e)
    {
        Hi.BLL.PAY_PaymentBank PAbll = new Hi.BLL.PAY_PaymentBank();
        Hi.Model.PAY_PaymentBank PAmodel = PAbll.GetModel(this.KeyID);

        if (PAmodel != null)
        {

            PAmodel.Start = 1;
            bool falg = PAbll.Update(PAmodel);
            if (falg)
            {
                JScript.ShowAlert(this, "操作成功！");
                Bind();
            }

        }
    }

    



   
    /// <summary>
    /// 获取收款类型
    /// </summary>
    /// <param name="type">收款类型Id</param>
    /// <returns></returns>
    public string GetType(int type)
    {
        string str = string.Empty;
        switch (type)
        {
            case 11:
                str = "个人账户";
                break;
            case 12:
                str = "企业账户";
                break;
            case 20:
                str = "支付账户";
                break;

        }
        return str;
    }

    /// <summary>
    /// 获取证件类型
    /// </summary>
    /// <param name="type">证件类型Id</param>
    /// <returns></returns>
    public string GetPesonType(string type)
    {
        string str = string.Empty;
        switch (type)
        {
            case "0":
                str = "身份证";
                break;
            case "1":
                str = "户口薄";
                break;
            case "2":
                str = "护照";
                break;
            case "3":
                str = "军官证";
                break;
            case "4":
                str = "士兵证";
                break;
            case "5":
                str = "港澳居民来往内地通行证";
                break;
            case "6":
                str = "台湾同胞来往内地通行证";
                break;
            case "7":
                str = "临时身份证";
                break;
            case "8":
                str = "外国人居留证";
                break;
            case "9":
                str = "警官证";
                break;
            default:
                str = "其他证件";
                break;


        }
        return str;
    }


}