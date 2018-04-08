using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

public partial class Company_Pay_PayCorrectInfo : CompPageBase
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
        if(KeyID>0)
        {

            Hi.Model.PAY_PrePayment Ppmodel = PAbll.GetModel(KeyID);
            this.lbldis.InnerText = Common.GetDis(Ppmodel.DisID, "DisName");
            this.lblcreatetime.InnerText = Convert.ToDateTime(Ppmodel.CreatDate).ToString("yyyy-MM-dd");
            this.lblauditstate.InnerText = Common.GetNameBYPreStart(Ppmodel.AuditState);
            this.lblcreateuser.InnerText =Common.GetUserName(Ppmodel.CrateUser);
            this.lblprice.InnerText = Convert.ToDecimal(Ppmodel.price).ToString("0.00");
            this.lblpaytype.InnerText = Common.GetPrePayStartName(Ppmodel.PreType);
            this.lblRemark.InnerText = Ppmodel.vdef1;
            //判断审批按钮是否可见
           // this.Audit.Visible= Ppmodel.AuditState == 2 ? false : true;
            this.Audit.Visible =false;
            this.Log.Visible = false;
        }

        
    }


    /// <summary>
    /// 审核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAudit_Click(object sender, EventArgs e)
    {

        Hi.Model.PAY_PrePayment PAmodel =this.PAbll.GetModel(KeyID);

        if (PAmodel != null)
        {
            if (PAmodel.AuditState !=Convert.ToInt32(Enums.PrePayState.已审))
            {                
                PAmodel.AuditState = 2;
                PAmodel.ID = KeyID;
                if ( PAbll.Update(PAmodel))
                {

                    //sum代理商全部补录，冲正金额
                    //decimal sums = new Hi.BLL.PAY_PrePayment().sums(PAmodel.DisID,PAmodel.CompID);

                    ////修改代理商的企业钱包金额
                    ////调用model,对属性进行赋值
                    //Hi.Model.BD_Distributor dismodel = new Hi.BLL.BD_Distributor().GetModel(PAmodel.DisID);
                    //dismodel.DisAccount = sums;
                    //dismodel.ID = PAmodel.DisID;
                    ////调用修改方法    
                    //Hi.BLL.BD_Distributor disupdate = new Hi.BLL.BD_Distributor();
                    //bool disup = disupdate.Update(dismodel);

                    // if (disup)
                    //{

                    Utils.AddSysBusinessLog(this.CompID, "PrePayment", KeyID.ToString(), "预收款冲正审核", "");
                    JScript.AlertMsgOne(this, "操作成功！", JScript.IconOption.笑脸);
                    Bind();

                    //}

                }
            }
            else
            {
                JScript.AlertMsgOne(this, "数据状态不正确,不能进行审核!", JScript.IconOption.错误);
            }
        }
        else
        {
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