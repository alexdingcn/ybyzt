using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Pay_PayExamineList : CompPageBase
{
    public string page = "1";//默认初始页
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            this.txtPager.Value = "12";
            Bind();
        }
    }

    /// <summary>
    /// 查询转账汇款记录
    /// </summary>
    public void Bind()
    {
        //查询转账汇款记录
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        //查询需要审核转账汇款记录
       // strwhere += " and PreType=1";
        strwhere += " and AuditState=0";
        strwhere += " and Start=1 and CompID=" + CompID;

        if (ViewState["strwhere"] != null)
        {
            strwhere += ViewState["strwhere"].ToString();
        }
        if (this.txtPager.Value.Trim().ToString() != "")
        {
            if (this.txtPager.Value.Trim().Length >= 5)
            {
                Pager.PageSize = 100;
                this.txtPager.Value = "100";
            }
            else
            {
                Pager.PageSize = this.txtPager.Value.Trim().ToInt(0);
            }
        }
        List<Hi.Model.PAY_PrePayment> Exa = new Hi.BLL.PAY_PrePayment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreatDate", true, strwhere, out pageCount, out Counts);

        this.rptExa.DataSource = Exa;
        this.rptExa.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }
    /// <summary>
    /// 搜索条件代理商，转账汇款日期
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = string.Empty;

        if (this.DisListID.Disid != "")
        {
            strWhere += " and DisID =" + this.DisListID.Disid;
        }
        if (this.txtPayCreateDate.Value != "")
        {
            strWhere += " and Paytime between '" +Common.NoHTML( this.txtPayCreateDate.Value) + "' and '" +Convert.ToDateTime(this.txtPayCreateDate.Value).AddDays(1) + "'";
        }

        ViewState["strwhere"] = strWhere;
        Bind();
    }
    /// <summary>
    /// 分页控件
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Pager_PageChanged(object sender, EventArgs e)
    {
        page = Pager.CurrentPageIndex.ToString();
        Bind();
    }
    /// <summary>
    /// 转账汇款审核
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSh_Click(object sender, EventArgs e)
    {
        string shId = this.hidShId.Value;
        //调用model,对属性进行赋值
        Hi.Model.PAY_PrePayment shModel = new Hi.BLL.PAY_PrePayment().GetModel(Convert.ToInt32(shId));
        shModel.AuditState = 2;
        //调用修改方法    
        Hi.BLL.PAY_PrePayment shUpdate = new Hi.BLL.PAY_PrePayment();
        bool shUp = shUpdate.Update(shModel);
        if (shUp ==bool .Parse("true"))
        {
            int hidShIds = shModel.DisID;
            Hi.BLL.PAY_PrePayment prePayModel = new Hi.BLL.PAY_PrePayment();
            //sum代理商全部录入，冲正以及审核通过的转账汇款金额
            decimal sums = prePayModel.sums(hidShIds,shModel.CompID);
            //修改代理商的企业钱包金额
            //调用model,对属性进行赋值
            Hi.Model.BD_Distributor disSumModel = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(hidShIds));
            disSumModel.DisAccount = sums;
            disSumModel.ID = shModel.ID;
            //调用修改方法    
            Hi.BLL.BD_Distributor disupdate = new Hi.BLL.BD_Distributor();
            bool disup = disupdate.Update(disSumModel);
            if (disup)
            {
                JScript.AlertMethod(this, "审核成功", JScript.IconOption.笑脸, "function (){ location.replace('" + ("PayExamineList.aspx") + "'); }");
            }
          
        }

    }
   
}