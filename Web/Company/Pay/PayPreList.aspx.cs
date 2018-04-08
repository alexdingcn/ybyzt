using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Company_Pay_PayPreList : CompPageBase
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
    /// 代理商列表显示
    /// </summary>
    public void Bind()
    {
       
        //price = new Hi.BLL.PAY_PrePayment().sums(UserModel.DisID, UserModel.CompID).ToString("0.00");
        //查询录入明细
        int pageCount = 0;
        int Counts = 0;
        string strwhere = string.Empty;
        //查询全部手动录入冲正
        // strwhere += " and PreType=3";
        //有效数据显示
        // strwhere += " and IsEnabled = 1";
        //审核状态是已审的
        //strwhere += " and AuditState = 2";
        //付款状态是成功的
        strwhere += " and Start=1";

        //所属代理商
        strwhere += "  and PreType <>6 and PreType <> 7  and DisID=" +KeyID;
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
        List<Hi.Model.PAY_PrePayment> pay = new Hi.BLL.PAY_PrePayment().GetList(Pager.PageSize, Pager.CurrentPageIndex, "CreatDate", true, strwhere, out pageCount, out Counts);

        this.rptOrder.DataSource = pay;
        this.rptOrder.DataBind();

        Pager.RecordCount = Counts;
        page = Pager.CurrentPageIndex.ToString();
    }

    /// <summary>
    /// 代理商搜索
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnSearch_Click(object sender, EventArgs e)
    {
        string strWhere = string.Empty;
        string paycode =Common.NoHTML( this.txtPaycode.Value.Replace("'", "''"));

        if (!string.IsNullOrEmpty(paycode))
        {
            strWhere += " and ID like '%" + paycode + "%'";
        }
        if (this.txtPayCreateDate.Value != "")
        {
            strWhere += " and Paytime >= '" +Common.NoHTML( this.txtPayCreateDate.Value) + "'";
        }

        if (this.txtPayEnddate.Value != "")
        {
            strWhere += " and Convert(varchar(10),paytime,120) <='" +Common.NoHTML( this.txtPayEnddate.Value )+ "'";
        }

        if (this.txtPayCreateDate.Value != "" && this.txtPayEnddate.Value != "")
        {
            strWhere += " and Convert(varchar(10),paytime,120) between '" +Common.NoHTML( this.txtPayCreateDate.Value) + "' and   '" +Common.NoHTML( this.txtPayEnddate.Value) + "'";
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
    /// 新建冲正企业钱包
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnAdd_Click(object sender, EventArgs e)
    {
        //定义冲正的变量
        string txtPayCorrectDis = string.Empty;
        string txtPayCorrectPrice = string.Empty;
        //string txtPayCorrectDate = string.Empty;
        string txtPayCorrectType = string.Empty;
        string txtPayCorrectRemark = string.Empty;
        //获取冲正输入的数据
        txtPayCorrectDis =Common.NoHTML( this.txtPayCorrectDis.Value.Trim().ToString());
        txtPayCorrectPrice = Common.NoHTML(this.txtPayCorrectPrice.Value.Trim().ToString());
        //txtPayCorrectDate = this.txtPayCorrectDate.Value.Trim().ToString();
        txtPayCorrectType = Common.NoHTML(this.txtPayCorrectType.Value.Trim().ToString());
        txtPayCorrectRemark = Common.NoHTML(this.txtPayCorrectRemark.Value.Trim().ToString());
        //调用model,对属性进行赋值
        Hi.Model.PAY_PrePayment prepaymentmodel = new Hi.Model.PAY_PrePayment();
        prepaymentmodel.CompID = 1;
        prepaymentmodel.DisID = Convert.ToInt32(txtPayCorrectDis);
        prepaymentmodel.Start = 1;
        prepaymentmodel.PreType = Convert.ToInt32(txtPayCorrectType);
        prepaymentmodel.price =-Convert.ToDecimal(txtPayCorrectPrice);
        prepaymentmodel.Paytime = DateTime.Now;
        prepaymentmodel.CreatDate = DateTime.Now;
        prepaymentmodel.CrateUser = 1;
        prepaymentmodel.AuditState = 0;
        prepaymentmodel.IsEnabled = 1;
        prepaymentmodel.ts = DateTime.Now;
        prepaymentmodel.dr = 1;
        prepaymentmodel.modifyuser = 1;
        prepaymentmodel.vdef1 = txtPayCorrectRemark;
        //调用保存方法
        Hi.BLL.PAY_PrePayment prepaymentsave = new Hi.BLL.PAY_PrePayment();
        int reslut = prepaymentsave.Add(prepaymentmodel);
        //判断返回值int
        if (reslut > 0)
        {
            //sum代理商全部录入，冲正金额
            //decimal sums = prepaymentsave.sums(txtPayCorrectDis);
            //修改代理商的企业钱包金额
            //调用model,对属性进行赋值
            //Hi.Model.BD_Distributor dismodel = new Hi.BLL.BD_Distributor().GetModel(Convert.ToInt32(txtPayCorrectDis));
            //dismodel.DisAccount = sums;
            //调用修改方法    
            //Hi.BLL.BD_Distributor disupdate = new Hi.BLL.BD_Distributor();
            //bool disup = disupdate.Update(dismodel);

            JScript.AlertMethod(this, "冲正成功！", JScript.IconOption.笑脸, "function (){ location.replace('" + ("PayCorrectList.aspx") + "'); }");

        }
        else
        {
            JScript.AlertMethod(this, "冲正失败！", JScript.IconOption.错误, "function (){ location.replace('" + ("PayCorrectList.aspx") + "'); }");
        }



        
    }
}