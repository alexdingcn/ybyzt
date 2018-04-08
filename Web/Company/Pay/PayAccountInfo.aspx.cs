using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_Order_PayAccountInfo : CompPageBase
{

    Hi.BLL.PAY_PaymentAccount PAbll = new Hi.BLL.PAY_PaymentAccount();

    Hi.BLL.PAY_PaymentBank PbankBLL = new Hi.BLL.PAY_PaymentBank();

    public string page = "1";//默认初始页

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Bind();
        }
    }

    public void Bind()
    {
        //取消按钮显示
        //this.btnRemove.Attributes.Add("style", "display:none;");
        //this.btnRemove.Style["display"] = "none";
        if(KeyID>0)
        {
          
            Hi.Model.PAY_PaymentAccount PAmodel = PAbll.GetModel(KeyID);
            this.lblcomp.InnerText =new Hi.BLL.BD_Company().GetModel(PAmodel.CompID).CompName;
            this.lblqy.InnerText = Common.GetDisAreaNameById(Convert.ToInt32(PAmodel.Region==""?"0":PAmodel.Region));
            this.lblpayname.InnerText = PAmodel.payName;
            this.lblpaycode.InnerText = PAmodel.PayCode;
            this.lblorgcode.InnerText = PAmodel.OrgCode;
            this.lbltype.InnerText = GetType(PAmodel.type);
            this.lblRemark.InnerText = PAmodel.Remark;

            //绑定收款银行卡信息
            BindBankDetail(KeyID);
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
    /// 绑定银行卡明细
    /// </summary>
    public void BindBankDetail(int paymentAccountID)
    {
        DataTable banklist = PbankBLL.GetBankBYacountID(paymentAccountID);
        this.rpDtl.DataSource = banklist;
        this.rpDtl.DataBind();

    }
    //删除一行记录
    //modify  by ggh
    //
    protected void rptPAcount_ItemCommand(object source, RepeaterCommandEventArgs e)
    {
        string type = e.CommandName;
        int Id = Convert.ToInt32(e.CommandArgument);

        if (type == "del")
        {
          Hi.BLL.PAY_PaymentBank bankBll=  new Hi.BLL.PAY_PaymentBank();

          bool falg = bankBll.Delete(Id);
                if (falg)
                {
                    JScript.AlertMsgOne(this, "操作成功！", JScript.IconOption.笑脸);
                    Bind();
                }

           
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